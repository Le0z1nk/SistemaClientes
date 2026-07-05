namespace SistemaCliente.Model;

public class Cliente
{
    public string Codigo { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }

    public Cliente() { }

    public Cliente(string codigo, string nome, string telefone, string email)
    {
        this.Codigo = codigo;
        this.Nome = nome;
        this.Telefone = telefone;
        this.Email = email;
    }
}
