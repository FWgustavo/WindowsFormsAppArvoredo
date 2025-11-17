using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormNovoProduto : Form
    {
        public Produto ProdutoCriado { get; private set; }
        private bool _modoEdicao = false;

        // Listas auxiliares para ComboBoxes
        private List<MadeiraAPI> _madeiras;
        private List<TamanhoAPI> _tamanhos;

        public class MadeiraAPI
        {
            public int id { get; set; }
            public string nome { get; set; }
            public bool ativo { get; set; }
        }

        public class TamanhoAPI
        {
            public int id { get; set; }
            public string nome { get; set; }
            public bool ativo { get; set; }
        }
        public FormNovoProduto()
        {
            InitializeComponent();
            _modoEdicao = false;
            this.Text = "Novo Produto";
        }

        public FormNovoProduto(Produto produtoExistente) : this()
        {
            if (produtoExistente != null)
            {
                _modoEdicao = true;
                ProdutoCriado = produtoExistente;
                this.Text = "Editar Produto";

                txtNome.Text = produtoExistente.Descricao;

                decimal qtd = produtoExistente.Quantidade;
                if (qtd >= numQuantidade.Minimum && qtd <= numQuantidade.Maximum)
                    numQuantidade.Value = qtd;
                else
                    numQuantidade.Value = numQuantidade.Minimum;

                numQuantidadeMinima.Value = produtoExistente.QuantidadeMinima;
                txtUnidade.Text = produtoExistente.Unidade ?? string.Empty;
                numPreco.Value = produtoExistente.ValorUnitario;

                
            }
        }


        // ========================================
        // 🔹 CARREGAR DADOS AUXILIARES (OPCIONAL)
        // ========================================
        // Use este método se quiser preencher ComboBoxes com Fornecedores, Madeiras e Tamanhos


        private async Task CarregarDadosAuxiliares()
        {
            try
            {
               
                // 🔹 Busca madeiras ativas
                _madeiras = (await ApiClient.GetAsync<MadeiraAPI[]>("/madeiras?ativo=true"))?.ToList() ?? new List<MadeiraAPI>();

                // 🔹 Busca tamanhos ativos
                _tamanhos = (await ApiClient.GetAsync<TamanhoAPI[]>("/tamanhos?ativo=true"))?.ToList() ?? new List<TamanhoAPI>();

                cmbMadeira.DataSource = _madeiras;
                cmbMadeira.DisplayMember = "nome";
                cmbMadeira.ValueMember = "id";
                cmbMadeira.SelectedIndex = -1;

                cmbTamanho.DataSource = _tamanhos;
                cmbTamanho.DisplayMember = "nome";
                cmbTamanho.ValueMember = "id";
                cmbTamanho.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erro ao carregar dados auxiliares: {ex.Message}");
            }
        }


        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Informe a descrição do produto.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUnidade.Text))
            {
                MessageBox.Show("Informe a unidade do produto.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnidade.Focus();
                return;
            }

            if (numPreco.Value <= 0)
            {
                MessageBox.Show("Valor unitário deve ser maior que zero.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numPreco.Focus();
                return;
            }

            if (_modoEdicao && ProdutoCriado != null)
            {
                ProdutoCriado.Descricao = txtNome.Text.Trim();
                ProdutoCriado.Quantidade = numQuantidade.Value;
                ProdutoCriado.QuantidadeMinima = (int)numQuantidadeMinima.Value;
                ProdutoCriado.Unidade = txtUnidade.Text.Trim();
                ProdutoCriado.ValorUnitario = numPreco.Value;
                ProdutoCriado.UltimaAtualizacao = DateTime.Now;
                // ✅ Verifica se a quantidade está abaixo do mínimo
                ProdutoCriado.Acabando = numQuantidade.Value <= numQuantidadeMinima.Value;

                // 🔹 CAMPOS OPCIONAIS (se estiver usando ComboBoxes)
                ProdutoCriado.MadeiraId = cmbMadeira.SelectedValue as int?;
                ProdutoCriado.TamanhoId = cmbTamanho.SelectedValue as int?;
            }
            else
            {
                ProdutoCriado = new Produto
                {
                    Sequencia = 0,
                    Descricao = txtNome.Text.Trim(),
                   // Tipo = txtTipo.Text.Trim(),
                    Quantidade = numQuantidade.Value,
                    QuantidadeMinima = (int)numQuantidadeMinima.Value,
                    Unidade = txtUnidade.Text.Trim(),
                    ValorUnitario = numPreco.Value,
                    UltimaAtualizacao = DateTime.Now
                };
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void FormNovoProduto_Load(object sender, EventArgs e)
        {
            // 🔹 CARREGA DADOS AUXILIARES DA API (OPCIONAL)
            // Descomente se quiser usar ComboBoxes para Fornecedor, Madeira e Tamanho
            await CarregarDadosAuxiliares();
        }
    }
}