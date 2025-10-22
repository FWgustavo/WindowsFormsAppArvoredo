using System;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormCadastroCliente : Form
    {
        public Cliente ClienteCriado { get; private set; }
        private Cliente clienteEditando;

        public FormCadastroCliente()
        {
            InitializeComponent();
        }

        public FormCadastroCliente(Cliente cliente) : this()
        {
            clienteEditando = cliente;
            PreencherCampos(cliente);
            lblTitulo.Text = "EDITAR CLIENTE";
            btnCadastrar.Text = "SALVAR";
        }

        private void PreencherCampos(Cliente cliente)
        {
            txtNome.Text = cliente.Nome;
            txtCpfCnpj.Text = cliente.CpfCnpj;
            txtTelefone.Text = cliente.Telefone;
            txtCep.Text = cliente.Cep;
            txtEndereco.Text = cliente.Endereco;
            txtBairro.Text = cliente.Bairro;
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
                return;

            ClienteCriado = new Cliente
            {
                Id = clienteEditando?.Id ?? 0,
                Nome = txtNome.Text.Trim(),
                CpfCnpj = txtCpfCnpj.Text.Trim(),
                Telefone = txtTelefone.Text.Trim(),
                Cep = txtCep.Text.Trim(),
                Endereco = txtEndereco.Text.Trim(),
                Bairro = txtBairro.Text.Trim()
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Por favor, preencha o nome do cliente.", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCpfCnpj.Text))
            {
                MessageBox.Show("Por favor, preencha o CPF/CNPJ.", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCpfCnpj.Focus();
                return false;
            }

            return true;
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtCpfCnpj.Clear();
            txtTelefone.Clear();
            txtCep.Clear();
            txtEndereco.Clear();
            txtBairro.Clear();
            txtNome.Focus();
        }
    }
}