using System;

namespace WindowsFormsAppArvoredo
{
    public class ProdutoEstoque
    {
        public int Id { get; set; }                     // Identificador único
        public string Nome { get; set; }                // Nome ou descrição do produto
        public string Tipo { get; set; }                // Categoria ou tipo do produto
        public int QuantidadeDisponivel { get; set; }   // Quantidade atual em estoque
        public int QuantidadeMinima { get; set; }       // Quantidade mínima para alerta
        public string Unidade { get; set; }             // Unidade de medida (UN, KG, L)
        public decimal PrecoUnitario { get; set; }      // Preço de cada unidade
        public DateTime UltimaAtualizacao { get; set; } // Data da última atualização
    }
}
