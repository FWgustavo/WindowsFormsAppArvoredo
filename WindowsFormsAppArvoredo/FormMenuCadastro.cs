using System;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormMenuCadastro : Form
    {
        public FormMenuCadastro()
        {
            InitializeComponent();
        }

        private void btnCadastroClientes_Click(object sender, EventArgs e)
        {
            using (FormCadastroCliente formCadastro = new FormCadastroCliente())
            {
                if (formCadastro.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Cliente cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnCadastroFornecedores_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Abrir cadastro de fornecedores");
        }

        private void btnCadastroProdutos_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Abrir cadastro de produtos");
        }

        private void btnCadastroUsuarios_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Abrir cadastro de usuários");
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}