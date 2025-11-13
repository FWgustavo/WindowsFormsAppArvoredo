using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class TelaTitulos : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
           int nLeft, int nTop, int nRight, int nBottom,
           int nWidthEllipse, int nHeightEllipse);

        private Orcamento pedidoSelecionado;

        public TelaTitulos(Orcamento pedido)
        {
            InitializeComponent();
            pedidoSelecionado = pedido;
            this.Paint += TelaTitulos_Paint;
        }


        private void TelaTitulos_Load(object sender, EventArgs e)
        {
            CriarInterface();
        }

        private void TelaTitulos_Paint(object sender, PaintEventArgs e)
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

        private void CriarInterface()
        {
            // Logo
            PictureBox picLogo = new PictureBox();
            picLogo.Location = new Point(12, 12);
            picLogo.Size = new Size(165, 95);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            try
            {
                picLogo.Image = Properties.Resources.telalogo;
            }
            catch { }
            this.Controls.Add(picLogo);

            // Cabeçalho
            Label lblEndereco = new Label();
            lblEndereco.Text = "ENDEREÇO: AV. Netinho Prado, 1025";
            lblEndereco.Location = new Point(213, 31);
            lblEndereco.Size = new Size(400, 25);
            lblEndereco.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold);
            this.Controls.Add(lblEndereco);

            Label lblFone = new Label();
            lblFone.Text = "FONE: (14) 12345-6789";
            lblFone.Location = new Point(213, 67);
            lblFone.Size = new Size(300, 25);
            lblFone.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold);
            this.Controls.Add(lblFone);

            Label lblDataEmissao = new Label();
            lblDataEmissao.Text = "DATA DE EMISSÃO:";
            lblDataEmissao.Location = new Point(708, 31);
            lblDataEmissao.Size = new Size(220, 25);
            lblDataEmissao.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold);
            this.Controls.Add(lblDataEmissao);

            Label lblData = new Label();
            lblData.Text = pedidoSelecionado.DataEmissao.ToString("dd/MM/yyyy");
            lblData.Location = new Point(933, 31);
            lblData.Size = new Size(120, 25);
            lblData.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold);
            this.Controls.Add(lblData);

            Label lblPaginas = new Label();
            lblPaginas.Text = "PÁGINAS: 1 de 1";
            lblPaginas.Location = new Point(708, 67);
            lblPaginas.Size = new Size(300, 25);
            lblPaginas.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold);
            this.Controls.Add(lblPaginas);

            // Dados do Cliente
            Label lblDadosCliente = new Label();
            lblDadosCliente.Text = "DADOS DO CLIENTE";
            lblDadosCliente.Location = new Point(447, 127);
            lblDadosCliente.Size = new Size(226, 27);
            lblDadosCliente.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold);
            lblDadosCliente.BorderStyle = BorderStyle.FixedSingle;
            lblDadosCliente.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblDadosCliente);

            // Campo Cliente
            Label lblClienteLabel = new Label();
            lblClienteLabel.Text = "CLIENTE:";
            lblClienteLabel.Location = new Point(102, 176);
            lblClienteLabel.Size = new Size(80, 20);
            lblClienteLabel.Font = new Font("Microsoft Sans Serif", 12F);
            this.Controls.Add(lblClienteLabel);

            Label lblCliente = new Label();
            lblCliente.Text = pedidoSelecionado.Cliente;
            lblCliente.Location = new Point(190, 176);
            lblCliente.Size = new Size(500, 20);
            lblCliente.Font = new Font("Microsoft Sans Serif", 12F);
            this.Controls.Add(lblCliente);

            // Endereço
            Label lblEnderecoLabel = new Label();
            lblEnderecoLabel.Text = "ENDEREÇO:";
            lblEnderecoLabel.Location = new Point(82, 211);
            lblEnderecoLabel.Size = new Size(100, 20);
            lblEnderecoLabel.Font = new Font("Microsoft Sans Serif", 12F);
            this.Controls.Add(lblEnderecoLabel);

            Label lblEnderecoValor = new Label();
            lblEnderecoValor.Text = $"{pedidoSelecionado.Endereco}, {pedidoSelecionado.Numero}";
            lblEnderecoValor.Location = new Point(190, 211);
            lblEnderecoValor.Size = new Size(500, 20);
            lblEnderecoValor.Font = new Font("Microsoft Sans Serif", 12F);
            this.Controls.Add(lblEnderecoValor);

            // CEP/Município
            Label lblCepLabel = new Label();
            lblCepLabel.Text = "CEP/MUNICÍPIO:";
            lblCepLabel.Location = new Point(49, 243);
            lblCepLabel.Size = new Size(133, 20);
            lblCepLabel.Font = new Font("Microsoft Sans Serif", 12F);
            this.Controls.Add(lblCepLabel);

            Label lblCep = new Label();
            lblCep.Text = $"{pedidoSelecionado.CEP} - {pedidoSelecionado.Cidade}/{pedidoSelecionado.UF}";
            lblCep.Location = new Point(190, 243);
            lblCep.Size = new Size(500, 20);
            lblCep.Font = new Font("Microsoft Sans Serif", 12F);
            this.Controls.Add(lblCep);

            // Vendedor
            Label lblVendedorLabel = new Label();
            lblVendedorLabel.Text = "VENDEDOR:";
            lblVendedorLabel.Location = new Point(81, 275);
            lblVendedorLabel.Size = new Size(100, 20);
            lblVendedorLabel.Font = new Font("Microsoft Sans Serif", 12F);
            this.Controls.Add(lblVendedorLabel);

            Label lblVendedor = new Label();
            lblVendedor.Text = pedidoSelecionado.Vendedor;
            lblVendedor.Location = new Point(190, 275);
            lblVendedor.Size = new Size(300, 20);
            lblVendedor.Font = new Font("Microsoft Sans Serif", 12F);
            this.Controls.Add(lblVendedor);

            // CPF/CNPJ
            Label lblCpfLabel = new Label();
            lblCpfLabel.Text = "CNPJ/CPF:";
            lblCpfLabel.Location = new Point(702, 176);
            lblCpfLabel.Size = new Size(100, 20);
            lblCpfLabel.Font = new Font("Microsoft Sans Serif", 12F);
            this.Controls.Add(lblCpfLabel);

            Label lblCpf = new Label();
            lblCpf.Text = pedidoSelecionado.CPF_CNPJ;
            lblCpf.Location = new Point(810, 176);
            lblCpf.Size = new Size(250, 20);
            lblCpf.Font = new Font("Microsoft Sans Serif", 12F);
            this.Controls.Add(lblCpf);

            // Bairro
            Label lblBairroLabel = new Label();
            lblBairroLabel.Text = "BAIRRO:";
            lblBairroLabel.Location = new Point(735, 211);
            lblBairroLabel.Size = new Size(70, 20);
            lblBairroLabel.Font = new Font("Microsoft Sans Serif", 12F);
            this.Controls.Add(lblBairroLabel);

            Label lblBairro = new Label();
            lblBairro.Text = pedidoSelecionado.Bairro;
            lblBairro.Location = new Point(810, 211);
            lblBairro.Size = new Size(250, 20);
            lblBairro.Font = new Font("Microsoft Sans Serif", 12F);
            this.Controls.Add(lblBairro);

            // Telefone
            Label lblTelLabel = new Label();
            lblTelLabel.Text = "TEL/CEL:";
            lblTelLabel.Location = new Point(725, 243);
            lblTelLabel.Size = new Size(80, 20);
            lblTelLabel.Font = new Font("Microsoft Sans Serif", 12F);
            this.Controls.Add(lblTelLabel);

            Label lblTel = new Label();
            lblTel.Text = pedidoSelecionado.Telefone;
            lblTel.Location = new Point(810, 243);
            lblTel.Size = new Size(250, 20);
            lblTel.Font = new Font("Microsoft Sans Serif", 12F);
            this.Controls.Add(lblTel);

            // Fantasia (Número já foi usado, então vamos deixar como está ou criar outro campo se necessário)
            Label lblFantasiaLabel = new Label();
            lblFantasiaLabel.Text = "FANTASIA:";
            lblFantasiaLabel.Location = new Point(715, 275);
            lblFantasiaLabel.Size = new Size(90, 20);
            lblFantasiaLabel.Font = new Font("Microsoft Sans Serif", 12F);
            this.Controls.Add(lblFantasiaLabel);

            // Produtos
            Label lblProdutos = new Label();
            lblProdutos.Text = "PRODUTOS";
            lblProdutos.Location = new Point(491, 315);
            lblProdutos.Size = new Size(135, 27);
            lblProdutos.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold);
            lblProdutos.BorderStyle = BorderStyle.FixedSingle;
            lblProdutos.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblProdutos);

            // DataGridView para produtos
            DataGridView dgvProdutos = new DataGridView();
            dgvProdutos.Location = new Point(50, 360);
            dgvProdutos.Size = new Size(980, 200);
            dgvProdutos.AllowUserToAddRows = false;
            dgvProdutos.AllowUserToDeleteRows = false;
            dgvProdutos.ReadOnly = true;
            dgvProdutos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProdutos.MultiSelect = false;
            dgvProdutos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProdutos.BackgroundColor = Color.White;

            dgvProdutos.Columns.Add("SEQ", "SEQ");
            dgvProdutos.Columns.Add("DESCRICAO", "DESCRIÇÃO");
            dgvProdutos.Columns.Add("UNI", "UNI.");
            dgvProdutos.Columns.Add("QTD", "QTD.");
            dgvProdutos.Columns.Add("VLR_UNI", "VLR. UNI.");
            dgvProdutos.Columns.Add("VLR_TOTAL", "VLR. TOTAL");

            dgvProdutos.Columns["SEQ"].Width = 60;
            dgvProdutos.Columns["UNI"].Width = 80;
            dgvProdutos.Columns["QTD"].Width = 80;
            dgvProdutos.Columns["VLR_UNI"].Width = 100;
            dgvProdutos.Columns["VLR_TOTAL"].Width = 120;

            foreach (var item in pedidoSelecionado.Itens)
            {
                dgvProdutos.Rows.Add(
                    item.Sequencia,
                    item.Descricao,
                    item.Unidade,
                    item.Quantidade.ToString("F2"),
                    item.ValorUnitario.ToString("C2"),
                    item.ValorTotal.ToString("C2")
                );
            }

            this.Controls.Add(dgvProdutos);

            // Totais
            Label lblTotais = new Label();
            lblTotais.Text = "TOTAIS";
            lblTotais.Location = new Point(500, 580);
            lblTotais.Size = new Size(100, 25);
            lblTotais.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold);
            lblTotais.BorderStyle = BorderStyle.FixedSingle;
            lblTotais.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTotais);

            // 4x Sem Juros
            Label lbl4xLabel = new Label();
            lbl4xLabel.Text = "4X SEM JUROS:";
            lbl4xLabel.Location = new Point(100, 620);
            lbl4xLabel.Size = new Size(120, 20);
            lbl4xLabel.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lbl4xLabel);

            Label lbl4xValor = new Label();
            decimal valorParcela = pedidoSelecionado.TotalGeral / 4;
            lbl4xValor.Text = $"4x de {valorParcela:C2}";
            lbl4xValor.Location = new Point(230, 620);
            lbl4xValor.Size = new Size(150, 20);
            lbl4xValor.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lbl4xValor);

            // Descontos
            Label lblDescontosLabel = new Label();
            lblDescontosLabel.Text = "DESCONTOS:";
            lblDescontosLabel.Location = new Point(400, 620);
            lblDescontosLabel.Size = new Size(100, 20);
            lblDescontosLabel.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lblDescontosLabel);

            Label lblDescontos = new Label();
            lblDescontos.Text = pedidoSelecionado.Desconto.ToString("C2");
            lblDescontos.Location = new Point(510, 620);
            lblDescontos.Size = new Size(150, 20);
            lblDescontos.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lblDescontos);

            // Acréscimos
            Label lblAcrescimosLabel = new Label();
            lblAcrescimosLabel.Text = "ACRÉSCIMOS:";
            lblAcrescimosLabel.Location = new Point(680, 620);
            lblAcrescimosLabel.Size = new Size(100, 20);
            lblAcrescimosLabel.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lblAcrescimosLabel);

            Label lblAcrescimos = new Label();
            lblAcrescimos.Text = pedidoSelecionado.Acrescimo.ToString("C2");
            lblAcrescimos.Location = new Point(790, 620);
            lblAcrescimos.Size = new Size(150, 20);
            lblAcrescimos.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lblAcrescimos);

            // Forma de Pagamento
            Label lblFormaPgtoLabel = new Label();
            lblFormaPgtoLabel.Text = "FORMA DE PAGAMENTO:";
            lblFormaPgtoLabel.Location = new Point(100, 650);
            lblFormaPgtoLabel.Size = new Size(200, 20);
            lblFormaPgtoLabel.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lblFormaPgtoLabel);

            Label lblFormaPgto = new Label();
            lblFormaPgto.Text = pedidoSelecionado.FormaPagamento ?? "Dinheiro";
            lblFormaPgto.Location = new Point(310, 650);
            lblFormaPgto.Size = new Size(150, 20);
            lblFormaPgto.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lblFormaPgto);

            // Total à Vista
            Label lblTotalVistaLabel = new Label();
            lblTotalVistaLabel.Text = "TOTAL À VISTA:";
            lblTotalVistaLabel.Location = new Point(400, 680);
            lblTotalVistaLabel.Size = new Size(120, 20);
            lblTotalVistaLabel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.Controls.Add(lblTotalVistaLabel);

            Label lblTotalVista = new Label();
            lblTotalVista.Text = pedidoSelecionado.TotalGeral.ToString("C2");
            lblTotalVista.Location = new Point(530, 680);
            lblTotalVista.Size = new Size(150, 20);
            lblTotalVista.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            lblTotalVista.ForeColor = Color.Green;
            this.Controls.Add(lblTotalVista);

            // Botão Finalizar Pedido
            Button btnFinalizar = new Button();
            btnFinalizar.Text = "FINALIZAR PEDIDO";
            btnFinalizar.Location = new Point(920, 670);
            btnFinalizar.Size = new Size(140, 40);
            btnFinalizar.BackColor = Color.FromArgb(144, 238, 144);
            btnFinalizar.FlatStyle = FlatStyle.Flat;
            btnFinalizar.FlatAppearance.BorderSize = 0;
            btnFinalizar.Font = new Font("Gagalin", 9F, FontStyle.Bold);
            btnFinalizar.ForeColor = Color.Black;
            btnFinalizar.Cursor = Cursors.Hand;
            btnFinalizar.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0,
                btnFinalizar.Width, btnFinalizar.Height, 15, 15));
            btnFinalizar.Click += BtnFinalizar_Click;
            this.Controls.Add(btnFinalizar);
        }

        private void BtnFinalizar_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                $"Confirmar finalização do pedido?\n\n" +
                $"Cliente: {pedidoSelecionado.Cliente}\n" +
                $"Total: {pedidoSelecionado.TotalGeral:C}\n\n" +
                $"O pedido será marcado como finalizado.",
                "Finalizar Pedido",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                MessageBox.Show(
                    "Pedido finalizado com sucesso!",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}