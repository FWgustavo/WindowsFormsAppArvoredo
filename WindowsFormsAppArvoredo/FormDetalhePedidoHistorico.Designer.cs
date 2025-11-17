using System;
using System.Drawing;
using System.Drawing.Printing;
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
        private PrintDocument printDocument;
        private Bitmap memoryImage;

        public FormDetalhePedidoHistorico(Orcamento pedidoSelecionado)
        {
            InitializeComponent();
            pedido = pedidoSelecionado;

            // Configurar impressão
            printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
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
            lblEndereco.Size = new Size(300, 16);
            lblEndereco.Font = new Font("Arial", 9F, FontStyle.Bold);
            this.Controls.Add(lblEndereco);

            // Cabeçalho - Fone
            Label lblFone = new Label();
            lblFone.Text = "FONE: (14) 12345-6789";
            lblFone.Location = new Point(100, 40);
            lblFone.Size = new Size(220, 16);
            lblFone.Font = new Font("Arial", 9F, FontStyle.Bold);
            this.Controls.Add(lblFone);

            // Data de Emissão
            Label lblDataEmissaoLabel = new Label();
            lblDataEmissaoLabel.Text = "DATA DE EMISSÃO: " + pedido.DataEmissao.ToString("dd/MM/yyyy");
            lblDataEmissaoLabel.Location = new Point(410, 20);
            lblDataEmissaoLabel.Size = new Size(170, 16);
            lblDataEmissaoLabel.Font = new Font("Arial", 9F, FontStyle.Bold);
            this.Controls.Add(lblDataEmissaoLabel);

            // Páginas
            Label lblPaginas = new Label();
            lblPaginas.Text = "PÁGINAS: 1 de 1";
            lblPaginas.Location = new Point(410, 40);
            lblPaginas.Size = new Size(170, 16);
            lblPaginas.Font = new Font("Arial", 9F, FontStyle.Bold);
            this.Controls.Add(lblPaginas);

            // Seção DADOS DO CLIENTE
            Label lblDadosCliente = new Label();
            lblDadosCliente.Text = "DADOS DO CLIENTE";
            lblDadosCliente.Location = new Point(220, 85);
            lblDadosCliente.Size = new Size(160, 20);
            lblDadosCliente.Font = new Font("Arial", 9F, FontStyle.Bold);
            lblDadosCliente.TextAlign = ContentAlignment.MiddleCenter;
            lblDadosCliente.BackColor = Color.White;
            this.Controls.Add(lblDadosCliente);

            // Box dos dados do cliente
            Panel boxDadosCliente = new Panel();
            boxDadosCliente.Location = new Point(30, 95);
            boxDadosCliente.Size = new Size(540, 110);
            boxDadosCliente.BorderStyle = BorderStyle.FixedSingle;
            boxDadosCliente.BackColor = Color.White;
            this.Controls.Add(boxDadosCliente);

            // Dados do Cliente - Esquerda
            int yCliente = 105;

            // Cliente
            Label lblClienteLabel = new Label();
            lblClienteLabel.Text = $"CLIENTE: {pedido.Cliente}";
            lblClienteLabel.Location = new Point(40, yCliente);
            lblClienteLabel.Size = new Size(260, 16);
            lblClienteLabel.Font = new Font("Arial", 8.5F);
            this.Controls.Add(lblClienteLabel);

            // Endereço
            Label lblEnderecoLabel = new Label();
            lblEnderecoLabel.Text = $"ENDEREÇO: {pedido.Endereco}, {pedido.Numero}";
            lblEnderecoLabel.Location = new Point(40, yCliente + 22);
            lblEnderecoLabel.Size = new Size(260, 16);
            lblEnderecoLabel.Font = new Font("Arial", 8.5F);
            this.Controls.Add(lblEnderecoLabel);

            // CEP/Município
            Label lblCepLabel = new Label();
            lblCepLabel.Text = $"CEP/MUNICÍPIO: {pedido.CEP}, {pedido.Cidade}, SP";
            lblCepLabel.Location = new Point(40, yCliente + 44);
            lblCepLabel.Size = new Size(260, 16);
            lblCepLabel.Font = new Font("Arial", 8.5F);
            this.Controls.Add(lblCepLabel);

            // Vendedor
            Label lblVendedorLabel = new Label();
            lblVendedorLabel.Text = $"VENDEDOR: {pedido.Vendedor}";
            lblVendedorLabel.Location = new Point(40, yCliente + 66);
            lblVendedorLabel.Size = new Size(260, 16);
            lblVendedorLabel.Font = new Font("Arial", 8.5F);
            this.Controls.Add(lblVendedorLabel);

            // Dados do Cliente - Direita
            // CNPJ/CPF
            Label lblCpfLabel = new Label();
            lblCpfLabel.Text = $"CNPJ/CPF: {pedido.CPF_CNPJ}";
            lblCpfLabel.Location = new Point(310, yCliente);
            lblCpfLabel.Size = new Size(250, 16);
            lblCpfLabel.Font = new Font("Arial", 8.5F);
            lblCpfLabel.TextAlign = ContentAlignment.TopLeft;
            this.Controls.Add(lblCpfLabel);

            // Bairro
            Label lblBairroLabel = new Label();
            lblBairroLabel.Text = $"BAIRRO: ID. {pedido.Bairro}";
            lblBairroLabel.Location = new Point(310, yCliente + 22);
            lblBairroLabel.Size = new Size(250, 16);
            lblBairroLabel.Font = new Font("Arial", 8.5F);
            lblBairroLabel.TextAlign = ContentAlignment.TopLeft;
            this.Controls.Add(lblBairroLabel);

            // TEL/CELL
            Label lblTelLabel = new Label();
            lblTelLabel.Text = $"TEL/CELL: {pedido.Telefone}";
            lblTelLabel.Location = new Point(310, yCliente + 44);
            lblTelLabel.Size = new Size(250, 16);
            lblTelLabel.Font = new Font("Arial", 8.5F);
            lblTelLabel.TextAlign = ContentAlignment.TopLeft;
            this.Controls.Add(lblTelLabel);

            // Fantasia
            Label lblFantasiaLabel = new Label();
            lblFantasiaLabel.Text = "FANTASIA:";
            lblFantasiaLabel.Location = new Point(310, yCliente + 66);
            lblFantasiaLabel.Size = new Size(250, 16);
            lblFantasiaLabel.Font = new Font("Arial", 8.5F);
            lblFantasiaLabel.TextAlign = ContentAlignment.TopLeft;
            this.Controls.Add(lblFantasiaLabel);

            // Seção PRODUTOS
            Label lblProdutos = new Label();
            lblProdutos.Text = "PRODUTOS";
            lblProdutos.Location = new Point(250, 215);
            lblProdutos.Size = new Size(100, 20);
            lblProdutos.Font = new Font("Arial", 9F, FontStyle.Bold);
            lblProdutos.TextAlign = ContentAlignment.MiddleCenter;
            lblProdutos.BackColor = Color.White;
            this.Controls.Add(lblProdutos);

            // DataGridView
            DataGridView dgvProdutos = new DataGridView();
            dgvProdutos.Location = new Point(30, 220);
            dgvProdutos.Size = new Size(540, 180);
            dgvProdutos.AllowUserToAddRows = false;
            dgvProdutos.AllowUserToDeleteRows = false;
            dgvProdutos.ReadOnly = true;
            dgvProdutos.BackgroundColor = Color.White;
            dgvProdutos.Font = new Font("Arial", 8.5F);
            dgvProdutos.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8.5F, FontStyle.Bold);
            dgvProdutos.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgvProdutos.RowHeadersVisible = false;
            dgvProdutos.BorderStyle = BorderStyle.FixedSingle;
            dgvProdutos.GridColor = Color.Black;
            dgvProdutos.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvProdutos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvProdutos.EnableHeadersVisualStyles = false;
            dgvProdutos.AllowUserToResizeColumns = false;
            dgvProdutos.AllowUserToResizeRows = false;

            dgvProdutos.Columns.Add("SEQ", "SEQ");
            dgvProdutos.Columns.Add("DESCRICAO", "DESCRIÇÃO");
            dgvProdutos.Columns.Add("UNI", "UNI.");
            dgvProdutos.Columns.Add("QTD", "QTD.");
            dgvProdutos.Columns.Add("VLR_UNI", "VLR. UNI.");

            // Coluna de ícone impressora 1
            DataGridViewImageColumn colPrint1 = new DataGridViewImageColumn();
            colPrint1.Name = "PRINT1";
            colPrint1.HeaderText = "";
            colPrint1.Width = 20;
            dgvProdutos.Columns.Add(colPrint1);

            dgvProdutos.Columns.Add("VLR_TOTAL", "VLR. TOTAL");

            // Coluna de ícone impressora 2
            DataGridViewImageColumn colPrint2 = new DataGridViewImageColumn();
            colPrint2.Name = "PRINT2";
            colPrint2.HeaderText = "";
            colPrint2.Width = 20;
            dgvProdutos.Columns.Add(colPrint2);

            dgvProdutos.Columns["SEQ"].Width = 40;
            dgvProdutos.Columns["DESCRICAO"].Width = 200;
            dgvProdutos.Columns["UNI"].Width = 45;
            dgvProdutos.Columns["QTD"].Width = 50;
            dgvProdutos.Columns["VLR_UNI"].Width = 70;
            dgvProdutos.Columns["VLR_TOTAL"].Width = 75;

            dgvProdutos.Columns["SEQ"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvProdutos.Columns["UNI"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvProdutos.Columns["QTD"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProdutos.Columns["VLR_UNI"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProdutos.Columns["VLR_TOTAL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Criar ícone de impressora simples
            Bitmap printerIcon = CreatePrinterIcon();

            foreach (var item in pedido.Itens)
            {
                int rowIndex = dgvProdutos.Rows.Add(
                    item.Sequencia,
                    item.Descricao,
                    item.Unidade,
                    item.Quantidade.ToString("F0"),
                    item.ValorUnitario.ToString("C2"),
                    printerIcon,
                    item.ValorTotal.ToString("C2"),
                    printerIcon
                );
            }

            this.Controls.Add(dgvProdutos);

            // Seção TOTAIS
            Label lblTotais = new Label();
            lblTotais.Text = "TOTAIS";
            lblTotais.Location = new Point(270, 410);
            lblTotais.Size = new Size(60, 20);
            lblTotais.Font = new Font("Arial", 9F, FontStyle.Bold);
            lblTotais.TextAlign = ContentAlignment.MiddleCenter;
            lblTotais.BackColor = Color.White;
            this.Controls.Add(lblTotais);

            // 4X SEM JUROS
            Label lbl4xLabel = new Label();
            lbl4xLabel.Text = "4X SEM JUROS:";
            lbl4xLabel.Location = new Point(40, 425);
            lbl4xLabel.Size = new Size(120, 16);
            lbl4xLabel.Font = new Font("Arial", 8.5F);
            this.Controls.Add(lbl4xLabel);

            Label lbl4xValor = new Label();
            decimal valorParcela = pedido.TotalGeral / 4;
            lbl4xValor.Text = $"R$ {valorParcela:F2}";
            lbl4xValor.Location = new Point(420, 425);
            lbl4xValor.Size = new Size(145, 16);
            lbl4xValor.Font = new Font("Arial", 8.5F);
            lbl4xValor.TextAlign = ContentAlignment.TopRight;
            this.Controls.Add(lbl4xValor);

            // DESCONTOS
            Label lblDescontosLabel = new Label();
            lblDescontosLabel.Text = "DESCONTOS:";
            lblDescontosLabel.Location = new Point(40, 447);
            lblDescontosLabel.Size = new Size(120, 16);
            lblDescontosLabel.Font = new Font("Arial", 8.5F);
            this.Controls.Add(lblDescontosLabel);

            Label lblDescontos = new Label();
            lblDescontos.Text = $"R$ {pedido.Desconto:F2}";
            lblDescontos.Location = new Point(420, 447);
            lblDescontos.Size = new Size(145, 16);
            lblDescontos.Font = new Font("Arial", 8.5F);
            lblDescontos.TextAlign = ContentAlignment.TopRight;
            this.Controls.Add(lblDescontos);

            // ACRÉSCIMOS
            Label lblAcrescimosLabel = new Label();
            lblAcrescimosLabel.Text = "ACRÉSCIMOS:";
            lblAcrescimosLabel.Location = new Point(40, 469);
            lblAcrescimosLabel.Size = new Size(120, 16);
            lblAcrescimosLabel.Font = new Font("Arial", 8.5F);
            this.Controls.Add(lblAcrescimosLabel);

            Label lblAcrescimos = new Label();
            lblAcrescimos.Text = $"R$ {pedido.Acrescimo:F2}";
            lblAcrescimos.Location = new Point(420, 469);
            lblAcrescimos.Size = new Size(145, 16);
            lblAcrescimos.Font = new Font("Arial", 8.5F);
            lblAcrescimos.TextAlign = ContentAlignment.TopRight;
            this.Controls.Add(lblAcrescimos);

            // TOTAL À VISTA
            Label lblTotalVistaLabel = new Label();
            lblTotalVistaLabel.Text = "TOTAL À VISTA:";
            lblTotalVistaLabel.Location = new Point(40, 491);
            lblTotalVistaLabel.Size = new Size(120, 18);
            lblTotalVistaLabel.Font = new Font("Arial", 9F, FontStyle.Bold);
            this.Controls.Add(lblTotalVistaLabel);

            Label lblTotalVista = new Label();
            lblTotalVista.Text = $"R$ {pedido.TotalGeral:F2}";
            lblTotalVista.Location = new Point(420, 491);
            lblTotalVista.Size = new Size(145, 18);
            lblTotalVista.Font = new Font("Arial", 9F, FontStyle.Bold);
            lblTotalVista.ForeColor = Color.Green;
            lblTotalVista.TextAlign = ContentAlignment.TopRight;
            this.Controls.Add(lblTotalVista);

            // FORMA DE PAGAMENTO
            Label lblFormaPgtoLabel = new Label();
            lblFormaPgtoLabel.Text = "FORMA DE PAGAMENTO:";
            lblFormaPgtoLabel.Location = new Point(40, 530);
            lblFormaPgtoLabel.Size = new Size(180, 16);
            lblFormaPgtoLabel.Font = new Font("Arial", 8.5F, FontStyle.Bold);
            this.Controls.Add(lblFormaPgtoLabel);

            // Botão Débito (desabilitado)
            Button btnDebito = new Button();
            btnDebito.Text = "DÉBITO";
            btnDebito.Location = new Point(445, 525);
            btnDebito.Size = new Size(120, 28);
            btnDebito.Font = new Font("Arial", 9F, FontStyle.Bold);
            btnDebito.BackColor = Color.White;
            btnDebito.ForeColor = Color.Black;
            btnDebito.FlatStyle = FlatStyle.Flat;
            btnDebito.FlatAppearance.BorderSize = 2;
            btnDebito.FlatAppearance.BorderColor = Color.FromArgb(255, 140, 0);
            btnDebito.Enabled = false;
            btnDebito.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnDebito.Width, btnDebito.Height, 15, 15));
            this.Controls.Add(btnDebito);

            // Botão forma de pagamento selecionada
            Button btnFormaPgto = new Button();
            btnFormaPgto.Text = pedido.FormaPagamento ?? "DINHEIRO";
            btnFormaPgto.Location = new Point(230, 525);
            btnFormaPgto.Size = new Size(140, 28);
            btnFormaPgto.Font = new Font("Arial", 9F, FontStyle.Bold);
            btnFormaPgto.BackColor = Color.FromArgb(255, 140, 0);
            btnFormaPgto.ForeColor = Color.White;
            btnFormaPgto.FlatStyle = FlatStyle.Flat;
            btnFormaPgto.FlatAppearance.BorderSize = 2;
            btnFormaPgto.FlatAppearance.BorderColor = Color.FromArgb(200, 100, 0);
            btnFormaPgto.Enabled = false;
            btnFormaPgto.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnFormaPgto.Width, btnFormaPgto.Height, 15, 15));
            this.Controls.Add(btnFormaPgto);

            // Botão IMPRIMIR
            Button btnImprimir = new Button();
            btnImprimir.Text = "🖨 IMPRIMIR";
            btnImprimir.Location = new Point(40, 570);
            btnImprimir.Size = new Size(140, 35);
            btnImprimir.Font = new Font("Arial", 10F, FontStyle.Bold);
            btnImprimir.BackColor = Color.FromArgb(70, 130, 180);
            btnImprimir.ForeColor = Color.White;
            btnImprimir.FlatStyle = FlatStyle.Flat;
            btnImprimir.FlatAppearance.BorderSize = 1;
            btnImprimir.FlatAppearance.BorderColor = Color.FromArgb(50, 100, 150);
            btnImprimir.Cursor = Cursors.Hand;
            btnImprimir.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnImprimir.Width, btnImprimir.Height, 15, 15));
            btnImprimir.Click += BtnImprimir_Click;
            this.Controls.Add(btnImprimir);

            // Botão VISUALIZAR IMPRESSÃO
            Button btnVisualizarImpressao = new Button();
            btnVisualizarImpressao.Text = "👁 VISUALIZAR";
            btnVisualizarImpressao.Location = new Point(195, 570);
            btnVisualizarImpressao.Size = new Size(140, 35);
            btnVisualizarImpressao.Font = new Font("Arial", 10F, FontStyle.Bold);
            btnVisualizarImpressao.BackColor = Color.FromArgb(100, 149, 237);
            btnVisualizarImpressao.ForeColor = Color.White;
            btnVisualizarImpressao.FlatStyle = FlatStyle.Flat;
            btnVisualizarImpressao.FlatAppearance.BorderSize = 1;
            btnVisualizarImpressao.FlatAppearance.BorderColor = Color.FromArgb(80, 120, 200);
            btnVisualizarImpressao.Cursor = Cursors.Hand;
            btnVisualizarImpressao.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnVisualizarImpressao.Width, btnVisualizarImpressao.Height, 15, 15));
            btnVisualizarImpressao.Click += BtnVisualizarImpressao_Click;
            this.Controls.Add(btnVisualizarImpressao);

            // Botão VOLTAR
            Button btnVoltar = new Button();
            btnVoltar.Text = "VOLTAR";
            btnVoltar.Location = new Point(425, 570);
            btnVoltar.Size = new Size(140, 35);
            btnVoltar.Font = new Font("Arial", 10F, FontStyle.Bold);
            btnVoltar.BackColor = Color.FromArgb(200, 200, 200);
            btnVoltar.ForeColor = Color.Black;
            btnVoltar.FlatStyle = FlatStyle.Flat;
            btnVoltar.FlatAppearance.BorderSize = 1;
            btnVoltar.FlatAppearance.BorderColor = Color.Gray;
            btnVoltar.Cursor = Cursors.Hand;
            btnVoltar.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnVoltar.Width, btnVoltar.Height, 15, 15));
            btnVoltar.Click += (s, e) => this.Close();
            this.Controls.Add(btnVoltar);
        }

        // Criar ícone de impressora simples
        private Bitmap CreatePrinterIcon()
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);

                // Desenhar impressora simples
                using (Pen pen = new Pen(Color.FromArgb(255, 140, 0), 2))
                {
                    // Corpo da impressora
                    g.DrawRectangle(pen, 2, 5, 12, 7);
                    // Papel saindo
                    g.DrawRectangle(pen, 4, 2, 8, 4);
                    // Detalhes
                    g.DrawLine(pen, 4, 10, 12, 10);
                }
            }
            return bmp;
        }

        // Botão Imprimir
        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDocument;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao imprimir: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Botão Visualizar Impressão
        private void BtnVisualizarImpressao_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPreviewDialog previewDialog = new PrintPreviewDialog();
                previewDialog.Document = printDocument;
                previewDialog.Width = 800;
                previewDialog.Height = 600;
                previewDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao visualizar impressão: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento de impressão
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                Font fontTitulo = new Font("Arial", 11, FontStyle.Bold);
                Font fontNormal = new Font("Arial", 9);
                Font fontSmall = new Font("Arial", 8);
                Font fontBold = new Font("Arial", 9, FontStyle.Bold);

                Brush brush = Brushes.Black;
                Brush brushGreen = Brushes.Green;
                Pen pen = new Pen(Color.Black, 1);

                float yPos = 50;
                float leftMargin = 50;
                float rightMargin = e.PageBounds.Width - 50;

                // Cabeçalho
                g.DrawString("ARVOREDO", fontTitulo, brush, leftMargin, yPos);
                yPos += 25;

                g.DrawString("ENDEREÇO: AV. Netinho Prado, 1025", fontNormal, brush, leftMargin, yPos);
                g.DrawString($"DATA DE EMISSÃO: {pedido.DataEmissao:dd/MM/yyyy}", fontNormal, brush, rightMargin - 200, yPos);
                yPos += 20;

                g.DrawString("FONE: (14) 12345-6789", fontNormal, brush, leftMargin, yPos);
                g.DrawString("PÁGINAS: 1 de 1", fontNormal, brush, rightMargin - 200, yPos);
                yPos += 30;

                // DADOS DO CLIENTE
                g.DrawString("DADOS DO CLIENTE", fontBold, brush, e.PageBounds.Width / 2 - 80, yPos);
                yPos += 5;
                g.DrawRectangle(pen, leftMargin, yPos, rightMargin - leftMargin, 100);
                yPos += 15;

                g.DrawString($"CLIENTE: {pedido.Cliente}", fontSmall, brush, leftMargin + 10, yPos);
                g.DrawString($"CNPJ/CPF: {pedido.CPF_CNPJ}", fontSmall, brush, leftMargin + 300, yPos);
                yPos += 20;

                g.DrawString($"ENDEREÇO: {pedido.Endereco}, {pedido.Numero}", fontSmall, brush, leftMargin + 10, yPos);
                g.DrawString($"BAIRRO: ID. {pedido.Bairro}", fontSmall, brush, leftMargin + 300, yPos);
                yPos += 20;

                g.DrawString($"CEP/MUNICÍPIO: {pedido.CEP}, {pedido.Cidade}, SP", fontSmall, brush, leftMargin + 10, yPos);
                g.DrawString($"TEL/CELL: {pedido.Telefone}", fontSmall, brush, leftMargin + 300, yPos);
                yPos += 20;

                g.DrawString($"VENDEDOR: {pedido.Vendedor}", fontSmall, brush, leftMargin + 10, yPos);
                g.DrawString("FANTASIA:", fontSmall, brush, leftMargin + 300, yPos);
                yPos += 30;

                // PRODUTOS
                g.DrawString("PRODUTOS", fontBold, brush, e.PageBounds.Width / 2 - 40, yPos);
                yPos += 20;

                // Cabeçalho da tabela
                float col1 = leftMargin;
                float col2 = col1 + 40;
                float col3 = col2 + 250;
                float col4 = col3 + 50;
                float col5 = col4 + 50;
                float col6 = col5 + 80;
                float col7 = col6 + 80;

                g.FillRectangle(Brushes.LightGray, col1, yPos, rightMargin - leftMargin, 20);
                g.DrawRectangle(pen, col1, yPos, rightMargin - leftMargin, 20);

                g.DrawString("SEQ", fontBold, brush, col1 + 5, yPos + 3);
                g.DrawString("DESCRIÇÃO", fontBold, brush, col2 + 5, yPos + 3);
                g.DrawString("UNI.", fontBold, brush, col3 + 5, yPos + 3);
                g.DrawString("QTD.", fontBold, brush, col4 + 5, yPos + 3);
                g.DrawString("VLR. UNI.", fontBold, brush, col5 + 5, yPos + 3);
                g.DrawString("VLR. TOTAL", fontBold, brush, col6 + 5, yPos + 3);
                yPos += 20;

                // Itens da tabela
                foreach (var item in pedido.Itens)
                {
                    g.DrawRectangle(pen, col1, yPos, rightMargin - leftMargin, 20);

                    g.DrawString(item.Sequencia.ToString(), fontSmall, brush, col1 + 5, yPos + 3);
                    g.DrawString(item.Descricao, fontSmall, brush, col2 + 5, yPos + 3);
                    g.DrawString(item.Unidade, fontSmall, brush, col3 + 5, yPos + 3);
                    g.DrawString(item.Quantidade.ToString("F0"), fontSmall, brush, col4 + 5, yPos + 3);
                    g.DrawString(item.ValorUnitario.ToString("C2"), fontSmall, brush, col5 + 5, yPos + 3);
                    g.DrawString(item.ValorTotal.ToString("C2"), fontSmall, brush, col6 + 5, yPos + 3);

                    yPos += 20;
                }

                yPos += 20;

                // TOTAIS
                g.DrawString("TOTAIS", fontBold, brush, e.PageBounds.Width / 2 - 30, yPos);
                yPos += 25;

                g.DrawString("4X SEM JUROS:", fontSmall, brush, leftMargin, yPos);
                decimal valorParcela = pedido.TotalGeral / 4;
                g.DrawString($"R$ {valorParcela:F2}", fontSmall, brush, rightMargin - 100, yPos);
                yPos += 20;

                g.DrawString("DESCONTOS:", fontSmall, brush, leftMargin, yPos);
                g.DrawString($"R$ {pedido.Desconto:F2}", fontSmall, brush, rightMargin - 100, yPos);
                yPos += 20;

                g.DrawString("ACRÉSCIMOS:", fontSmall, brush, leftMargin, yPos);
                g.DrawString($"R$ {pedido.Acrescimo:F2}", fontSmall, brush, rightMargin - 100, yPos);
                yPos += 20;

                g.DrawString("TOTAL À VISTA:", fontBold, brush, leftMargin, yPos);
                g.DrawString($"R$ {pedido.TotalGeral:F2}", fontBold, brushGreen, rightMargin - 100, yPos);
                yPos += 30;

                g.DrawString("FORMA DE PAGAMENTO:", fontBold, brush, leftMargin, yPos);
                g.DrawString(pedido.FormaPagamento ?? "DINHEIRO", fontBold, brush, leftMargin + 200, yPos);

                // Rodapé
                yPos = e.PageBounds.Height - 100;
                g.DrawString("_________________________________________", fontSmall, brush, e.PageBounds.Width / 2 - 150, yPos);
                yPos += 20;
                g.DrawString("Assinatura do Cliente", fontSmall, brush, e.PageBounds.Width / 2 - 70, yPos);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar página de impressão: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            e.HasMorePages = false;
        }
    }
}