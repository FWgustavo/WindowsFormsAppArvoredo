using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormBackup : Form
    {
        public FormBackup()
        {
            InitializeComponent();
            CarregarListaBackups();
        }

        private void FormBackup_Paint(object sender, PaintEventArgs e)
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

        private void CarregarListaBackups()
        {
            panelListaArquivos.Controls.Clear();

            string pastaBackup = Path.Combine(Application.StartupPath, "Backups");
            if (!Directory.Exists(pastaBackup))
                return;

            string[] arquivos = Directory.GetFiles(pastaBackup, "*.bak");
            int yPos = 10;

            foreach (string arquivo in arquivos)
            {
                FileInfo fileInfo = new FileInfo(arquivo);

                Panel itemBackup = new Panel();
                itemBackup.Location = new Point(10, yPos);
                itemBackup.Size = new Size(590, 30);
                itemBackup.BackColor = Color.White;
                itemBackup.BorderStyle = BorderStyle.FixedSingle;

                Label lblNome = new Label();
                lblNome.Text = Path.GetFileNameWithoutExtension(arquivo);
                lblNome.Location = new Point(5, 5);
                lblNome.Size = new Size(180, 20);
                lblNome.Font = new Font("Arial", 8F);
                itemBackup.Controls.Add(lblNome);

                Label lblData = new Label();
                lblData.Text = fileInfo.LastWriteTime.ToString("dd/MM/yyyy HH:mm");
                lblData.Location = new Point(195, 5);
                lblData.Size = new Size(120, 20);
                lblData.Font = new Font("Arial", 8F);
                itemBackup.Controls.Add(lblData);

                Label lblTamanho = new Label();
                lblTamanho.Text = $"{fileInfo.Length / 1024} KB";
                lblTamanho.Location = new Point(325, 5);
                lblTamanho.Size = new Size(80, 20);
                lblTamanho.Font = new Font("Arial", 8F);
                itemBackup.Controls.Add(lblTamanho);

                Label lblCompactado = new Label();
                lblCompactado.Text = "Sim";
                lblCompactado.Location = new Point(415, 5);
                lblCompactado.Size = new Size(80, 20);
                lblCompactado.Font = new Font("Arial", 8F);
                itemBackup.Controls.Add(lblCompactado);

                Button btnExcluirItem = new Button();
                btnExcluirItem.Text = "🗑️";
                btnExcluirItem.Location = new Point(505, 2);
                btnExcluirItem.Size = new Size(30, 25);
                btnExcluirItem.Font = new Font("Arial", 12F);
                btnExcluirItem.FlatStyle = FlatStyle.Flat;
                btnExcluirItem.FlatAppearance.BorderSize = 0;
                btnExcluirItem.Cursor = Cursors.Hand;
                btnExcluirItem.Tag = arquivo;
                btnExcluirItem.Click += BtnExcluirItem_Click;
                itemBackup.Controls.Add(btnExcluirItem);

                itemBackup.Cursor = Cursors.Hand;
                itemBackup.Tag = arquivo;
                itemBackup.Click += (s, e) => SelecionarBackup(arquivo);

                panelListaArquivos.Controls.Add(itemBackup);
                yPos += 35;
            }

            if (arquivos.Length == 0)
            {
                Label lblVazio = new Label();
                lblVazio.Text = "Nenhum backup encontrado";
                lblVazio.Location = new Point(200, 70);
                lblVazio.Size = new Size(200, 30);
                lblVazio.Font = new Font("Arial", 10F, FontStyle.Italic);
                lblVazio.ForeColor = Color.Gray;
                lblVazio.TextAlign = ContentAlignment.MiddleCenter;
                panelListaArquivos.Controls.Add(lblVazio);
            }
        }

        private void BtnCriarArquivo_Click(object sender, EventArgs e)
        {
            try
            {
                string pastaBackup = Path.Combine(Application.StartupPath, "Backups");
                if (!Directory.Exists(pastaBackup))
                    Directory.CreateDirectory(pastaBackup);

                string nomeBackup = string.IsNullOrWhiteSpace(txtNomeBackup.Text)
                    ? $"Backup_{DateTime.Now:yyyyMMdd_HHmmss}"
                    : txtNomeBackup.Text.Trim();

                string caminhoArquivo = Path.Combine(pastaBackup, $"{nomeBackup}.bak");

                // Criar arquivo de backup (simulado)
                File.WriteAllText(caminhoArquivo, $"Backup criado em: {DateTime.Now}\nCompactado: {chkCompactar.Checked}");

                MessageBox.Show($"Backup criado com sucesso!\n\nLocal: {caminhoArquivo}",
                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtNomeBackup.Clear();
                CarregarListaBackups();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar backup:\n{ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRestaurarBackup_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Arquivos de Backup (*.bak)|*.bak|Todos os arquivos (*.*)|*.*";
                openFileDialog.Title = "Selecione um arquivo de backup";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DialogResult resultado = MessageBox.Show(
                        $"Deseja restaurar o backup:\n{Path.GetFileName(openFileDialog.FileName)}?\n\n" +
                        "ATENÇÃO: Esta ação irá sobrescrever os dados atuais!",
                        "Confirmar Restauração",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (resultado == DialogResult.Yes)
                    {
                        MessageBox.Show("Backup restaurado com sucesso!",
                            "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void BtnExcluirBackup_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Arquivos de Backup (*.bak)|*.bak";
                openFileDialog.Title = "Selecione um arquivo para excluir";
                openFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, "Backups");

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DialogResult resultado = MessageBox.Show(
                        $"Deseja realmente excluir o backup:\n{Path.GetFileName(openFileDialog.FileName)}?",
                        "Confirmar Exclusão",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                    {
                        try
                        {
                            File.Delete(openFileDialog.FileName);
                            MessageBox.Show("Backup excluído com sucesso!",
                                "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CarregarListaBackups();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Erro ao excluir backup:\n{ex.Message}",
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void BtnAbrirPasta_Click(object sender, EventArgs e)
        {
            try
            {
                string pastaBackup = Path.Combine(Application.StartupPath, "Backups");
                if (!Directory.Exists(pastaBackup))
                    Directory.CreateDirectory(pastaBackup);

                System.Diagnostics.Process.Start("explorer.exe", pastaBackup);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao abrir pasta:\n{ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExcluirItem_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn?.Tag == null) return;

            string arquivo = btn.Tag.ToString();

            DialogResult resultado = MessageBox.Show(
                $"Deseja realmente excluir o backup:\n{Path.GetFileName(arquivo)}?",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    File.Delete(arquivo);
                    MessageBox.Show("Backup excluído com sucesso!",
                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CarregarListaBackups();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao excluir backup:\n{ex.Message}",
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SelecionarBackup(string arquivo)
        {
            MessageBox.Show($"Backup selecionado:\n{Path.GetFileName(arquivo)}\n\nTamanho: {new FileInfo(arquivo).Length / 1024} KB\nData: {File.GetLastWriteTime(arquivo):dd/MM/yyyy HH:mm}",
                "Detalhes do Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}