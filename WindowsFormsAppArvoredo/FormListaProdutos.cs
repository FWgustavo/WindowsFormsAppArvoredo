using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormListaProdutos : Form
    {
        private List<Produto> _produtos;

        public FormListaProdutos(List<Produto> produtos)
        {
            InitializeComponent();
            _produtos = produtos ?? new List<Produto>();
            CarregarProdutos();
        }

        private void CarregarProdutos()
        {
            dgvProdutos.Rows.Clear();

            foreach (var p in _produtos)
            {
                dgvProdutos.Rows.Add(
                    p.Sequencia,
                    p.Descricao,
                    p.Tipo,
                    p.Quantidade,
                    p.QuantidadeMinima,
                    p.Unidade,
                    p.ValorUnitario.ToString("C"),
                    p.UltimaAtualizacao.ToString("dd/MM/yyyy")
                );
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProdutos.CurrentRow == null)
            {
                MessageBox.Show("Selecione um produto para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvProdutos.CurrentRow.Cells[0].Value);
            var produto = _produtos.Find(x => x.Sequencia == id);

            if (produto != null)
            {
                using (var form = new FormNovoProduto(produto))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // Monta objeto para API
                        var produtoAPI = new ProdutoAPICreate
                        {
                            nome = produto.Descricao,
                            valor = (double)produto.ValorUnitario,
                            unidade = produto.Unidade,
                            quantidade = (int)produto.Quantidade,
                            quantidadeMin = produto.QuantidadeMinima,
                            ativo = true,
                            madeiraId = produto.MadeiraId,
                            tamanhoId = produto.TamanhoId
                        };

                        try
                        {
                            // PUT na API
                            await ApiClient.PutAsync<ProdutoAPICreate, ProdutoAPI>(
                                $"/produtos/{produto.Sequencia}",
                                produtoAPI
                            );

                            produto.UltimaAtualizacao = DateTime.Now;

                            CarregarProdutos();

                            MessageBox.Show(
                                "Produto atualizado com sucesso!",
                                "Sucesso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(
                                $"Erro ao atualizar produto na API:\n\n{ex.Message}",
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                }
            }
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvProdutos.CurrentRow == null)
            {
                MessageBox.Show("Selecione um produto para excluir.");
                return;
            }

            int id = Convert.ToInt32(dgvProdutos.CurrentRow.Cells[0].Value);
            var produto = _produtos.Find(x => x.Sequencia == id);

            if (produto == null) return;

            var confirm = MessageBox.Show(
                $"Deseja realmente excluir '{produto.Descricao}'?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    // DELETE na API
                    bool apagou = await ApiClient.DeleteAsync($"/produtos/{produto.Sequencia}");

                    if (!apagou)
                    {
                        MessageBox.Show("Erro ao excluir produto na API.");
                        return;
                    }

                    _produtos.Remove(produto);

                    // Reindexa
                    for (int i = 0; i < _produtos.Count; i++)
                        _produtos[i].Sequencia = i + 1;

                    CarregarProdutos();

                    MessageBox.Show(
                        "Produto excluído com sucesso!",
                        "Sucesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Erro ao excluir produto na API:\n\n{ex.Message}",
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
