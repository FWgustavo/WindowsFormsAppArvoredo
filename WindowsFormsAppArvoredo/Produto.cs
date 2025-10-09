using System;

namespace WindowsFormsAppArvoredo
{
    /// <summary>
    /// Classe que representa um produto no estoque
    /// </summary>
    public class Produto
    {
        public int Sequencia { get; set; }
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
}