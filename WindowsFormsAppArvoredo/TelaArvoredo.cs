using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
        private List<Orcamento> pedidosFinalizados = new List<Orcamento>();
        private List<Usuario> usuarios = new List<Usuario>();
        private string abaCadastroAtiva = "clientes";

        // VARIÁVEIS DO HISTÓRICO
        private int anoSelecionado = 0;
        private string mesSelecionado = "";
        private int[] todosAnos = { 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025 };
        private int indiceAnoInicial = 0;

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
            btnTitulos.TabStop = false;
            btnTitulos.FlatAppearance.BorderSize = 0;
            btnPedidos.TabStop = false;
            btnPedidos.FlatAppearance.BorderSize = 0;
            btnOrcamento.TabStop = false;
            btnOrcamento.FlatAppearance.BorderSize = 0;
            btnEstoque.TabStop = false;
            btnEstoque.FlatAppearance.BorderSize = 0;
            btnHistorico.TabStop = false;
            btnHistorico.FlatAppearance.BorderSize = 0;
            btnCadastro.TabStop = false;
            btnCadastro.FlatAppearance.BorderSize = 0;
            btnCaixa.TabStop = false;
            btnCaixa.FlatAppearance.BorderSize = 0;
            btnSair.TabStop = false;
            btnSair.FlatAppearance.BorderSize = 0;
            btnNovoProduto.TabStop = false;
            btnNovoProduto.FlatAppearance.BorderSize = 0;
            btnAtualizarEstoque.TabStop = false;
            btnAtualizarEstoque.FlatAppearance.BorderSize=0;
            btnRelatorioEstoque.TabStop = false;
            btnRelatorioEstoque.FlatAppearance.BorderSize = 0;
            btnOrcamento.TabStop= false;
            btnOrcamento.FlatAppearance.BorderSize=0;
            btnNewOrc.TabStop = false;
            btnNewOrc.FlatAppearance.BorderSize = 0;

            // CORREÇÃO: Retirar panelTitulos de dentro do panelOrcamento
            if (panelTitulos != null && panelTitulos.Parent == panelOrcamento)
            {
                panelOrcamento.Controls.Remove(panelTitulos);
                this.Controls.Add(panelTitulos);
            }

            // CORREÇÃO: Retirar panelCadastro de dentro do panel2
            if (panelCadastro != null && panelCadastro.Parent == panel2)
            {
                panel2.Controls.Remove(panelCadastro);
                this.Controls.Add(panelCadastro);
            }

            // Configurar posição e tamanho corretos
            if (panelTitulos != null)
            {
                panelTitulos.Location = new Point(301, 74);
                panelTitulos.Size = new Size(783, 587);
                panelTitulos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            }

            if (panelCadastro != null)
            {
                panelCadastro.Location = new Point(301, 74);
                panelCadastro.Size = new Size(783, 587);
                panelCadastro.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            }

            // Ocultar todos os painéis primeiro
            if (panelOrcamento != null) panelOrcamento.Visible = false;
            if (panelEstoque != null) panelEstoque.Visible = false;
            if (panelPedidos != null) panelPedidos.Visible = false;
            if (panelCadastro != null) panelCadastro.Visible = false;
            if (panelTitulos != null) panelTitulos.Visible = false;
            if (panelHistorico != null) panelHistorico.Visible = false;

            ConfigurarListViewOrcamentos();
            ConfigurarEstoque();
            ConfigurarPedidos();
            ConfigurarPanelTitulos();
            CarregarProdutosDaAPIAsync();
            CarregarDadosExemploClientes();
            ConfigurarPainelCadastro();
            ConfigurarPanelHistorico();
            VincularEventos();
            panelDegrade?.Invalidate();

            // MOSTRAR O PAINEL DE ORÇAMENTOS COMO PADRÃO
            btnOrcamento_Click(null, null);
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
                    btnNewOrc.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnNewOrc.Width, btnNewOrc.Height, 20, 20));
                btnNovoProduto.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnNovoProduto.Width, btnNovoProduto.Height, 20, 20));
                btnAtualizarEstoque.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnAtualizarEstoque.Width, btnAtualizarEstoque.Height, 20, 20));
                btnRelatorioEstoque.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnRelatorioEstoque.Width, btnRelatorioEstoque.Height, 20, 20));

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
            if (btnHistorico != null)
            {
                btnHistorico.Click -= btnHistorico_Click;
                btnHistorico.Click += btnHistorico_Click;
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

            listViewOrcamentos.OwnerDraw = false;

            listViewOrcamentos.DrawItem -= ListViewOrcamentos_DrawItem;
            listViewOrcamentos.DrawSubItem -= ListViewOrcamentos_DrawSubItem;
            listViewOrcamentos.DrawColumnHeader -= ListViewOrcamentos_DrawColumnHeader;

            listViewOrcamentos.FullRowSelect = true;
            listViewOrcamentos.HideSelection = false;
            listViewOrcamentos.HotTracking = false;
            listViewOrcamentos.BackColor = Color.FromArgb(239, 212, 172);
            listViewOrcamentos.ForeColor = Color.FromArgb(57, 21, 1);
        }

        private void CarregarDadosExemplo()
        {
            
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
            Font font = new Font("Arial", 9F, FontStyle.Regular);
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

            // DESABILITA OwnerDraw - vamos usar o desenho padrão
            listViewEstoque.OwnerDraw = false;

            // Remove todos os eventos de desenho
            listViewEstoque.DrawItem -= ListViewEstoque_DrawItem;
            listViewEstoque.DrawSubItem -= ListViewEstoque_DrawSubItem;
            listViewEstoque.DrawColumnHeader -= ListViewEstoque_DrawColumnHeader;

            // Configurações do ListView
            listViewEstoque.View = View.Details;
            listViewEstoque.FullRowSelect = true;
            listViewEstoque.HideSelection = false;
            listViewEstoque.HotTracking = false;
            listViewEstoque.BackColor = Color.FromArgb(239, 212, 172);
            listViewEstoque.ForeColor = Color.FromArgb(57, 27, 1);
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

            Font font = new Font("Arial", 9F, FontStyle.Regular);
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

        private async void btnNovoProduto_Click(object sender, EventArgs e)
        {
            using (var form = new FormNovoProduto())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var novo = form.ProdutoCriado;
                    novo.UltimaAtualizacao = DateTime.Now;

                    // Salvar na API
                    bool salvou = await SalvarProdutoNaAPIAsync(novo);

                    if (salvou)
                    {
                        // Adicionar à lista local
                        produtos.Add(novo);

                        // Reindexar
                        for (int i = 0; i < produtos.Count; i++)
                            produtos[i].Sequencia = produtos[i].Sequencia > 0 ? produtos[i].Sequencia : i + 1;

                        AtualizarListaEstoque();

                        MessageBox.Show(
                            "Produto cadastrado com sucesso!",
                            "Sucesso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
        }

        private async void EditarProduto(Produto produto)
        {
            if (produto == null) return;

            using (var form = new FormNovoProduto(produto))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Salvar na API
                    bool salvou = await SalvarProdutoNaAPIAsync(produto);

                    if (salvou)
                    {
                        AtualizarListaEstoque();
                        MessageBox.Show(
                            "Produto atualizado com sucesso!",
                            "Sucesso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
        }

        private async Task CarregarProdutosDaAPIAsync()
        {
            try
            {
                // Buscar produtos da API
                var produtosAPI = await ApiClient.GetAsync<List<ProdutoAPI>>("/produtos");

                if (produtosAPI != null && produtosAPI.Count > 0)
                {
                    produtos.Clear();

                    foreach (var prodAPI in produtosAPI)
                    {
                        produtos.Add(new Produto
                        {
                            Sequencia = prodAPI.id,
                            Descricao = prodAPI.nome,
                            Tipo = ObterNomeMadeira(prodAPI.madeiraId),
                            Quantidade = prodAPI.quantidade,
                            QuantidadeMinima = prodAPI.quantidadeMin,
                            ValorUnitario = (decimal)prodAPI.valor,
                            Unidade = prodAPI.unidade ?? "m",
                            UltimaAtualizacao = DateTime.Now
                        });
                    }

                    AtualizarListaEstoque();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao carregar produtos da API: {ex.Message}\n\nUsando dados locais.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                // Carrega dados de exemplo se a API falhar
                CarregarDadosExemplo();
            }
        }

        /// <summary>
        /// Salva um produto na API
        /// </summary>
        private async Task<bool> SalvarProdutoNaAPIAsync(Produto produto)
        {
            try
            {
                var produtoAPI = new ProdutoAPICreate
                {
                    nome = produto.Descricao,
                    valor = (double)produto.ValorUnitario,
                    unidade = produto.Unidade,
                    quantidade = (int)produto.Quantidade,
                    quantidadeMin = produto.QuantidadeMinima,
                    ativo = true
                };

                if (produto.Sequencia > 0)
                {
                    // Atualizar produto existente
                    await ApiClient.PutAsync<ProdutoAPICreate, ProdutoAPI>(
                        $"/produtos/{produto.Sequencia}",
                        produtoAPI);
                }
                else
                {
                    // Criar novo produto
                    var novoProduto = await ApiClient.PostAsync<ProdutoAPICreate, ProdutoAPI>(
                        "/produtos",
                        produtoAPI);

                    if (novoProduto != null)
                    {
                        produto.Sequencia = novoProduto.id;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao salvar produto na API: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }

        /// <summary>
        /// Exclui um produto da API
        /// </summary>
        private async Task<bool> ExcluirProdutoDaAPIAsync(int produtoId)
        {
            try
            {
                bool sucesso = await ApiClient.DeleteAsync($"/produtos/{produtoId}");
                return sucesso;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao excluir produto da API: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }

        /// <summary>
        /// Obtém o nome da madeira por ID (mock - você pode buscar da API também)
        /// </summary>
        private string ObterNomeMadeira(int? madeiraId)
        {
            if (!madeiraId.HasValue) return "Sem tipo";

            // Aqui você pode fazer uma chamada à API para buscar o nome real da madeira
            // Por enquanto, retornamos tipos genéricos
            switch (madeiraId)
            {
                case 1: return "Eucalipto";
                case 2: return "Peroba";
                case 3: return "Câmbara";
                case 4: return "Pinnus";
                default: return $"Madeira {madeiraId}";
            }
        }


        private async void ExcluirProduto(Produto produto)
        {
            if (produto == null) return;

            var result = MessageBox.Show(
                $"Tem certeza que deseja excluir o produto '{produto.Descricao}'?",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Excluir da API
                bool excluiu = await ExcluirProdutoDaAPIAsync(produto.Sequencia);

                if (excluiu)
                {
                    produtos.Remove(produto);

                    // Reindexar
                    for (int i = 0; i < produtos.Count; i++)
                        produtos[i].Sequencia = i + 1;

                    AtualizarListaEstoque();
                    MessageBox.Show(
                        "Produto excluído com sucesso!",
                        "Sucesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
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

            // Container principal
            Panel containerPrincipal = new Panel();
            containerPrincipal.Name = "containerPrincipal";
            containerPrincipal.Location = new Point(30, 30);
            containerPrincipal.Size = new Size(720, 530);
            containerPrincipal.BackColor = Color.FromArgb(239, 212, 172);
            containerPrincipal.BorderStyle = BorderStyle.FixedSingle;
            panelCadastro.Controls.Add(containerPrincipal);

            // Painel de abas
            Panel panelAbas = new Panel();
            panelAbas.Name = "panelAbas";
            panelAbas.Location = new Point(20, 20);
            panelAbas.Size = new Size(680, 50);
            panelAbas.BackColor = Color.Transparent;
            containerPrincipal.Controls.Add(panelAbas);

            // Botão aba Clientes
            Button btnAbaClientes = new Button();
            btnAbaClientes.Name = "btnAbaClientes";
            btnAbaClientes.Text = "👥 CLIENTES";
            btnAbaClientes.Location = new Point(0, 0);
            btnAbaClientes.Size = new Size(200, 50);
            btnAbaClientes.Font = new Font("Gagalin", 12F, FontStyle.Bold);
            btnAbaClientes.BackColor = Color.FromArgb(144, 238, 144);
            btnAbaClientes.ForeColor = Color.Black;
            btnAbaClientes.FlatStyle = FlatStyle.Flat;
            btnAbaClientes.FlatAppearance.BorderSize = 0;
            btnAbaClientes.Cursor = Cursors.Hand;
            btnAbaClientes.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnAbaClientes.Width, btnAbaClientes.Height, 15, 15));
            btnAbaClientes.Click += (s, e) => TrocarAbaCadastro("clientes");
            panelAbas.Controls.Add(btnAbaClientes);

            // Botão aba Usuários
            Button btnAbaUsuarios = new Button();
            btnAbaUsuarios.Name = "btnAbaUsuarios";
            btnAbaUsuarios.Text = "👤 USUÁRIOS";
            btnAbaUsuarios.Location = new Point(210, 0);
            btnAbaUsuarios.Size = new Size(200, 50);
            btnAbaUsuarios.Font = new Font("Gagalin", 12F, FontStyle.Bold);
            btnAbaUsuarios.BackColor = Color.FromArgb(221, 160, 221);
            btnAbaUsuarios.ForeColor = Color.Black;
            btnAbaUsuarios.FlatStyle = FlatStyle.Flat;
            btnAbaUsuarios.FlatAppearance.BorderSize = 0;
            btnAbaUsuarios.Cursor = Cursors.Hand;
            btnAbaUsuarios.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnAbaUsuarios.Width, btnAbaUsuarios.Height, 15, 15));
            btnAbaUsuarios.Click += (s, e) => TrocarAbaCadastro("usuarios");
            panelAbas.Controls.Add(btnAbaUsuarios);

            // Barra de pesquisa
            TextBox txtPesquisaCadastro = new TextBox();
            txtPesquisaCadastro.Name = "txtPesquisaCadastro";
            txtPesquisaCadastro.Location = new Point(20, 85);
            txtPesquisaCadastro.Size = new Size(520, 35);
            txtPesquisaCadastro.Font = new Font("Arial", 10F);
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
            txtPesquisaCadastro.TextChanged += (s, e) => FiltrarCadastros(txtPesquisaCadastro.Text);
            containerPrincipal.Controls.Add(txtPesquisaCadastro);

            // Botão pesquisar
            Button btnPesquisar = new Button();
            btnPesquisar.Location = new Point(545, 85);
            btnPesquisar.Size = new Size(35, 35);
            btnPesquisar.BackColor = Color.White;
            btnPesquisar.FlatStyle = FlatStyle.Flat;
            btnPesquisar.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnPesquisar.FlatAppearance.BorderSize = 1;
            btnPesquisar.Font = new Font("Arial", 16F, FontStyle.Bold);
            btnPesquisar.ForeColor = Color.FromArgb(57, 27, 1);
            btnPesquisar.Text = "🔍";
            btnPesquisar.Cursor = Cursors.Hand;
            btnPesquisar.Click += (s, e) => FiltrarCadastros(txtPesquisaCadastro.Text);
            containerPrincipal.Controls.Add(btnPesquisar);

            // Botão adicionar
            Button btnAdicionar = new Button();
            btnAdicionar.Name = "btnAdicionar";
            btnAdicionar.Location = new Point(585, 85);
            btnAdicionar.Size = new Size(35, 35);
            btnAdicionar.Text = "+";
            btnAdicionar.Font = new Font("Arial", 18F, FontStyle.Bold);
            btnAdicionar.BackColor = Color.FromArgb(144, 238, 144);
            btnAdicionar.ForeColor = Color.Black;
            btnAdicionar.FlatStyle = FlatStyle.Flat;
            btnAdicionar.FlatAppearance.BorderSize = 1;
            btnAdicionar.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnAdicionar.Cursor = Cursors.Hand;
            btnAdicionar.Click += btnAdicionarCadastro_Click;
            containerPrincipal.Controls.Add(btnAdicionar);

            // Container de conteúdo
            Panel containerConteudo = new Panel();
            containerConteudo.Name = "containerConteudo";
            containerConteudo.Location = new Point(20, 135);
            containerConteudo.Size = new Size(680, 380);
            containerConteudo.BackColor = Color.Transparent;
            containerConteudo.AutoScroll = true;
            containerPrincipal.Controls.Add(containerConteudo);

            CarregarDadosExemploUsuarios();
            TrocarAbaCadastro("clientes");
        }

        private void TrocarAbaCadastro(string aba)
        {
            abaCadastroAtiva = aba;

            // Atualizar cores dos botões de aba
            if (panelCadastro == null) return;
            Panel containerPrincipal = panelCadastro.Controls.Find("containerPrincipal", false).FirstOrDefault() as Panel;
            if (containerPrincipal == null) return;

            Panel panelAbas = containerPrincipal.Controls.Find("panelAbas", false).FirstOrDefault() as Panel;
            if (panelAbas != null)
            {
                Button btnAbaClientes = panelAbas.Controls.Find("btnAbaClientes", false).FirstOrDefault() as Button;
                Button btnAbaUsuarios = panelAbas.Controls.Find("btnAbaUsuarios", false).FirstOrDefault() as Button;

                if (btnAbaClientes != null)
                {
                    btnAbaClientes.BackColor = aba == "clientes" ? Color.FromArgb(144, 238, 144) : Color.FromArgb(200, 200, 200);
                }

                if (btnAbaUsuarios != null)
                {
                    btnAbaUsuarios.BackColor = aba == "usuarios" ? Color.FromArgb(221, 160, 221) : Color.FromArgb(200, 200, 200);
                }
            }

            // Atualizar lista
            if (aba == "clientes")
            {
                AtualizarListaClientes();
            }
            else
            {
                AtualizarListaUsuarios();
            }
        }

        private void FiltrarCadastros(string filtro)
        {
            if (abaCadastroAtiva == "clientes")
            {
                FiltrarClientes(filtro);
            }
            else
            {
                FiltrarUsuarios(filtro);
            }
        }

        private void btnAdicionarCadastro_Click(object sender, EventArgs e)
        {
            if (abaCadastroAtiva == "clientes")
            {
                btnAdicionarClienteDireto_Click(sender, e);
            }
            else
            {
                btnAdicionarUsuarioDireto_Click(sender, e);
            }
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

        private void btnAdicionarUsuarioDireto_Click(object sender, EventArgs e)
        {
            using (FormCadastroUsuario formCadastro = new FormCadastroUsuario())
            {
                if (formCadastro.ShowDialog() == DialogResult.OK)
                {
                    var novoUsuario = formCadastro.UsuarioCriado;
                    novoUsuario.Id = usuarios.Count + 1;
                    usuarios.Add(novoUsuario);
                    AtualizarListaUsuarios();
                    MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void AtualizarListaClientes(List<Cliente> clientesFiltrados = null)
        {
            if (panelCadastro == null) return;

            Panel containerPrincipal = panelCadastro.Controls.Find("containerPrincipal", false).FirstOrDefault() as Panel;
            if (containerPrincipal == null) return;

            Panel containerConteudo = containerPrincipal.Controls.Find("containerConteudo", false).FirstOrDefault() as Panel;
            if (containerConteudo == null) return;

            containerConteudo.Controls.Clear();

            var listaExibir = clientesFiltrados ?? clientes;
            int yPosition = 10;

            foreach (var cliente in listaExibir)
            {
                Panel cardCliente = CriarCardCliente(cliente);
                cardCliente.Location = new Point(10, yPosition);
                containerConteudo.Controls.Add(cardCliente);
                yPosition += cardCliente.Height + 15;
            }

            if (listaExibir.Count == 0)
            {
                Label lblVazio = new Label();
                lblVazio.Text = "Nenhum cliente encontrado.";
                lblVazio.Location = new Point(150, 150);
                lblVazio.Size = new Size(400, 30);
                lblVazio.Font = new Font("Arial", 12F);
                lblVazio.ForeColor = Color.FromArgb(57, 27, 1);
                lblVazio.TextAlign = ContentAlignment.MiddleCenter;
                containerConteudo.Controls.Add(lblVazio);
            }
        }

        private void AtualizarListaUsuarios(List<Usuario> usuariosFiltrados = null)
        {
            if (panelCadastro == null) return;

            Panel containerPrincipal = panelCadastro.Controls.Find("containerPrincipal", false).FirstOrDefault() as Panel;
            if (containerPrincipal == null) return;

            Panel containerConteudo = containerPrincipal.Controls.Find("containerConteudo", false).FirstOrDefault() as Panel;
            if (containerConteudo == null) return;

            containerConteudo.Controls.Clear();

            var listaExibir = usuariosFiltrados ?? usuarios;
            int yPosition = 10;

            foreach (var usuario in listaExibir)
            {
                Panel cardUsuario = CriarCardUsuario(usuario);
                cardUsuario.Location = new Point(10, yPosition);
                containerConteudo.Controls.Add(cardUsuario);
                yPosition += cardUsuario.Height + 15;
            }

            if (listaExibir.Count == 0)
            {
                Label lblVazio = new Label();
                lblVazio.Text = "Nenhum usuário encontrado.";
                lblVazio.Location = new Point(150, 150);
                lblVazio.Size = new Size(400, 30);
                lblVazio.Font = new Font("Arial", 12F);
                lblVazio.ForeColor = Color.FromArgb(57, 27, 1);
                lblVazio.TextAlign = ContentAlignment.MiddleCenter;
                containerConteudo.Controls.Add(lblVazio);
            }
        }

        private Panel CriarCardCliente(Cliente cliente)
        {
            Panel card = new Panel();
            card.Size = new Size(640, 150);
            card.BackColor = Color.FromArgb(198, 143, 86);
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Margin = new Padding(0, 0, 0, 15);

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

            Label lblCpfLabel = new Label();
            lblCpfLabel.Text = "CPF/CNPJ:";
            lblCpfLabel.Location = new Point(15, 50);
            lblCpfLabel.Size = new Size(200, 16);
            lblCpfLabel.Font = new Font("Arial", 7F, FontStyle.Regular);
            lblCpfLabel.ForeColor = Color.FromArgb(57, 27, 1);
            card.Controls.Add(lblCpfLabel);

            Label lblCpf = new Label();
            lblCpf.Text = cliente.CpfCnpj;
            lblCpf.Location = new Point(320, 50);
            lblCpf.Size = new Size(300, 16);
            lblCpf.Font = new Font("Arial", 7F);
            lblCpf.ForeColor = Color.FromArgb(57, 27, 1);
            lblCpf.TextAlign = ContentAlignment.TopRight;
            card.Controls.Add(lblCpf);

            Label lblTelLabel = new Label();
            lblTelLabel.Text = "TELEFONE/CELL:";
            lblTelLabel.Location = new Point(15, 70);
            lblTelLabel.Size = new Size(200, 16);
            lblTelLabel.Font = new Font("Arial", 7F, FontStyle.Regular);
            lblTelLabel.ForeColor = Color.FromArgb(57, 27, 1);
            card.Controls.Add(lblTelLabel);

            Label lblTelefone = new Label();
            lblTelefone.Text = cliente.Telefone;
            lblTelefone.Location = new Point(320, 70);
            lblTelefone.Size = new Size(300, 16);
            lblTelefone.Font = new Font("Arial", 7F);
            lblTelefone.ForeColor = Color.FromArgb(57, 27, 1);
            lblTelefone.TextAlign = ContentAlignment.TopRight;
            card.Controls.Add(lblTelefone);

            Label lblCepLabel = new Label();
            lblCepLabel.Text = "CEP/MUNICÍPIO:";
            lblCepLabel.Location = new Point(15, 90);
            lblCepLabel.Size = new Size(200, 16);
            lblCepLabel.Font = new Font("Arial", 7F, FontStyle.Regular);
            lblCepLabel.ForeColor = Color.FromArgb(57, 27, 1);
            card.Controls.Add(lblCepLabel);

            Label lblCep = new Label();
            lblCep.Text = cliente.Cep;
            lblCep.Location = new Point(320, 90);
            lblCep.Size = new Size(300, 16);
            lblCep.Font = new Font("Arial", 7F);
            lblCep.ForeColor = Color.FromArgb(57, 27, 1);
            lblCep.TextAlign = ContentAlignment.TopRight;
            card.Controls.Add(lblCep);

            Label lblEndLabel = new Label();
            lblEndLabel.Text = "ENDEREÇO:";
            lblEndLabel.Location = new Point(15, 110);
            lblEndLabel.Size = new Size(200, 16);
            lblEndLabel.Font = new Font("Arial", 7F, FontStyle.Regular);
            lblEndLabel.ForeColor = Color.FromArgb(57, 27, 1);
            card.Controls.Add(lblEndLabel);

            Label lblEndereco = new Label();
            lblEndereco.Text = cliente.Endereco;
            lblEndereco.Location = new Point(220, 110);
            lblEndereco.Size = new Size(400, 16);
            lblEndereco.Font = new Font("Arial", 7F);
            lblEndereco.ForeColor = Color.FromArgb(57, 27, 1);
            lblEndereco.TextAlign = ContentAlignment.TopRight;
            card.Controls.Add(lblEndereco);

            Label lblBairroLabel = new Label();
            lblBairroLabel.Text = "BAIRRO:";
            lblBairroLabel.Location = new Point(15, 130);
            lblBairroLabel.Size = new Size(70, 16);
            lblBairroLabel.Font = new Font("Arial", 7F, FontStyle.Regular);
            lblBairroLabel.ForeColor = Color.FromArgb(57, 27, 1);
            card.Controls.Add(lblBairroLabel);

            Label lblBairro = new Label();
            lblBairro.Text = cliente.Bairro;
            lblBairro.Location = new Point(90, 130);
            lblBairro.Size = new Size(350, 16);
            lblBairro.Font = new Font("Arial", 7F);
            lblBairro.ForeColor = Color.FromArgb(57, 27, 1);
            card.Controls.Add(lblBairro);

            Button btnDetalhes = new Button();
            btnDetalhes.Text = "DETALHES";
            btnDetalhes.Location = new Point(520, 120);
            btnDetalhes.Size = new Size(100, 30);
            btnDetalhes.Font = new Font("Arial", 7F, FontStyle.Bold);
            btnDetalhes.BackColor = Color.FromArgb(239, 212, 172);
            btnDetalhes.ForeColor = Color.FromArgb(57, 27, 1);
            btnDetalhes.FlatStyle = FlatStyle.Flat;
            btnDetalhes.FlatAppearance.BorderSize = 0;
            btnDetalhes.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnDetalhes.Cursor = Cursors.Hand;
            btnDetalhes.Click += (s, e) => AbrirDetalhesCliente(cliente);
            card.Controls.Add(btnDetalhes);

            return card;
        }

        private Panel CriarCardUsuario(Usuario usuario)
        {
            Panel card = new Panel();
            card.Size = new Size(640, 120);
            card.BackColor = Color.FromArgb(198, 143, 86);
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Margin = new Padding(0, 0, 0, 15);

            // Painel do nome
            Panel panelNome = new Panel();
            panelNome.Location = new Point(0, 0);
            panelNome.Size = new Size(640, 40);
            panelNome.BackColor = Color.FromArgb(239, 212, 172);
            card.Controls.Add(panelNome);

            Label lblNome = new Label();
            lblNome.Text = usuario.Nome.ToUpper();
            lblNome.Location = new Point(15, 8);
            lblNome.Size = new Size(450, 24);
            lblNome.Font = new Font("Gagalin", 10F, FontStyle.Bold);
            lblNome.ForeColor = Color.FromArgb(57, 27, 1);
            lblNome.TextAlign = ContentAlignment.MiddleLeft;
            panelNome.Controls.Add(lblNome);

            // Status (Ativo/Inativo)
            Label lblStatus = new Label();
            lblStatus.Text = usuario.Ativo ? "✅ ATIVO" : "❌ INATIVO";
            lblStatus.Location = new Point(480, 8);
            lblStatus.Size = new Size(145, 24);
            lblStatus.Font = new Font("Gagalin", 9F, FontStyle.Bold);
            lblStatus.ForeColor = usuario.Ativo ? Color.Green : Color.Red;
            lblStatus.TextAlign = ContentAlignment.MiddleRight;
            panelNome.Controls.Add(lblStatus);

            // Login
            Label lblLoginLabel = new Label();
            lblLoginLabel.Text = "LOGIN:";
            lblLoginLabel.Location = new Point(15, 50);
            lblLoginLabel.Size = new Size(100, 16);
            lblLoginLabel.Font = new Font("Arial", 7F, FontStyle.Regular);
            lblLoginLabel.ForeColor = Color.FromArgb(57, 27, 1);
            card.Controls.Add(lblLoginLabel);

            Label lblLogin = new Label();
            lblLogin.Text = usuario.Login;
            lblLogin.Location = new Point(320, 50);
            lblLogin.Size = new Size(300, 16);
            lblLogin.Font = new Font("Arial", 7F);
            lblLogin.ForeColor = Color.FromArgb(57, 27, 1);
            lblLogin.TextAlign = ContentAlignment.TopRight;
            card.Controls.Add(lblLogin);

            // E-mail
            Label lblEmailLabel = new Label();
            lblEmailLabel.Text = "E-MAIL:";
            lblEmailLabel.Location = new Point(15, 70);
            lblEmailLabel.Size = new Size(100, 16);
            lblEmailLabel.Font = new Font("Arial", 7F, FontStyle.Regular);
            lblEmailLabel.ForeColor = Color.FromArgb(57, 27, 1);
            card.Controls.Add(lblEmailLabel);

            Label lblEmail = new Label();
            lblEmail.Text = usuario.Email;
            lblEmail.Location = new Point(220, 70);
            lblEmail.Size = new Size(400, 16);
            lblEmail.Font = new Font("Arial", 7F);
            lblEmail.ForeColor = Color.FromArgb(57, 27, 1);
            lblEmail.TextAlign = ContentAlignment.TopRight;
            card.Controls.Add(lblEmail);

            // Perfil
            Label lblPerfilLabel = new Label();
            lblPerfilLabel.Text = "PERFIL:";
            lblPerfilLabel.Location = new Point(15, 90);
            lblPerfilLabel.Size = new Size(100, 16);
            lblPerfilLabel.Font = new Font("Arial", 7F, FontStyle.Regular);
            lblPerfilLabel.ForeColor = Color.FromArgb(57, 27, 1);
            card.Controls.Add(lblPerfilLabel);

            Label lblPerfil = new Label();
            lblPerfil.Text = usuario.Perfil;
            lblPerfil.Location = new Point(320, 90);
            lblPerfil.Size = new Size(200, 16);
            lblPerfil.Font = new Font("Arial", 7F, FontStyle.Bold);
            lblPerfil.ForeColor = Color.FromArgb(57, 27, 1);
            lblPerfil.TextAlign = ContentAlignment.TopRight;
            card.Controls.Add(lblPerfil);

            // Botão detalhes
            Button btnDetalhes = new Button();
            btnDetalhes.Text = "DETALHES";
            btnDetalhes.Location = new Point(520, 85);
            btnDetalhes.Size = new Size(100, 30);
            btnDetalhes.Font = new Font("Arial", 7F, FontStyle.Bold);
            btnDetalhes.BackColor = Color.FromArgb(239, 212, 172);
            btnDetalhes.ForeColor = Color.FromArgb(57, 27, 1);
            btnDetalhes.FlatStyle = FlatStyle.Flat;
            btnDetalhes.FlatAppearance.BorderSize = 0;
            btnDetalhes.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnDetalhes.Cursor = Cursors.Hand;
            btnDetalhes.Click += (s, e) => AbrirDetalhesUsuario(usuario);
            card.Controls.Add(btnDetalhes);

            return card;
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

        private void FiltrarUsuarios(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro) || filtro == "BARRA DE PESQUISA")
            {
                AtualizarListaUsuarios();
                return;
            }

            var usuariosFiltrados = usuarios.Where(u =>
                u.Nome.ToLower().Contains(filtro.ToLower()) ||
                u.Login.ToLower().Contains(filtro.ToLower()) ||
                u.Email.ToLower().Contains(filtro.ToLower())
            ).ToList();

            AtualizarListaUsuarios(usuariosFiltrados);
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

        private void AbrirDetalhesUsuario(Usuario usuario)
        {
            using (FormCadastroUsuario formDetalhes = new FormCadastroUsuario(usuario))
            {
                if (formDetalhes.ShowDialog() == DialogResult.OK)
                {
                    var usuarioAtualizado = formDetalhes.UsuarioCriado;
                    int index = usuarios.FindIndex(u => u.Id == usuario.Id);
                    if (index >= 0)
                    {
                        usuarios[index] = usuarioAtualizado;
                        AtualizarListaUsuarios();
                        MessageBox.Show("Usuário atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void CarregarDadosExemploUsuarios()
        {
            if (usuarios.Count > 0) return;

            usuarios.Clear();
            usuarios.Add(new Usuario
            {
                Id = 1,
                Nome = "Administrador",
                Login = "admin",
                Senha = "admin123",
                Email = "admin@arvoredo.com.br",
                Perfil = "Admin",
                Ativo = true
            });
            usuarios.Add(new Usuario
            {
                Id = 2,
                Nome = "João Silva",
                Login = "joao.silva",
                Senha = "senha123",
                Email = "joao.silva@arvoredo.com.br",
                Perfil = "Vendedor",
                Ativo = true
            });
            usuarios.Add(new Usuario
            {
                Id = 3,
                Nome = "Maria Santos",
                Login = "maria.santos",
                Senha = "senha123",
                Email = "maria.santos@arvoredo.com.br",
                Perfil = "Usuario",
                Ativo = false
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
            if (panelHistorico != null) panelHistorico.Visible = false;
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
            if (panelHistorico != null) panelHistorico.Visible = false;
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
            if (panelHistorico != null) panelHistorico.Visible = false;
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
            if (panelTitulos != null) panelTitulos.Visible = false;
            if (panelHistorico != null) panelHistorico.Visible = false;

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

                Label lblNumero = new Label();
                lblNumero.Text = $"N°{pedido.Id} - {pedido.Cliente.ToUpper()}";
                lblNumero.Location = new Point(15, 10);
                lblNumero.Size = new Size(500, 25);
                lblNumero.Font = new Font("Gagalin", 12F, FontStyle.Bold);
                lblNumero.ForeColor = Color.FromArgb(57, 27, 1);
                cardPedido.Controls.Add(lblNumero);

                Label lblValor = new Label();
                lblValor.Text = $"Valor: {pedido.TotalGeral:C}";
                lblValor.Location = new Point(15, 40);
                lblValor.Size = new Size(200, 20);
                lblValor.Font = new Font("Gagalin", 10F, FontStyle.Regular);
                lblValor.ForeColor = Color.FromArgb(57, 27, 1);
                cardPedido.Controls.Add(lblValor);

                Label lblData = new Label();
                lblData.Text = $"Data: {pedido.DataEmissao:dd/MM/yyyy}";
                lblData.Location = new Point(230, 40);
                lblData.Size = new Size(150, 20);
                lblData.Font = new Font("Gagalin", 10F, FontStyle.Regular);
                lblData.ForeColor = Color.FromArgb(57, 27, 1);
                cardPedido.Controls.Add(lblData);

                Label lblStatus = new Label();
                lblStatus.Text = $"Status: {pedido.Status}";
                lblStatus.Location = new Point(400, 40);
                lblStatus.Size = new Size(150, 20);
                lblStatus.Font = new Font("Gagalin", 10F, FontStyle.Regular);
                lblStatus.ForeColor = Color.FromArgb(57, 27, 1);
                cardPedido.Controls.Add(lblStatus);

                Label lblPagamento = new Label();
                lblPagamento.Text = $"Pagamento: {pedido.FormaPagamento}";
                lblPagamento.Location = new Point(15, 70);
                lblPagamento.Size = new Size(250, 20);
                lblPagamento.Font = new Font("Gagalin", 10F, FontStyle.Regular);
                lblPagamento.ForeColor = Color.FromArgb(57, 27, 1);
                cardPedido.Controls.Add(lblPagamento);

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

            card.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0,
                card.Width, card.Height, 20, 20));

            Label lblNumero = new Label();
            lblNumero.Text = $"N°{numero} {pedido.Cliente.ToUpper()}";
            lblNumero.Location = new Point(15, 15);
            lblNumero.Size = new Size(500, 25);
            lblNumero.Font = new Font("Gagalin", 12F, FontStyle.Bold);
            lblNumero.ForeColor = Color.FromArgb(57, 27, 1);
            lblNumero.BackColor = Color.Transparent;
            card.Controls.Add(lblNumero);

            Label lblIcone = new Label();
            lblIcone.Text = "👆";
            lblIcone.Location = new Point(520, 10);
            lblIcone.Size = new Size(50, 35);
            lblIcone.Font = new Font("Arial", 20F);
            lblIcone.BackColor = Color.Transparent;
            lblIcone.TextAlign = ContentAlignment.MiddleCenter;
            card.Controls.Add(lblIcone);

            Label lblValor = new Label();
            lblValor.Text = $"valor: {pedido.TotalGeral:N2}";
            lblValor.Location = new Point(580, 15);
            lblValor.Size = new Size(130, 25);
            lblValor.Font = new Font("Gagalin", 10F, FontStyle.Regular);
            lblValor.ForeColor = Color.FromArgb(57, 27, 1);
            lblValor.BackColor = Color.Transparent;
            lblValor.TextAlign = ContentAlignment.MiddleRight;
            card.Controls.Add(lblValor);

            card.MouseEnter += (s, e) => {
                card.BackColor = Color.FromArgb(220, 195, 155);
            };
            card.MouseLeave += (s, e) => {
                card.BackColor = Color.FromArgb(239, 212, 172);
            };

            card.Click += (s, e) => AbrirDetalhesPedido(pedido);

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
                    // Adicionar ao histórico de pedidos finalizados
                    pedido.Status = "Finalizado";
                    pedidosFinalizados.Add(pedido);

                    // Remover da lista de pendentes
                    pedidos.Remove(pedido);
                    AtualizarPanelTitulos();

                    MessageBox.Show(
                        $"Pedido de {pedido.Cliente} finalizado com sucesso!\n" +
                        $"O pedido foi salvo no histórico de {pedido.DataEmissao:MMMM/yyyy}.",
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
            if (panelHistorico != null) panelHistorico.Visible = false;

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

        #region Caixa

        private void ConfigurarPanelCaixa()
        {
            if (panelCaixa == null)
            {
                panelCaixa = new Panel();
                panelCaixa.Name = "panelCaixa";
                panelCaixa.Location = new Point(301, 74);
                panelCaixa.Size = new Size(783, 587);
                panelCaixa.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                panelCaixa.BackColor = Color.Transparent;
                panelCaixa.Visible = false;
                this.Controls.Add(panelCaixa);
            }

            panelCaixa.Controls.Clear();

            // Logo
            PictureBox picLogo = new PictureBox();
            picLogo.Location = new Point(20, 15);
            picLogo.Size = new Size(80, 60);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            try { picLogo.Image = Properties.Resources.logo1; } catch { }
            panelCaixa.Controls.Add(picLogo);

            // Título HISTÓRICO
            Label lblHistorico = new Label();
            lblHistorico.Text = "HISTÓRICO";
            lblHistorico.Location = new Point(120, 15);
            lblHistorico.Size = new Size(200, 30);
            lblHistorico.Font = new Font("Gagalin", 14F, FontStyle.Bold);
            lblHistorico.ForeColor = Color.FromArgb(57, 27, 1);
            lblHistorico.BackColor = Color.Transparent;
            panelCaixa.Controls.Add(lblHistorico);

            // Botão CADASTRO
            Button btnCadastro = new Button();
            btnCadastro.Text = "CADASTRO";
            btnCadastro.Location = new Point(340, 20);
            btnCadastro.Size = new Size(130, 30);
            btnCadastro.Font = new Font("Arial", 10F, FontStyle.Bold);
            btnCadastro.BackColor = Color.FromArgb(239, 212, 172);
            btnCadastro.ForeColor = Color.FromArgb(57, 27, 1);
            btnCadastro.FlatStyle = FlatStyle.Flat;
            btnCadastro.FlatAppearance.BorderSize = 0;
            btnCadastro.Cursor = Cursors.Hand;
            btnCadastro.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnCadastro.Width, btnCadastro.Height, 15, 15));
            panelCaixa.Controls.Add(btnCadastro);

            // Botão CAIXA (ativo)
            Button btnCaixaAtivo = new Button();
            btnCaixaAtivo.Text = "CAIXA";
            btnCaixaAtivo.Location = new Point(480, 20);
            btnCaixaAtivo.Size = new Size(130, 30);
            btnCaixaAtivo.Font = new Font("Arial", 10F, FontStyle.Bold);
            btnCaixaAtivo.BackColor = Color.FromArgb(255, 140, 0);
            btnCaixaAtivo.ForeColor = Color.White;
            btnCaixaAtivo.FlatStyle = FlatStyle.Flat;
            btnCaixaAtivo.FlatAppearance.BorderSize = 0;
            btnCaixaAtivo.Cursor = Cursors.Hand;
            btnCaixaAtivo.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnCaixaAtivo.Width, btnCaixaAtivo.Height, 15, 15));
            panelCaixa.Controls.Add(btnCaixaAtivo);

            // Botão SAIR (X)
            Button btnSairCaixa = new Button();
            btnSairCaixa.Text = "X";
            btnSairCaixa.Location = new Point(620, 20);
            btnSairCaixa.Size = new Size(40, 30);
            btnSairCaixa.Font = new Font("Arial", 12F, FontStyle.Bold);
            btnSairCaixa.BackColor = Color.FromArgb(239, 212, 172);
            btnSairCaixa.ForeColor = Color.FromArgb(57, 27, 1);
            btnSairCaixa.FlatStyle = FlatStyle.Flat;
            btnSairCaixa.FlatAppearance.BorderSize = 0;
            btnSairCaixa.Cursor = Cursors.Hand;
            btnSairCaixa.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSairCaixa.Width, btnSairCaixa.Height, 15, 15));
            btnSairCaixa.Click += (s, e) => { panelCaixa.Visible = false; panelOrcamento.Visible = true; btnOrcamento_Click(null, null); };
            panelCaixa.Controls.Add(btnSairCaixa);

            // Container de transações
            Panel containerTransacoes = new Panel();
            containerTransacoes.Location = new Point(20, 70);
            containerTransacoes.Size = new Size(740, 420);
            containerTransacoes.BackColor = Color.FromArgb(239, 212, 172);
            containerTransacoes.BorderStyle = BorderStyle.FixedSingle;
            containerTransacoes.AutoScroll = true;
            panelCaixa.Controls.Add(containerTransacoes);

            // Cabeçalho das transações
            Label lblCabecalho = new Label();
            lblCabecalho.Text = "DATA           COMPROU/RECEBEU                    VALOR          D/L";
            lblCabecalho.Location = new Point(20, 10);
            lblCabecalho.Size = new Size(690, 20);
            lblCabecalho.Font = new Font("Arial", 9F, FontStyle.Bold);
            lblCabecalho.ForeColor = Color.FromArgb(57, 27, 1);
            lblCabecalho.BackColor = Color.Transparent;
            containerTransacoes.Controls.Add(lblCabecalho);

            // Adicionar transações de exemplo
            AdicionarTransacoesCaixa(containerTransacoes);

            // Painel de ação inferior
            Panel panelAcaoInferior = new Panel();
            panelAcaoInferior.Location = new Point(20, 500);
            panelAcaoInferior.Size = new Size(740, 70);
            panelAcaoInferior.BackColor = Color.Transparent;
            panelCaixa.Controls.Add(panelAcaoInferior);

            // Label TOTAL
            Label lblTotal = new Label();
            lblTotal.Text = "TOTAL:";
            lblTotal.Location = new Point(480, 15);
            lblTotal.Size = new Size(80, 25);
            lblTotal.Font = new Font("Arial", 14F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(57, 27, 1);
            lblTotal.TextAlign = ContentAlignment.MiddleRight;
            panelAcaoInferior.Controls.Add(lblTotal);

            // Valor total
            Label lblValorTotal = new Label();
            lblValorTotal.Text = "12.000,00";
            lblValorTotal.Location = new Point(565, 15);
            lblValorTotal.Size = new Size(150, 25);
            lblValorTotal.Font = new Font("Arial", 14F, FontStyle.Bold);
            lblValorTotal.ForeColor = Color.FromArgb(57, 27, 1);
            lblValorTotal.TextAlign = ContentAlignment.MiddleRight;
            panelAcaoInferior.Controls.Add(lblValorTotal);

            // Botão GERAR RELATÓRIO ANUAL
            Button btnGerarRelatorio = new Button();
            btnGerarRelatorio.Text = "GERAR RELATÓRIO ANUAL";
            btnGerarRelatorio.Location = new Point(250, 10);
            btnGerarRelatorio.Size = new Size(240, 40);
            btnGerarRelatorio.Font = new Font("Arial", 10F, FontStyle.Bold);
            btnGerarRelatorio.BackColor = Color.FromArgb(239, 212, 172);
            btnGerarRelatorio.ForeColor = Color.FromArgb(57, 27, 1);
            btnGerarRelatorio.FlatStyle = FlatStyle.Flat;
            btnGerarRelatorio.FlatAppearance.BorderSize = 2;
            btnGerarRelatorio.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnGerarRelatorio.Cursor = Cursors.Hand;
            btnGerarRelatorio.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnGerarRelatorio.Width, btnGerarRelatorio.Height, 20, 20));
            btnGerarRelatorio.Click += BtnGerarRelatorioAnual_Click;
            panelAcaoInferior.Controls.Add(btnGerarRelatorio);
        }

        private void AdicionarTransacoesCaixa(Panel container)
        {
            // Dados de exemplo
            var transacoes = new[]
            {
                new { Data = "20/03/2025", Tipo = "COMPROU MADEIRA", Valor = "40.000,00", Status = "D" },
                new { Data = "20/03/2025", Tipo = "RECEBEU PÁSCANO", Valor = "7.850,95", Status = "L" }
            };

            int yPos = 40;
            foreach (var transacao in transacoes)
            {
                Panel itemTransacao = new Panel();
                itemTransacao.Location = new Point(20, yPos);
                itemTransacao.Size = new Size(690, 30);
                itemTransacao.BackColor = Color.White;
                itemTransacao.BorderStyle = BorderStyle.FixedSingle;

                Label lblData = new Label();
                lblData.Text = transacao.Data;
                lblData.Location = new Point(10, 5);
                lblData.Size = new Size(90, 20);
                lblData.Font = new Font("Arial", 9F);
                lblData.ForeColor = Color.FromArgb(57, 27, 1);
                itemTransacao.Controls.Add(lblData);

                Label lblTipo = new Label();
                lblTipo.Text = transacao.Tipo;
                lblTipo.Location = new Point(110, 5);
                lblTipo.Size = new Size(300, 20);
                lblTipo.Font = new Font("Arial", 9F);
                lblTipo.ForeColor = Color.FromArgb(57, 27, 1);
                itemTransacao.Controls.Add(lblTipo);

                Label lblValor = new Label();
                lblValor.Text = transacao.Valor;
                lblValor.Location = new Point(420, 5);
                lblValor.Size = new Size(150, 20);
                lblValor.Font = new Font("Arial", 9F);
                lblValor.ForeColor = Color.FromArgb(57, 27, 1);
                lblValor.TextAlign = ContentAlignment.TopRight;
                itemTransacao.Controls.Add(lblValor);

                Label lblStatus = new Label();
                lblStatus.Text = transacao.Status;
                lblStatus.Location = new Point(580, 5);
                lblStatus.Size = new Size(100, 20);
                lblStatus.Font = new Font("Arial", 9F, FontStyle.Bold);
                lblStatus.ForeColor = transacao.Status == "D" ? Color.Red : Color.Green;
                lblStatus.TextAlign = ContentAlignment.TopRight;
                itemTransacao.Controls.Add(lblStatus);

                container.Controls.Add(itemTransacao);
                yPos += 35;
            }
        }

        private void BtnGerarRelatorioAnual_Click(object sender, EventArgs e)
        {
            // Criar e exibir o form de seleção de ano
            using (FormSelecionarAnoRelatorio formAno = new FormSelecionarAnoRelatorio())
            {
                if (formAno.ShowDialog() == DialogResult.OK)
                {
                    int anoSelecionado = formAno.AnoSelecionado;
                    MessageBox.Show($"Relatório anual de {anoSelecionado} será gerado!",
                        "Gerar Relatório", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Aqui você implementaria a lógica de geração do relatório
                }
            }
        }

        #endregion

        #region Histórico

        private void ConfigurarPanelHistorico()
        {
            if (panelHistorico == null)
            {
                panelHistorico = new Panel();
                panelHistorico.Name = "panelHistorico";
                panelHistorico.Location = new Point(301, 74);
                panelHistorico.Size = new Size(783, 587);
                panelHistorico.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                panelHistorico.BackColor = Color.Transparent;
                panelHistorico.Visible = false;
                this.Controls.Add(panelHistorico);
            }

            panelHistorico.Controls.Clear();

            // Título HISTÓRICO
            Label lblTitulo = new Label();
            lblTitulo.Text = "HISTÓRICO";
            lblTitulo.Location = new Point(20, 20);
            lblTitulo.Size = new Size(740, 40);
            lblTitulo.Font = new Font("Gagalin", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(57, 27, 1);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.BackColor = Color.FromArgb(239, 212, 172);
            lblTitulo.BorderStyle = BorderStyle.FixedSingle;
            panelHistorico.Controls.Add(lblTitulo);

            // Container de Anos
            Panel containerAnos = new Panel();
            containerAnos.Name = "containerAnos";
            containerAnos.Location = new Point(20, 80);
            containerAnos.Size = new Size(740, 80);
            containerAnos.BackColor = Color.FromArgb(239, 212, 172);
            containerAnos.BorderStyle = BorderStyle.FixedSingle;
            panelHistorico.Controls.Add(containerAnos);

            // Seta Esquerda Anos
            Button btnSetaEsqAnos = new Button();
            btnSetaEsqAnos.Name = "btnSetaEsqAnos";
            btnSetaEsqAnos.Text = "◄";
            btnSetaEsqAnos.Location = new Point(10, 25);
            btnSetaEsqAnos.Size = new Size(50, 30);
            btnSetaEsqAnos.Font = new Font("Arial", 16F, FontStyle.Bold);
            btnSetaEsqAnos.BackColor = Color.FromArgb(239, 212, 172);
            btnSetaEsqAnos.FlatStyle = FlatStyle.Flat;
            btnSetaEsqAnos.FlatAppearance.BorderSize = 0;
            btnSetaEsqAnos.Cursor = Cursors.Hand;
            btnSetaEsqAnos.Click += BtnSetaEsqAnos_Click;
            containerAnos.Controls.Add(btnSetaEsqAnos);

            // Container para os botões de anos
            Panel panelBotoesAnos = new Panel();
            panelBotoesAnos.Name = "panelBotoesAnos";
            panelBotoesAnos.Location = new Point(80, 20);
            panelBotoesAnos.Size = new Size(580, 40);
            panelBotoesAnos.BackColor = Color.Transparent;
            containerAnos.Controls.Add(panelBotoesAnos);

            AtualizarBotoesAnos(panelBotoesAnos);

            // Seta Direita Anos
            Button btnSetaDirAnos = new Button();
            btnSetaDirAnos.Name = "btnSetaDirAnos";
            btnSetaDirAnos.Text = "►";
            btnSetaDirAnos.Location = new Point(680, 25);
            btnSetaDirAnos.Size = new Size(50, 30);
            btnSetaDirAnos.Font = new Font("Arial", 16F, FontStyle.Bold);
            btnSetaDirAnos.BackColor = Color.FromArgb(239, 212, 172);
            btnSetaDirAnos.FlatStyle = FlatStyle.Flat;
            btnSetaDirAnos.FlatAppearance.BorderSize = 0;
            btnSetaDirAnos.Cursor = Cursors.Hand;
            btnSetaDirAnos.Click += BtnSetaDirAnos_Click;
            containerAnos.Controls.Add(btnSetaDirAnos);

            // Container de Meses
            Panel containerMeses = new Panel();
            containerMeses.Name = "containerMeses";
            containerMeses.Location = new Point(20, 180);
            containerMeses.Size = new Size(740, 380);
            containerMeses.BackColor = Color.FromArgb(239, 212, 172);
            containerMeses.BorderStyle = BorderStyle.FixedSingle;
            panelHistorico.Controls.Add(containerMeses);

            // Criar botões de meses
            string[] meses = { "JANEIRO", "FEVEREIRO", "MARÇO", "ABRIL", "MAIO", "JUNHO",
                      "JULHO", "AGOSTO", "SETEMBRO", "OUTUBRO", "NOVEMBRO", "DEZEMBRO" };

            int mesX = 30;
            int mesY = 30;
            int mesIndex = 0;

            foreach (string mes in meses)
            {
                Button btnMes = new Button();
                btnMes.Text = mes;
                btnMes.Location = new Point(mesX, mesY);
                btnMes.Size = new Size(150, 60);
                btnMes.Font = new Font("Arial", 10F, FontStyle.Bold);
                btnMes.BackColor = Color.FromArgb(198, 143, 86);
                btnMes.ForeColor = Color.FromArgb(57, 27, 1);
                btnMes.FlatStyle = FlatStyle.Flat;
                btnMes.FlatAppearance.BorderSize = 0;
                btnMes.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
                btnMes.Cursor = Cursors.Hand;
                btnMes.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnMes.Width, btnMes.Height, 15, 15));
                btnMes.Tag = mes;
                btnMes.Click += BtnMes_Click;
                containerMeses.Controls.Add(btnMes);

                mesIndex++;
                mesX += 170;

                if (mesIndex % 4 == 0)
                {
                    mesX = 30;
                    mesY += 80;
                }
            }

            // Botão BACKUP
            Button btnBackup = new Button();
            btnBackup.Text = "BACKUP";
            btnBackup.Location = new Point(295, 325);
            btnBackup.Size = new Size(150, 40);
            btnBackup.Font = new Font("Arial", 12F, FontStyle.Bold);
            btnBackup.BackColor = Color.FromArgb(144, 238, 144);
            btnBackup.ForeColor = Color.Black;
            btnBackup.FlatStyle = FlatStyle.Flat;
            btnBackup.FlatAppearance.BorderSize = 0;
            btnBackup.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnBackup.Cursor = Cursors.Hand;
            btnBackup.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnBackup.Width, btnBackup.Height, 15, 15));
            btnBackup.Click += BtnBackup_Click;
            containerMeses.Controls.Add(btnBackup);

            // Carregar alguns pedidos de exemplo para teste
            CarregarPedidosFinalizadosExemplo();
        }

        private void CarregarPedidosFinalizadosExemplo()
        {
            // Se já existem pedidos finalizados, não adiciona exemplos
            if (pedidosFinalizados.Count > 0) return;

            // Criar pedidos de exemplo para diferentes meses
            Random rand = new Random();

            for (int i = 1; i <= 15; i++)
            {
                Orcamento pedidoExemplo = new Orcamento
                {
                    Id = 1000 + i,
                    Cliente = $"Cliente Exemplo {i}",
                    CPF_CNPJ = $"123.456.789-{i:00}",
                    Endereco = $"Rua Exemplo, {i * 10}",
                    Numero = $"{i * 10}",
                    Bairro = "Centro",
                    CEP = "12345-678",
                    Cidade = "Jaú",
                    UF = "SP",
                    Telefone = $"(14) 9{i:0000}-{rand.Next(1000, 9999)}",
                    Vendedor = "Vendedor Sistema",
                    FormaPagamento = i % 2 == 0 ? "Dinheiro" : "Cartão",
                    Status = "Finalizado",
                    DataEmissao = new DateTime(2024 + (i % 2), (i % 12) + 1, rand.Next(1, 28)),
                    Desconto = 0,
                    Acrescimo = 0
                };

                // Adicionar alguns itens
                decimal valorTotal = 0;
                for (int j = 1; j <= 3; j++)
                {
                    decimal valorUnit = rand.Next(20, 150);
                    decimal qtd = rand.Next(1, 10);
                    pedidoExemplo.Itens.Add(new ItemOrcamento
                    {
                        Sequencia = j,
                        Descricao = $"Produto Exemplo {j}",
                        Unidade = "m",
                        Quantidade = qtd,
                        ValorUnitario = valorUnit,
                        ValorTotal = valorUnit * qtd
                    });
                    valorTotal += valorUnit * qtd;
                }

                pedidoExemplo.TotalGeral = valorTotal;
                pedidosFinalizados.Add(pedidoExemplo);
            }
        }

        private void AtualizarBotoesAnos(Panel panelBotoesAnos)
        {
            panelBotoesAnos.Controls.Clear();

            int xPos = 0;
            for (int i = 0; i < 5 && (indiceAnoInicial + i) < todosAnos.Length; i++)
            {
                int ano = todosAnos[indiceAnoInicial + i];

                Button btnAno = new Button();
                btnAno.Text = ano.ToString();
                btnAno.Name = $"btnAno{ano}";
                btnAno.Location = new Point(xPos, 0);
                btnAno.Size = new Size(100, 40);
                btnAno.Font = new Font("Arial", 12F, FontStyle.Bold);
                btnAno.BackColor = anoSelecionado == ano ? Color.FromArgb(198, 143, 86) : Color.White;
                btnAno.ForeColor = anoSelecionado == ano ? Color.White : Color.FromArgb(57, 27, 1);
                btnAno.FlatStyle = FlatStyle.Flat;
                btnAno.FlatAppearance.BorderSize = 0;
                btnAno.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
                btnAno.Cursor = Cursors.Hand;
                btnAno.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnAno.Width, btnAno.Height, 15, 15));
                btnAno.Tag = ano;
                btnAno.Click += BtnAno_Click;

                panelBotoesAnos.Controls.Add(btnAno);
                xPos += 115;
            }
        }

        private void BtnSetaEsqAnos_Click(object sender, EventArgs e)
        {
            if (indiceAnoInicial > 0)
            {
                indiceAnoInicial--;

                Panel containerAnos = panelHistorico.Controls.Find("containerAnos", false).FirstOrDefault() as Panel;
                if (containerAnos != null)
                {
                    Panel panelBotoesAnos = containerAnos.Controls.Find("panelBotoesAnos", false).FirstOrDefault() as Panel;
                    if (panelBotoesAnos != null)
                    {
                        AtualizarBotoesAnos(panelBotoesAnos);
                    }
                }
            }
        }

        private void BtnSetaDirAnos_Click(object sender, EventArgs e)
        {
            if (indiceAnoInicial + 5 < todosAnos.Length)
            {
                indiceAnoInicial++;

                Panel containerAnos = panelHistorico.Controls.Find("containerAnos", false).FirstOrDefault() as Panel;
                if (containerAnos != null)
                {
                    Panel panelBotoesAnos = containerAnos.Controls.Find("panelBotoesAnos", false).FirstOrDefault() as Panel;
                    if (panelBotoesAnos != null)
                    {
                        AtualizarBotoesAnos(panelBotoesAnos);
                    }
                }
            }
        }

        private void BtnAno_Click(object sender, EventArgs e)
        {
            Button btnClicado = sender as Button;
            if (btnClicado == null) return;

            anoSelecionado = (int)btnClicado.Tag;

            // Atualizar cores de todos os botões de ano visíveis
            Panel containerAnos = panelHistorico.Controls.Find("containerAnos", false).FirstOrDefault() as Panel;
            if (containerAnos != null)
            {
                Panel panelBotoesAnos = containerAnos.Controls.Find("panelBotoesAnos", false).FirstOrDefault() as Panel;
                if (panelBotoesAnos != null)
                {
                    AtualizarBotoesAnos(panelBotoesAnos);
                }
            }
        }

        private void BtnMes_Click(object sender, EventArgs e)
        {
            Button btnClicado = sender as Button;
            if (btnClicado == null) return;

            mesSelecionado = btnClicado.Tag as string;

            if (anoSelecionado == 0)
            {
                MessageBox.Show("Por favor, selecione um ano primeiro!",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Resetar cores de todos os botões de mês
            Panel containerMeses = panelHistorico.Controls.Find("containerMeses", false).FirstOrDefault() as Panel;
            if (containerMeses != null)
            {
                foreach (Control ctrl in containerMeses.Controls)
                {
                    if (ctrl is Button btn && btn.Tag is string)
                    {
                        btn.BackColor = Color.FromArgb(198, 143, 86);
                        btn.ForeColor = Color.FromArgb(57, 27, 1);
                    }
                }
            }

            // Destacar botão selecionado
            btnClicado.BackColor = Color.FromArgb(144, 238, 144);
            btnClicado.ForeColor = Color.Black;

            // Mostrar lista de pedidos do mês/ano selecionados
            MostrarListaPedidosMes(anoSelecionado, mesSelecionado);
        }

        private void MostrarListaPedidosMes(int ano, string mes)
        {
            // Converter nome do mês para número
            string[] meses = { "JANEIRO", "FEVEREIRO", "MARÇO", "ABRIL", "MAIO", "JUNHO",
                      "JULHO", "AGOSTO", "SETEMBRO", "OUTUBRO", "NOVEMBRO", "DEZEMBRO" };
            int numeroMes = Array.IndexOf(meses, mes) + 1;

            // Filtrar pedidos finalizados pelo ano e mês
            var pedidosFiltrados = pedidosFinalizados.Where(p =>
                p.DataEmissao.Year == ano &&
                p.DataEmissao.Month == numeroMes
            ).ToList();

            if (pedidosFiltrados.Count == 0)
            {
                MessageBox.Show($"Nenhum pedido finalizado encontrado para {mes}/{ano}.",
                    "Histórico", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Abrir tela de lista de histórico
            using (FormListaHistorico formLista = new FormListaHistorico(pedidosFiltrados, mes, ano))
            {
                formLista.ShowDialog();
            }
        }

        private void BtnBackup_Click(object sender, EventArgs e)
        {
            // Abrir tela de backup
            using (FormBackup formBackup = new FormBackup())
            {
                formBackup.ShowDialog();
            }
        }

        private void btnHistorico_Click(object sender, EventArgs e)
        {
            if (panelOrcamento != null) panelOrcamento.Visible = false;
            if (panelEstoque != null) panelEstoque.Visible = false;
            if (panelPedidos != null) panelPedidos.Visible = false;
            if (panelCadastro != null) panelCadastro.Visible = false;
            if (panelTitulos != null) panelTitulos.Visible = false;

            if (panelHistorico != null)
            {
                panelHistorico.Visible = true;
                panelHistorico.BringToFront();
            }

            ResetarCoresBotoes();
        }

        #endregion
    }
}