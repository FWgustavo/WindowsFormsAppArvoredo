using System;

namespace WindowsFormsAppArvoredo
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Telefone { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Municipio { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }

        public Cliente()
        {
            DataCadastro = DateTime.Now;
        }
    }

    public class Fornecedor
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CnpjCpf { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public DateTime DataCadastro { get; set; }

        public Fornecedor()
        {
            DataCadastro = DateTime.Now;
        }
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Perfil { get; set; } // Admin, Usuario, Vendedor
        public string Email { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }

        public Usuario()
        {
            Ativo = true;
            DataCadastro = DateTime.Now;
        }
    }
}