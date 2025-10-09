using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class TelaArvoredo : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
           int nLeft, int nTop, int nRight, int nBottom,
           int nWidthEllipse, int nHeightEllipse);

        private List<Orcamento> orcamentos = new List<Orcamento>();
        private List<Produto> produtos = new List<Produto>();

        public TelaArvoredo()
        {
            InitializeComponent();

            this.Paint += Form1_Paint;

            if (panelDegrade != null)
            {
                panelDegrade.BackColor = Color.Transparent;
                typeof(Panel).InvokeMember("DoubleBuffered",
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, panelDegrade, new object[] { true });
                panelDegrade.Paint += PanelDegrade_Paint;
            }

            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.Text = "Sistema Arvoredo";
        }

        private void TelaArvoredo_Load(object sender, EventArgs e)
        {
            AplicarArredondamentoBotoes();

            if (panelOrcamento != null) panelOrcamento.Visible = true;
            if (panelEstoque != null) panelEstoque.Visible = false;

            ConfigurarListViewOrcamentos();
            ConfigurarEstoque();
            CarregarDadosExemplo();

            VincularEventos();
            panelDegrade?.Invalidate();
        }

        private void AplicarArredondamentoBotoes()
        {
            try
            {
                if (btnEstoque != null)
                    btnEstoque.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnEstoque.Width, btnEstoque.Height, 50, 100));
                if (btnOrcamento != null)
                    btnOrcamento.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnOrcamento.Width, btnOrcamento.Height, 50, 100));
                if (btnPedidos != null)
                    btnPedidos.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnPedidos.Width, btnPedidos.Height, 50, 100));
                if (btnTitulos != null)
                    btnTitulos.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnTitulos.Width, btnTitulos.Height, 50, 100));
                if (btnCadastro != null)
                    btnCadastro.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnCadastro.Width, btnCadastro.Height, 15, 15));
                if (btnCaixa != null)
                    btnCaixa.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnCaixa.Width, btnCaixa.Height, 15, 15));
                if (btnHistorico != null)
                    btnHistorico.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnHistorico.Width, btnHistorico.Height, 15, 15));
                if (btnSair != null)
                    btnSair.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSair.Width, btnSair.Height, 15, 15));
                if (btnNewOrc != null)
                    btnNewOrc.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnNewOrc.Width, btnNewOrc.Height, 10, 10));
            }
            catch { }
        }

        private void VincularEventos()
        {
            if (btnOrcamento != null)
            {
                btnOrcamento.Click -= btnOrcamento_Click;
                btnOrcamento.Click += btnOrcamento_Click;
            }
            if (btnEstoque != null)
            {
                btnEstoque.Click -= btnEstoque_Click;
                btnEstoque.Click += btnEstoque_Click;
            }
            if (btnNewOrc != null)
            {
                btnNewOrc.Click -= btnNewOrc_Click;
                btnNewOrc.Click += btnNewOrc_Click;
            }
            if (btnNovoProduto != null)
            {
                btnNovoProduto.Click -= btnNovoProduto_Click;
                btnNovoProduto.Click += btnNovoProduto_Click;
            }
            if (btnAtualizarEstoque != null)
            {
                btnAtualizarEstoque.Click -= btnAtualizarEstoque_Click;
                btnAtualizarEstoque.Click += btnAtualizarEstoque_Click;
            }
            if (btnRelatorioEstoque != null)
            {
                btnRelatorioEstoque.Click -= btnRelatorioEstoque_Click;
                btnRelatorioEstoque.Click += btnRelatorioEstoque_Click;
            }
            if (listViewOrcamentos != null)
            {
                listViewOrcamentos.MouseClick -= listViewOrcamentos_MouseClick;
                listViewOrcamentos.MouseClick += listViewOrcamentos_MouseClick;
            }
            if (listViewEstoque != null)
            {
                listViewEstoque.MouseClick -= listViewEstoque_MouseClick;
                listViewEstoque.MouseClick += listViewEstoque_MouseClick;
            }
        }

        #region Gradiente e Pintura

        private void Form1_Paint(object sender, PaintEventArgs e)
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

        private void PanelDegrade_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null) return;

            Graphics graphics = e.Graphics;
            Rectangle gradient_rect = new Rectangle(0, 0, panel.Width, panel.Height);
            graphics.Clear(Color.Transparent);

            using (LinearGradientBrush br = new LinearGradientBrush(
                gradient_rect,
                Color.FromArgb(0xb4, 0x7b, 0x39),
                Color.FromArgb(0xc6, 0x8f, 0x56),
                LinearGradientMode.Vertical))
            {
                graphics.FillRectangle(br, gradient_rect);
            }
        }

        #endregion

        #region Orçamentos

        private void ConfigurarListViewOrcamentos()
        {
            if (listViewOrcamentos == null) return;

            listViewOrcamentos.Columns.Clear();
            listViewOrcamentos.Columns.Add("Orçamento", 120);
            listViewOrcamentos.Columns.Add("Cliente", 200);
            listViewOrcamentos.Columns.Add("Data", 100);
            listViewOrcamentos.Columns.Add("Valor", 100);
            listViewOrcamentos.Columns.Add("Status", 100);
            listViewOrcamentos.Columns.Add("Ações", 100);

            listViewOrcamentos.OwnerDraw = true;
            listViewOrcamentos.DrawItem += ListViewOrcamentos_DrawItem;
            listViewOrcamentos.DrawSubItem += ListViewOrcamentos_DrawSubItem;
            listViewOrcamentos.DrawColumnHeader += ListViewOrcamentos_DrawColumnHeader;
        }

        private void CarregarDadosExemplo()
        {
            produtos.Clear();
            produtos.Add(new Produto { Sequencia = 1, Descricao = "Tábua Eucalipto 2x10", Tipo = "Eucalipto", Quantidade = 45, QuantidadeMinima = 20, ValorUnitario = 35.50m, UltimaAtualizacao = DateTime.Now.AddDays(-2), Unidade = "m" });
            produtos.Add(new Produto { Sequencia = 2, Descricao = "Viga Peroba 6x12", Tipo = "Peroba", Quantidade = 15, QuantidadeMinima = 25, ValorUnitario = 125.00m, UltimaAtualizacao = DateTime.Now.AddDays(-1), Unidade = "m" });
            produtos.Add(new Produto { Sequencia = 3, Descricao = "Ripão Câmbara 5x7", Tipo = "Câmbara", Quantidade = 32, QuantidadeMinima = 15, ValorUnitario = 28.75m, UltimaAtualizacao = DateTime.Now.AddDays(-3), Unidade = "m" });
            produtos.Add(new Produto { Sequencia = 4, Descricao = "Caibro Pinnus 5x6", Tipo = "Pinnus", Quantidade = 67, QuantidadeMinima = 30, ValorUnitario = 18.90m, UltimaAtualizacao = DateTime.Now, Unidade = "m" });
            produtos.Add(new Produto { Sequencia = 5, Descricao = "Testeira 2x20", Tipo = "Testeira", Quantidade = 8, QuantidadeMinima = 12, ValorUnitario = 42.30m, UltimaAtualizacao = DateTime.Now.AddDays(-4), Unidade = "m" });
            produtos.Add(new Produto { Sequencia = 6, Descricao = "Prancha Eucalipto 3x30", Tipo = "Eucalipto", Quantidade = 22, QuantidadeMinima = 10, ValorUnitario = 65.80m, UltimaAtualizacao = DateTime.Now.AddDays(-1), Unidade = "m" });

            AtualizarListaEstoque();
        }

        private void AtualizarListViewOrcamentos()
        {
            if (listViewOrcamentos == null) return;
            listViewOrcamentos.Items.Clear();

            foreach (var orcamento in orcamentos)
            {
                var item = new ListViewItem($"Orçamento N° {orcamento.Id}");
                item.SubItems.Add($"{orcamento.Cliente}");
                item.SubItems.Add(orcamento.DataEmissao.ToString("dd/MM/yyyy"));
                item.SubItems.Add(orcamento.TotalGeral.ToString("C"));
                item.SubItems.Add("Pendente");
                item.SubItems.Add("🗑️");
                item.Tag = orcamento;
                listViewOrcamentos.Items.Add(item);
            }
        }

        private void ListViewOrcamentos_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void ListViewOrcamentos_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = false;
            Color backgroundColor = e.ItemIndex % 2 == 0 ? Color.FromArgb(239, 212, 172) : Color.FromArgb(250, 230, 194);
            if (e.Item.Selected) backgroundColor = Color.FromArgb(198, 143, 86);

            using (SolidBrush brush = new SolidBrush(backgroundColor))
                e.Graphics.FillRectangle(brush, e.Bounds);
            using (Pen pen = new Pen(Color.FromArgb(57, 27, 1), 1))
                e.Graphics.DrawRectangle(pen, e.Bounds);
        }

        private void ListViewOrcamentos_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            Color textColor = Color.FromArgb(57, 27, 1);
            Font font = new Font("Gagalin", 9F, FontStyle.Regular);
            StringFormat format = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near };

            using (SolidBrush brush = new SolidBrush(textColor))
                e.Graphics.DrawString(e.SubItem.Text, font, brush, e.Bounds, format);
            using (Pen pen = new Pen(Color.FromArgb(57, 27, 1), 1))
                e.Graphics.DrawRectangle(pen, e.Bounds);
        }

        private void listViewOrcamentos_MouseClick(object sender, MouseEventArgs e)
        {
            var hit = listViewOrcamentos.HitTest(e.Location);
            if (hit.Item != null && hit.SubItem != null)
            {
                if (hit.Item.SubItems.IndexOf(hit.SubItem) == listViewOrcamentos.Columns.Count - 1)
                {
                    var result = MessageBox.Show("Tem certeza que deseja excluir este orçamento?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        Orcamento toRemove = (Orcamento)hit.Item.Tag;
                        orcamentos.Remove(toRemove);
                        AtualizarListViewOrcamentos();
                        MessageBox.Show("Orçamento excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnNewOrc_Click(object sender, EventArgs e)
        {
            var novoOrcamento = new Orcamento
            {
                Id = orcamentos.Count + 1,
                Cliente = "Novo Cliente",
                DataEmissao = DateTime.Now
            };

            orcamentos.Add(novoOrcamento);
            AtualizarListViewOrcamentos();
            MessageBox.Show("Novo orçamento criado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Estoque

        private void ConfigurarEstoque()
        {
            if (listViewEstoque == null) return;

            listViewEstoque.Columns.Clear();
            listViewEstoque.Columns.Add("Produto", 150);
            listViewEstoque.Columns.Add("Tipo", 120);
            listViewEstoque.Columns.Add("Qtd Disponível", 100);
            listViewEstoque.Columns.Add("Qtd Mínima", 100);
            listViewEstoque.Columns.Add("Unidade", 80);
            listViewEstoque.Columns.Add("Preço Unit.", 100);
            listViewEstoque.Columns.Add("Última Atualização", 120);
            listViewEstoque.Columns.Add("Status", 100);
            listViewEstoque.Columns.Add("Ações", 80);

            listViewEstoque.OwnerDraw = true;
            listViewEstoque.DrawItem += ListViewEstoque_DrawItem;
            listViewEstoque.DrawSubItem += ListViewEstoque_DrawSubItem;
            listViewEstoque.DrawColumnHeader += ListViewEstoque_DrawColumnHeader;
        }

        private void AtualizarListaEstoque()
        {
            if (listViewEstoque == null) return;
            listViewEstoque.Items.Clear();
            int countBaixo = 0;

            foreach (var p in produtos)
            {
                var item = new ListViewItem(p.Descricao ?? "Produto sem nome");
                item.SubItems.Add(p.Tipo);
                item.SubItems.Add(p.Quantidade.ToString());
                item.SubItems.Add(p.QuantidadeMinima.ToString());
                item.SubItems.Add(p.Unidade);
                item.SubItems.Add(p.ValorUnitario.ToString("C"));
                item.SubItems.Add(p.UltimaAtualizacao.ToString("dd/MM/yyyy"));

                string status = p.Quantidade <= p.QuantidadeMinima ? "⚠️ BAIXO" : "✅ OK";
                item.SubItems.Add(status);
                item.SubItems.Add("✏️ 🗑️");
                item.Tag = p;

                if (p.Quantidade <= p.QuantidadeMinima)
                {
                    item.BackColor = Color.FromArgb(255, 220, 220);
                    countBaixo++;
                }

                listViewEstoque.Items.Add(item);
            }

            if (lblProdutosBaixoEstoque != null)
                lblProdutosBaixoEstoque.Text = $"⚠️ Produtos com estoque baixo: {countBaixo}";
        }

        private void ListViewEstoque_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void ListViewEstoque_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = false;
            Color backgroundColor = e.ItemIndex % 2 == 0 ? Color.FromArgb(239, 212, 172) : Color.FromArgb(250, 230, 194);

            var produto = e.Item.Tag as Produto;
            if (produto != null && produto.Quantidade <= produto.QuantidadeMinima)
                backgroundColor = Color.FromArgb(255, 220, 220);

            if (e.Item.Selected)
                backgroundColor = Color.FromArgb(198, 143, 86);

            using (SolidBrush brush = new SolidBrush(backgroundColor))
                e.Graphics.FillRectangle(brush, e.Bounds);
            using (Pen pen = new Pen(Color.FromArgb(57, 27, 1), 1))
                e.Graphics.DrawRectangle(pen, e.Bounds);
        }

        private void ListViewEstoque_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            Color textColor = Color.FromArgb(57, 27, 1);

            if (e.ColumnIndex == 7)
            {
                var produto = e.Item.Tag as Produto;
                if (produto != null && produto.Quantidade <= produto.QuantidadeMinima)
                    textColor = Color.Red;
                else
                    textColor = Color.Green;
            }

            Font font = new Font("Gagalin", 9F, FontStyle.Regular);
            StringFormat format = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near };

            using (SolidBrush brush = new SolidBrush(textColor))
                e.Graphics.DrawString(e.SubItem.Text, font, brush, e.Bounds, format);
            using (Pen pen = new Pen(Color.FromArgb(57, 27, 1), 1))
                e.Graphics.DrawRectangle(pen, e.Bounds);
        }

        private void listViewEstoque_MouseClick(object sender, MouseEventArgs e)
        {
            var hit = listViewEstoque.HitTest(e.Location);
            if (hit.Item != null && hit.SubItem != null)
            {
                int idxAcoes = listViewEstoque.Columns.Count - 1;
                if (hit.Item.SubItems.IndexOf(hit.SubItem) == idxAcoes)
                {
                    var produto = hit.Item.Tag as Produto;
                    ContextMenuStrip menu = new ContextMenuStrip();

                    ToolStripMenuItem editarItem = new ToolStripMenuItem("✏️ Editar Produto");
                    editarItem.Click += (s, ev) => EditarProduto(produto);
                    menu.Items.Add(editarItem);

                    ToolStripMenuItem excluirItem = new ToolStripMenuItem("🗑️ Excluir Produto");
                    excluirItem.Click += (s, ev) => ExcluirProduto(produto);
                    menu.Items.Add(excluirItem);

                    menu.Show(listViewEstoque, e.Location);
                }
            }
        }

        private void EditarProduto(Produto produto)
        {
            if (produto == null) return;

            using (var form = new FormNovoProduto(produto))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    AtualizarListaEstoque();
                    MessageBox.Show("Produto atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ExcluirProduto(Produto produto)
        {
            if (produto == null) return;

            var result = MessageBox.Show($"Tem certeza que deseja excluir o produto '{produto.Descricao}'?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                produtos.Remove(produto);
                for (int i = 0; i < produtos.Count; i++)
                    produtos[i].Sequencia = i + 1;

                AtualizarListaEstoque();
                MessageBox.Show("Produto excluído.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnNovoProduto_Click(object sender, EventArgs e)
        {
            using (var form = new FormNovoProduto())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var novo = form.ProdutoCriado;
                    novo.Sequencia = produtos.Count + 1;
                    novo.UltimaAtualizacao = DateTime.Now;
                    produtos.Add(novo);
                    AtualizarListaEstoque();
                }
            }
        }

        private void btnAtualizarEstoque_Click(object sender, EventArgs e)
        {
            using (FormListaProdutos formLista = new FormListaProdutos(produtos))
            {
                formLista.ShowDialog();
                AtualizarListaEstoque();
            }
        }

        private void btnRelatorioEstoque_Click(object sender, EventArgs e)
        {
            int produtosBaixoEstoque = produtos.Count(p => p.Quantidade <= p.QuantidadeMinima);
            decimal valorTotalEstoque = produtos.Sum(p => p.Quantidade * p.ValorUnitario);

            string relatorio = $"RELATÓRIO DE ESTOQUE\n\n" +
                              $"Total de produtos: {produtos.Count}\n" +
                              $"Produtos com estoque baixo: {produtosBaixoEstoque}\n" +
                              $"Valor total do estoque: {valorTotalEstoque:C}\n\n" +
                              "PRODUTOS COM ESTOQUE BAIXO:\n";

            foreach (var produto in produtos.Where(p => p.Quantidade <= p.QuantidadeMinima))
            {
                relatorio += $"• {produto.Descricao} - Disponível: {produto.Quantidade} {produto.Unidade} (Mín: {produto.QuantidadeMinima})\n";
            }

            using (FormRelatorio formRel = new FormRelatorio(relatorio, "Relatório de Estoque"))
            {
                formRel.ShowDialog();
            }
        }

        #endregion

        #region Navegação

        private void btnOrcamento_Click(object sender, EventArgs e)
        {
            if (panelEstoque != null) panelEstoque.Visible = false;
            if (panelOrcamento != null)
            {
                panelOrcamento.Visible = true;
                panelOrcamento.BringToFront();
            }
            ResetarCoresBotoes();
            if (btnOrcamento != null) btnOrcamento.BackColor = Color.FromArgb(206, 186, 157);
        }

        private void btnEstoque_Click(object sender, EventArgs e)
        {
            if (panelOrcamento != null) panelOrcamento.Visible = false;
            if (panelEstoque != null)
            {
                panelEstoque.Visible = true;
                panelEstoque.BringToFront();
            }
            ResetarCoresBotoes();
            if (btnEstoque != null) btnEstoque.BackColor = Color.FromArgb(206, 186, 157);
            AtualizarListaEstoque();
        }

        private void ResetarCoresBotoes()
        {
            Color corPadrao = Color.FromArgb(239, 212, 172);
            if (btnTitulos != null) btnTitulos.BackColor = corPadrao;
            if (btnPedidos != null) btnPedidos.BackColor = corPadrao;
            if (btnOrcamento != null) btnOrcamento.BackColor = corPadrao;
            if (btnEstoque != null) btnEstoque.BackColor = corPadrao;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}