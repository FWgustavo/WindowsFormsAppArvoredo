using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormListaHistorico : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
           int nLeft, int nTop, int nRight, int nBottom,
           int nWidthEllipse, int nHeightEllipse);

        private List<Orcamento> pedidos;
        private List<Orcamento> pedidosFiltrados;
        private string mesSelecionado;
        private int anoSelecionado;

        public FormListaHistorico(List<Orcamento> listaPedidos, string mes, int ano)
        {
            InitializeComponent();
            pedidos = listaPedidos;
            pedidosFiltrados = listaPedidos;
            mesSelecionado = mes;
            anoSelecionado = ano;

            this.Paint += FormListaHistorico_Paint;
        }

        private void FormListaHistorico_Load(object sender, EventArgs e)
        {
            this.Text = $"Histórico - {mesSelecionado}/{anoSelecionado}";

            // Carregar logo
            try
            {
                pictureBoxLogo.Image = Properties.Resources.logo1;
            }
            catch { }

            // Aplicar bordas arredondadas
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));
            btnVoltar.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnVoltar.Width, btnVoltar.Height, 30, 30));
            txtPesquisa.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtPesquisa.Width, txtPesquisa.Height, 20, 20));
            btnPesquisar.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnPesquisar.Width, btnPesquisar.Height, 20, 20));

            // Configurar pesquisa
            txtPesquisa.Enter += (s, ev) =>
            {
                if (txtPesquisa.Text == "BARRA DE PESQUISA")
                {
                    txtPesquisa.Text = "";
                    txtPesquisa.ForeColor = Color.FromArgb(57, 27, 1);
                }
            };

            txtPesquisa.Leave += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtPesquisa.Text))
                {
                    txtPesquisa.Text = "BARRA DE PESQUISA";
                    txtPesquisa.ForeColor = Color.Gray;
                }
            };

            txtPesquisa.TextChanged += (s, ev) => FiltrarPedidos();
            btnPesquisar.Click += (s, ev) => FiltrarPedidos();

            CarregarPedidos();
        }

        private void FormListaHistorico_Paint(object sender, PaintEventArgs e)
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

        private void FiltrarPedidos()
        {
            if (txtPesquisa.Text == "BARRA DE PESQUISA" || string.IsNullOrWhiteSpace(txtPesquisa.Text))
            {
                pedidosFiltrados = pedidos;
            }
            else
            {
                string filtro = txtPesquisa.Text.ToLower();
                pedidosFiltrados = pedidos.Where(p =>
                    p.Cliente.ToLower().Contains(filtro) ||
                    p.Id.ToString().Contains(filtro) ||
                    p.DataEmissao.ToString("dd/MM/yyyy").Contains(filtro)
                ).ToList();
            }

            CarregarPedidos();
        }

        private void CarregarPedidos()
        {
            panelPedidos.Controls.Clear();

            int yPos = 10;
            int contador = 1;

            foreach (var pedido in pedidosFiltrados)
            {
                Panel cardPedido = CriarCardPedido(pedido, contador);
                cardPedido.Location = new Point(10, yPos);
                panelPedidos.Controls.Add(cardPedido);

                yPos += cardPedido.Height + 10;
                contador++;
            }

            if (pedidosFiltrados.Count == 0)
            {
                Label lblVazio = new Label();
                lblVazio.Text = "Nenhum pedido encontrado.";
                lblVazio.Location = new Point(150, 150);
                lblVazio.Size = new Size(300, 40);
                lblVazio.Font = new Font("Gagalin", 12F);
                lblVazio.ForeColor = Color.FromArgb(57, 27, 1);
                lblVazio.TextAlign = ContentAlignment.MiddleCenter;
                panelPedidos.Controls.Add(lblVazio);
            }
        }

        private Panel CriarCardPedido(Orcamento pedido, int numero)
        {
            Panel card = new Panel();
            card.Size = new Size(590, 50);
            card.BackColor = Color.FromArgb(198, 143, 86);
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, card.Width, card.Height, 25, 25));

            // N° do pedido
            Label lblNumero = new Label();
            lblNumero.Text = $"N° {numero}";
            lblNumero.Location = new Point(15, 15);
            lblNumero.Size = new Size(60, 20);
            lblNumero.Font = new Font("Gagalin", 10F, FontStyle.Bold);
            lblNumero.ForeColor = Color.FromArgb(57, 27, 1);
            lblNumero.BackColor = Color.Transparent;
            card.Controls.Add(lblNumero);

            // Cliente
            Label lblCliente = new Label();
            lblCliente.Text = $"CLIENTE: {pedido.Cliente.ToUpper()}";
            lblCliente.Location = new Point(90, 15);
            lblCliente.Size = new Size(250, 20);
            lblCliente.Font = new Font("Gagalin", 9F, FontStyle.Bold);
            lblCliente.ForeColor = Color.FromArgb(57, 27, 1);
            lblCliente.BackColor = Color.Transparent;
            card.Controls.Add(lblCliente);

            // Data
            Label lblData = new Label();
            lblData.Text = $"DATA: {pedido.DataEmissao:dd/MM/yyyy}";
            lblData.Location = new Point(350, 15);
            lblData.Size = new Size(130, 20);
            lblData.Font = new Font("Gagalin", 8F, FontStyle.Regular);
            lblData.ForeColor = Color.FromArgb(57, 27, 1);
            lblData.BackColor = Color.Transparent;
            card.Controls.Add(lblData);

            // Botão DETALHES
            Button btnDetalhes = new Button();
            btnDetalhes.Text = "DETALHES";
            btnDetalhes.Location = new Point(490, 10);
            btnDetalhes.Size = new Size(85, 30);
            btnDetalhes.Font = new Font("Gagalin", 7F, FontStyle.Bold);
            btnDetalhes.BackColor = Color.FromArgb(239, 212, 172);
            btnDetalhes.ForeColor = Color.FromArgb(57, 27, 1);
            btnDetalhes.FlatStyle = FlatStyle.Flat;
            btnDetalhes.FlatAppearance.BorderSize = 2;
            btnDetalhes.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnDetalhes.Cursor = Cursors.Hand;
            btnDetalhes.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnDetalhes.Width, btnDetalhes.Height, 15, 15));
            btnDetalhes.Click += (s, e) => AbrirDetalhesPedido(pedido);
            card.Controls.Add(btnDetalhes);

            return card;
        }

        private void AbrirDetalhesPedido(Orcamento pedido)
        {
            using (FormDetalhePedidoHistorico formDetalhe = new FormDetalhePedidoHistorico(pedido))
            {
                formDetalhe.ShowDialog();
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}