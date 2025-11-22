using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormRelatorio : Form
    {
        private PrintDocument printDocument;
        private string[] linhasParaImprimir;
        private int indiceLinhaAtual;

        public FormRelatorio(string conteudo, string titulo = "Relatório")
        {
            InitializeComponent();
            this.Text = titulo;
            textBoxRelatorio.Text = conteudo;

            // Inicializa o componente de impressão
            printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDocument;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    // Prepara o texto para impressão
                    linhasParaImprimir = textBoxRelatorio.Text.Split('\n');
                    indiceLinhaAtual = 0;

                    // Inicia a impressão
                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao imprimir: {ex.Message}",
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Configurações de impressão
            Font fonteImpressao = new Font("Courier New", 10);
            float margemEsquerda = e.MarginBounds.Left;
            float margemTopo = e.MarginBounds.Top;
            float linhasPorPagina = e.MarginBounds.Height / fonteImpressao.GetHeight(e.Graphics);
            int contador = 0;
            float posicaoY = margemTopo;

            // Imprime cada linha
            while (contador < linhasPorPagina && indiceLinhaAtual < linhasParaImprimir.Length)
            {
                string linha = linhasParaImprimir[indiceLinhaAtual].TrimEnd('\r');
                e.Graphics.DrawString(linha, fonteImpressao, Brushes.Black,
                                     margemEsquerda, posicaoY, new StringFormat());

                posicaoY += fonteImpressao.GetHeight(e.Graphics);
                indiceLinhaAtual++;
                contador++;
            }

            // Verifica se há mais páginas para imprimir
            e.HasMorePages = (indiceLinhaAtual < linhasParaImprimir.Length);
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Arquivo de Texto (*.txt)|*.txt|Todos os Arquivos (*.*)|*.*";
                saveDialog.Title = "Salvar Relatório";
                saveDialog.FileName = $"Relatorio_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveDialog.FileName, textBoxRelatorio.Text);
                    MessageBox.Show("Relatório salvo com sucesso!", "Salvar",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar arquivo: {ex.Message}",
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (printDocument != null)
                {
                    printDocument.Dispose();
                }
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}