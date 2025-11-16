using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormSobre : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
           int nLeft, int nTop, int nRight, int nBottom,
           int nWidthEllipse, int nHeightEllipse);

        public FormSobre()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Paint += FormSobre_Paint;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private void FormSobre_Paint(object sender, PaintEventArgs e)
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

        private void BtnSobre_Click(object sender, EventArgs e)
        {
            string mensagem = "SISTEMA ARVOREDO\n\n" +
                            "Versão: 1.0\n" +
                            "Data de Lançamento: 25/12/2025\n\n" +
                            "Sistema de Gestão Comercial desenvolvido para a Madeireira Arvoredo.\n\n" +
                            "Funcionalidades:\n" +
                            "• Gerenciamento de Orçamentos\n" +
                            "• Controle de Pedidos\n" +
                            "• Gestão de Estoque\n" +
                            "• Cadastro de Clientes\n" +
                            "• Histórico Completo\n" +
                            "• Sistema de Backup\n\n" +
                            "Desenvolvido por: Marcio's Company\n" +
                            "Telefone: 10987-6543\n\n" +
                            "© 2025 - Todos os direitos reservados";

            MessageBox.Show(mensagem, "Sobre o Sistema Arvoredo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSobre_Load(object sender, EventArgs e)
        {

            this.SuspendLayout();

            // Configurações do Form
            this.ClientSize = new Size(700, 600);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Configurações - Sistema Arvoredo";
            this.BackColor = Color.FromArgb(250, 230, 194);

            // Aplicar bordas arredondadas ao form
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));

            // Logo
            PictureBox picLogo = new PictureBox();
            picLogo.Location = new Point(280, 20);
            picLogo.Size = new Size(140, 120);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            try
            {
                picLogo.Image = Properties.Resources.logo1;
            }
            catch { }
            this.Controls.Add(picLogo);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "CONFIGURAÇÕES";
            lblTitulo.Location = new Point(200, 150);
            lblTitulo.Size = new Size(300, 40);
            lblTitulo.Font = new Font("Gagalin", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(57, 27, 1);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.BackColor = Color.FromArgb(239, 212, 172);
            lblTitulo.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(lblTitulo);

            // Container Principal
            Panel containerConfig = new Panel();
            containerConfig.Location = new Point(50, 210);
            containerConfig.Size = new Size(600, 320);
            containerConfig.BackColor = Color.FromArgb(239, 212, 172);
            containerConfig.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(containerConfig);

            // Seção: Informações do Sistema
            Label lblInfoSistema = new Label();
            lblInfoSistema.Text = "INFORMAÇÕES DO SISTEMA";
            lblInfoSistema.Location = new Point(20, 20);
            lblInfoSistema.Size = new Size(560, 25);
            lblInfoSistema.Font = new Font("Gagalin", 11F, FontStyle.Bold);
            lblInfoSistema.ForeColor = Color.FromArgb(57, 27, 1);
            lblInfoSistema.TextAlign = ContentAlignment.MiddleCenter;
            lblInfoSistema.BackColor = Color.FromArgb(198, 143, 86);
            lblInfoSistema.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, lblInfoSistema.Width, lblInfoSistema.Height, 10, 10));
            containerConfig.Controls.Add(lblInfoSistema);

            // Versão
            Label lblVersao = new Label();
            lblVersao.Text = "Versão do Programa: 1.0";
            lblVersao.Location = new Point(30, 60);
            lblVersao.Size = new Size(540, 20);
            lblVersao.Font = new Font("Gagalin", 9F);
            lblVersao.ForeColor = Color.FromArgb(57, 27, 1);
            containerConfig.Controls.Add(lblVersao);

            // Última Sincronização
            Label lblUltimaSync = new Label();
            lblUltimaSync.Text = $"Última Sincronização: {DateTime.Now:dd/MM/yyyy HH:mm}";
            lblUltimaSync.Location = new Point(30, 85);
            lblUltimaSync.Size = new Size(540, 20);
            lblVersao.Font = new Font("Gagalin", 9F);
            lblUltimaSync.ForeColor = Color.FromArgb(57, 27, 1);
            containerConfig.Controls.Add(lblUltimaSync);

            // Atualizações Pendentes
            Label lblAtualizacoes = new Label();
            lblAtualizacoes.Text = "Atualizações Pendentes: Nenhuma";
            lblAtualizacoes.Location = new Point(30, 110);
            lblAtualizacoes.Size = new Size(540, 20);
            lblAtualizacoes.Font = new Font("Gagalin", 9F);
            lblAtualizacoes.ForeColor = Color.FromArgb(57, 27, 1);
            containerConfig.Controls.Add(lblAtualizacoes);

            // Seção: Contato
            Label lblContato = new Label();
            lblContato.Text = "CONTATO E SUPORTE";
            lblContato.Location = new Point(20, 150);
            lblContato.Size = new Size(560, 25);
            lblContato.Font = new Font("Gagalin", 11F, FontStyle.Bold);
            lblContato.ForeColor = Color.FromArgb(57, 27, 1);
            lblContato.TextAlign = ContentAlignment.MiddleCenter;
            lblContato.BackColor = Color.FromArgb(198, 143, 86);
            lblContato.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, lblContato.Width, lblContato.Height, 10, 10));
            containerConfig.Controls.Add(lblContato);

            // Telefone
            Label lblTelefone = new Label();
            lblTelefone.Text = "Telefone para Contato: 10987-6543";
            lblTelefone.Location = new Point(30, 190);
            lblTelefone.Size = new Size(540, 20);
            lblTelefone.Font = new Font("Gagalin", 9F);
            lblTelefone.ForeColor = Color.FromArgb(57, 27, 1);
            containerConfig.Controls.Add(lblTelefone);

            // Desenvolvedor
            Label lblDesenvolvedor = new Label();
            lblDesenvolvedor.Text = "Desenvolvido por: Marcio's Company";
            lblDesenvolvedor.Location = new Point(30, 215);
            lblDesenvolvedor.Size = new Size(540, 20);
            lblDesenvolvedor.Font = new Font("Gagalin", 9F);
            lblDesenvolvedor.ForeColor = Color.FromArgb(57, 27, 1);
            containerConfig.Controls.Add(lblDesenvolvedor);


            // Botão Sobre
            Button btnSobre = new Button();
            btnSobre.Text = "SOBRE O SISTEMA";
            btnSobre.Location = new Point((containerConfig.Width - btnSobre.Width) / 3, 260);
            btnSobre.Size = new Size(250, 40);
            btnSobre.Font = new Font("Gagalin", 10F, FontStyle.Bold);
            btnSobre.BackColor = Color.FromArgb(255, 218, 185);
            btnSobre.ForeColor = Color.Black;
            btnSobre.FlatStyle = FlatStyle.Flat;
            btnSobre.FlatAppearance.BorderSize = 2;
            btnSobre.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnSobre.Cursor = Cursors.Hand;
            btnSobre.Click += BtnSobre_Click;
            containerConfig.Controls.Add(btnSobre);

            // Botão Voltar
            Button btnVoltar = new Button();
            btnVoltar.Text = "VOLTAR";
            btnVoltar.Location = new Point(275, 545);
            btnVoltar.Size = new Size(150, 40);
            btnVoltar.Font = new Font("Gagalin", 12F, FontStyle.Bold);
            btnVoltar.BackColor = Color.FromArgb(239, 212, 172);
            btnVoltar.ForeColor = Color.FromArgb(57, 27, 1);
            btnVoltar.FlatStyle = FlatStyle.Flat;
            btnVoltar.FlatAppearance.BorderSize = 2;
            btnVoltar.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnVoltar.Cursor = Cursors.Hand;
            btnVoltar.Click += BtnVoltar_Click;
            this.Controls.Add(btnVoltar);

            this.ResumeLayout(false);
        }
    }
}
