using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormListaProdutos : Form
    {
        private List<TelaArvoredo.ProdutoEstoque> _produtos;

        public FormListaProdutos(List<TelaArvoredo.ProdutoEstoque> produtos)
        {
            InitializeComponent();
            _produtos = produtos ?? new List<TelaArvoredo.ProdutoEstoque>();
            CarregarProdutos();
        }

        private void CarregarProdutos()
        {
            dgvProdutos.Rows.Clear();

            foreach (var p in _produtos)
            {
                dgvProdutos.Rows.Add(
                    p.Id,
                    p.Nome,
                    p.Tipo,
                    p.QuantidadeDisponivel,
                    p.QuantidadeMinima,
                    p.Unidade,
                    p.PrecoUnitario.ToString("C"), // formato moeda
                    p.UltimaAtualizacao.ToString("dd/MM/yyyy") // formato data
                );
            }
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Novo produto (abrir form de cadastro).");
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProdutos.CurrentRow == null)
            {
                MessageBox.Show("Selecione um produto para editar.");
                return;
            }

            var id = dgvProdutos.CurrentRow.Cells["Id"].Value.ToString();
            MessageBox.Show($"Editar produto ID {id} (abrir form de edição).");
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvProdutos.CurrentRow == null)
            {
                MessageBox.Show("Selecione um produto para excluir.");
                return;
            }

            var nome = dgvProdutos.CurrentRow.Cells["Nome"].Value.ToString();

            var confirm = MessageBox.Show($"Deseja realmente excluir '{nome}'?",
                                          "Confirmação",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                // Aqui você remove da lista de produtos
                var id = Convert.ToInt32(dgvProdutos.CurrentRow.Cells["Id"].Value);
                _produtos.RemoveAll(p => p.Id == id);

                CarregarProdutos();

                MessageBox.Show("Produto excluído com sucesso!");
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
