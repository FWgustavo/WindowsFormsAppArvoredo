using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormSelecionarAnoRelatorio : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
           int nLeft, int nTop, int nRight, int nBottom,
           int nWidthEllipse, int nHeightEllipse);

        public int AnoSelecionado { get; private set; }
        private int[] todosAnos = { 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025 };
        private int indiceAnoInicial = 6;

        private Panel panelBotoesAnos;
        private Button btnSetaEsquerda;
        private Button btnSetaDireita;

        public FormSelecionarAnoRelatorio()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Configurações do Form
            this.ClientSize = new Size(700, 400);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(250, 230, 194);
            this.Paint += FormSelecionarAno_Paint;

            // Aplicar bordas arredondadas
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));

            // Label de instrução
            Label lblInstrucao = new Label();
            lblInstrucao.Text = "SELECIONE O ANO QUE DESEJA CRIAR O RELATÓRIO";
            lblInstrucao.Location = new Point(50, 30);
            lblInstrucao.Size = new Size(600, 40);
            lblInstrucao.Font = new Font("Gagalin", 12F, FontStyle.Bold);
            lblInstrucao.ForeColor = Color.FromArgb(57, 27, 1);
            lblInstrucao.TextAlign = ContentAlignment.MiddleCenter;
            lblInstrucao.BackColor = Color.FromArgb(239, 212, 172);
            lblInstrucao.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(lblInstrucao);

            // Container de anos
            Panel containerAnos = new Panel();
            containerAnos.Location = new Point(50, 100);
            containerAnos.Size = new Size(600, 200);
            containerAnos.BackColor = Color.Transparent;
            this.Controls.Add(containerAnos);

            // Seta Esquerda
            btnSetaEsquerda = new Button();
            btnSetaEsquerda.Text = "◄";
            btnSetaEsquerda.Location = new Point(20, 80);
            btnSetaEsquerda.Size = new Size(60, 40);
            btnSetaEsquerda.Font = new Font("Arial", 20F, FontStyle.Bold);
            btnSetaEsquerda.BackColor = Color.FromArgb(239, 212, 172);
            btnSetaEsquerda.ForeColor = Color.FromArgb(57, 27, 1);
            btnSetaEsquerda.FlatStyle = FlatStyle.Flat;
            btnSetaEsquerda.FlatAppearance.BorderSize = 0;
            btnSetaEsquerda.Cursor = Cursors.Hand;
            btnSetaEsquerda.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSetaEsquerda.Width, btnSetaEsquerda.Height, 20, 20));
            btnSetaEsquerda.Click += BtnSetaEsquerda_Click;
            containerAnos.Controls.Add(btnSetaEsquerda);

            // Panel para botões de anos
            panelBotoesAnos = new Panel();
            panelBotoesAnos.Location = new Point(100, 20);
            panelBotoesAnos.Size = new Size(400, 160);
            panelBotoesAnos.BackColor = Color.Transparent;
            containerAnos.Controls.Add(panelBotoesAnos);

            // Seta Direita
            btnSetaDireita = new Button();
            btnSetaDireita.Text = "►";
            btnSetaDireita.Location = new Point(520, 80);
            btnSetaDireita.Size = new Size(60, 40);
            btnSetaDireita.Font = new Font("Arial", 20F, FontStyle.Bold);
            btnSetaDireita.BackColor = Color.FromArgb(239, 212, 172);
            btnSetaDireita.ForeColor = Color.FromArgb(57, 27, 1);
            btnSetaDireita.FlatStyle = FlatStyle.Flat;
            btnSetaDireita.FlatAppearance.BorderSize = 0;
            btnSetaDireita.Cursor = Cursors.Hand;
            btnSetaDireita.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSetaDireita.Width, btnSetaDireita.Height, 20, 20));
            btnSetaDireita.Click += BtnSetaDireita_Click;
            containerAnos.Controls.Add(btnSetaDireita);

            // Carregar anos iniciais
            CarregarBotoesAnos();

            // Botão Fechar
            Button btnFechar = new Button();
            btnFechar.Text = "FECHAR";
            btnFechar.Location = new Point(275, 330);
            btnFechar.Size = new Size(150, 40);
            btnFechar.Font = new Font("Arial", 12F, FontStyle.Bold);
            btnFechar.BackColor = Color.FromArgb(239, 212, 172);
            btnFechar.ForeColor = Color.FromArgb(57, 27, 1);
            btnFechar.FlatStyle = FlatStyle.Flat;
            btnFechar.FlatAppearance.BorderSize = 2;
            btnFechar.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnFechar.Cursor = Cursors.Hand;
            btnFechar.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnFechar.Width, btnFechar.Height, 20, 20));
            btnFechar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            this.Controls.Add(btnFechar);

            this.ResumeLayout(false);
        }

        private void FormSelecionarAno_Paint(object sender, PaintEventArgs e)
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

        private void CarregarBotoesAnos()
        {
            panelBotoesAnos.Controls.Clear();

            int xPos = 0;
            int yPos = 0;
            int contador = 0;

            for (int i = 0; i < 4 && (indiceAnoInicial + i) < todosAnos.Length; i++)
            {
                int ano = todosAnos[indiceAnoInicial + i];

                Button btnAno = new Button();
                btnAno.Text = ano.ToString();
                btnAno.Location = new Point(xPos, yPos);
                btnAno.Size = new Size(180, 70);
                btnAno.Font = new Font("Arial", 16F, FontStyle.Bold);
                btnAno.BackColor = Color.FromArgb(198, 143, 86);
                btnAno.ForeColor = Color.FromArgb(57, 27, 1);
                btnAno.FlatStyle = FlatStyle.Flat;
                btnAno.FlatAppearance.BorderSize = 2;
                btnAno.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
                btnAno.Cursor = Cursors.Hand;
                btnAno.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnAno.Width, btnAno.Height, 20, 20));
                btnAno.Tag = ano;
                btnAno.Click += BtnAno_Click;

                // Efeito hover
                btnAno.MouseEnter += (s, e) => {
                    Button btn = s as Button;
                    btn.BackColor = Color.FromArgb(144, 238, 144);
                };
                btnAno.MouseLeave += (s, e) => {
                    Button btn = s as Button;
                    btn.BackColor = Color.FromArgb(198, 143, 86);
                };

                panelBotoesAnos.Controls.Add(btnAno);

                contador++;
                xPos += 200;

                if (contador % 2 == 0)
                {
                    xPos = 0;
                    yPos += 80;
                }
            }
        }

        private void BtnSetaEsquerda_Click(object sender, EventArgs e)
        {
            if (indiceAnoInicial > 0)
            {
                indiceAnoInicial--;
                CarregarBotoesAnos();
            }
        }

        private void BtnSetaDireita_Click(object sender, EventArgs e)
        {
            if (indiceAnoInicial + 4 < todosAnos.Length)
            {
                indiceAnoInicial++;
                CarregarBotoesAnos();
            }
        }

        private void BtnAno_Click(object sender, EventArgs e)
        {
            Button btnClicado = sender as Button;
            if (btnClicado == null) return;

            AnoSelecionado = (int)btnClicado.Tag;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}