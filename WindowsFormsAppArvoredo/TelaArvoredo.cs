using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MySqlX.XDevAPI;

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
        private List<Orcamento> pedidos = new List<Orcamento>();
        private List<Cliente> clientes = new List<Cliente>();

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
            if (panelPedidos != null) panelPedidos.Visible = false;
            if (panelCadastro != null) panelCadastro.Visible = false;
            if (panelTitulos != null) panelTitulos.Visible = false;  // ADICIONAR ESTA LINHA

            ConfigurarListViewOrcamentos();
            ConfigurarEstoque();
            ConfigurarPedidos();
            ConfigurarPanelTitulos();  // ADICIONAR ESTA LINHA
            CarregarDadosExemplo();
            CarregarDadosExemploClientes();
            ConfigurarPainelCadastro();
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
            if (btnPedidos != null)
            {
                btnPedidos.Click -= btnPedidos_Click;
                btnPedidos.Click += btnPedidos_Click;
            }
            if (btnTitulos != null)
            {
                btnTitulos.Click -= btnTitulos_Click;
                btnTitulos.Click += btnTitulos_Click;
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
                listViewOrcamentos.DoubleClick -= listViewOrcamentos_DoubleClick;
                listViewOrcamentos.DoubleClick += listViewOrcamentos_DoubleClick;
            }
            if (listViewEstoque != null)
            {
                listViewEstoque.MouseClick -= listViewEstoque_MouseClick;
                listViewEstoque.MouseClick += listViewEstoque_MouseClick;
            }
            if (btnCadastro != null)
            {
                btnCadastro.Click -= btnCadastro_Click;
                btnCadastro.Click += btnCadastro_Click;
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

        #region Pedidos

        private void ConfigurarPedidos()
        {
            if (panelPedidos == null) return;

            panelPedidos.BackColor = Color.Transparent;
            panelPedidos.AutoScroll = true;
            panelPedidos.Padding = new Padding(20);
        }

        private void AtualizarPanelPedidos()
        {
            if (panelPedidos == null) return;

            panelPedidos.Controls.Clear();

            int yPosition = 20;
            int cardNumber = 1;

            foreach (var pedido in pedidos)
            {
                Panel cardPedido = new Panel();
                cardPedido.Location = new Point(20, yPosition);
                cardPedido.Size = new Size(720, 180);
                cardPedido.BackColor = Color.FromArgb(239, 212, 172);
                cardPedido.BorderStyle = BorderStyle.FixedSingle;

                Label lblTitulo = new Label();
                lblTitulo.Text = "PEDIDOS";
                lblTitulo.Location = new Point(10, 10);
                lblTitulo.Size = new Size(700, 25);
                lblTitulo.Font = new Font("Gagalin", 14F, FontStyle.Bold);
                lblTitulo.ForeColor = Color.FromArgb(57, 27, 1);
                lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
                cardPedido.Controls.Add(lblTitulo);

                Label lblNumeroCliente = new Label();
                lblNumeroCliente.Text = $"N°{cardNumber}                    CLIENTE: {pedido.Cliente.ToUpper()}";
                lblNumeroCliente.Location = new Point(15, 45);
                lblNumeroCliente.Size = new Size(690, 20);
                lblNumeroCliente.Font = new Font("Gagalin", 10F, FontStyle.Bold);
                lblNumeroCliente.ForeColor = Color.FromArgb(57, 27, 1);
                cardPedido.Controls.Add(lblNumeroCliente);

                int produtoY = 75;
                foreach (var item in pedido.Itens)
                {
                    Label lblProduto = new Label();
                    lblProduto.Text = $"{item.Descricao} {item.Quantidade} {item.Unidade}";
                    lblProduto.Location = new Point(15, produtoY);
                    lblProduto.Size = new Size(690, 18);
                    lblProduto.Font = new Font("Gagalin", 9F, FontStyle.Regular);
                    lblProduto.ForeColor = Color.FromArgb(57, 27, 1);
                    cardPedido.Controls.Add(lblProduto);

                    produtoY += 20;

                    if (produtoY > 145)
                    {
                        if (pedido.Itens.Count > 4)
                        {
                            Label lblMais = new Label();
                            lblMais.Text = $"... e mais {pedido.Itens.Count - 4} produto(s)";
                            lblMais.Location = new Point(15, produtoY);
                            lblMais.Size = new Size(690, 18);
                            lblMais.Font = new Font("Gagalin", 8F, FontStyle.Italic);
                            lblMais.ForeColor = Color.Gray;
                            cardPedido.Controls.Add(lblMais);
                        }
                        break;
                    }
                }

                panelPedidos.Controls.Add(cardPedido);
                yPosition += 200;
                cardNumber++;
            }

            if (pedidos.Count == 0)
            {
                Label lblVazio = new Label();
                lblVazio.Text = "Nenhum pedido confirmado ainda.\n\nCrie um orçamento e confirme para aparecer aqui.";
                lblVazio.Location = new Point(50, 100);
                lblVazio.Size = new Size(680, 100);
                lblVazio.Font = new Font("Gagalin", 12F, FontStyle.Regular);
                lblVazio.ForeColor = Color.FromArgb(57, 27, 1);
                lblVazio.TextAlign = ContentAlignment.MiddleCenter;
                panelPedidos.Controls.Add(lblVazio);
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
            listViewOrcamentos.DrawItem -= ListViewOrcamentos_DrawItem;
            listViewOrcamentos.DrawSubItem -= ListViewOrcamentos_DrawSubItem;
            listViewOrcamentos.DrawColumnHeader -= ListViewOrcamentos_DrawColumnHeader;

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
            Color backgroundColor = e.ItemIndex % 2 == 0 ? Color.FromArgb(239, 212, 172) : Color.FromArgb(250, 230, 194);

            if (e.Item.Selected)
                backgroundColor = Color.FromArgb(198, 143, 86);

            if (e.State.HasFlag(ListViewItemStates.Hot))
                backgroundColor = Color.FromArgb(220, 200, 150);

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

        private void listViewOrcamentos_DoubleClick(object sender, EventArgs e)
        {
            if (listViewOrcamentos.SelectedItems.Count == 0) return;

            var item = listViewOrcamentos.SelectedItems[0];
            var orcamentoSelecionado = item.Tag as Orcamento;

            if (orcamentoSelecionado != null)
            {
                AbrirOrcamentoParaEdicao(orcamentoSelecionado);
            }
        }

        private void AbrirOrcamentoParaEdicao(Orcamento orcamento)
        {
            using (var telaOrcamento = new TelaOrcamento(produtos, orcamento))
            {
                var resultado = telaOrcamento.ShowDialog();

                if (resultado == DialogResult.OK)
                {
                    if (telaOrcamento.OrcamentoConfirmado)
                    {
                        orcamentos.Remove(orcamento);

                        var pedido = telaOrcamento.OrcamentoCriado;
                        pedido.Id = pedidos.Count + 1;
                        pedido.Status = "Confirmado";
                        pedidos.Add(pedido);

                        AtualizarListViewOrcamentos();
                        AtualizarPanelPedidos();

                        MessageBox.Show(
                            $"Pedido #{pedido.Id} confirmado com sucesso!\n\n" +
                            $"Cliente: {pedido.Cliente}\n" +
                            $"Total: {pedido.TotalGeral:C}",
                            "Pedido Confirmado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else if (telaOrcamento.OrcamentoSalvo)
                    {
                        var orcamentoAtualizado = telaOrcamento.OrcamentoCriado;

                        int index = orcamentos.IndexOf(orcamento);
                        if (index >= 0)
                        {
                            orcamentoAtualizado.Id = orcamento.Id;
                            orcamentos[index] = orcamentoAtualizado;
                        }

                        AtualizarListViewOrcamentos();

                        MessageBox.Show(
                            "Orçamento atualizado com sucesso!",
                            "Orçamento Salvo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnNewOrc_Click(object sender, EventArgs e)
        {
            using (var telaOrcamento = new TelaOrcamento(produtos))
            {
                var resultado = telaOrcamento.ShowDialog();

                if (resultado == DialogResult.OK && telaOrcamento.OrcamentoCriado != null)
                {
                    if (telaOrcamento.OrcamentoConfirmado)
                    {
                        var novoPedido = telaOrcamento.OrcamentoCriado;
                        novoPedido.Id = pedidos.Count + 1;
                        novoPedido.Status = "Confirmado";
                        pedidos.Add(novoPedido);

                        AtualizarPanelPedidos();

                        MessageBox.Show(
                            $"Pedido #{novoPedido.Id} criado com sucesso!\n\n" +
                            $"Cliente: {novoPedido.Cliente}\n" +
                            $"Total: {novoPedido.TotalGeral:C}\n" +
                            $"Itens: {novoPedido.QuantidadeItens}",
                            "Pedido Confirmado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else if (telaOrcamento.OrcamentoSalvo)
                    {
                        var novoOrcamento = telaOrcamento.OrcamentoCriado;
                        novoOrcamento.Id = orcamentos.Count + 1;
                        novoOrcamento.Status = "Pendente";
                        orcamentos.Add(novoOrcamento);

                        AtualizarListViewOrcamentos();

                        MessageBox.Show(
                            $"Orçamento #{novoOrcamento.Id} salvo com sucesso!\n\n" +
                            $"Cliente: {novoOrcamento.Cliente}\n" +
                            $"Total: {novoOrcamento.TotalGeral:C}\n" +
                            $"Itens: {novoOrcamento.QuantidadeItens}",
                            "Orçamento Salvo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
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

            if (e.State.HasFlag(ListViewItemStates.Hot))
                backgroundColor = Color.FromArgb(220, 200, 150);

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

        #region Cadastro

        private void ConfigurarPainelCadastro()
        {
            if (panelCadastro == null) return;

            panelCadastro.BackColor = Color.Transparent;
            panelCadastro.Controls.Clear();

            // Painel principal com borda arredondada (container de tudo)
            Panel containerPrincipal = new Panel();
            containerPrincipal.Name = "containerPrincipal";
            containerPrincipal.Location = new Point(30, 30);
            containerPrincipal.Size = new Size(720, 530);
            containerPrincipal.BackColor = Color.FromArgb(239, 212, 172);
            containerPrincipal.BorderStyle = BorderStyle.FixedSingle;

            // Aplicar bordas arredondadas
            containerPrincipal.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0,
                containerPrincipal.Width, containerPrincipal.Height, 30, 30));

            panelCadastro.Controls.Add(containerPrincipal);

            // Barra de pesquisa
            TextBox txtPesquisaCadastro = new TextBox();
            txtPesquisaCadastro.Name = "txtPesquisaCadastro";
            txtPesquisaCadastro.Location = new Point(260, 25);
            txtPesquisaCadastro.Size = new Size(320, 35);
            txtPesquisaCadastro.Font = new Font("Gagalin", 10F);
            txtPesquisaCadastro.ForeColor = Color.Gray;
            txtPesquisaCadastro.Text = "BARRA DE PESQUISA";
            txtPesquisaCadastro.BorderStyle = BorderStyle.FixedSingle;
            txtPesquisaCadastro.BackColor = Color.White;
            txtPesquisaCadastro.Enter += (s, e) =>
            {
                if (txtPesquisaCadastro.Text == "BARRA DE PESQUISA")
                {
                    txtPesquisaCadastro.Text = "";
                    txtPesquisaCadastro.ForeColor = Color.FromArgb(57, 27, 1);
                }
            };
            txtPesquisaCadastro.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtPesquisaCadastro.Text))
                {
                    txtPesquisaCadastro.Text = "BARRA DE PESQUISA";
                    txtPesquisaCadastro.ForeColor = Color.Gray;
                }
            };
            txtPesquisaCadastro.TextChanged += (s, e) => FiltrarClientes(txtPesquisaCadastro.Text);
            containerPrincipal.Controls.Add(txtPesquisaCadastro);

            // Botão de pesquisa (lupa)
            Button btnPesquisar = new Button();
            btnPesquisar.Location = new Point(585, 25);
            btnPesquisar.Size = new Size(35, 35);
            btnPesquisar.BackColor = Color.White;
            btnPesquisar.FlatStyle = FlatStyle.Flat;
            btnPesquisar.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnPesquisar.FlatAppearance.BorderSize = 1;
            btnPesquisar.Font = new Font("Arial", 16F, FontStyle.Bold);
            btnPesquisar.ForeColor = Color.FromArgb(57, 27, 1);
            btnPesquisar.Text = "🔍";
            btnPesquisar.Cursor = Cursors.Hand;
            btnPesquisar.Click += (s, e) => FiltrarClientes(txtPesquisaCadastro.Text);
            containerPrincipal.Controls.Add(btnPesquisar);

            // Botão adicionar (+)
            Button btnAdicionarCliente = new Button();
            btnAdicionarCliente.Name = "btnAdicionarCliente";
            btnAdicionarCliente.Location = new Point(625, 25);
            btnAdicionarCliente.Size = new Size(35, 35);
            btnAdicionarCliente.Text = "+";
            btnAdicionarCliente.Font = new Font("Arial", 18F, FontStyle.Bold);
            btnAdicionarCliente.BackColor = Color.FromArgb(144, 238, 144);
            btnAdicionarCliente.ForeColor = Color.Black;
            btnAdicionarCliente.FlatStyle = FlatStyle.Flat;
            btnAdicionarCliente.FlatAppearance.BorderSize = 1;
            btnAdicionarCliente.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnAdicionarCliente.Cursor = Cursors.Hand;
            btnAdicionarCliente.Click += btnAdicionarClienteDireto_Click;
            containerPrincipal.Controls.Add(btnAdicionarCliente);

            // Container dos clientes (com scroll)
            Panel containerClientes = new Panel();
            containerClientes.Name = "containerClientes";
            containerClientes.Location = new Point(20, 75);
            containerClientes.Size = new Size(680, 440);
            containerClientes.BackColor = Color.Transparent;
            containerClientes.AutoScroll = true;
            containerPrincipal.Controls.Add(containerClientes);

            AtualizarListaClientes();
        }

        private void btnAdicionarClienteDireto_Click(object sender, EventArgs e)
        {
            using (FormCadastroCliente formCadastro = new FormCadastroCliente())
            {
                if (formCadastro.ShowDialog() == DialogResult.OK)
                {
                    var novoCliente = formCadastro.ClienteCriado;
                    novoCliente.Id = clientes.Count + 1;
                    clientes.Add(novoCliente);
                    AtualizarListaClientes();
                    MessageBox.Show("Cliente cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private Panel CriarCardCliente(Cliente cliente)
        {
            Panel card = new Panel();
            card.Size = new Size(640, 150);
            card.BackColor = Color.FromArgb(198, 143, 86);
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Margin = new Padding(0, 0, 0, 15);

            // Aplicar bordas arredondadas ao card
            card.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0,
                card.Width, card.Height, 20, 20));

            // Painel superior com nome do cliente
            Panel panelNome = new Panel();
            panelNome.Location = new Point(0, 0);
            panelNome.Size = new Size(640, 40);
            panelNome.BackColor = Color.FromArgb(239, 212, 172);
            card.Controls.Add(panelNome);

            Label lblNome = new Label();
            lblNome.Text = cliente.Nome.ToUpper();
            lblNome.Location = new Point(15, 8);
            lblNome.Size = new Size(610, 24);
            lblNome.Font = new Font("Gagalin", 10F, FontStyle.Bold);
            lblNome.ForeColor = Color.FromArgb(57, 27, 1);
            lblNome.TextAlign = ContentAlignment.MiddleLeft;
            panelNome.Controls.Add(lblNome);

            // CPF/CNPJ
            Label lblCpfLabel = new Label();
            lblCpfLabel.Text = "CPF/CNPJ:";
            lblCpfLabel.Location = new Point(15, 50);
            lblCpfLabel.Size = new Size(200, 16);
            lblCpfLabel.Font = new Font("Gagalin", 7F, FontStyle.Regular);
            lblCpfLabel.ForeColor = Color.FromArgb(57, 27, 1);
            card.Controls.Add(lblCpfLabel);

            Label lblCpf = new Label();
            lblCpf.Text = cliente.CpfCnpj;
            lblCpf.Location = new Point(320, 50);
            lblCpf.Size = new Size(300, 16);
            lblCpf.Font = new Font("Gagalin", 7F);
            lblCpf.ForeColor = Color.FromArgb(57, 27, 1);
            lblCpf.TextAlign = ContentAlignment.TopRight;
            card.Controls.Add(lblCpf);

            // Telefone
            Label lblTelLabel = new Label();
            lblTelLabel.Text = "TELEFONE/CELL:";
            lblTelLabel.Location = new Point(15, 70);
            lblTelLabel.Size = new Size(200, 16);
            lblTelLabel.Font = new Font("Gagalin", 7F, FontStyle.Regular);
            lblTelLabel.ForeColor = Color.FromArgb(57, 27, 1);
            card.Controls.Add(lblTelLabel);

            Label lblTelefone = new Label();
            lblTelefone.Text = cliente.Telefone;
            lblTelefone.Location = new Point(320, 70);
            lblTelefone.Size = new Size(300, 16);
            lblTelefone.Font = new Font("Gagalin", 7F);
            lblTelefone.ForeColor = Color.FromArgb(57, 27, 1);
            lblTelefone.TextAlign = ContentAlignment.TopRight;
            card.Controls.Add(lblTelefone);

            // CEP/Município
            Label lblCepLabel = new Label();
            lblCepLabel.Text = "CEP/MUNICÍPIO:";
            lblCepLabel.Location = new Point(15, 90);
            lblCepLabel.Size = new Size(200, 16);
            lblCepLabel.Font = new Font("Gagalin", 7F, FontStyle.Regular);
            lblCepLabel.ForeColor = Color.FromArgb(57, 27, 1);
            card.Controls.Add(lblCepLabel);

            Label lblCep = new Label();
            lblCep.Text = cliente.Cep;
            lblCep.Location = new Point(320, 90);
            lblCep.Size = new Size(300, 16);
            lblCep.Font = new Font("Gagalin", 7F);
            lblCep.ForeColor = Color.FromArgb(57, 27, 1);
            lblCep.TextAlign = ContentAlignment.TopRight;
            card.Controls.Add(lblCep);

            // Endereço
            Label lblEndLabel = new Label();
            lblEndLabel.Text = "ENDEREÇO:";
            lblEndLabel.Location = new Point(15, 110);
            lblEndLabel.Size = new Size(200, 16);
            lblEndLabel.Font = new Font("Gagalin", 7F, FontStyle.Regular);
            lblEndLabel.ForeColor = Color.FromArgb(57, 27, 1);
            card.Controls.Add(lblEndLabel);

            Label lblEndereco = new Label();
            lblEndereco.Text = cliente.Endereco;
            lblEndereco.Location = new Point(220, 110);
            lblEndereco.Size = new Size(400, 16);
            lblEndereco.Font = new Font("Gagalin", 7F);
            lblEndereco.ForeColor = Color.FromArgb(57, 27, 1);
            lblEndereco.TextAlign = ContentAlignment.TopRight;
            card.Controls.Add(lblEndereco);

            // Bairro + Botão DETALHES na mesma linha
            Label lblBairroLabel = new Label();
            lblBairroLabel.Text = "BAIRRO:";
            lblBairroLabel.Location = new Point(15, 130);
            lblBairroLabel.Size = new Size(70, 16);
            lblBairroLabel.Font = new Font("Gagalin", 7F, FontStyle.Regular);
            lblBairroLabel.ForeColor = Color.FromArgb(57, 27, 1);
            card.Controls.Add(lblBairroLabel);

            Label lblBairro = new Label();
            lblBairro.Text = cliente.Bairro;
            lblBairro.Location = new Point(90, 130);
            lblBairro.Size = new Size(350, 16);
            lblBairro.Font = new Font("Gagalin", 7F);
            lblBairro.ForeColor = Color.FromArgb(57, 27, 1);
            card.Controls.Add(lblBairro);

            // Botão DETALHES
            Button btnDetalhes = new Button();
            btnDetalhes.Text = "DETALHES";
            btnDetalhes.Location = new Point(520, 125);
            btnDetalhes.Size = new Size(100, 20);
            btnDetalhes.Font = new Font("Gagalin", 7F, FontStyle.Bold);
            btnDetalhes.BackColor = Color.FromArgb(239, 212, 172);
            btnDetalhes.ForeColor = Color.FromArgb(57, 27, 1);
            btnDetalhes.FlatStyle = FlatStyle.Flat;
            btnDetalhes.FlatAppearance.BorderSize = 1;
            btnDetalhes.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnDetalhes.Cursor = Cursors.Hand;
            btnDetalhes.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0,
                btnDetalhes.Width, btnDetalhes.Height, 10, 10));
            btnDetalhes.Click += (s, e) => AbrirDetalhesCliente(cliente);
            card.Controls.Add(btnDetalhes);

            return card;
        }

        private void AtualizarListaClientes(List<Cliente> clientesFiltrados = null)
        {
            if (panelCadastro == null) return;

            Panel containerPrincipal = panelCadastro.Controls.Find("containerPrincipal", false).FirstOrDefault() as Panel;
            if (containerPrincipal == null) return;

            Panel containerClientes = containerPrincipal.Controls.Find("containerClientes", false).FirstOrDefault() as Panel;
            if (containerClientes == null) return;

            containerClientes.Controls.Clear();

            var listaExibir = clientesFiltrados ?? clientes;
            int yPosition = 10;

            foreach (var cliente in listaExibir)
            {
                Panel cardCliente = CriarCardCliente(cliente);
                cardCliente.Location = new Point(10, yPosition);
                containerClientes.Controls.Add(cardCliente);
                yPosition += cardCliente.Height + 15;
            }

            if (listaExibir.Count == 0)
            {
                Label lblVazio = new Label();
                lblVazio.Text = "Nenhum cliente encontrado.";
                lblVazio.Location = new Point(150, 150);
                lblVazio.Size = new Size(400, 30);
                lblVazio.Font = new Font("Gagalin", 12F);
                lblVazio.ForeColor = Color.FromArgb(57, 27, 1);
                lblVazio.TextAlign = ContentAlignment.MiddleCenter;
                containerClientes.Controls.Add(lblVazio);
            }
        }

        private void FiltrarClientes(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro) || filtro == "BARRA DE PESQUISA")
            {
                AtualizarListaClientes();
                return;
            }

            var clientesFiltrados = clientes.Where(c =>
                c.Nome.ToLower().Contains(filtro.ToLower()) ||
                c.CpfCnpj.Contains(filtro) ||
                c.Telefone.Contains(filtro)
            ).ToList();

            AtualizarListaClientes(clientesFiltrados);
        }

        private void AbrirDetalhesCliente(Cliente cliente)
        {
            using (FormCadastroCliente formDetalhes = new FormCadastroCliente(cliente))
            {
                if (formDetalhes.ShowDialog() == DialogResult.OK)
                {
                    var clienteAtualizado = formDetalhes.ClienteCriado;
                    int index = clientes.FindIndex(c => c.Id == cliente.Id);
                    if (index >= 0)
                    {
                        clientes[index] = clienteAtualizado;
                        AtualizarListaClientes();
                        MessageBox.Show("Cliente atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void CarregarDadosExemploClientes()
        {
            if (clientes.Count > 0) return;

            clientes.Clear();
            clientes.Add(new Cliente
            {
                Id = 1,
                Nome = "Eduardo Castro de Souza",
                CpfCnpj = "123.456.789-10",
                Telefone = "(44) 12345-6789",
                Cep = "12345-678",
                Endereco = "Rua Tales Santos, 190",
                Bairro = "Jd. Santo Amaro"
            });
            clientes.Add(new Cliente
            {
                Id = 2,
                Nome = "Jordana Gleyse",
                CpfCnpj = "987.654.321-00",
                Telefone = "(44) 98765-4321",
                Cep = "87654-321",
                Endereco = "Av. Principal, 500",
                Bairro = "Centro"
            });
            clientes.Add(new Cliente
            {
                Id = 3,
                Nome = "Denise Maria de Souza",
                CpfCnpj = "111.222.333-44",
                Telefone = "(44) 91111-2222",
                Cep = "11111-222",
                Endereco = "Rua das Flores, 123",
                Bairro = "Jardim das Acácias"
            });
            clientes.Add(new Cliente
            {
                Id = 4,
                Nome = "Rayanne Ferreira",
                CpfCnpj = "555.666.777-88",
                Telefone = "(44) 95555-6666",
                Cep = "55555-666",
                Endereco = "Rua XV de Novembro, 789",
                Bairro = "Vila Nova"
            });
        }

        #endregion

        #region Navegação

        private void btnOrcamento_Click(object sender, EventArgs e)
        {
            if (panelEstoque != null) panelEstoque.Visible = false;
            if (panelPedidos != null) panelPedidos.Visible = false;
            if (panelCadastro != null) panelCadastro.Visible = false;
            if (panelTitulos != null) panelTitulos.Visible = false;
            if (panelOrcamento != null)
            {
                panelOrcamento.Visible = true;
                panelOrcamento.BringToFront();
            }
            ResetarCoresBotoes();
            if (btnOrcamento != null) btnOrcamento.BackColor = Color.FromArgb(206, 186, 157);
        }

        private void btnPedidos_Click(object sender, EventArgs e)
        {
            if (panelEstoque != null) panelEstoque.Visible = false;
            if (panelOrcamento != null) panelOrcamento.Visible = false;
            if (panelCadastro != null) panelCadastro.Visible = false;
            if (panelTitulos != null) panelTitulos.Visible = false;
            if (panelPedidos != null)
            {
                panelPedidos.Visible = true;
                panelPedidos.BringToFront();
                AtualizarPanelPedidos();
            }
            ResetarCoresBotoes();
            if (btnPedidos != null) btnPedidos.BackColor = Color.FromArgb(206, 186, 157);
        }

        private void btnEstoque_Click(object sender, EventArgs e)
        {
            if (panelOrcamento != null) panelOrcamento.Visible = false;
            if (panelPedidos != null) panelPedidos.Visible = false;
            if (panelCadastro != null) panelCadastro.Visible = false;
            if (panelTitulos != null) panelTitulos.Visible = false;
            if (panelEstoque != null)
            {
                panelEstoque.Visible = true;
                panelEstoque.BringToFront();
            }
            ResetarCoresBotoes();
            if (btnEstoque != null) btnEstoque.BackColor = Color.FromArgb(206, 186, 157);
            AtualizarListaEstoque();
        }

        private void btnCadastro_Click(object sender, EventArgs e)
        {
            if (panelOrcamento != null) panelOrcamento.Visible = false;
            if (panelEstoque != null) panelEstoque.Visible = false;
            if (panelPedidos != null) panelPedidos.Visible = false;

            if (panelCadastro != null)
            {
                panelCadastro.Visible = true;
                panelCadastro.BringToFront();
                AtualizarListaClientes();
            }

            ResetarCoresBotoes();
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

        #region Títulos

        private void ConfigurarPanelTitulos()
        {
            if (panelTitulos == null) return;

            panelTitulos.BackColor = Color.Transparent;
            panelTitulos.AutoScroll = true;
            panelTitulos.Padding = new Padding(20);
        }

        private void AtualizarPanelTitulos()
        {
            if (panelTitulos == null) return;

            panelTitulos.Controls.Clear();

            Label lblTitulo = new Label();
            lblTitulo.Text = "TÍTULOS PENDENTES";
            lblTitulo.Location = new Point(300, 20);
            lblTitulo.Size = new Size(200, 40);
            lblTitulo.Font = new Font("Gagalin", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(57, 27, 1);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            panelTitulos.Controls.Add(lblTitulo);

            int yPosition = 80;
            int cardNumber = 1;

            foreach (var pedido in pedidos)
            {
                Panel cardPedido = new Panel();
                cardPedido.Size = new Size(720, 120);
                cardPedido.Location = new Point(20, yPosition);
                cardPedido.BackColor = Color.FromArgb(239, 212, 172);
                cardPedido.BorderStyle = BorderStyle.FixedSingle;
                cardPedido.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, cardPedido.Width, cardPedido.Height, 20, 20));

                // Número e Cliente
                Label lblNumero = new Label();
                lblNumero.Text = $"N°{pedido.Id} - {pedido.Cliente.ToUpper()}";
                lblNumero.Location = new Point(15, 10);
                lblNumero.Size = new Size(500, 25);
                lblNumero.Font = new Font("Gagalin", 12F, FontStyle.Bold);
                lblNumero.ForeColor = Color.FromArgb(57, 27, 1);
                cardPedido.Controls.Add(lblNumero);

                // Valor
                Label lblValor = new Label();
                lblValor.Text = $"Valor: {pedido.TotalGeral:C}";
                lblValor.Location = new Point(15, 40);
                lblValor.Size = new Size(200, 20);
                lblValor.Font = new Font("Gagalin", 10F, FontStyle.Regular);
                lblValor.ForeColor = Color.FromArgb(57, 27, 1);
                cardPedido.Controls.Add(lblValor);

                // Data de Emissão
                Label lblData = new Label();
                lblData.Text = $"Data: {pedido.DataEmissao:dd/MM/yyyy}";
                lblData.Location = new Point(230, 40);
                lblData.Size = new Size(150, 20);
                lblData.Font = new Font("Gagalin", 10F, FontStyle.Regular);
                lblData.ForeColor = Color.FromArgb(57, 27, 1);
                cardPedido.Controls.Add(lblData);

                // Status
                Label lblStatus = new Label();
                lblStatus.Text = $"Status: {pedido.Status}";
                lblStatus.Location = new Point(400, 40);
                lblStatus.Size = new Size(150, 20);
                lblStatus.Font = new Font("Gagalin", 10F, FontStyle.Regular);
                lblStatus.ForeColor = Color.FromArgb(57, 27, 1);
                cardPedido.Controls.Add(lblStatus);

                // Forma de Pagamento
                Label lblPagamento = new Label();
                lblPagamento.Text = $"Pagamento: {pedido.FormaPagamento}";
                lblPagamento.Location = new Point(15, 70);
                lblPagamento.Size = new Size(250, 20);
                lblPagamento.Font = new Font("Gagalin", 10F, FontStyle.Regular);
                lblPagamento.ForeColor = Color.FromArgb(57, 27, 1);
                cardPedido.Controls.Add(lblPagamento);

                // Botão de detalhes
                Button btnDetalhes = new Button();
                btnDetalhes.Text = "DETALHES";
                btnDetalhes.Location = new Point(600, 80);
                btnDetalhes.Size = new Size(100, 25);
                btnDetalhes.Font = new Font("Gagalin", 9F, FontStyle.Bold);
                btnDetalhes.BackColor = Color.FromArgb(239, 212, 172);
                btnDetalhes.ForeColor = Color.FromArgb(57, 27, 1);
                btnDetalhes.FlatStyle = FlatStyle.Flat;
                btnDetalhes.FlatAppearance.BorderSize = 1;
                btnDetalhes.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
                btnDetalhes.Cursor = Cursors.Hand;
                btnDetalhes.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnDetalhes.Width, btnDetalhes.Height, 10, 10));
                btnDetalhes.Click += (s, e) => AbrirDetalhesPedido(pedido);
                cardPedido.Controls.Add(btnDetalhes);

                panelTitulos.Controls.Add(cardPedido);
                yPosition += cardPedido.Height + 15;
                cardNumber++;
            }

            if (pedidos.Count == 0)
            {
                Label lblVazio = new Label();
                lblVazio.Text = "Nenhum título pendente no momento.";
                lblVazio.Location = new Point(150, 150);
                lblVazio.Size = new Size(480, 50);
                lblVazio.Font = new Font("Gagalin", 12F, FontStyle.Regular);
                lblVazio.ForeColor = Color.FromArgb(57, 27, 1);
                lblVazio.TextAlign = ContentAlignment.MiddleCenter;
                panelTitulos.Controls.Add(lblVazio);
            }
        }

        private Panel CriarCardTitulo(Orcamento pedido, int numero)
        {
            Panel card = new Panel();
            card.Size = new Size(720, 70);
            card.BackColor = Color.FromArgb(239, 212, 172);
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Cursor = Cursors.Hand;

            // Aplicar bordas arredondadas
            card.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0,
                card.Width, card.Height, 20, 20));

            // Número do pedido
            Label lblNumero = new Label();
            lblNumero.Text = $"N°{numero} {pedido.Cliente.ToUpper()}";
            lblNumero.Location = new Point(15, 15);
            lblNumero.Size = new Size(500, 25);
            lblNumero.Font = new Font("Gagalin", 12F, FontStyle.Bold);
            lblNumero.ForeColor = Color.FromArgb(57, 27, 1);
            lblNumero.BackColor = Color.Transparent;
            card.Controls.Add(lblNumero);

            // Ícone de clique
            Label lblIcone = new Label();
            lblIcone.Text = "👆";
            lblIcone.Location = new Point(520, 10);
            lblIcone.Size = new Size(50, 35);
            lblIcone.Font = new Font("Arial", 20F);
            lblIcone.BackColor = Color.Transparent;
            lblIcone.TextAlign = ContentAlignment.MiddleCenter;
            card.Controls.Add(lblIcone);

            // Valor
            Label lblValor = new Label();
            lblValor.Text = $"valor: {pedido.TotalGeral:N2}";
            lblValor.Location = new Point(580, 15);
            lblValor.Size = new Size(130, 25);
            lblValor.Font = new Font("Gagalin", 10F, FontStyle.Regular);
            lblValor.ForeColor = Color.FromArgb(57, 27, 1);
            lblValor.BackColor = Color.Transparent;
            lblValor.TextAlign = ContentAlignment.MiddleRight;
            card.Controls.Add(lblValor);

            // Eventos de hover
            card.MouseEnter += (s, e) => {
                card.BackColor = Color.FromArgb(220, 195, 155);
            };
            card.MouseLeave += (s, e) => {
                card.BackColor = Color.FromArgb(239, 212, 172);
            };

            // Evento de clique
            card.Click += (s, e) => AbrirDetalhesPedido(pedido);

            // Fazer com que os labels também disparem o clique
            foreach (Control ctrl in card.Controls)
            {
                ctrl.Click += (s, e) => AbrirDetalhesPedido(pedido);
                ctrl.MouseEnter += (s, e) => {
                    card.BackColor = Color.FromArgb(220, 195, 155);
                };
                ctrl.MouseLeave += (s, e) => {
                    card.BackColor = Color.FromArgb(239, 212, 172);
                };
            }

            return card;
        }

        private void AbrirDetalhesPedido(Orcamento pedido)
        {
            using (TelaTitulos telaDetalhes = new TelaTitulos(pedido))
            {
                var resultado = telaDetalhes.ShowDialog();

                if (resultado == DialogResult.OK)
                {
                    // Pedido foi finalizado
                    pedidos.Remove(pedido);
                    AtualizarPanelTitulos();

                    MessageBox.Show(
                        $"Pedido de {pedido.Cliente} finalizado com sucesso!",
                        "Pedido Finalizado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        private void btnTitulos_Click(object sender, EventArgs e)
        {
            if (panelOrcamento != null) panelOrcamento.Visible = false;
            if (panelEstoque != null) panelEstoque.Visible = false;
            if (panelPedidos != null) panelPedidos.Visible = false;
            if (panelCadastro != null) panelCadastro.Visible = false;

            if (panelTitulos != null)
            {
                panelTitulos.Visible = true;
                panelTitulos.BringToFront();
                AtualizarPanelTitulos();
            }

            ResetarCoresBotoes();
            if (btnTitulos != null) btnTitulos.BackColor = Color.FromArgb(206, 186, 157);
        }

        #endregion
    }
}