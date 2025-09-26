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
            // TODO: abrir o form de cadastro de clientes
            MessageBox.Show("Abrir cadastro de clientes");
        }

        private void btnCadastroFornecedores_Click(object sender, EventArgs e)
        {
            // TODO: abrir o form de cadastro de fornecedores
            MessageBox.Show("Abrir cadastro de fornecedores");
        }

        private void btnCadastroProdutos_Click(object sender, EventArgs e)
        {
            // TODO: abrir o form de cadastro de produtos
            MessageBox.Show("Abrir cadastro de produtos");
        }

        private void btnCadastroUsuarios_Click(object sender, EventArgs e)
        {
            // TODO: abrir o form de cadastro de usuários
            MessageBox.Show("Abrir cadastro de usuários");
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
