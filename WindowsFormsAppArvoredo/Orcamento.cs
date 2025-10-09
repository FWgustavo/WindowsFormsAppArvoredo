using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsAppArvoredo
{
    /// <summary>
    /// Classe que representa um orçamento completo
    /// </summary>
    public class Orcamento
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public DateTime DataEmissao { get; set; }
        public string Endereco { get; set; }
        public string CEP { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CPF_CNPJ { get; set; }
        public string Telefone { get; set; }
        public string Numero { get; set; }
        public string Vendedor { get; set; }
        public List<ItemOrcamento> Itens { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Desconto { get; set; }
        public decimal Acrescimo { get; set; }
        public decimal TotalGeral { get; set; }
        public string Status { get; set; }

        public Orcamento()
        {
            Itens = new List<ItemOrcamento>();
            DataEmissao = DateTime.Now;
            Status = "Pendente";
        }

        /// <summary>
        /// Retorna a quantidade total de itens no orçamento
        /// </summary>
        public int QuantidadeItens => Itens?.Count ?? 0;

        /// <summary>
        /// Retorna um resumo formatado do orçamento
        /// </summary>
        public string ObterResumo()
        {
            return $"Orçamento #{Id} - {Cliente} - {DataEmissao:dd/MM/yyyy} - {TotalGeral:C}";
        }
    }

    /// <summary>
    /// Classe que representa um item (produto) dentro de um orçamento
    /// </summary>
    public class ItemOrcamento
    {
        public int Sequencia { get; set; }
        public string Descricao { get; set; }
        public string Unidade { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
        public Produto ProdutoOrigem { get; set; }

        public ItemOrcamento()
        {
            Quantidade = 1;
        }

        /// <summary>
        /// Recalcula o valor total baseado na quantidade e valor unitário
        /// </summary>
        public void RecalcularTotal()
        {
            ValorTotal = Quantidade * ValorUnitario;
        }

        /// <summary>
        /// Retorna uma representação em string do item
        /// </summary>
        public override string ToString()
        {
            return $"{Sequencia} - {Descricao} - {Quantidade} {Unidade} x {ValorUnitario:C} = {ValorTotal:C}";
        }
    }
}