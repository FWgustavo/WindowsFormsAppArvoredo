using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class TelaArvoredo : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
           int nLeft,
           int nTop,
           int nRight,
           int nBottom,
           int nWidthEllipse,
           int nHeightEllipse
        );

        public TelaArvoredo()
        {
            InitializeComponent();

            // Pintura de fundo (gradiente)
            this.Paint += new PaintEventHandler(Form1_Paint);

            // Configurações do painel de degrade (double-buffer)
            this.panelDegrade.BackColor = Color.Transparent;
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, panelDegrade, new object[] { true });
            this.panelDegrade.Paint += new PaintEventHandler(PanelDegrade_Paint);

            // Otimizações de rendering
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.Text = "Sistema Arvoredo";
        }

        #region Gradiente e pintura

        private void SetBackColorDegrade(object sender, PaintEventArgs e)
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

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            SetBackColorDegrade(sender, e);
        }

        #endregion

        #region Modelos de dados

        public class Orcamento
        {
            public int Id { get; set; }
            public string Cliente { get; set; }
            public DateTime DataCriacao { get; set; }
            public decimal Valor { get; set; }
            public string Status { get; set; }
        }

        public class ProdutoEstoque
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Tipo { get; set; } // Eucalipto, Peroba, ...
            public int QuantidadeDisponivel { get; set; }
            public int QuantidadeMinima { get; set; }
            public decimal PrecoUnitario { get; set; }
            public DateTime UltimaAtualizacao { get; set; }
            public string Unidade { get; set; } // m, unidade, etc.
            public bool EstoqueAbaixoMinimo => QuantidadeDisponivel <= QuantidadeMinima;
        }

        #endregion

        #region Dados em memória

        private List<Orcamento> orcamentos = new List<Orcamento>();
        private List<ProdutoEstoque> produtosEstoque = new List<ProdutoEstoque>();

        #endregion

        private void TelaArvoredo_Load(object sender, EventArgs e)
        {
            // Aplica arredondamento nos botões (UI já existe no Designer)
            try
            {
                btnEstoque.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnEstoque.Width, btnEstoque.Height, 50, 100));
                btnOrcamento.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnOrcamento.Width, btnOrcamento.Height, 50, 100));
                btnPedidos.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnPedidos.Width, btnPedidos.Height, 50, 100));
                btnTitulos.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnTitulos.Width, btnTitulos.Height, 50, 100));
                btnCadastro.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnCadastro.Width, btnCadastro.Height, 15, 15));
                btnCaixa.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnCaixa.Width, btnCaixa.Height, 15, 15));
                btnHistorico.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnHistorico.Width, btnHistorico.Height, 15, 15));
                btnSair.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSair.Width, btnSair.Height, 15, 15));
                btnNewOrc.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnNewOrc.Width, btnNewOrc.Height, 10, 10));
            }
            catch
            {
                // Se o designer chamou Load antes de algumas medidas estarem prontas, ignore (não crítico)
            }

            // Inicializa painéis e ListViews (somente lógica — visual está no Designer)
            panelOrcamento.Visible = true;   // Inicia mostrando orçamentos
            panelEstoque.Visible = false;    // Estoque oculto até o clique

            // Configuração e dados
            ConfigurarListViewOrcamentos();
            ConfigurarEstoque();

            // Eventos (se não estiverem ligados no Designer)
            btnOrcamento.Click -= btnOrcamento_Click;
            btnOrcamento.Click += btnOrcamento_Click;

            btnEstoque.Click -= btnEstoque_Click;
            btnEstoque.Click += btnEstoque_Click;

            btnNewOrc.Click -= btnNewOrc_Click;
            btnNewOrc.Click += btnNewOrc_Click;

            btnNovoProduto.Click -= btnNovoProduto_Click;
            btnNovoProduto.Click += btnNovoProduto_Click;

            btnAtualizarEstoque.Click -= btnAtualizarEstoque_Click;
            btnAtualizarEstoque.Click += btnAtualizarEstoque_Click;

            btnRelatorioEstoque.Click -= btnRelatorioEstoque_Click;
            btnRelatorioEstoque.Click += btnRelatorioEstoque_Click;

            listViewOrcamentos.MouseClick -= listViewOrcamentos_MouseClick;
            listViewOrcamentos.MouseClick += listViewOrcamentos_MouseClick;

            listViewEstoque.MouseClick -= listViewEstoque_MouseClick;
            listViewEstoque.MouseClick += listViewEstoque_MouseClick;

            // Força o redesenho do panelDegrade
            panelDegrade.Invalidate();
        }

        #region Orçamentos (já integrados ao Designer)

        private void ConfigurarListViewOrcamentos()
        {
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

            CarregarDadosExemplo();
        }

        private void CarregarDadosExemplo()
        {
            orcamentos.Clear();

            orcamentos.Add(new Orcamento { Id = 1, Cliente = "Nilda", DataCriacao = DateTime.Now.AddDays(-5), Valor = 1500.00m, Status = "Pendente" });
            orcamentos.Add(new Orcamento { Id = 2, Cliente = "Fernando", DataCriacao = DateTime.Now.AddDays(-3), Valor = 2300.00m, Status = "Pendente" });
            orcamentos.Add(new Orcamento { Id = 3, Cliente = "Bernardo", DataCriacao = DateTime.Now.AddDays(-1), Valor = 890.00m, Status = "Pendente" });
            orcamentos.Add(new Orcamento { Id = 4, Cliente = "Jana", DataCriacao = DateTime.Now, Valor = 1200.00m, Status = "Pendente" });

            AtualizarListViewOrcamentos();
        }

        private void AtualizarListViewOrcamentos()
        {
            listViewOrcamentos.Items.Clear();

            foreach (var orcamento in orcamentos)
            {
                var item = new ListViewItem($"Orçamento N° {orcamento.Id}");
                item.SubItems.Add($"Cliente: {orcamento.Cliente}");
                item.SubItems.Add(orcamento.DataCriacao.ToString("dd/MM/yyyy"));
                item.SubItems.Add(orcamento.Valor.ToString("C"));
                item.SubItems.Add(orcamento.Status);
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

            Color backgroundColor = e.ItemIndex % 2 == 0 ?
                Color.FromArgb(239, 212, 172) :
                Color.FromArgb(250, 230, 194);

            if (e.Item.Selected)
                backgroundColor = Color.FromArgb(198, 143, 86);

            using (SolidBrush brush = new SolidBrush(backgroundColor))
                e.Graphics.FillRectangle(brush, e.Bounds);

            using (Pen pen = new Pen(Color.FromArgb(57, 27, 1), 1))
                e.Graphics.DrawRectangle(pen, e.Bounds);
        }

        private void ListViewOrcamentos_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            Color textColor = Color.FromArgb(57, 27, 1);
            Font font = new Font("Gagalin", 9F, FontStyle.Regular);

            StringFormat format = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near
            };

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
            TelaOrcamento telaorcamento = new TelaOrcamento();
            telaorcamento.ShowDialog();

            var novoOrcamento = new Orcamento
            {
                Id = orcamentos.Count + 1,
                Cliente = "Novo Cliente",
                DataCriacao = DateTime.Now,
                Valor = 0.00m,
                Status = "Pendente"
            };

            orcamentos.Add(novoOrcamento);
            AtualizarListViewOrcamentos();

            MessageBox.Show("Novo orçamento criado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Estoque (lógica)

        private void ConfigurarEstoque()
        {
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

            CarregarDadosEstoqueExemplo();
        }

        private void CarregarDadosEstoqueExemplo()
        {
            produtosEstoque.Clear();

            produtosEstoque.Add(new ProdutoEstoque
            {
                Id = 1,
                Nome = "Tábua Eucalipto 2x10",
                Tipo = "Eucalipto",
                QuantidadeDisponivel = 45,
                QuantidadeMinima = 20,
                PrecoUnitario = 35.50m,
                UltimaAtualizacao = DateTime.Now.AddDays(-2),
                Unidade = "m"
            });

            produtosEstoque.Add(new ProdutoEstoque
            {
                Id = 2,
                Nome = "Viga Peroba 6x12",
                Tipo = "Peroba",
                QuantidadeDisponivel = 15,
                QuantidadeMinima = 25,
                PrecoUnitario = 125.00m,
                UltimaAtualizacao = DateTime.Now.AddDays(-1),
                Unidade = "m"
            });

            produtosEstoque.Add(new ProdutoEstoque
            {
                Id = 3,
                Nome = "Ripão Câmbara 5x7",
                Tipo = "Câmbara",
                QuantidadeDisponivel = 32,
                QuantidadeMinima = 15,
                PrecoUnitario = 28.75m,
                UltimaAtualizacao = DateTime.Now.AddDays(-3),
                Unidade = "m"
            });

            produtosEstoque.Add(new ProdutoEstoque
            {
                Id = 4,
                Nome = "Caibro Pinnus 5x6",
                Tipo = "Pinnus",
                QuantidadeDisponivel = 67,
                QuantidadeMinima = 30,
                PrecoUnitario = 18.90m,
                UltimaAtualizacao = DateTime.Now,
                Unidade = "m"
            });

            produtosEstoque.Add(new ProdutoEstoque
            {
                Id = 5,
                Nome = "Testeira 2x20",
                Tipo = "Testeira",
                QuantidadeDisponivel = 8,
                QuantidadeMinima = 12,
                PrecoUnitario = 42.30m,
                UltimaAtualizacao = DateTime.Now.AddDays(-4),
                Unidade = "m"
            });

            produtosEstoque.Add(new ProdutoEstoque
            {
                Id = 6,
                Nome = "Prancha Eucalipto 3x30",
                Tipo = "Eucalipto",
                QuantidadeDisponivel = 22,
                QuantidadeMinima = 10,
                PrecoUnitario = 65.80m,
                UltimaAtualizacao = DateTime.Now.AddDays(-1),
                Unidade = "m"
            });

            AtualizarListViewEstoque();
        }

        private void AtualizarListViewEstoque()
        {
            listViewEstoque.Items.Clear();

            foreach (var produto in produtosEstoque)
            {
                var item = new ListViewItem(produto.Nome);
                item.SubItems.Add(produto.Tipo);
                item.SubItems.Add(produto.QuantidadeDisponivel.ToString());
                item.SubItems.Add(produto.QuantidadeMinima.ToString());
                item.SubItems.Add(produto.Unidade);
                item.SubItems.Add(produto.PrecoUnitario.ToString("C"));
                item.SubItems.Add(produto.UltimaAtualizacao.ToString("dd/MM/yyyy"));

                string status = produto.EstoqueAbaixoMinimo ? "⚠️ BAIXO" : "✅ OK";
                item.SubItems.Add(status);

                item.SubItems.Add("✏️ 🗑️");
                item.Tag = produto;

                if (produto.EstoqueAbaixoMinimo)
                    item.BackColor = Color.FromArgb(255, 200, 200);

                listViewEstoque.Items.Add(item);
            }

            // Atualiza label de produtos com estoque baixo
            lblProdutosBaixoEstoque.Text = $"⚠️ Produtos com estoque baixo: {produtosEstoque.Count(p => p.EstoqueAbaixoMinimo)}";
        }

        private void ListViewEstoque_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void ListViewEstoque_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = false;

            Color backgroundColor = e.ItemIndex % 2 == 0 ?
                Color.FromArgb(239, 212, 172) :
                Color.FromArgb(250, 230, 194);

            ProdutoEstoque produto = (ProdutoEstoque)e.Item.Tag;
            if (produto != null && produto.EstoqueAbaixoMinimo)
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

            if (e.ColumnIndex == 7) // coluna Status
            {
                ProdutoEstoque produto = (ProdutoEstoque)e.Item.Tag;
                if (produto != null && produto.EstoqueAbaixoMinimo)
                    textColor = Color.Red;
                else
                    textColor = Color.Green;
            }

            Font font = new Font("Gagalin", 9F, FontStyle.Regular);

            StringFormat format = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near
            };

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
                if (hit.Item.SubItems.IndexOf(hit.SubItem) == listViewEstoque.Columns.Count - 1)
                {
                    ProdutoEstoque produto = (ProdutoEstoque)hit.Item.Tag;

                    ContextMenuStrip menu = new ContextMenuStrip();

                    ToolStripMenuItem editarItem = new ToolStripMenuItem("✏️ Editar Produto");
                    editarItem.Click += (s, args) => EditarProduto(produto);
                    menu.Items.Add(editarItem);

                    ToolStripMenuItem excluirItem = new ToolStripMenuItem("🗑️ Excluir Produto");
                    excluirItem.Click += (s, args) => ExcluirProduto(produto);
                    menu.Items.Add(excluirItem);

                    menu.Show(listViewEstoque, e.Location);
                }
            }
        }

        private void EditarProduto(ProdutoEstoque produto)
        {
            MessageBox.Show($"Editar produto: {produto.Nome}", "Editar Produto", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Aqui você pode abrir um form de edição real e, ao salvar, chamar AtualizarListViewEstoque()
        }

        private void ExcluirProduto(ProdutoEstoque produto)
        {
            var result = MessageBox.Show($"Tem certeza que deseja excluir o produto '{produto.Nome}'?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                produtosEstoque.Remove(produto);
                AtualizarListViewEstoque();
                MessageBox.Show("Produto excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnNovoProduto_Click(object sender, EventArgs e)
        {
            using (FormNovoProduto formNovo = new FormNovoProduto())
            {
                if (formNovo.ShowDialog() == DialogResult.OK)
                {
                    // Produto é um tipo definido no seu FormNovoProduto (você já enviou esse form)
                    Produto novoProduto = formNovo.ProdutoCriado;

                    produtosEstoque.Add(new ProdutoEstoque
                    {
                        Id = produtosEstoque.Count + 1,
                        Nome = novoProduto.Descricao,
                        Tipo = "Novo",
                        QuantidadeDisponivel = (int)novoProduto.Quantidade,
                        QuantidadeMinima = 5,
                        PrecoUnitario = novoProduto.ValorUnitario,
                        UltimaAtualizacao = DateTime.Now,
                        Unidade = novoProduto.Unidade
                    });

                    AtualizarListViewEstoque();
                }
            }
        }

        private void btnAtualizarEstoque_Click(object sender, EventArgs e)
        {
            using (FormListaProdutos formLista = new FormListaProdutos(produtosEstoque))
    {
        formLista.ShowDialog();
        AtualizarListViewEstoque(); // recarrega a ListView depois de fechar
    }
        }

        private void btnRelatorioEstoque_Click(object sender, EventArgs e)
        {
            int produtosBaixoEstoque = produtosEstoque.Count(p => p.EstoqueAbaixoMinimo);
            decimal valorTotalEstoque = produtosEstoque.Sum(p => p.QuantidadeDisponivel * p.PrecoUnitario);

            string relatorio = $"RELATÓRIO DE ESTOQUE\n\n" +
                              $"Total de produtos: {produtosEstoque.Count}\n" +
                              $"Produtos com estoque baixo: {produtosBaixoEstoque}\n" +
                              $"Valor total do estoque: {valorTotalEstoque:C}\n\n" +
                              "PRODUTOS COM ESTOQUE BAIXO:\n";

            foreach (var produto in produtosEstoque.Where(p => p.EstoqueAbaixoMinimo))
            {
                relatorio += $"• {produto.Nome} - Disponível: {produto.QuantidadeDisponivel} {produto.Unidade} (Mín: {produto.QuantidadeMinima})\n";
            }

            using (FormRelatorio formRel = new FormRelatorio(relatorio, "Relatório de Estoque"))
            {
                formRel.ShowDialog();
            }
        }

        #endregion

        #region Navegação e utilitários

        private void btnOrcamento_Click(object sender, EventArgs e)
        {
            panelEstoque.Visible = false;
            panelOrcamento.Visible = true;
            panelOrcamento.BringToFront();

            ResetarCoresBotoes();
            btnOrcamento.BackColor = Color.FromArgb(206, 186, 157);
        }

        private void btnEstoque_Click(object sender, EventArgs e)
        {
            panelOrcamento.Visible = false;
            panelEstoque.Visible = true;
            panelEstoque.BringToFront();

            ResetarCoresBotoes();
            btnEstoque.BackColor = Color.FromArgb(206, 186, 157);

            AtualizarListViewEstoque();
        }

        private void ResetarCoresBotoes()
        {
            Color corPadrao = Color.FromArgb(239, 212, 172);

            try
            {
                btnTitulos.BackColor = corPadrao;
                btnPedidos.BackColor = corPadrao;
                btnOrcamento.BackColor = corPadrao;
                btnEstoque.BackColor = corPadrao;
            }
            catch
            {
                // Caso algum botão não exista por alguma alteração futura no Designer
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
