using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormRelatorioAnual : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeft, int nTop, int nRight, int nBottom,
            int nWidthEllipse, int nHeightEllipse);

        private List<Orcamento> pedidosAno;
        private int ano;
        private PrintDocument printDocument;
        private Panel panelRelatorio;

        // Dados calculados
        private decimal receitaTotal;
        private int totalPedidos;
        private Dictionary<string, decimal> receitaPorMes;
        private Dictionary<string, int> pedidosPorMes;

        public FormRelatorioAnual(List<Orcamento> pedidos, int anoSelecionado)
        {
            InitializeComponent();
            this.pedidosAno = pedidos;
            this.ano = anoSelecionado;
            this.receitaPorMes = new Dictionary<string, decimal>();
            this.pedidosPorMes = new Dictionary<string, int>();

            CalcularDados();
            ConfigurarFormulario();
            CriarRelatorio();

            // Configurar impressão
            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.ClientSize = new Size(900, 700);
            this.Text = "Relatório Anual - Madeireira Arvoredo";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(250, 230, 194);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.ResumeLayout(false);
        }

        private void ConfigurarFormulario()
        {
            // Botão Imprimir
            Button btnImprimir = new Button();
            btnImprimir.Text = "🖨️ IMPRIMIR";
            btnImprimir.Location = new Point(650, 20);
            btnImprimir.Size = new Size(220, 45);
            btnImprimir.Font = new Font("Arial", 14F, FontStyle.Bold);
            btnImprimir.BackColor = Color.FromArgb(144, 238, 144);
            btnImprimir.ForeColor = Color.Black;
            btnImprimir.FlatStyle = FlatStyle.Flat;
            btnImprimir.FlatAppearance.BorderSize = 0;
            btnImprimir.Cursor = Cursors.Hand;
            btnImprimir.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnImprimir.Width, btnImprimir.Height, 15, 15));
            btnImprimir.Click += BtnImprimir_Click;
            this.Controls.Add(btnImprimir);

            // Botão Fechar
            Button btnFechar = new Button();
            btnFechar.Text = "✖ FECHAR";
            btnFechar.Location = new Point(30, 20);
            btnFechar.Size = new Size(150, 45);
            btnFechar.Font = new Font("Arial", 12F, FontStyle.Bold);
            btnFechar.BackColor = Color.FromArgb(255, 140, 0);
            btnFechar.ForeColor = Color.White;
            btnFechar.FlatStyle = FlatStyle.Flat;
            btnFechar.FlatAppearance.BorderSize = 0;
            btnFechar.Cursor = Cursors.Hand;
            btnFechar.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnFechar.Width, btnFechar.Height, 15, 15));
            btnFechar.Click += (s, e) => this.Close();
            this.Controls.Add(btnFechar);
        }

        private void CalcularDados()
        {
            string[] meses = { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                              "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };

            // Inicializar dicionários
            foreach (string mes in meses)
            {
                receitaPorMes[mes] = 0;
                pedidosPorMes[mes] = 0;
            }

            // Calcular totais
            receitaTotal = 0;
            totalPedidos = pedidosAno.Count;

            foreach (var pedido in pedidosAno)
            {
                string mesNome = meses[pedido.DataEmissao.Month - 1];
                receitaPorMes[mesNome] += pedido.TotalGeral;
                pedidosPorMes[mesNome]++;
                receitaTotal += pedido.TotalGeral;
            }
        }

        private void CriarRelatorio()
        {
            // Container principal do relatório
            panelRelatorio = new Panel();
            panelRelatorio.Location = new Point(30, 80);
            panelRelatorio.Size = new Size(840, 590);
            panelRelatorio.BackColor = Color.White;
            panelRelatorio.BorderStyle = BorderStyle.FixedSingle;
            panelRelatorio.AutoScroll = true;
            this.Controls.Add(panelRelatorio);

            int yPos = 20;

            // Cabeçalho com logo e título
            yPos = CriarCabecalho(yPos);

            // Resumo executivo
            yPos = CriarResumoExecutivo(yPos);

            // Tabela mensal
            yPos = CriarTabelaMensal(yPos);

            // Análise geral
            yPos = CriarAnaliseGeral(yPos);
        }

        private int CriarCabecalho(int yPos)
        {
            // Título principal
            Label lblTitulo = new Label();
            lblTitulo.Text = "RELATÓRIO ANUAL DE DESEMPENHO";
            lblTitulo.Location = new Point(20, yPos);
            lblTitulo.Size = new Size(800, 35);
            lblTitulo.Font = new Font("Arial", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(57, 27, 1);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            panelRelatorio.Controls.Add(lblTitulo);
            yPos += 40;

            // Subtítulo com empresa e ano
            Label lblSubtitulo = new Label();
            lblSubtitulo.Text = $"MADEIREIRA ARVOREDO ({ano})";
            lblSubtitulo.Location = new Point(20, yPos);
            lblSubtitulo.Size = new Size(800, 25);
            lblSubtitulo.Font = new Font("Arial", 14F, FontStyle.Bold);
            lblSubtitulo.ForeColor = Color.FromArgb(57, 27, 1);
            lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;
            panelRelatorio.Controls.Add(lblSubtitulo);
            yPos += 35;

            // Linha separadora
            Panel linha1 = new Panel();
            linha1.Location = new Point(40, yPos);
            linha1.Size = new Size(760, 2);
            linha1.BackColor = Color.FromArgb(57, 27, 1);
            panelRelatorio.Controls.Add(linha1);
            yPos += 15;

            return yPos;
        }

        private int CriarResumoExecutivo(int yPos)
        {
            // Título da seção
            Label lblResumoTitulo = new Label();
            lblResumoTitulo.Text = "■ RESUMO EXECUTIVO DO ANO";
            lblResumoTitulo.Location = new Point(40, yPos);
            lblResumoTitulo.Size = new Size(760, 25);
            lblResumoTitulo.Font = new Font("Arial", 12F, FontStyle.Bold);
            lblResumoTitulo.ForeColor = Color.FromArgb(57, 27, 1);
            panelRelatorio.Controls.Add(lblResumoTitulo);
            yPos += 35;

            // Painel com cards de resumo
            Panel panelCards = new Panel();
            panelCards.Location = new Point(40, yPos);
            panelCards.Size = new Size(760, 120);
            panelCards.BackColor = Color.Transparent;
            panelRelatorio.Controls.Add(panelCards);

            // Card 1: Faturamento
            Panel card1 = CriarCard("Faturamento Total", receitaTotal.ToString("C"),
                Color.FromArgb(144, 238, 144), 0);
            panelCards.Controls.Add(card1);

            // Card 2: Total de Pedidos
            Panel card2 = CriarCard("Total de Pedidos", totalPedidos.ToString() + " pedidos",
                Color.FromArgb(173, 216, 230), 260);
            panelCards.Controls.Add(card2);

            // Card 3: Média Mensal
            decimal mediaMensal = receitaTotal / 12;
            Panel card3 = CriarCard("Média Mensal", mediaMensal.ToString("C"),
                Color.FromArgb(255, 218, 185), 520);
            panelCards.Controls.Add(card3);

            yPos += 135;

            return yPos;
        }

        private Panel CriarCard(string titulo, string valor, Color cor, int xPos)
        {
            Panel card = new Panel();
            card.Location = new Point(xPos, 0);
            card.Size = new Size(240, 120);
            card.BackColor = cor;
            card.BorderStyle = BorderStyle.FixedSingle;

            Label lblTitulo = new Label();
            lblTitulo.Text = titulo;
            lblTitulo.Location = new Point(10, 15);
            lblTitulo.Size = new Size(220, 30);
            lblTitulo.Font = new Font("Arial", 11F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(57, 27, 1);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            card.Controls.Add(lblTitulo);

            Label lblValor = new Label();
            lblValor.Text = valor;
            lblValor.Location = new Point(10, 55);
            lblValor.Size = new Size(220, 45);
            lblValor.Font = new Font("Arial", 18F, FontStyle.Bold);
            lblValor.ForeColor = Color.FromArgb(57, 27, 1);
            lblValor.TextAlign = ContentAlignment.MiddleCenter;
            card.Controls.Add(lblValor);

            return card;
        }

        private int CriarTabelaMensal(int yPos)
        {
            // Título da seção
            Label lblTabelaTitulo = new Label();
            lblTabelaTitulo.Text = "■ DESEMPENHO MENSAL";
            lblTabelaTitulo.Location = new Point(40, yPos);
            lblTabelaTitulo.Size = new Size(760, 25);
            lblTabelaTitulo.Font = new Font("Arial", 12F, FontStyle.Bold);
            lblTabelaTitulo.ForeColor = Color.FromArgb(57, 27, 1);
            panelRelatorio.Controls.Add(lblTabelaTitulo);
            yPos += 35;

            // Cabeçalho da tabela
            Panel cabecalho = new Panel();
            cabecalho.Location = new Point(40, yPos);
            cabecalho.Size = new Size(760, 35);
            cabecalho.BackColor = Color.FromArgb(198, 143, 86);
            cabecalho.BorderStyle = BorderStyle.FixedSingle;
            panelRelatorio.Controls.Add(cabecalho);

            Label lblMes = new Label();
            lblMes.Text = "Mês";
            lblMes.Location = new Point(20, 8);
            lblMes.Size = new Size(200, 20);
            lblMes.Font = new Font("Arial", 10F, FontStyle.Bold);
            lblMes.ForeColor = Color.White;
            cabecalho.Controls.Add(lblMes);

            Label lblFaturamento = new Label();
            lblFaturamento.Text = "Faturamento";
            lblFaturamento.Location = new Point(250, 8);
            lblFaturamento.Size = new Size(200, 20);
            lblFaturamento.Font = new Font("Arial", 10F, FontStyle.Bold);
            lblFaturamento.ForeColor = Color.White;
            lblFaturamento.TextAlign = ContentAlignment.TopRight;
            cabecalho.Controls.Add(lblFaturamento);

            Label lblPedidos = new Label();
            lblPedidos.Text = "N° Pedidos";
            lblPedidos.Location = new Point(480, 8);
            lblPedidos.Size = new Size(120, 20);
            lblPedidos.Font = new Font("Arial", 10F, FontStyle.Bold);
            lblPedidos.ForeColor = Color.White;
            lblPedidos.TextAlign = ContentAlignment.TopRight;
            cabecalho.Controls.Add(lblPedidos);

            Label lblMedia = new Label();
            lblMedia.Text = "Média/Pedido";
            lblMedia.Location = new Point(620, 8);
            lblMedia.Size = new Size(120, 20);
            lblMedia.Font = new Font("Arial", 10F, FontStyle.Bold);
            lblMedia.ForeColor = Color.White;
            lblMedia.TextAlign = ContentAlignment.TopRight;
            cabecalho.Controls.Add(lblMedia);

            yPos += 35;

            // Linhas da tabela
            string[] meses = { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                              "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };

            bool corAlternada = false;
            foreach (string mes in meses)
            {
                Panel linha = new Panel();
                linha.Location = new Point(40, yPos);
                linha.Size = new Size(760, 30);
                linha.BackColor = corAlternada ? Color.FromArgb(239, 212, 172) : Color.White;
                linha.BorderStyle = BorderStyle.FixedSingle;
                panelRelatorio.Controls.Add(linha);

                Label lblMesNome = new Label();
                lblMesNome.Text = mes;
                lblMesNome.Location = new Point(20, 6);
                lblMesNome.Size = new Size(200, 18);
                lblMesNome.Font = new Font("Arial", 9F, FontStyle.Regular);
                lblMesNome.ForeColor = Color.FromArgb(57, 27, 1);
                linha.Controls.Add(lblMesNome);

                Label lblFat = new Label();
                lblFat.Text = receitaPorMes[mes].ToString("C");
                lblFat.Location = new Point(250, 6);
                lblFat.Size = new Size(200, 18);
                lblFat.Font = new Font("Arial", 9F, FontStyle.Regular);
                lblFat.ForeColor = Color.FromArgb(57, 27, 1);
                lblFat.TextAlign = ContentAlignment.TopRight;
                linha.Controls.Add(lblFat);

                Label lblQtd = new Label();
                lblQtd.Text = pedidosPorMes[mes].ToString();
                lblQtd.Location = new Point(480, 6);
                lblQtd.Size = new Size(120, 18);
                lblQtd.Font = new Font("Arial", 9F, FontStyle.Regular);
                lblQtd.ForeColor = Color.FromArgb(57, 27, 1);
                lblQtd.TextAlign = ContentAlignment.TopRight;
                linha.Controls.Add(lblQtd);

                decimal mediaPedido = pedidosPorMes[mes] > 0 ? receitaPorMes[mes] / pedidosPorMes[mes] : 0;
                Label lblMed = new Label();
                lblMed.Text = mediaPedido.ToString("C");
                lblMed.Location = new Point(620, 6);
                lblMed.Size = new Size(120, 18);
                lblMed.Font = new Font("Arial", 9F, FontStyle.Regular);
                lblMed.ForeColor = Color.FromArgb(57, 27, 1);
                lblMed.TextAlign = ContentAlignment.TopRight;
                linha.Controls.Add(lblMed);

                corAlternada = !corAlternada;
                yPos += 30;
            }

            // Linha de total
            Panel linhaTotal = new Panel();
            linhaTotal.Location = new Point(40, yPos);
            linhaTotal.Size = new Size(760, 35);
            linhaTotal.BackColor = Color.FromArgb(198, 143, 86);
            linhaTotal.BorderStyle = BorderStyle.FixedSingle;
            panelRelatorio.Controls.Add(linhaTotal);

            Label lblTotalTexto = new Label();
            lblTotalTexto.Text = "TOTAL ANUAL";
            lblTotalTexto.Location = new Point(20, 8);
            lblTotalTexto.Size = new Size(200, 20);
            lblTotalTexto.Font = new Font("Arial", 10F, FontStyle.Bold);
            lblTotalTexto.ForeColor = Color.White;
            linhaTotal.Controls.Add(lblTotalTexto);

            Label lblTotalFat = new Label();
            lblTotalFat.Text = receitaTotal.ToString("C");
            lblTotalFat.Location = new Point(250, 8);
            lblTotalFat.Size = new Size(200, 20);
            lblTotalFat.Font = new Font("Arial", 10F, FontStyle.Bold);
            lblTotalFat.ForeColor = Color.White;
            lblTotalFat.TextAlign = ContentAlignment.TopRight;
            linhaTotal.Controls.Add(lblTotalFat);

            Label lblTotalPed = new Label();
            lblTotalPed.Text = totalPedidos.ToString();
            lblTotalPed.Location = new Point(480, 8);
            lblTotalPed.Size = new Size(120, 20);
            lblTotalPed.Font = new Font("Arial", 10F, FontStyle.Bold);
            lblTotalPed.ForeColor = Color.White;
            lblTotalPed.TextAlign = ContentAlignment.TopRight;
            linhaTotal.Controls.Add(lblTotalPed);

            decimal mediaGeralPedido = totalPedidos > 0 ? receitaTotal / totalPedidos : 0;
            Label lblTotalMed = new Label();
            lblTotalMed.Text = mediaGeralPedido.ToString("C");
            lblTotalMed.Location = new Point(620, 8);
            lblTotalMed.Size = new Size(120, 20);
            lblTotalMed.Font = new Font("Arial", 10F, FontStyle.Bold);
            lblTotalMed.ForeColor = Color.White;
            lblTotalMed.TextAlign = ContentAlignment.TopRight;
            linhaTotal.Controls.Add(lblTotalMed);

            yPos += 50;

            return yPos;
        }

        private int CriarAnaliseGeral(int yPos)
        {
            // Título da seção
            Label lblAnaliseTitulo = new Label();
            lblAnaliseTitulo.Text = "■ ANÁLISE GERAL";
            lblAnaliseTitulo.Location = new Point(40, yPos);
            lblAnaliseTitulo.Size = new Size(760, 25);
            lblAnaliseTitulo.Font = new Font("Arial", 12F, FontStyle.Bold);
            lblAnaliseTitulo.ForeColor = Color.FromArgb(57, 27, 1);
            panelRelatorio.Controls.Add(lblAnaliseTitulo);
            yPos += 35;

            // Encontrar melhor e pior mês
            string melhorMes = receitaPorMes.OrderByDescending(x => x.Value).First().Key;
            decimal melhorValor = receitaPorMes[melhorMes];

            string piorMes = receitaPorMes.Where(x => x.Value > 0).OrderBy(x => x.Value).First().Key;
            decimal piorValor = receitaPorMes[piorMes];

            // Pontos fortes
            Label lblPontosFortesT = new Label();
            lblPontosFortesT.Text = "● Pontos Fortes:";
            lblPontosFortesT.Location = new Point(60, yPos);
            lblPontosFortesT.Size = new Size(720, 20);
            lblPontosFortesT.Font = new Font("Arial", 10F, FontStyle.Bold);
            lblPontosFortesT.ForeColor = Color.FromArgb(57, 27, 1);
            panelRelatorio.Controls.Add(lblPontosFortesT);
            yPos += 25;

            Label lblPontosFortes = new Label();
            lblPontosFortes.Text = $"   - Dezembro teve o melhor mês do ano, com faturamento de {melhorValor:C}\n" +
                                   $"   - Total de {totalPedidos} pedidos processados no ano\n" +
                                   $"   - Média mensal de {(receitaTotal / 12):C} mantida consistente";
            lblPontosFortes.Location = new Point(60, yPos);
            lblPontosFortes.Size = new Size(720, 60);
            lblPontosFortes.Font = new Font("Arial", 9F, FontStyle.Regular);
            lblPontosFortes.ForeColor = Color.FromArgb(57, 27, 1);
            panelRelatorio.Controls.Add(lblPontosFortes);
            yPos += 70;

            // Desafios
            Label lblDesafiosT = new Label();
            lblDesafiosT.Text = "● Desafios Enfrentados:";
            lblDesafiosT.Location = new Point(60, yPos);
            lblDesafiosT.Size = new Size(720, 20);
            lblDesafiosT.Font = new Font("Arial", 10F, FontStyle.Bold);
            lblDesafiosT.ForeColor = Color.FromArgb(57, 27, 1);
            panelRelatorio.Controls.Add(lblDesafiosT);
            yPos += 25;

            Label lblDesafios = new Label();
            lblDesafios.Text = $"   - Meses de baixa demanda impactaram o faturamento\n" +
                              $"   - Necessidade de otimização de estoque";
            lblDesafios.Location = new Point(60, yPos);
            lblDesafios.Size = new Size(720, 40);
            lblDesafios.Font = new Font("Arial", 9F, FontStyle.Regular);
            lblDesafios.ForeColor = Color.FromArgb(57, 27, 1);
            panelRelatorio.Controls.Add(lblDesafios);
            yPos += 50;

            // Perspectivas
            Label lblPerspectivasT = new Label();
            lblPerspectivasT.Text = $"■ PERSPECTIVAS PARA {ano + 1}";
            lblPerspectivasT.Location = new Point(40, yPos);
            lblPerspectivasT.Size = new Size(760, 25);
            lblPerspectivasT.Font = new Font("Arial", 12F, FontStyle.Bold);
            lblPerspectivasT.ForeColor = Color.FromArgb(57, 27, 1);
            panelRelatorio.Controls.Add(lblPerspectivasT);
            yPos += 35;

            Label lblPerspectivas = new Label();
            lblPerspectivas.Text = "   Com base nos resultados consolidados, a empresa projeta um crescimento de 10 a 15% para o\n" +
                                   $"   próximo ano. As estratégias incluem expansão de produtos e melhoria no atendimento.";
            lblPerspectivas.Location = new Point(60, yPos);
            lblPerspectivas.Size = new Size(720, 40);
            lblPerspectivas.Font = new Font("Arial", 9F, FontStyle.Regular);
            lblPerspectivas.ForeColor = Color.FromArgb(57, 27, 1);
            panelRelatorio.Controls.Add(lblPerspectivas);
            yPos += 60;

            return yPos;
        }

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
                MessageBox.Show(
                    $"Erro ao imprimir relatório:\n{ex.Message}",
                    "Erro de Impressão",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font fonteTitulo = new Font("Arial", 18F, FontStyle.Bold);
            Font fonteSubtitulo = new Font("Arial", 14F, FontStyle.Bold);
            Font fonteCabecalho = new Font("Arial", 10F, FontStyle.Bold);
            Font fonteTexto = new Font("Arial", 9F, FontStyle.Regular);
            Font fonteTextoNegrito = new Font("Arial", 9F, FontStyle.Bold);

            Brush brushTexto = new SolidBrush(Color.FromArgb(57, 27, 1));
            Brush brushBranco = Brushes.White;
            Pen penLinha = new Pen(Color.FromArgb(57, 27, 1), 2);

            float yPos = 50;
            float margemEsq = 50;
            float larguraPagina = e.PageBounds.Width - 100;

            // Cabeçalho
            g.DrawString("RELATÓRIO ANUAL DE DESEMPENHO", fonteTitulo, brushTexto,
                new RectangleF(margemEsq, yPos, larguraPagina, 35),
                new StringFormat { Alignment = StringAlignment.Center });
            yPos += 40;

            g.DrawString($"MADEIREIRA ARVOREDO ({ano})", fonteSubtitulo, brushTexto,
                new RectangleF(margemEsq, yPos, larguraPagina, 25),
                new StringFormat { Alignment = StringAlignment.Center });
            yPos += 35;

            g.DrawLine(penLinha, margemEsq, yPos, margemEsq + larguraPagina, yPos);
            yPos += 20;

            // Resumo Executivo
            g.DrawString("■ RESUMO EXECUTIVO DO ANO", fonteCabecalho, brushTexto, margemEsq, yPos);
            yPos += 30;

            decimal mediaMensal = receitaTotal / 12;
            g.DrawString($"Faturamento Total: {receitaTotal:C}", fonteTextoNegrito, brushTexto, margemEsq + 20, yPos);
            yPos += 20;
            g.DrawString($"Total de Pedidos: {totalPedidos} pedidos", fonteTexto, brushTexto, margemEsq + 20, yPos);
            yPos += 20;
            g.DrawString($"Média Mensal: {mediaMensal:C}", fonteTexto, brushTexto, margemEsq + 20, yPos);
            yPos += 30;

            // Desempenho Mensal
            g.DrawString("■ DESEMPENHO MENSAL", fonteCabecalho, brushTexto, margemEsq, yPos);
            yPos += 30;

            // Cabeçalho da tabela
            RectangleF rectCabecalho = new RectangleF(margemEsq, yPos, larguraPagina, 25);
            g.FillRectangle(new SolidBrush(Color.FromArgb(198, 143, 86)), rectCabecalho);
            g.DrawRectangle(Pens.Black, Rectangle.Round(rectCabecalho));

            g.DrawString("Mês", fonteCabecalho, brushBranco, margemEsq + 10, yPos + 5);
            g.DrawString("Faturamento", fonteCabecalho, brushBranco, margemEsq + 250, yPos + 5);
            g.DrawString("N° Pedidos", fonteCabecalho, brushBranco, margemEsq + 430, yPos + 5);
            g.DrawString("Média/Pedido", fonteCabecalho, brushBranco, margemEsq + 580, yPos + 5);
            yPos += 25;

            // Linhas da tabela
            string[] meses = { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                              "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };

            foreach (string mes in meses)
            {
                RectangleF rectLinha = new RectangleF(margemEsq, yPos, larguraPagina, 20);
                g.DrawRectangle(Pens.Black, Rectangle.Round(rectLinha));

                g.DrawString(mes, fonteTexto, brushTexto, margemEsq + 10, yPos + 3);
                g.DrawString(receitaPorMes[mes].ToString("C"), fonteTexto, brushTexto, margemEsq + 250, yPos + 3);
                g.DrawString(pedidosPorMes[mes].ToString(), fonteTexto, brushTexto, margemEsq + 470, yPos + 3);

                decimal mediaPedido = pedidosPorMes[mes] > 0 ? receitaPorMes[mes] / pedidosPorMes[mes] : 0;
                g.DrawString(mediaPedido.ToString("C"), fonteTexto, brushTexto, margemEsq + 600, yPos + 3);

                yPos += 20;
            }

            // Linha de total
            RectangleF rectTotal = new RectangleF(margemEsq, yPos, larguraPagina, 25);
            g.FillRectangle(new SolidBrush(Color.FromArgb(198, 143, 86)), rectTotal);
            g.DrawRectangle(Pens.Black, Rectangle.Round(rectTotal));

            g.DrawString("TOTAL ANUAL", fonteCabecalho, brushBranco, margemEsq + 10, yPos + 5);
            g.DrawString(receitaTotal.ToString("C"), fonteCabecalho, brushBranco, margemEsq + 250, yPos + 5);
            g.DrawString(totalPedidos.ToString(), fonteCabecalho, brushBranco, margemEsq + 470, yPos + 5);

            decimal mediaGeralPedido = totalPedidos > 0 ? receitaTotal / totalPedidos : 0;
            g.DrawString(mediaGeralPedido.ToString("C"), fonteCabecalho, brushBranco, margemEsq + 600, yPos + 5);
            yPos += 35;

            // Análise Geral
            g.DrawString("■ ANÁLISE GERAL", fonteCabecalho, brushTexto, margemEsq, yPos);
            yPos += 25;

            string melhorMes = receitaPorMes.OrderByDescending(x => x.Value).First().Key;
            decimal melhorValor = receitaPorMes[melhorMes];

            g.DrawString("● Pontos Fortes:", fonteCabecalho, brushTexto, margemEsq + 10, yPos);
            yPos += 20;
            g.DrawString($"   - {melhorMes} teve o melhor mês do ano, com faturamento de {melhorValor:C}",
                fonteTexto, brushTexto, margemEsq + 20, yPos);
            yPos += 18;
            g.DrawString($"   - Total de {totalPedidos} pedidos processados no ano",
                fonteTexto, brushTexto, margemEsq + 20, yPos);
            yPos += 18;
            g.DrawString($"   - Média mensal de {(receitaTotal / 12):C} mantida consistente",
                fonteTexto, brushTexto, margemEsq + 20, yPos);
            yPos += 25;

            g.DrawString("● Desafios Enfrentados:", fonteCabecalho, brushTexto, margemEsq + 10, yPos);
            yPos += 20;
            g.DrawString("   - Meses de baixa demanda impactaram o faturamento",
                fonteTexto, brushTexto, margemEsq + 20, yPos);
            yPos += 18;
            g.DrawString("   - Necessidade de otimização de estoque",
                fonteTexto, brushTexto, margemEsq + 20, yPos);
            yPos += 30;

            // Perspectivas
            g.DrawString($"■ PERSPECTIVAS PARA {ano + 1}", fonteCabecalho, brushTexto, margemEsq, yPos);
            yPos += 25;
            g.DrawString("   Com base nos resultados consolidados, a empresa projeta um crescimento de 10 a 15%",
                fonteTexto, brushTexto, margemEsq + 20, yPos);
            yPos += 18;
            g.DrawString("   para o próximo ano. As estratégias incluem expansão de produtos e melhoria no atendimento.",
                fonteTexto, brushTexto, margemEsq + 20, yPos);

            // Rodapé
            yPos = e.PageBounds.Height - 80;
            g.DrawLine(penLinha, margemEsq, yPos, margemEsq + larguraPagina, yPos);
            yPos += 10;

            Font fonteRodape = new Font("Arial", 8F, FontStyle.Italic);
            g.DrawString($"Relatório gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}",
                fonteRodape, brushTexto, margemEsq, yPos);
            g.DrawString("Madeireira Arvoredo - Sistema de Gestão",
                fonteRodape, brushTexto,
                new RectangleF(margemEsq, yPos, larguraPagina, 20),
                new StringFormat { Alignment = StringAlignment.Far });

            e.HasMorePages = false;
        }
    }
}