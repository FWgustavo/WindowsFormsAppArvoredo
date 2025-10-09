using System;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormNovoProduto : Form
    {
        public Produto ProdutoCriado { get; private set; }
        private bool _modoEdicao = false;

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
                txtTipo.Text = produtoExistente.Tipo ?? string.Empty;

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
                ProdutoCriado.Tipo = txtTipo.Text.Trim();
                ProdutoCriado.Quantidade = numQuantidade.Value;
                ProdutoCriado.QuantidadeMinima = (int)numQuantidadeMinima.Value;
                ProdutoCriado.Unidade = txtUnidade.Text.Trim();
                ProdutoCriado.ValorUnitario = numPreco.Value;
                ProdutoCriado.UltimaAtualizacao = DateTime.Now;
            }
            else
            {
                ProdutoCriado = new Produto
                {
                    Sequencia = 0,
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