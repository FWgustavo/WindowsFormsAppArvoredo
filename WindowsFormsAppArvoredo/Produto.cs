using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppArvoredo
{
    public class Produto
    {
        public int Sequencia { get; set; }            // id / índice
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public string Unidade { get; set; }
        public decimal Quantidade { get; set; }
        public int QuantidadeMinima { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal => Quantidade * ValorUnitario;
        public DateTime UltimaAtualizacao { get; set; }

        public Produto()
        {
            Sequencia = 0;
            Descricao = string.Empty;
            Tipo = string.Empty;
            Unidade = string.Empty;
            Quantidade = 0;
            QuantidadeMinima = 0;
            ValorUnitario = 0;
            UltimaAtualizacao = DateTime.Now;
        }
    }

    public class Orcamento
    {
        public int Id { get; set; }
        public DateTime DataEmissao { get; set; }
        public string Cliente { get; set; }
        public string Endereco { get; set; }
        public string CepMunicipio { get; set; }
        public string Vendedor { get; set; }
        public string CnpjCpf { get; set; }
        public string Bairro { get; set; }
        public string Telefone { get; set; }
        public string NomeFantasia { get; set; }
        public List<Produto> Produtos { get; set; }
        public decimal Desconto { get; set; }
        public decimal Acrescimo { get; set; }
        public decimal TotalProdutos
        {
            get
            {
                return Produtos?.Sum(p => p.ValorTotal) ?? 0;
            }
        }
        public decimal TotalGeral
        {
            get
            {
                return TotalProdutos - Desconto + Acrescimo;
            }
        }

        public Orcamento()
        {
            Id = 0;
            DataEmissao = DateTime.Now;
            Cliente = string.Empty;
            Endereco = string.Empty;
            CepMunicipio = string.Empty;
            Vendedor = string.Empty;
            CnpjCpf = string.Empty;
            Bairro = string.Empty;
            Telefone = string.Empty;
            NomeFantasia = string.Empty;
            Produtos = new List<Produto>();
            Desconto = 0;
            Acrescimo = 0;
        }

        public void AdicionarProduto(Produto produto)
        {
            if (produto != null)
            {
                produto.Sequencia = Produtos.Count + 1;
                Produtos.Add(produto);
            }
        }

        public void RemoverProduto(int sequencia)
        {
            var produto = Produtos.FirstOrDefault(p => p.Sequencia == sequencia);
            if (produto != null)
            {
                Produtos.Remove(produto);
                // Reordenar sequências
                for (int i = 0; i < Produtos.Count; i++)
                {
                    Produtos[i].Sequencia = i + 1;
                }
            }
        }

        public bool ValidarOrcamento()
        {
            if (string.IsNullOrWhiteSpace(Cliente))
                return false;

            if (string.IsNullOrWhiteSpace(CnpjCpf))
                return false;

            if (Produtos == null || !Produtos.Any())
                return false;

            return true;
        }
    }
}