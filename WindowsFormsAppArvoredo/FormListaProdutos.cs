using System;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormListaProdutos : Form
    {
        public FormListaProdutos()
        {
            InitializeComponent();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            // TODO: abrir tela de cadastro de novo produto
            MessageBox.Show("Novo produto");
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProdutos.CurrentRow == null)
            {
                MessageBox.Show("Selecione um produto para editar.");
                return;
            }

            // TODO: abrir form de edição com dados do produto selecionado
            MessageBox.Show("Editar produto selecionado");
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvProdutos.CurrentRow == null)
            {
                MessageBox.Show("Selecione um produto para excluir.");
                return;
            }

            var confirm = MessageBox.Show("Deseja realmente excluir este produto?",
                                          "Confirmação",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                // TODO: excluir produto selecionado
                MessageBox.Show("Produto excluído (simulação)");
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
