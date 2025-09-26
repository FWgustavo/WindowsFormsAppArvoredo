using System;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormRelatorio : Form
    {
        public FormRelatorio(string conteudo, string titulo = "Relatório")
        {
            InitializeComponent();
            this.Text = titulo;
            textBoxRelatorio.Text = conteudo;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Função de impressão será implementada em versão futura.",
                           "Imprimir", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
