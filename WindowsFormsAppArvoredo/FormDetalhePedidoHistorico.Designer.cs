using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormDetalhePedidoHistorico : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
           int nLeft, int nTop, int nRight, int nBottom,
           int nWidthEllipse, int nHeightEllipse);

        private Orcamento pedido;

        public FormDetalhePedidoHistorico(Orcamento pedidoSelecionado)
        {
            InitializeComponent();
            pedido = pedidoSelecionado;
        }

        private void FormDetalhePedidoHistorico_Load(object sender, EventArgs e)
        {
            CriarInterface();
        }

        private void CriarInterface()
        {
            // Logo
            PictureBox picLogo = new PictureBox();
            picLogo.Location = new Point(20, 15);
            picLogo.Size = new Size(60, 60);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            try
            {
                picLogo.Image = Properties.Resources.logo1;
            }
            catch { }
            this.Controls.Add(picLogo);

            // Cabeçalho - Endereço
            Label lblEndereco = new Label();
            lblEndereco.Text = "ENDEREÇO: AV. Netinho Prado, 1025";
            lblEndereco.Location = new Point(100, 20);
            lblEndereco.Size = new Size(320, 20);
            lblEndereco.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.Controls.Add(lblEndereco);

            // Cabeçalho - Fone
            Label lblFone = new Label();
            lblFone.Text = "FONE: (14) 12345-6789";
            lblFone.Location = new Point(100, 45);
            lblFone.Size = new Size(220, 20);
            lblFone.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.Controls.Add(lblFone);

            // Data de Emissão
            Label lblDataEmissaoLabel = new Label();
            lblDataEmissaoLabel.Text = "DATA DE EMISSÃO:";
            lblDataEmissaoLabel.Location = new Point(430, 20);
            lblDataEmissaoLabel.Size = new Size(130, 20);
            lblDataEmissaoLabel.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.Controls.Add(lblDataEmissaoLabel);

            Label lblDataEmissao = new Label();
            lblDataEmissao.Text = pedido.DataEmissao.ToString("dd/MM/yyyy");
            lblDataEmissao.Location = new Point(430, 45);
            lblDataEmissao.Size = new Size(130, 20);
            lblDataEmissao.Font = new Font("Microsoft Sans Serif", 9F);
            this.Controls.Add(lblDataEmissao);

            // Páginas
            Label lblPaginas = new Label();
            lblPaginas.Text = "PÁGINAS: 1 de 1";
            lblPaginas.Location = new Point(430, 65);
            lblPaginas.Size = new Size(130, 20);
            lblPaginas.Font = new Font("Microsoft Sans Serif", 9F);
            this.Controls.Add(lblPaginas);

            // Seção DADOS DO CLIENTE
            Panel panelDadosCliente = new Panel();
            panelDadosCliente.Location = new Point(45, 100);
            panelDadosCliente.Size = new Size(510, 2);
            panelDadosCliente.BackColor = Color.Black;
            panelDadosCliente.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(panelDadosCliente);

            Label lblDadosCliente = new Label();
            lblDadosCliente.Text = "DADOS DO CLIENTE";
            lblDadosCliente.Location = new Point(220, 90);
            lblDadosCliente.Size = new Size(180, 25);
            lblDadosCliente.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            lblDadosCliente.TextAlign = ContentAlignment.MiddleCenter;
            lblDadosCliente.BorderStyle = BorderStyle.FixedSingle;
            lblDadosCliente.BackColor = Color.White;
            this.Controls.Add(lblDadosCliente);

            // Dados do Cliente - Esquerda
            int yCliente = 120;

            // Cliente
            Label lblClienteLabel = new Label();
            lblClienteLabel.Text = $"CLIENTE: {pedido.Cliente}";
            lblClienteLabel.Location = new Point(50, yCliente);
            lblClienteLabel.Size = new Size(500, 20);
            lblClienteLabel.Font = new Font("Microsoft Sans Serif", 9F);
            this.Controls.Add(lblClienteLabel);

            // Endereço
            Label lblEnderecoLabel = new Label();
            lblEnderecoLabel.Text = $"ENDEREÇO: {pedido.Endereco}, {pedido.Numero}";
            lblEnderecoLabel.Location = new Point(50, yCliente + 25);
            lblEnderecoLabel.Size = new Size(500, 20);
            lblEnderecoLabel.Font = new Font("Microsoft Sans Serif", 9F);
            this.Controls.Add(lblEnderecoLabel);

            // CEP/Município
            Label lblCepLabel = new Label();
            lblCepLabel.Text = $"CEP/MUNICÍPIO: {pedido.CEP}, {pedido.Cidade}, SP";
            lblCepLabel.Location = new Point(50, yCliente + 50);
            lblCepLabel.Size = new Size(500, 20);
            lblCepLabel.Font = new Font("Microsoft Sans Serif", 9F);
            this.Controls.Add(lblCepLabel);

            // Vendedor
            Label lblVendedorLabel = new Label();
            lblVendedorLabel.Text = $"VENDEDOR: {pedido.Vendedor}";
            lblVendedorLabel.Location = new Point(50, yCliente + 75);
            lblVendedorLabel.Size = new Size(250, 20);
            lblVendedorLabel.Font = new Font("Microsoft Sans Serif", 9F);
            this.Controls.Add(lblVendedorLabel);

            // Dados do Cliente - Direita
            // CNPJ/CPF
            Label lblCpfLabel = new Label();
            lblCpfLabel.Text = $"CNPJ/CPF: {pedido.CPF_CNPJ}";
            lblCpfLabel.Location = new Point(320, yCliente);
            lblCpfLabel.Size = new Size(250, 20);
            lblCpfLabel.Font = new Font("Microsoft Sans Serif", 9F);
            lblCpfLabel.TextAlign = ContentAlignment.TopRight;
            this.Controls.Add(lblCpfLabel);

            // Bairro
            Label lblBairroLabel = new Label();
            lblBairroLabel.Text = $"BAIRRO: ID. {pedido.Bairro}";
            lblBairroLabel.Location = new Point(320, yCliente + 25);
            lblBairroLabel.Size = new Size(250, 20);
            lblBairroLabel.Font = new Font("Microsoft Sans Serif", 9F);
            lblBairroLabel.TextAlign = ContentAlignment.TopRight;
            this.Controls.Add(lblBairroLabel);

            // TEL/CELL
            Label lblTelLabel = new Label();
            lblTelLabel.Text = $"TEL/CELL: {pedido.Telefone}";
            lblTelLabel.Location = new Point(320, yCliente + 50);
            lblTelLabel.Size = new Size(250, 20);
            lblTelLabel.Font = new Font("Microsoft Sans Serif", 9F);
            lblTelLabel.TextAlign = ContentAlignment.TopRight;
            this.Controls.Add(lblTelLabel);

            // Fantasia
            Label lblFantasiaLabel = new Label();
            lblFantasiaLabel.Text = "FANTASIA:";
            lblFantasiaLabel.Location = new Point(320, yCliente + 75);
            lblFantasiaLabel.Size = new Size(250, 20);
            lblFantasiaLabel.Font = new Font("Microsoft Sans Serif", 9F);
            lblFantasiaLabel.TextAlign = ContentAlignment.TopRight;
            this.Controls.Add(lblFantasiaLabel);

            // Seção PRODUTOS
            Panel panelProdutos = new Panel();
            panelProdutos.Location = new Point(45, 230);
            panelProdutos.Size = new Size(510, 2);
            panelProdutos.BackColor = Color.Black;
            panelProdutos.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(panelProdutos);

            Label lblProdutos = new Label();
            lblProdutos.Text = "PRODUTOS";
            lblProdutos.Location = new Point(250, 220);
            lblProdutos.Size = new Size(110, 25);
            lblProdutos.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            lblProdutos.TextAlign = ContentAlignment.MiddleCenter;
            lblProdutos.BorderStyle = BorderStyle.FixedSingle;
            lblProdutos.BackColor = Color.White;
            this.Controls.Add(lblProdutos);

            // DataGridView
            DataGridView dgvProdutos = new DataGridView();
            dgvProdutos.Location = new Point(45, 250);
            dgvProdutos.Size = new Size(510, 200);
            dgvProdutos.AllowUserToAddRows = false;
            dgvProdutos.AllowUserToDeleteRows = false;
            dgvProdutos.ReadOnly = true;
            dgvProdutos.BackgroundColor = Color.White;
            dgvProdutos.Font = new Font("Microsoft Sans Serif", 8F);

            dgvProdutos.Columns.Add("SEQ", "SEQ");
            dgvProdutos.Columns.Add("DESCRICAO", "DESCRIÇÃO");
            dgvProdutos.Columns.Add("UNI", "UNI.");
            dgvProdutos.Columns.Add("QTD", "QTD.");
            dgvProdutos.Columns.Add("VLR_UNI", "VLR. UNI.");
            dgvProdutos.Columns.Add("VLR_TOTAL", "VLR. TOTAL");

            dgvProdutos.Columns["SEQ"].Width = 40;
            dgvProdutos.Columns["DESCRICAO"].Width = 180;
            dgvProdutos.Columns["UNI"].Width = 50;
            dgvProdutos.Columns["QTD"].Width = 50;
            dgvProdutos.Columns["VLR_UNI"].Width = 80;
            dgvProdutos.Columns["VLR_TOTAL"].Width = 90;

            foreach (var item in pedido.Itens)
            {
                dgvProdutos.Rows.Add(
                    item.Sequencia,
                    item.Descricao,
                    item.Unidade,
                    item.Quantidade.ToString("F2"),
                    item.ValorUnitario.ToString("C"),
                    item.ValorTotal.ToString("C")
                );
            }

            this.Controls.Add(dgvProdutos);

            // Seção TOTAIS
            Panel panelTotais = new Panel();
            panelTotais.Location = new Point(45, 465);
            panelTotais.Size = new Size(510, 2);
            panelTotais.BackColor = Color.Black;
            panelTotais.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(panelTotais);

            Label lblTotais = new Label();
            lblTotais.Text = "TOTAIS";
            lblTotais.Location = new Point(270, 455);
            lblTotais.Size = new Size(70, 25);
            lblTotais.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            lblTotais.TextAlign = ContentAlignment.MiddleCenter;
            lblTotais.BorderStyle = BorderStyle.FixedSingle;
            lblTotais.BackColor = Color.White;
            this.Controls.Add(lblTotais);

            // 4X SEM JUROS
            Label lbl4xLabel = new Label();
            lbl4xLabel.Text = "4X SEM JUROS:";
            lbl4xLabel.Location = new Point(50, 490);
            lbl4xLabel.Size = new Size(120, 20);
            lbl4xLabel.Font = new Font("Microsoft Sans Serif", 9F);
            this.Controls.Add(lbl4xLabel);

            Label lbl4xValor = new Label();
            decimal valorParcela = pedido.TotalGeral / 4;
            lbl4xValor.Text = $"R$ {valorParcela:F2}";
            lbl4xValor.Location = new Point(310, 490);
            lbl4xValor.Size = new Size(245, 20);
            lbl4xValor.Font = new Font("Microsoft Sans Serif", 9F);
            lbl4xValor.TextAlign = ContentAlignment.TopRight;
            this.Controls.Add(lbl4xValor);

            // DESCONTOS
            Label lblDescontosLabel = new Label();
            lblDescontosLabel.Text = "DESCONTOS:";
            lblDescontosLabel.Location = new Point(50, 515);
            lblDescontosLabel.Size = new Size(120, 20);
            lblDescontosLabel.Font = new Font("Microsoft Sans Serif", 9F);
            this.Controls.Add(lblDescontosLabel);

            Label lblDescontos = new Label();
            lblDescontos.Text = $"R$ {pedido.Desconto:F2}";
            lblDescontos.Location = new Point(310, 515);
            lblDescontos.Size = new Size(245, 20);
            lblDescontos.Font = new Font("Microsoft Sans Serif", 9F);
            lblDescontos.TextAlign = ContentAlignment.TopRight;
            this.Controls.Add(lblDescontos);

            // ACRÉSCIMOS
            Label lblAcrescimosLabel = new Label();
            lblAcrescimosLabel.Text = "ACRÉSCIMOS:";
            lblAcrescimosLabel.Location = new Point(50, 540);
            lblAcrescimosLabel.Size = new Size(120, 20);
            lblAcrescimosLabel.Font = new Font("Microsoft Sans Serif", 9F);
            this.Controls.Add(lblAcrescimosLabel);

            Label lblAcrescimos = new Label();
            lblAcrescimos.Text = $"R$ {pedido.Acrescimo:F2}";
            lblAcrescimos.Location = new Point(310, 540);
            lblAcrescimos.Size = new Size(245, 20);
            lblAcrescimos.Font = new Font("Microsoft Sans Serif", 9F);
            lblAcrescimos.TextAlign = ContentAlignment.TopRight;
            this.Controls.Add(lblAcrescimos);

            // TOTAL À VISTA
            Label lblTotalVistaLabel = new Label();
            lblTotalVistaLabel.Text = "TOTAL À VISTA:";
            lblTotalVistaLabel.Location = new Point(50, 565);
            lblTotalVistaLabel.Size = new Size(120, 20);
            lblTotalVistaLabel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.Controls.Add(lblTotalVistaLabel);

            Label lblTotalVista = new Label();
            lblTotalVista.Text = $"R$ {pedido.TotalGeral:F2}";
            lblTotalVista.Location = new Point(310, 565);
            lblTotalVista.Size = new Size(245, 20);
            lblTotalVista.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            lblTotalVista.ForeColor = Color.Green;
            lblTotalVista.TextAlign = ContentAlignment.TopRight;
            this.Controls.Add(lblTotalVista);

            // FORMA DE PAGAMENTO
            Label lblFormaPgtoLabel = new Label();
            lblFormaPgtoLabel.Text = "FORMA DE PAGAMENTO:";
            lblFormaPgtoLabel.Location = new Point(50, 600);
            lblFormaPgtoLabel.Size = new Size(180, 20);
            lblFormaPgtoLabel.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.Controls.Add(lblFormaPgtoLabel);

            Button btnFormaPgto = new Button();
            btnFormaPgto.Text = pedido.FormaPagamento ?? "DINHEIRO";
            btnFormaPgto.Location = new Point(240, 595);
            btnFormaPgto.Size = new Size(140, 30);
            btnFormaPgto.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            btnFormaPgto.BackColor = Color.FromArgb(255, 215, 0);
            btnFormaPgto.FlatStyle = FlatStyle.Flat;
            btnFormaPgto.FlatAppearance.BorderSize = 2;
            btnFormaPgto.FlatAppearance.BorderColor = Color.Black;
            btnFormaPgto.Enabled = false;
            btnFormaPgto.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnFormaPgto.Width, btnFormaPgto.Height, 15, 15));
            this.Controls.Add(btnFormaPgto);

            // Botão VOLTAR
            Button btnVoltar = new Button();
            btnVoltar.Text = "VOLTAR";
            btnVoltar.Location = new Point(420, 595);
            btnVoltar.Size = new Size(135, 30);
            btnVoltar.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            btnVoltar.BackColor = Color.FromArgb(255, 140, 0);
            btnVoltar.ForeColor = Color.White;
            btnVoltar.FlatStyle = FlatStyle.Flat;
            btnVoltar.FlatAppearance.BorderSize = 2;
            btnVoltar.FlatAppearance.BorderColor = Color.Black;
            btnVoltar.Cursor = Cursors.Hand;
            btnVoltar.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnVoltar.Width, btnVoltar.Height, 15, 15));
            btnVoltar.Click += (s, e) => this.Close();
            this.Controls.Add(btnVoltar);
        }
    }
}