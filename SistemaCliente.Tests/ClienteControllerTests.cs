using Microsoft.AspNetCore.Mvc;
using SistemaCliente.Controllers;
using SistemaCliente.Model;

namespace SistemaCliente.Tests;

[Collection("Sequential")]
public class ClienteControllerTests
{
    private readonly ClienteController _controller;
    private readonly string _caminhoCobolTestes = @"C:\Users\leoam\source\repos\ProjetoFinal\SistemaCliente\CobolTests";


    public ClienteControllerTests()
    {
        

        if (!Directory.Exists(_caminhoCobolTestes))
            Directory.CreateDirectory(_caminhoCobolTestes);

        string dllOriginal = @"C:\Users\leoam\source\repos\ProjetoFinal\SistemaCliente\Cobol\sistcli.dll";
        File.Copy(dllOriginal, Path.Combine(_caminhoCobolTestes, "sistcli.dll"), true);
        _controller = new ClienteController(_caminhoCobolTestes);
    }

    [Fact]
    public void CadastrarTestes()
    {
        string arquivoTesteClientes = Path.Combine(_caminhoCobolTestes, "clientes.dat");
        if (File.Exists(arquivoTesteClientes))
            File.Delete(arquivoTesteClientes);

        var novoCliente = new Cliente
        {
            Codigo = "99999",
            Nome = "Cliente Teste",
            Telefone = "11999999999",
            Email = "teste@automatizado.com"
        };

        var resultado = _controller.Cadastrar(novoCliente) as CreatedAtActionResult;
        Assert.NotNull(resultado);
        Assert.Equal(201, resultado.StatusCode);
        var clienteRetornado = resultado.Value as Cliente;
        Assert.NotNull(clienteRetornado);
        Assert.Equal("99999", clienteRetornado.Codigo);
        Assert.Equal("Cliente Teste", clienteRetornado.Nome);
    }

}
