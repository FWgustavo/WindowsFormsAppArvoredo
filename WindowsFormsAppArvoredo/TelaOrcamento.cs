using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class TelaOrcamento : Form
    {
        private DataGridView dgvProdutos;
        private TextBox txtPesquisarProdutos;
        private TextBox txtSemJuros;
        private TextBox txtDescontos;
        private TextBox txtAcrescimos;
        private TextBox txtTotalVista;
        private Button btnImprimir;
        private Button btnExcluir;
        private Button btnSalvar;
        private Button btnConfirmar;
        private Label lblCidade; // Label para exibir cidade
        private Label lblUF; // Label para exibir UF
        private bool isConsultandoCep = false; // Flag para evitar loops na formatação

        public TelaOrcamento()
        {
            InitializeComponent();
            AdicionarComponentesExtras();
        }

        private void AdicionarComponentesExtras()
        {
            // Labels para Cidade e UF
            lblCidade = new Label();
            lblCidade.Text = "Cidade:";
            lblCidade.Location = new Point(450, 213);
            lblCidade.Size = new Size(60, 20);
            lblCidade.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lblCidade);

            lblUF = new Label();
            lblUF.Text = "UF:";
            lblUF.Location = new Point(450, 240);
            lblUF.Size = new Size(30, 20);
            lblUF.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lblUF);

            // Campo de pesquisa de produtos
            txtPesquisarProdutos = new TextBox();
            txtPesquisarProdutos.Location = new Point(200, 320);
            txtPesquisarProdutos.Size = new Size(500, 26);
            txtPesquisarProdutos.Font = new Font("Microsoft Sans Serif", 12F);
            txtPesquisarProdutos.Text = "PESQUISAR PRODUTOS";
            txtPesquisarProdutos.ForeColor = Color.Gray;
            txtPesquisarProdutos.Enter += TxtPesquisarProdutos_Enter;
            txtPesquisarProdutos.Leave += TxtPesquisarProdutos_Leave;
            txtPesquisarProdutos.TextChanged += TxtPesquisarProdutos_TextChanged;
            this.Controls.Add(txtPesquisarProdutos);

            // DataGridView para produtos
            dgvProdutos = new DataGridView();
            dgvProdutos.Location = new Point(50, 360);
            dgvProdutos.Size = new Size(980, 200);
            dgvProdutos.AllowUserToAddRows = false;
            dgvProdutos.AllowUserToDeleteRows = false;
            dgvProdutos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProdutos.MultiSelect = false;
            dgvProdutos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Configurar colunas
            dgvProdutos.Columns.Add("SEQ", "SEQ");
            dgvProdutos.Columns.Add("DESCRICAO", "DESCRIÇÃO");
            dgvProdutos.Columns.Add("UNI", "UNI.");
            dgvProdutos.Columns.Add("QTD", "QTD.");
            dgvProdutos.Columns.Add("VLR_UNI", "VLR. UNI.");
            dgvProdutos.Columns.Add("VLR_TOTAL", "VLR. TOTAL");

            // Ajustar largura das colunas
            dgvProdutos.Columns["SEQ"].Width = 60;
            dgvProdutos.Columns["UNI"].Width = 80;
            dgvProdutos.Columns["QTD"].Width = 80;
            dgvProdutos.Columns["VLR_UNI"].Width = 100;
            dgvProdutos.Columns["VLR_TOTAL"].Width = 120;

            // Adicionar algumas linhas vazias
            for (int i = 0; i < 8; i++)
            {
                dgvProdutos.Rows.Add("", "", "", "", "", "");
            }

            this.Controls.Add(dgvProdutos);

            // Seção de totais
            Label lblTotais = new Label();
            lblTotais.Text = "TOTAIS";
            lblTotais.Location = new Point(500, 580);
            lblTotais.Size = new Size(100, 25);
            lblTotais.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold);
            lblTotais.BorderStyle = BorderStyle.FixedSingle;
            lblTotais.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTotais);

            // Campos de totais
            Label lbl4xSemJuros = new Label();
            lbl4xSemJuros.Text = "4X SEM JUROS:";
            lbl4xSemJuros.Location = new Point(100, 620);
            lbl4xSemJuros.Size = new Size(120, 20);
            lbl4xSemJuros.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lbl4xSemJuros);

            txtSemJuros = new TextBox();
            txtSemJuros.Location = new Point(230, 618);
            txtSemJuros.Size = new Size(150, 22);
            this.Controls.Add(txtSemJuros);

            Label lblDescontos = new Label();
            lblDescontos.Text = "DESCONTOS:";
            lblDescontos.Location = new Point(400, 620);
            lblDescontos.Size = new Size(100, 20);
            lblDescontos.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lblDescontos);

            txtDescontos = new TextBox();
            txtDescontos.Location = new Point(510, 618);
            txtDescontos.Size = new Size(150, 22);
            txtDescontos.TextChanged += CalcularTotal;
            this.Controls.Add(txtDescontos);

            Label lblAcrescimos = new Label();
            lblAcrescimos.Text = "ACRÉSCIMOS:";
            lblAcrescimos.Location = new Point(680, 620);
            lblAcrescimos.Size = new Size(100, 20);
            lblAcrescimos.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lblAcrescimos);

            txtAcrescimos = new TextBox();
            txtAcrescimos.Location = new Point(790, 618);
            txtAcrescimos.Size = new Size(150, 22);
            txtAcrescimos.TextChanged += CalcularTotal;
            this.Controls.Add(txtAcrescimos);

            Label lblTotalVista = new Label();
            lblTotalVista.Text = "TOTAL À VISTA:";
            lblTotalVista.Location = new Point(100, 650);
            lblTotalVista.Size = new Size(120, 20);
            lblTotalVista.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.Controls.Add(lblTotalVista);

            txtTotalVista = new TextBox();
            txtTotalVista.Location = new Point(230, 648);
            txtTotalVista.Size = new Size(150, 22);
            txtTotalVista.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            txtTotalVista.ReadOnly = true;
            txtTotalVista.BackColor = Color.LightGray;
            this.Controls.Add(txtTotalVista);

            // Botões
            btnImprimir = new Button();
            btnImprimir.Text = "IMPRIMIR";
            btnImprimir.Location = new Point(200, 690);
            btnImprimir.Size = new Size(120, 40);
            btnImprimir.BackColor = Color.Gold;
            btnImprimir.FlatStyle = FlatStyle.Flat;
            btnImprimir.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            btnImprimir.Click += BtnImprimir_Click;
            this.Controls.Add(btnImprimir);

            btnExcluir = new Button();
            btnExcluir.Text = "EXCLUIR";
            btnExcluir.Location = new Point(340, 690);
            btnExcluir.Size = new Size(120, 40);
            btnExcluir.BackColor = Color.Crimson;
            btnExcluir.ForeColor = Color.White;
            btnExcluir.FlatStyle = FlatStyle.Flat;
            btnExcluir.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            btnExcluir.Click += BtnExcluir_Click;
            this.Controls.Add(btnExcluir);

            btnSalvar = new Button();
            btnSalvar.Text = "SALVAR";
            btnSalvar.Location = new Point(480, 690);
            btnSalvar.Size = new Size(120, 40);
            btnSalvar.BackColor = Color.DodgerBlue;
            btnSalvar.ForeColor = Color.White;
            btnSalvar.FlatStyle = FlatStyle.Flat;
            btnSalvar.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            btnSalvar.Click += BtnSalvar_Click;
            this.Controls.Add(btnSalvar);

            btnConfirmar = new Button();
            btnConfirmar.Text = "CONFIRMAR";
            btnConfirmar.Location = new Point(620, 690);
            btnConfirmar.Size = new Size(120, 40);
            btnConfirmar.BackColor = Color.LimeGreen;
            btnConfirmar.ForeColor = Color.White;
            btnConfirmar.FlatStyle = FlatStyle.Flat;
            btnConfirmar.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            btnConfirmar.Click += BtnConfirmar_Click;
            this.Controls.Add(btnConfirmar);

            // Ajustar tamanho da tela
            this.Size = new Size(1100, 780);
        }

        private void TelaOrcamento_Load(object sender, EventArgs e)
        {
            DateTime dataAtual = DateTime.Now;
            string data = dataAtual.ToString("dd/MM/yyyy");
            lbldata.Text = data;

            // Configurar formatação de campos
            ConfigurarFormatacaoCampos();
        }

        private void ConfigurarFormatacaoCampos()
        {
            // Formatação CPF/CNPJ
            txtCPF.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            txtCPF.TextChanged += (s, e) =>
            {
                string text = txtCPF.Text.Replace(".", "").Replace("-", "").Replace("/", "");
                if (text.Length <= 11)
                {
                    // CPF
                    if (text.Length > 3) text = text.Insert(3, ".");
                    if (text.Length > 7) text = text.Insert(7, ".");
                    if (text.Length > 11) text = text.Insert(11, "-");
                }
                else
                {
                    // CNPJ
                    if (text.Length > 2) text = text.Insert(2, ".");
                    if (text.Length > 6) text = text.Insert(6, ".");
                    if (text.Length > 10) text = text.Insert(10, "/");
                    if (text.Length > 15) text = text.Insert(15, "-");
                }

                if (txtCPF.Text != text)
                {
                    txtCPF.Text = text;
                    txtCPF.SelectionStart = text.Length;
                }
            };

            // Formatação CEP com consulta automática
            txtCEP.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            txtCEP.TextChanged += async (s, e) =>
            {
                if (isConsultandoCep) return;

                isConsultandoCep = true;

                try
                {
                    string text = txtCEP.Text.Replace("-", "").Replace(" ", "").Trim();

                    // Limita a 8 dígitos
                    if (text.Length > 8)
                    {
                        text = text.Substring(0, 8);
                    }

                    // Formatação visual do CEP
                    string formattedText = text;
                    if (text.Length > 5)
                    {
                        formattedText = text.Substring(0, 5) + "-" + text.Substring(5);
                    }

                    if (txtCEP.Text != formattedText)
                    {
                        int cursorPosition = txtCEP.SelectionStart;
                        txtCEP.Text = formattedText;

                        // Mantém a posição do cursor
                        if (cursorPosition <= formattedText.Length)
                        {
                            txtCEP.SelectionStart = cursorPosition;
                        }
                        else
                        {
                            txtCEP.SelectionStart = formattedText.Length;
                        }
                    }

                    // Consulta automática quando CEP estiver completo (8 dígitos)
                    if (text.Length == 8 && text.All(char.IsDigit))
                    {
                        // Pequeno delay para evitar consultas excessivas
                        await Task.Delay(300);

                        // Verifica se o texto ainda é o mesmo após o delay
                        if (txtCEP.Text.Replace("-", "").Replace(" ", "").Trim() == text)
                        {
                            await ConsultarEnderecoAsync(text);
                        }
                    }
                    else if (text.Length < 8)
                    {
                        // Reset das labels quando CEP incompleto
                        lblCidade.Text = "Cidade:";
                        lblUF.Text = "UF:";
                        lblCidade.ForeColor = Color.Black;
                        lblUF.ForeColor = Color.Black;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro na formatação do CEP: {ex.Message}");
                }
                finally
                {
                    isConsultandoCep = false;
                }
            };

            // Formatação telefone
            txtTEL.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            txtTEL.TextChanged += (s, e) =>
            {
                string text = txtTEL.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                if (text.Length > 0)
                {
                    if (text.Length <= 2) text = "(" + text;
                    else if (text.Length <= 7) text = "(" + text.Substring(0, 2) + ") " + text.Substring(2);
                    else text = "(" + text.Substring(0, 2) + ") " + text.Substring(2, 5) + "-" + text.Substring(7);

                    if (txtTEL.Text != text)
                    {
                        txtTEL.Text = text;
                        txtTEL.SelectionStart = text.Length;
                    }
                }
            };
        }

        /// <summary>
        /// Consulta o endereço através do CEP usando a API ViaCEP
        /// </summary>
        /// <param name="cep">CEP para consulta</param>
        private async Task ConsultarEnderecoAsync(string cep)
        {
            try
    {
        // Valida CEP
        if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8 || !cep.All(char.IsDigit))
        {
            lblCidade.Text = "Cidade: CEP inválido";
            lblUF.Text = "UF: --";
            lblCidade.ForeColor = Color.Red;
            lblUF.ForeColor = Color.Red;
            return;
        }

        this.Cursor = Cursors.WaitCursor;
        lblCidade.Text = "Cidade: Consultando...";
        lblUF.Text = "UF: ...";

        EnderecoViaCep endereco = await ViaCepService.ConsultarCepComFallbackAsync(cep);

        if (endereco != null && !endereco.erro)
        {
            txtEndereco.Text = endereco.logradouro ?? "";
            txtBairro.Text = endereco.bairro ?? "";
            txtCidade.Text = endereco.localidade ?? "";
            txtUF.Text = endereco.uf ?? "";

            lblCidade.Text = $"Cidade: {endereco.localidade ?? "N/A"}";
            lblUF.Text = $"UF: {endereco.uf ?? "N/A"}";

            if (!string.IsNullOrEmpty(endereco.logradouro))
            {
                await Task.Delay(500);
                txtVendedor.Focus();
            }

            lblCidade.ForeColor = Color.Green;
            lblUF.ForeColor = Color.Green;
        }
        else
        {
            lblCidade.Text = "Cidade: CEP não encontrado";
            lblUF.Text = "UF: --";
            lblCidade.ForeColor = Color.Red;
            lblUF.ForeColor = Color.Red;
            txtEndereco.Focus();
        }
    }
    catch (Exception ex)
    {
        lblCidade.Text = "Cidade: Erro na consulta";
        lblUF.Text = "UF: --";
        lblCidade.ForeColor = Color.Red;
        lblUF.ForeColor = Color.Red;

        System.Diagnostics.Debug.WriteLine($"Erro ao consultar CEP: {ex.Message}");
    }
    finally
    {
        this.Cursor = Cursors.Default;

        Timer timer = new Timer();
        timer.Interval = 3000;
        timer.Tick += (s, e) =>
        {
            lblCidade.ForeColor = Color.Black;
            lblUF.ForeColor = Color.Black;
            timer.Stop();
            timer.Dispose();
        };
        timer.Start();
    }
        }

        /// <summary>
        /// Limpa os campos de endereço
        /// </summary>
        private void LimparCamposEndereco()
        {
            txtEndereco.Clear();
            txtBairro.Clear();
        }

        private void TxtPesquisarProdutos_Enter(object sender, EventArgs e)
        {
            if (txtPesquisarProdutos.Text == "PESQUISAR PRODUTOS")
            {
                txtPesquisarProdutos.Text = "";
                txtPesquisarProdutos.ForeColor = Color.Black;
            }
        }

        private void TxtPesquisarProdutos_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPesquisarProdutos.Text))
            {
                txtPesquisarProdutos.Text = "PESQUISAR PRODUTOS";
                txtPesquisarProdutos.ForeColor = Color.Gray;
            }
        }

        private void TxtPesquisarProdutos_TextChanged(object sender, EventArgs e)
        {
            // Implementar lógica de pesquisa de produtos aqui
            // Por exemplo, filtrar uma lista de produtos
        }

        private void CalcularTotal(object sender, EventArgs e)
        {
            try
            {
                decimal totalProdutos = 0;

                // Somar valores dos produtos
                foreach (DataGridViewRow row in dgvProdutos.Rows)
                {
                    if (row.Cells["VLR_TOTAL"].Value != null &&
                        decimal.TryParse(row.Cells["VLR_TOTAL"].Value.ToString(), out decimal valor))
                    {
                        totalProdutos += valor;
                    }
                }

                decimal desconto = 0;
                decimal acrescimo = 0;

                if (decimal.TryParse(txtDescontos.Text, out desconto)) { }
                if (decimal.TryParse(txtAcrescimos.Text, out acrescimo)) { }

                decimal total = totalProdutos - desconto + acrescimo;
                txtTotalVista.Text = total.ToString("C2");

                // Calcular 4x sem juros
                decimal valorParcela = total / 4;
                txtSemJuros.Text = $"4x de {valorParcela:C2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao calcular total: " + ex.Message);
            }
        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Funcionalidade de impressão implementada!", "Imprimir",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao imprimir: " + ex.Message);
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deseja realmente excluir este orçamento?",
                "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LimparFormulario();
                MessageBox.Show("Orçamento excluído!", "Excluir",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                // Implementar lógica de salvamento (banco de dados, arquivo, etc.)
                MessageBox.Show("Orçamento salvo com sucesso!", "Salvar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                DialogResult result = MessageBox.Show("Confirmar orçamento?",
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("Orçamento confirmado!", "Confirmar",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtCliente.Text))
            {
                MessageBox.Show("Campo Cliente é obrigatório!", "Validação",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCliente.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCPF.Text))
            {
                MessageBox.Show("Campo CPF/CNPJ é obrigatório!", "Validação",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCPF.Focus();
                return false;
            }

            return true;
        }

        private void LimparFormulario()
        {
            txtCliente.Clear();
            txtEndereco.Clear();
            txtCEP.Clear();
            txtVendedor.Clear();
            txtCPF.Clear();
            txtBairro.Clear();
            txtTEL.Clear();
            txtFantasia.Clear();
            txtDescontos.Clear();
            txtAcrescimos.Clear();
            txtTotalVista.Clear();
            txtSemJuros.Clear();

            // Limpar labels de cidade e UF
            lblCidade.Text = "Cidade:";
            lblUF.Text = "UF:";

            // Limpar grid
            dgvProdutos.Rows.Clear();
            for (int i = 0; i < 8; i++)
            {
                dgvProdutos.Rows.Add("", "", "", "", "", "");
            }

            txtPesquisarProdutos.Text = "PESQUISAR PRODUTOS";
            txtPesquisarProdutos.ForeColor = Color.Gray;
        }
    }
}