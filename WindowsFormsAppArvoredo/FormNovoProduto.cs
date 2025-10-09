using System;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormNovoProduto : Form
    {
        public Produto ProdutoCriado { get; private set; }
        private bool _modoEdicao = false;

        // Construtor padrão → novo produto
        public FormNovoProduto()
        {
            InitializeComponent();
            _modoEdicao = false;
        }

        // Construtor para edição → recebe um Produto existente
        public FormNovoProduto(Produto produtoExistente) : this()
        {
            if (produtoExistente != null)
            {
                _modoEdicao = true;
                ProdutoCriado = produtoExistente;

                // Preenche controles
                txtNome.Text = produtoExistente.Descricao;
                txtTipo.Text = produtoExistente.Tipo ?? string.Empty;
                numQuantidade.Value = produtoExistente.Quantidade >= numQuantidade.Minimum && produtoExistente.Quantidade <= numQuantidade.Maximum
                                      ? produtoExistente.Quantidade : numQuantidade.Minimum;
                numQuantidadeMinima.Value = produtoExistente.QuantidadeMinima;
                txtUnidade.Text = produtoExistente.Unidade ?? string.Empty;
                numPreco.Value = produtoExistente.ValorUnitario;
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validações básicas
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Informe a descrição do produto.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtUnidade.Text))
            {
                MessageBox.Show("Informe a unidade do produto.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (numPreco.Value <= 0)
            {
                MessageBox.Show("Valor unitário deve ser maior que zero.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_modoEdicao && ProdutoCriado != null)
            {
                // Atualiza produto existente (referência)
                ProdutoCriado.Descricao = txtNome.Text.Trim();
                ProdutoCriado.Tipo = txtTipo.Text.Trim();
                ProdutoCriado.Quantidade = numQuantidade.Value;
                ProdutoCriado.QuantidadeMinima = (int)numQuantidadeMinima.Value;
                ProdutoCriado.Unidade = txtUnidade.Text.Trim();
                ProdutoCriado.ValorUnitario = numPreco.Value;
                ProdutoCriado.UltimaAtualizacao = DateTime.Now;
            }
            else
            {
                // Cria novo
                ProdutoCriado = new Produto
                {
                    Sequencia = 0, // será definido ao adicionar na lista
                    Descricao = txtNome.Text.Trim(),
                    Tipo = txtTipo.Text.Trim(),
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
    }
}