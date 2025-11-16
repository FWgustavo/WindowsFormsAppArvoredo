using System;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormCadastroUsuario : Form
    {
        public Usuario UsuarioCriado { get; private set; }
        private Usuario usuarioEditando;

        public FormCadastroUsuario()
        {
            InitializeComponent();
        }

        public FormCadastroUsuario(Usuario usuario) : this()
        {
            usuarioEditando = usuario;
            PreencherCampos(usuario);
            lblTitulo.Text = "EDITAR USUÁRIO";
            btnCadastrar.Text = "SALVAR";
        }

        private void PreencherCampos(Usuario usuario)
        {
            txtNome.Text = usuario.Nome;
            txtLogin.Text = usuario.Login;
            txtSenha.Text = usuario.Senha;
            txtEmail.Text = usuario.Email;
            cbPerfil.SelectedItem = usuario.Perfil;
            chkAtivo.Checked = usuario.Ativo;
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
                return;

            UsuarioCriado = new Usuario
            {
                Id = usuarioEditando?.Id ?? 0,
                Nome = txtNome.Text.Trim(),
                Login = txtLogin.Text.Trim(),
                Senha = txtSenha.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Perfil = cbPerfil.SelectedItem?.ToString() ?? "Usuario",
                Ativo = chkAtivo.Checked
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Por favor, preencha o nome do usuário.", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Por favor, preencha o login.", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Por favor, preencha a senha.", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenha.Focus();
                return false;
            }

            if (cbPerfil.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, selecione um perfil.", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbPerfil.Focus();
                return false;
            }

            return true;
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtLogin.Clear();
            txtSenha.Clear();
            txtEmail.Clear();
            cbPerfil.SelectedIndex = -1;
            chkAtivo.Checked = true;
            txtNome.Focus();
        }
    }
}