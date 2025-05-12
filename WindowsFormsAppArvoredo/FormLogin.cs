using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormLogin : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
           int nLeft,
           int nTop,
           int nRight,
           int nBottom,
           int nWidthEllipse,
           int nHeightEllipse
        );
        public static class Conexao
        {
            // String de conexão com o banco de dados MySQL
            public static string CaminhoConexao = "Server=localhost;Database=Arvoredo;Uid=root;Pwd=Gms30052007.;";

            // Método para obter uma conexão com o banco de dados
            public static MySqlConnection ObterConexao()
            {
                MySqlConnection conexao = new MySqlConnection(CaminhoConexao);
                conexao.Open();
                return conexao;
            }
        }

        public static class UsuarioLogado
        {
            public static int ID { get; set; }
            public static string Login { get; set; }
            public static string Nome { get; set; }
            public static int NivelAcesso { get; set; }

            public static bool TemPermissaoAdmin()
            {
                return NivelAcesso >= 3;
            }

            public static bool TemPermissaoGerencial()
            {
                return NivelAcesso >= 2;
            }
        }

        public FormLogin()
        {
            InitializeComponent();

            // Registra o evento de pintura
            this.Paint += new PaintEventHandler(FormLogin_Paint);

            // Registra o evento de redimensionamento
            this.Resize += new EventHandler(FormLogin_Resize);

            // Aplica as bordas arredondadas
            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            this.Text = "Login Arvoredo";
        }

        private void SetBackColorDegrade(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Rectangle gradient_rect = new Rectangle(0, 0, Width, Height);

            using (LinearGradientBrush br = new LinearGradientBrush(
                gradient_rect,
                Color.FromArgb(250, 230, 194),
                Color.FromArgb(180, 123, 57),
                90f))
            {
                ColorBlend colorBlend = new ColorBlend(3);
                colorBlend.Colors = new Color[]
                {
                   Color.FromArgb(250, 230, 194),
                   Color.FromArgb(198, 143, 86),
                   Color.FromArgb(180, 123, 57)
                };
                colorBlend.Positions = new float[] { 0f, 0.5f, 1f };
                br.InterpolationColors = colorBlend;
                graphics.FillRectangle(br, gradient_rect);
            }
        }

        private void FormLogin_Paint(object sender, PaintEventArgs e)
        {
            SetBackColorDegrade(sender, e);
        }

        private void FormLogin_Resize(object sender, EventArgs e)
        {

            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));


            this.Invalidate();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            btnEntrar.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnEntrar.Width, btnEntrar.Height, 100, 100));
            btnVoltar.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnVoltar.Width, btnVoltar.Height, 100, 100));
            Txt_Usuario.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Txt_Usuario.Width, Txt_Usuario.Height, 20, 20));
            Txt_Senha.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Txt_Senha.Width, Txt_Senha.Height, 20, 20));

            Txt_Senha.PasswordChar = '•';

            Txt_Senha.KeyPress += (s, args) =>
            {
                if (args.KeyChar == (char)Keys.Enter) 
                { 
                    btnEntrar_Click(sender, e);
                    args.Handled = true;
                }
            };
        }
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 formPrincipal = Application.OpenForms.OfType<Form1>().FirstOrDefault();

                formPrincipal = new Form1();
                formPrincipal.Show();
            
            this.Close();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            
        
            string usuario = Txt_Usuario.Text.Trim();
            string senha = Txt_Senha.Text.Trim();

            // Verifica se os campos estão preenchidos
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Verifica as credenciais
                if (VerificarCredenciais(usuario, senha, out int idUsuario, out string nomeUsuario, out int nivelAcesso))
                {
                    // Armazena informações do usuário logado
                    UsuarioLogado.ID = idUsuario;
                    UsuarioLogado.Login = usuario;
                    UsuarioLogado.Nome = nomeUsuario;
                    UsuarioLogado.NivelAcesso = nivelAcesso;

                    // Exibe mensagem de boas-vindas
                    MessageBox.Show($"Bem-vindo ao Sistema Arvoredo, {nomeUsuario}!",
                        "Login realizado com sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Oculta o formulário de login (não fecha completamente)
                    this.Hide();

                    // Cria e exibe a tela principal do sistema
                    TelaArvoredo telaArvoredo = new TelaArvoredo();
                    telaArvoredo.FormClosed += (s, args) => this.Close(); // Fecha o form de login quando a tela principal for fechada
                    telaArvoredo.Show();

                    
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Usuário ou senha incorretos.", "Erro de autenticação",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Txt_Senha.Clear();
                    Txt_Senha.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao realizar login: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Fecha o aplicativo
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Método que verifica as credenciais no banco de dados
        private bool VerificarCredenciais(string login, string senha, out int idUsuario, out string nomeUsuario, out int nivelAcesso)
        {
            // Inicializa os valores de saída
            idUsuario = 0;
            nomeUsuario = "";
            nivelAcesso = 0;

            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    // Query para verificar se existe o usuário com a senha informada
                    // e obter o ID, nome e nível de acesso do usuário
                    string query = @"SELECT ID, Nome, NivelAcesso FROM Usuarios 
                                   WHERE Login = @Login AND Senha = @Senha AND Ativo = 1";

                    using (MySqlCommand comando = new MySqlCommand(query, conexao))
                    {
                        // Adiciona os parâmetros (evita SQL Injection)
                        comando.Parameters.AddWithValue("@Login", login);
                        comando.Parameters.AddWithValue("@Senha", senha);

                        using (MySqlDataReader leitor = comando.ExecuteReader())
                        {
                            if (leitor.Read())
                            {
                                // Lê os dados do usuário
                                idUsuario = Convert.ToInt32(leitor["ID"]);
                                nomeUsuario = leitor["Nome"].ToString();
                                nivelAcesso = Convert.ToInt32(leitor["NivelAcesso"]);
                                return true;
                            }
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao verificar credenciais: " + ex.Message);
            }
        }

        private void pictureBoxMostrarSenha_Click(object sender, EventArgs e)
        {
            if (Txt_Senha.PasswordChar == '•')
            {
                // Mostra a senha
                Txt_Senha.PasswordChar = '\0';
                // Muda para o ícone de olho aberto
                ((PictureBox)sender).Image = Properties.Resources.senha_do_olho;
            }
            else
            {
                // Oculta a senha
                Txt_Senha.PasswordChar = '•';
                // Muda para o ícone de olho fechado
                ((PictureBox)sender).Image = Properties.Resources.senha_do_olho;
            }
        }
    }
}
