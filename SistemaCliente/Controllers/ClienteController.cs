using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCliente.DTO;
using SistemaCliente.Model;
using System.Diagnostics;

namespace SistemaCliente.Controllers;

[ApiController]
[Route("api/clientes")]
[Produces("application/json")]
public class ClienteController : ControllerBase
{
    private readonly string _caminhoUtils = @"C:\Program Files (x86)\OpenCobolIDE\GnuCOBOL\bin\cobcrun.exe";
    private readonly string _caminhoCobol = @"C:\Users\leoam\source\repos\ProjetoFinal\SistemaCliente\Cobol";
    private readonly string _arquivoResposta = @"C:\Users\leoam\source\repos\ProjetoFinal\SistemaCliente\Cobol\resposta.txt";

    [HttpPost]
    public IActionResult Cadastrar([FromBody] Cliente cliente)
    {
        //serve para formatação das strings 
        string codigo = (cliente.Codigo ?? "").PadRight(5)[..5];
        string nome = (cliente.Nome ?? "").PadRight(50)[..50];
        string telefone = (cliente.Telefone ?? "").PadRight(15)[..15];
        string email = (cliente.Email ?? "").PadRight(40)[..40];
        string args = $"CAD{codigo}{nome}{telefone}{email}";
        ExecutarCobol(args);
        if (!System.IO.File.Exists(_arquivoResposta))
            return StatusCode(500, "Erro no cadastro");

        string resultado = System.IO.File.ReadAllText(_arquivoResposta).TrimEnd();
        if (resultado.StartsWith("DUPLICATE_KEY"))
            return Conflict(new { mensagem = "Este código de cliente já existe no arquivo indexado." });

        if (resultado.StartsWith("NOT_FOUND"))
            return NotFound(new { mensagem = "Cliente não encontrado." });
        var novoCliente = LayoutCobol(resultado);
        return CreatedAtAction(nameof(ConsultarPorCodigo), new { codigo = novoCliente.Codigo }, novoCliente);

    }

    [HttpGet("{codigo}")]
    public IActionResult ConsultarPorCodigo(string codigo)
    {
        codigo = codigo.PadRight(5)[..5];
        string args = $"CON{codigo}";
        ExecutarCobol(args);
        if (!System.IO.File.Exists(_arquivoResposta))
            return StatusCode(500, "Erro na consulta");

        string resultado = System.IO.File.ReadAllText(_arquivoResposta).TrimEnd();
        var cliente = LayoutCobol(resultado);
        return Ok(cliente);
    }

    [HttpPut("{codigo}")]
    public IActionResult Atualizar(string codigo, [FromBody] Cliente cliente)
    {
        codigo = codigo = codigo.PadRight(5)[..5];
        string nome = (cliente.Nome ?? "").PadRight(50)[..50];
        string telefone = (cliente.Telefone ?? "").PadRight(15)[..15];
        string email = (cliente.Email ?? "").PadRight(40)[..40];
        string args = $"ALT{codigo}{nome}{telefone}{email}";
        ExecutarCobol(args);
        if (!System.IO.File.Exists(_arquivoResposta))
            return StatusCode(500, "Erro na alteração");

        string resultado = System.IO.File.ReadAllText(_arquivoResposta).TrimEnd();
        if (resultado.StartsWith("NOT_FOUND"))
            return NotFound(new { mensagem = "Cliente não encontrado para a alteração do contato" });

        var clienteAtualizado = LayoutCobol(resultado);
        return Ok(new { mensagem = "Dados atualizados", cliente = clienteAtualizado });
    }

    private void ExecutarCobol(string args)
    {
        //serve para evitar leitura suja
        if (System.IO.File.Exists(_arquivoResposta))
            System.IO.File.Delete(_arquivoResposta);

        //string caminhoDll = Path.Combine(_caminhoCobol, "SistCli.dll");

        var startInfo = new ProcessStartInfo
        {
            FileName = _caminhoUtils,
            Arguments = $"sistcli \"{args}\"",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = _caminhoCobol
        };

        string gnuCobolBin = Path.GetDirectoryName(_caminhoUtils);
        startInfo.EnvironmentVariables["PATH"] = $"{gnuCobolBin};{Environment.GetEnvironmentVariable("PATH")}";
        startInfo.EnvironmentVariables["COBOL_LIBRARY_PATH"] = _caminhoCobol;

        try
        {
            using (var process = Process.Start(startInfo))
            {
                if (process == null)
                {
                    Debug.WriteLine("CRÍTICO: O processo COBOL não pôde ser iniciado (Retornou nulo).");
                    return;
                }

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (!string.IsNullOrEmpty(output))
                    Debug.WriteLine($"[COBOL OUTPUT]: {output}");

                if (!string.IsNullOrEmpty(error))
                    Debug.WriteLine($"[COBOL RUNTIME ERROR]: {error}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERRO AO DISPARAR PROCESSO]: {ex.Message}");
        }

    }

    private Cliente LayoutCobol(string linha)
    {
        if (linha.Length < 110)
        {
            linha = linha.PadRight(110);
        }

        return new Cliente
        {
            Codigo = linha[..5].Trim(),
            Nome = linha.Substring(5, 50).Trim(),
            Telefone = linha.Substring(55, 15).Trim(),
            Email = linha.Substring(70, 40).Trim()
        };
    }
}
