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


            this.Paint += new PaintEventHandler(Form1_Paint);

            // Configura o panelDegrade para aceitar o degradê
            this.panelDegrade.BackColor = Color.Transparent;

            // Habilita double buffering para o panel
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, panelDegrade, new object[] { true });

            this.panelDegrade.Paint += new PaintEventHandler(PanelDegrade_Paint);


            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);




            this.Text = "Sistema Arvoredo";
        }

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
            Graphics graphics = e.Graphics;
            Rectangle gradient_rect = new Rectangle(0, 0, panel.Width, panel.Height);

            // Limpa o fundo primeiro
            graphics.Clear(Color.Transparent);

            using (LinearGradientBrush br = new LinearGradientBrush(
                gradient_rect,
                Color.FromArgb(0xb4, 0x7b, 0x39), // #b47b39
                Color.FromArgb(0xc6, 0x8f, 0x56), // #c68f56
                LinearGradientMode.Vertical)) // 180 graus (vertical)
            {
                graphics.FillRectangle(br, gradient_rect);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            SetBackColorDegrade(sender, e);
        }

        private void TelaArvoredo_Load(object sender, EventArgs e)
        {
            // Configura o estilo dos botões
            btnEstoque.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnEstoque.Width, btnEstoque.Height, 50, 100));
            btnOrcamento.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnOrcamento.Width, btnOrcamento.Height, 50, 100));
            btnPedidos.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnPedidos.Width, btnPedidos.Height, 50, 100));
            btnTitulos.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnTitulos.Width, btnTitulos.Height, 50, 100));
            btnCadastro.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnCadastro.Width, btnCadastro.Height, 15, 15));
            btnCaixa.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnCaixa.Width, btnCadastro.Height, 15, 15));
            btnHistorico.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnHistorico.Width, btnCadastro.Height, 15, 15));
            btnSair.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSair.Width, btnCadastro.Height, 15, 15));
            btnNewOrc.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnNewOrc.Width, btnNewOrc.Height, 10, 10));
            //
            panelOrcamento.Visible = false;
            btnOrcamento.Click += btnOrcamento_Click;
            ConfigurarListViewOrcamentos();
            listViewOrcamentos.MouseClick += listViewOrcamentos_MouseClick;
            //
            btnEstoque.Click += btnEstoque_Click;
            ConfigurarEstoque();
            listViewEstoque.MouseClick += listViewEstoque_MouseClick;
            panelEstoque.Visible = false;

            // Força o redesenho do panelDegrade
            panelDegrade.Invalidate();
        }

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
            public string Tipo { get; set; } // Eucalipto, Peroba, Câmbara, Pinnus, Testeira
            public int QuantidadeDisponivel { get; set; }
            public int QuantidadeMinima { get; set; }
            public decimal PrecoUnitario { get; set; }
            public DateTime UltimaAtualizacao { get; set; }
            public string Unidade { get; set; } // m³, m², unidade, etc.
            public bool EstoqueAbaixoMinimo => QuantidadeDisponivel <= QuantidadeMinima;
        }

            private List<Orcamento> orcamentos = new List<Orcamento>();
            private List<ProdutoEstoque> produtosEstoque = new List<ProdutoEstoque>();

        // Método para configurar a ListView e carregar dados de exemplo
        private void ConfigurarListViewOrcamentos()
        {
            // Configurar colunas da ListView
            listViewOrcamentos.Columns.Clear();
            listViewOrcamentos.Columns.Add("Orçamento", 120);
            listViewOrcamentos.Columns.Add("Cliente", 200);
            listViewOrcamentos.Columns.Add("Data", 100);
            listViewOrcamentos.Columns.Add("Valor", 100);
            listViewOrcamentos.Columns.Add("Status", 100);
            listViewOrcamentos.Columns.Add("Ações", 100);

            // Configurar aparência personalizada
            listViewOrcamentos.OwnerDraw = true;
            listViewOrcamentos.DrawItem += ListViewOrcamentos_DrawItem;
            listViewOrcamentos.DrawSubItem += ListViewOrcamentos_DrawSubItem;
            listViewOrcamentos.DrawColumnHeader += ListViewOrcamentos_DrawColumnHeader;

            // Carregar dados de exemplo
            CarregarDadosExemplo();
        }

        private void CarregarDadosExemplo()
        {
            orcamentos.Clear();

            // Adicionar orçamentos de exemplo
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
                item.SubItems.Add("🗑️"); // Ícone de lixeira
                item.Tag = orcamento; // Armazenar o objeto completo

                listViewOrcamentos.Items.Add(item);
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

        // Personalizar o desenho dos cabeçalhos das colunas
        private void ListViewOrcamentos_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        // Personalizar o desenho dos itens
        private void ListViewOrcamentos_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = false;

            // Cor de fundo alternada
            Color backgroundColor = e.ItemIndex % 2 == 0 ?
                Color.FromArgb(239, 212, 172) :
                Color.FromArgb(250, 230, 194);

            // Destacar item selecionado
            if (e.Item.Selected)
            {
                backgroundColor = Color.FromArgb(198, 143, 86);
            }

            using (SolidBrush brush = new SolidBrush(backgroundColor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            // Desenhar borda
            using (Pen pen = new Pen(Color.FromArgb(57, 27, 1), 1))
            {
                e.Graphics.DrawRectangle(pen, e.Bounds);
            }
        }

        // Personalizar o desenho dos subitens
        private void ListViewOrcamentos_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // Cor do texto
            Color textColor = Color.FromArgb(57, 27, 1);

            // Fonte
            Font font = new Font("Gagalin", 9F, FontStyle.Regular);

            // Formato do texto
            StringFormat format = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near
            };

            // Desenhar texto
            using (SolidBrush brush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(e.SubItem.Text, font, brush, e.Bounds, format);
            }

            // Desenhar borda do subitem
            using (Pen pen = new Pen(Color.FromArgb(57, 27, 1), 1))
            {
                e.Graphics.DrawRectangle(pen, e.Bounds);
            }
        }

        // Evento de clique na ListView (para detectar clique no botão de deletar)
        private void listViewOrcamentos_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo hitTest = listViewOrcamentos.HitTest(e.Location);

            if (hitTest.Item != null && hitTest.SubItem != null)
            {
                // Verificar se clicou na coluna de ações (última coluna)
                if (hitTest.Item.SubItems.IndexOf(hitTest.SubItem) == listViewOrcamentos.Columns.Count - 1)
                {
                    // Confirmar exclusão
                    DialogResult result = MessageBox.Show(
                        "Tem certeza que deseja excluir este orçamento?",
                        "Confirmar Exclusão",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Remover orçamento da lista
                        Orcamento orcamentoParaRemover = (Orcamento)hitTest.Item.Tag;
                        orcamentos.Remove(orcamentoParaRemover);
                        AtualizarListViewOrcamentos();

                        MessageBox.Show("Orçamento excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void ConfigurarEstoque()
        {
            // Configurar colunas da ListView de Estoque
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

            // Configurar aparência personalizada
            listViewEstoque.OwnerDraw = true;
            listViewEstoque.DrawItem += ListViewEstoque_DrawItem;
            listViewEstoque.DrawSubItem += ListViewEstoque_DrawSubItem;
            listViewEstoque.DrawColumnHeader += ListViewEstoque_DrawColumnHeader;

            // Carregar dados de exemplo
            CarregarDadosEstoqueExemplo();
        }
        private void CarregarDadosEstoqueExemplo()
        {
            produtosEstoque.Clear();

            // Adicionar produtos de exemplo baseados na imagem
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

                // Status baseado no estoque
                string status = produto.EstoqueAbaixoMinimo ? "⚠️ BAIXO" : "✅ OK";
                item.SubItems.Add(status);

                item.SubItems.Add("✏️ 🗑️"); // Ícones de editar e deletar
                item.Tag = produto; // Armazenar o objeto completo

                // Colorir linha se estoque estiver baixo
                if (produto.EstoqueAbaixoMinimo)
                {
                    item.BackColor = Color.FromArgb(255, 200, 200); // Fundo avermelhado para alerta
                }

                listViewEstoque.Items.Add(item);
            }
        }

        // Eventos de desenho personalizado para a ListView de Estoque
        private void ListViewEstoque_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void ListViewEstoque_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = false;

            // Cor de fundo alternada
            Color backgroundColor = e.ItemIndex % 2 == 0 ?
                Color.FromArgb(239, 212, 172) :
                Color.FromArgb(250, 230, 194);

            // Verificar se o produto tem estoque baixo
            ProdutoEstoque produto = (ProdutoEstoque)e.Item.Tag;
            if (produto != null && produto.EstoqueAbaixoMinimo)
            {
                backgroundColor = Color.FromArgb(255, 220, 220); // Cor de alerta
            }

            // Destacar item selecionado
            if (e.Item.Selected)
            {
                backgroundColor = Color.FromArgb(198, 143, 86);
            }

            using (SolidBrush brush = new SolidBrush(backgroundColor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            // Desenhar borda
            using (Pen pen = new Pen(Color.FromArgb(57, 27, 1), 1))
            {
                e.Graphics.DrawRectangle(pen, e.Bounds);
            }
        }

        private void ListViewEstoque_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // Cor do texto
            Color textColor = Color.FromArgb(57, 27, 1);

            // Verificar se é a coluna de status e ajustar cor se necessário
            if (e.ColumnIndex == 7) // Coluna de Status
            {
                ProdutoEstoque produto = (ProdutoEstoque)e.Item.Tag;
                if (produto != null && produto.EstoqueAbaixoMinimo)
                {
                    textColor = Color.Red;
                }
                else
                {
                    textColor = Color.Green;
                }
            }

            // Fonte
            Font font = new Font("Gagalin", 9F, FontStyle.Regular);

            // Formato do texto
            StringFormat format = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near
            };

            // Desenhar texto
            using (SolidBrush brush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(e.SubItem.Text, font, brush, e.Bounds, format);
            }

            // Desenhar borda do subitem
            using (Pen pen = new Pen(Color.FromArgb(57, 27, 1), 1))
            {
                e.Graphics.DrawRectangle(pen, e.Bounds);
            }
        }

        // Evento de clique na ListView de Estoque
        private void listViewEstoque_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo hitTest = listViewEstoque.HitTest(e.Location);

            if (hitTest.Item != null && hitTest.SubItem != null)
            {
                // Verificar se clicou na coluna de ações (última coluna)
                if (hitTest.Item.SubItems.IndexOf(hitTest.SubItem) == listViewEstoque.Columns.Count - 1)
                {
                    ProdutoEstoque produto = (ProdutoEstoque)hitTest.Item.Tag;

                    // Você pode determinar se clicou em editar ou excluir baseado na posição do clique
                    // Por simplicidade, vamos mostrar um menu de contexto
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
            // Aqui você abriria um formulário de edição
            // Por enquanto, vamos apenas mostrar uma mensagem
            MessageBox.Show($"Editar produto: {produto.Nome}", "Editar Produto", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExcluirProduto(ProdutoEstoque produto)
        {
            DialogResult result = MessageBox.Show(
                $"Tem certeza que deseja excluir o produto '{produto.Nome}'?",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                produtosEstoque.Remove(produto);
                AtualizarListViewEstoque();
                MessageBox.Show("Produto excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnNovoProduto_Click(object sender, EventArgs e)
        {
            FormNovoProduto formNovo = new FormNovoProduto();

            if (formNovo.ShowDialog() == DialogResult.OK)
            {
                Produto novoProduto = formNovo.ProdutoCriado;

                var item = new ListViewItem(novoProduto.Sequencia.ToString());
                item.SubItems.Add(novoProduto.Descricao);
                item.SubItems.Add(novoProduto.Unidade);
                item.SubItems.Add(novoProduto.Quantidade.ToString());
                item.SubItems.Add(novoProduto.ValorUnitario.ToString("C"));
                item.SubItems.Add(novoProduto.ValorTotal.ToString("C"));

                listViewEstoque.Items.Add(item);
            }
        }

        private void btnAtualizarEstoque_Click(object sender, EventArgs e)
        {
            // Abre o form com a lista de produtos
            using (FormListaProdutos formLista = new FormListaProdutos())
            {
                formLista.ShowDialog();
            }
        }

        private void btnRelatorioEstoque_Click(object sender, EventArgs e)
        {
            // Gera relatório detalhado e abre no FormRelatorio
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
        private void btnOrcamento_Click(object sender, EventArgs e)
        {
            // Tornar o painel de orçamentos visível
            panelOrcamento.Visible = true;
            panelOrcamento.BringToFront();

            // Opcional: Destacar o botão ativo
            ResetarCoresBotoes();
            btnOrcamento.BackColor = Color.FromArgb(206, 186, 157); // Cor mais escura para indicar seleção
        }

        // Método auxiliar para resetar as cores dos botões do menu (opcional)
        private void ResetarCoresBotoes()
        {
            Color corPadrao = Color.FromArgb(239, 212, 172);

            btnTitulos.BackColor = corPadrao;
            btnPedidos.BackColor = corPadrao;
            btnOrcamento.BackColor = corPadrao;
            btnEstoque.BackColor = corPadrao;
        }

        // Se você quiser criar outros painéis para os outros botões, pode fazer assim:

        private void btnTitulos_Click(object sender, EventArgs e)
        {
            // Esconder painel atual
            panelOrcamento.Visible = false;

            // Aqui você criaria e mostraria o painel de títulos
            // panelTitulos.Visible = true;
            // panelTitulos.BringToFront();

            // Destacar botão ativo
            ResetarCoresBotoes();
            btnTitulos.BackColor = Color.FromArgb(206, 186, 157);
        }

        private void btnPedidos_Click(object sender, EventArgs e)
        {
            // Esconder painel atual
            panelOrcamento.Visible = false;

            // Aqui você criaria e mostraria o painel de pedidos
            // panelPedidos.Visible = true;
            // panelPedidos.BringToFront();

            // Destacar botão ativo
            ResetarCoresBotoes();
            btnPedidos.BackColor = Color.FromArgb(206, 186, 157);
        }

        private void btnEstoque_Click(object sender, EventArgs e)
        {
            // Esconder outros painéis
            panelOrcamento.Visible = false;

            // Mostrar painel de estoque
            panelEstoque.Visible = true;
            panelEstoque.BringToFront();

            // Destacar botão ativo
            ResetarCoresBotoes();
            btnEstoque.BackColor = Color.FromArgb(206, 186, 157);
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close(); // Fecha a aplicação
        }
    }
}