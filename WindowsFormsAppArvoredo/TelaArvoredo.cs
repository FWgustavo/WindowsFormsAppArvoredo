using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
        private List<Madeira> madeira = new List<Madeira>();
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

        // VARIÁVEIS DO CAIXA
        private DateTime dataAberturaCaixa = DateTime.Now;
        private bool caixaAberto = false;
        private List<TransacaoCaixa> transacoesCaixa = new List<TransacaoCaixa>();

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


        // ============================================
        // ADICIONAR ESTAS PROPRIEDADES À CLASSE TelaArvoredo
        // ============================================

        private int usuarioIdAtual = 1; // ID do usuário logado - deve vir do sistema de login
        private bool usandoAPI = true;  // Flag para controlar se usa API ou dados locais

        // ============================================
        // SUBSTITUIR O MÉTODO TelaArvoredo_Load EXISTENTE
        // ============================================

        private async void TelaArvoredo_Load(object sender, EventArgs e)
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
            btnAtualizarEstoque.FlatAppearance.BorderSize = 0;
            btnRelatorioEstoque.TabStop = false;
            btnRelatorioEstoque.FlatAppearance.BorderSize = 0;
            btnOrcamento.TabStop = false;
            btnOrcamento.FlatAppearance.BorderSize = 0;
            btnNewOrc.TabStop = false;
            btnNewOrc.FlatAppearance.BorderSize = 0;

            // Correções de painéis (mantém código original)
            if (panelTitulos != null && panelTitulos.Parent == panelOrcamento)
            {
                panelOrcamento.Controls.Remove(panelTitulos);
                this.Controls.Add(panelTitulos);
            }

            if (panelCadastro != null && panelCadastro.Parent == panel2)
            {
                panel2.Controls.Remove(panelCadastro);
                this.Controls.Add(panelCadastro);
            }

            if (panelCaixa != null && panelCaixa.Parent == panelHistorico)
            {
                panelHistorico.Controls.Remove(panelCaixa);
                this.Controls.Add(panelCaixa);
            }

            // Configurar posições e tamanhos
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

            if (panelCaixa != null)
            {
                panelCaixa.Location = new Point(301, 74);
                panelCaixa.Size = new Size(783, 587);
                panelCaixa.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            }

            // Ocultar todos os painéis
            if (panelOrcamento != null) panelOrcamento.Visible = false;
            if (panelEstoque != null) panelEstoque.Visible = false;
            if (panelPedidos != null) panelPedidos.Visible = false;
            if (panelCadastro != null) panelCadastro.Visible = false;
            if (panelTitulos != null) panelTitulos.Visible = false;
            if (panelHistorico != null) panelHistorico.Visible = false;
            if (panelCaixa != null) panelCaixa.Visible = false;

            ConfigurarListViewOrcamentos();
            ConfigurarEstoque();
            ConfigurarPedidos();
            ConfigurarPanelTitulos();

            // NOVA IMPLEMENTAÇÃO: Carrega produtos e orçamentos da API
            await CarregarDadosIniciais();

            CarregarDadosExemploClientes();
            ConfigurarPainelCadastro();
            ConfigurarPanelHistorico();
            ConfigurarPanelCaixa();
            VincularEventos();
            panelDegrade?.Invalidate();

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
            if (btnCaixa != null)
            {
                btnCaixa.Click -= btnCaixa_Click;
                btnCaixa.Click += btnCaixa_Click;
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

        private async void listViewOrcamentos_MouseClick(object sender, MouseEventArgs e)
        {
            var hit = listViewOrcamentos.HitTest(e.Location);
            if (hit.Item != null && hit.SubItem != null)
            {
                if (hit.Item.SubItems.IndexOf(hit.SubItem) == listViewOrcamentos.Columns.Count - 1)
                {
                    var result = MessageBox.Show(
                        "Tem certeza que deseja excluir este orçamento?",
                        "Confirmar Exclusão",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        Orcamento toRemove = (Orcamento)hit.Item.Tag;
                        await ExcluirOrcamentoAsync(toRemove);
                    }
                }
            }
        }

        // ============================================
        // NOVO MÉTODO: Exclui orçamento
        // ============================================

        private async Task ExcluirOrcamentoAsync(Orcamento orcamento)
        {
            try
            {
                if (usandoAPI && orcamento.Id > 0)
                {
                    this.Cursor = Cursors.WaitCursor;

                    bool sucesso = await OrcamentoService.DeletarOrcamentoAsync(orcamento.Id);

                    if (sucesso)
                    {
                        orcamentos.Remove(orcamento);
                        AtualizarListViewOrcamentos();

                        this.Cursor = Cursors.Default;

                        MessageBox.Show(
                            "Orçamento excluído com sucesso!",
                            "Sucesso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }
                else
                {
                    // Modo local
                    orcamentos.Remove(orcamento);
                    AtualizarListViewOrcamentos();

                    MessageBox.Show(
                        "Orçamento excluído localmente!",
                        "Sucesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(
                    $"Erro ao excluir orçamento:\n{ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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


        private async Task CarregarDadosIniciais()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Carrega produtos da API
                await CarregarProdutosDaAPIAsync();

                // Carrega orçamentos da API
                if (usandoAPI)
                {
                    await CarregarOrcamentosDaAPIAsync();
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(
                    $"Erro ao carregar dados iniciais:\n{ex.Message}\n\nContinuando com dados locais.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                usandoAPI = false;
            }
        }

        // ============================================
        // NOVO MÉTODO: Carrega orçamentos da API
        // ============================================

        private async Task CarregarOrcamentosDaAPIAsync()
        {
            try
            {
                var orcamentosCarregados = await OrcamentoService.CarregarOrcamentosAsync();

                orcamentos.Clear();
                foreach (var orc in orcamentosCarregados)
                {
                    orcamentos.Add(orc);
                }

                AtualizarListViewOrcamentos();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar orçamentos da API: {ex.Message}");
            }
        }

        private async Task SalvarNovoOrcamentoAsync(Orcamento novoOrcamento)
        {
            try
            {
                if (usandoAPI)
                {
                    this.Cursor = Cursors.WaitCursor;

                    var orcamentoAPI = await OrcamentoService.CriarOrcamentoAsync(
                        novoOrcamento,
                        usuarioIdAtual
                    );

                    // Atualiza com ID da API
                    novoOrcamento.Id = orcamentoAPI.id;
                    novoOrcamento.Status = "Pendente";

                    orcamentos.Add(novoOrcamento);
                    AtualizarListViewOrcamentos();

                    this.Cursor = Cursors.Default;

                    MessageBox.Show(
                        $"Orçamento #{novoOrcamento.Id} salvo com sucesso!\n\n" +
                        $"Cliente: {novoOrcamento.Cliente}\n" +
                        $"Total: {novoOrcamento.TotalGeral:C}\n" +
                        $"Itens: {novoOrcamento.QuantidadeItens}",
                        "Orçamento Salvo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    // Modo local (código original)
                    novoOrcamento.Id = orcamentos.Count + 1;
                    novoOrcamento.Status = "Pendente";
                    orcamentos.Add(novoOrcamento);
                    AtualizarListViewOrcamentos();

                    MessageBox.Show(
                        $"Orçamento #{novoOrcamento.Id} salvo localmente!\n\n" +
                        $"Cliente: {novoOrcamento.Cliente}\n" +
                        $"Total: {novoOrcamento.TotalGeral:C}\n" +
                        $"Itens: {novoOrcamento.QuantidadeItens}",
                        "Orçamento Salvo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(
                    $"Erro ao salvar orçamento:\n{ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ============================================
        // NOVO MÉTODO: Confirma orçamento (transforma em venda)
        // ============================================

        private async Task ConfirmarOrcamentoAsync(Orcamento novoPedido)
        {
            try
            {
                if (usandoAPI)
                {
                    this.Cursor = Cursors.WaitCursor;

                    // Primeiro salva como orçamento
                    var orcamentoAPI = await OrcamentoService.CriarOrcamentoAsync(
                        novoPedido,
                        usuarioIdAtual
                    );

                    // Depois converte para venda
                    var vendaAPI = await OrcamentoService.ConverterOrcamentoParaVendaAsync(
                        orcamentoAPI.id,
                        usuarioIdAtual
                    );

                    // Adiciona aos pedidos locais
                    novoPedido.Id = vendaAPI.id;
                    novoPedido.Status = "Confirmado";
                    pedidos.Add(novoPedido);

                    AtualizarPanelPedidos();

                    this.Cursor = Cursors.Default;

                    MessageBox.Show(
                        $"Pedido #{novoPedido.Id} criado com sucesso!\n\n" +
                        $"Cliente: {novoPedido.Cliente}\n" +
                        $"Total: {novoPedido.TotalGeral:C}\n" +
                        $"Itens: {novoPedido.QuantidadeItens}",
                        "Pedido Confirmado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    // Modo local (código original)
                    novoPedido.Id = pedidos.Count + 1;
                    novoPedido.Status = "Confirmado";
                    pedidos.Add(novoPedido);
                    AtualizarPanelPedidos();

                    MessageBox.Show(
                        $"Pedido #{novoPedido.Id} criado localmente!\n\n" +
                        $"Cliente: {novoPedido.Cliente}\n" +
                        $"Total: {novoPedido.TotalGeral:C}\n" +
                        $"Itens: {novoPedido.QuantidadeItens}",
                        "Pedido Confirmado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(
                    $"Erro ao confirmar orçamento:\n{ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        // ============================================
        // SUBSTITUIR O MÉTODO AbrirOrcamentoParaEdicao EXISTENTE
        // ============================================

        private async void AbrirOrcamentoParaEdicao(Orcamento orcamento)
        {
            using (var telaOrcamento = new TelaOrcamento(produtos, orcamento))
            {
                var resultado = telaOrcamento.ShowDialog();

                if (resultado == DialogResult.OK)
                {
                    if (telaOrcamento.OrcamentoConfirmado)
                    {
                        // Confirmar orçamento existente
                        await ConfirmarOrcamentoExistenteAsync(orcamento, telaOrcamento.OrcamentoCriado);
                    }
                    else if (telaOrcamento.OrcamentoSalvo)
                    {
                        // Atualizar orçamento existente
                        await AtualizarOrcamentoExistenteAsync(orcamento, telaOrcamento.OrcamentoCriado);
                    }
                }
            }
        }

        // ============================================
        // NOVO MÉTODO: Atualiza orçamento existente
        // ============================================

        private async Task AtualizarOrcamentoExistenteAsync(Orcamento orcamentoOriginal, Orcamento orcamentoAtualizado)
        {
            try
            {
                if (usandoAPI)
                {
                    this.Cursor = Cursors.WaitCursor;

                    await OrcamentoService.AtualizarOrcamentoAsync(
                        orcamentoOriginal.Id,
                        orcamentoAtualizado
                    );

                    // Atualiza localmente
                    int index = orcamentos.IndexOf(orcamentoOriginal);
                    if (index >= 0)
                    {
                        orcamentoAtualizado.Id = orcamentoOriginal.Id;
                        orcamentos[index] = orcamentoAtualizado;
                    }

                    AtualizarListViewOrcamentos();

                    this.Cursor = Cursors.Default;

                    MessageBox.Show(
                        "Orçamento atualizado com sucesso!",
                        "Orçamento Salvo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    // Modo local (código original)
                    int index = orcamentos.IndexOf(orcamentoOriginal);
                    if (index >= 0)
                    {
                        orcamentoAtualizado.Id = orcamentoOriginal.Id;
                        orcamentos[index] = orcamentoAtualizado;
                    }
                    AtualizarListViewOrcamentos();

                    MessageBox.Show(
                        "Orçamento atualizado localmente!",
                        "Orçamento Salvo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(
                    $"Erro ao atualizar orçamento:\n{ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ============================================
        // NOVO MÉTODO: Confirma orçamento existente
        // ============================================

        private async Task ConfirmarOrcamentoExistenteAsync(Orcamento orcamentoOriginal, Orcamento pedidoAtualizado)
        {
            try
            {
                if (usandoAPI)
                {
                    this.Cursor = Cursors.WaitCursor;

                    // Converte o orçamento existente para venda
                    var vendaAPI = await OrcamentoService.ConverterOrcamentoParaVendaAsync(
                        orcamentoOriginal.Id,
                        usuarioIdAtual
                    );

                    // Remove dos orçamentos
                    orcamentos.Remove(orcamentoOriginal);

                    // Adiciona aos pedidos
                    pedidoAtualizado.Id = vendaAPI.id;
                    pedidoAtualizado.Status = "Confirmado";
                    pedidos.Add(pedidoAtualizado);

                    // Deleta o orçamento original
                    await OrcamentoService.DeletarOrcamentoAsync(orcamentoOriginal.Id);

                    AtualizarListViewOrcamentos();
                    AtualizarPanelPedidos();

                    this.Cursor = Cursors.Default;

                    MessageBox.Show(
                        $"Pedido #{pedidoAtualizado.Id} confirmado com sucesso!\n\n" +
                        $"Cliente: {pedidoAtualizado.Cliente}\n" +
                        $"Total: {pedidoAtualizado.TotalGeral:C}",
                        "Pedido Confirmado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    // Modo local (código original)
                    orcamentos.Remove(orcamentoOriginal);
                    pedidoAtualizado.Id = pedidos.Count + 1;
                    pedidoAtualizado.Status = "Confirmado";
                    pedidos.Add(pedidoAtualizado);

                    AtualizarListViewOrcamentos();
                    AtualizarPanelPedidos();

                    MessageBox.Show(
                        $"Pedido #{pedidoAtualizado.Id} confirmado localmente!\n\n" +
                        $"Cliente: {pedidoAtualizado.Cliente}\n" +
                        $"Total: {pedidoAtualizado.TotalGeral:C}",
                        "Pedido Confirmado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(
                    $"Erro ao confirmar orçamento:\n{ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ============================================
        // SUBSTITUIR O MÉTODO btnNewOrc_Click EXISTENTE
        // ============================================

        private async void btnNewOrc_Click(object sender, EventArgs e)
        {
            using (var telaOrcamento = new TelaOrcamento(produtos))
            {
                var resultado = telaOrcamento.ShowDialog();

                if (resultado == DialogResult.OK && telaOrcamento.OrcamentoCriado != null)
                {
                    if (telaOrcamento.OrcamentoConfirmado)
                    {
                        // Orçamento confirmado - converte para pedido/venda
                        await ConfirmarOrcamentoAsync(telaOrcamento.OrcamentoCriado);
                    }
                    else if (telaOrcamento.OrcamentoSalvo)
                    {
                        // Orçamento salvo como pendente
                        await SalvarNovoOrcamentoAsync(telaOrcamento.OrcamentoCriado);
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

            listViewEstoque.OwnerDraw = false;

            listViewEstoque.DrawItem -= ListViewEstoque_DrawItem;
            listViewEstoque.DrawSubItem -= ListViewEstoque_DrawSubItem;
            listViewEstoque.DrawColumnHeader -= ListViewEstoque_DrawColumnHeader;

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

                    bool salvou = await SalvarProdutoNaAPIAsync(novo);

                    if (salvou)
                    {
                        await CarregarProdutosDaAPIAsync();

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
                            Tipo = prodAPI.madeira?.nome ?? "Produto Genérico",
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
            }
        }

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
                    ativo = true,
                    madeiraId = produto.MadeiraId,
                    tamanhoId = produto.TamanhoId,
                };

                if (produto.Sequencia > 0)
                {
                    await ApiClient.PutAsync<ProdutoAPICreate, ProdutoAPI>(
                        $"/produtos/{produto.Sequencia}",
                        produtoAPI);
                }
                else
                {
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

        private string ObterNomeMadeira(int? madeiraId)
        {
            if (!madeiraId.HasValue) return "Sem tipo";

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
                bool excluiu = await ExcluirProdutoDaAPIAsync(produto.Sequencia);

                if (excluiu)
                {
                    produtos.Remove(produto);

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

            Panel containerPrincipal = new Panel();
            containerPrincipal.Name = "containerPrincipal";
            containerPrincipal.Location = new Point(30, 30);
            containerPrincipal.Size = new Size(720, 530);
            containerPrincipal.BackColor = Color.FromArgb(239, 212, 172);
            containerPrincipal.BorderStyle = BorderStyle.FixedSingle;
            panelCadastro.Controls.Add(containerPrincipal);

            Panel panelAbas = new Panel();
            panelAbas.Name = "panelAbas";
            panelAbas.Location = new Point(20, 20);
            panelAbas.Size = new Size(680, 50);
            panelAbas.BackColor = Color.Transparent;
            containerPrincipal.Controls.Add(panelAbas);

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

            Label lblStatus = new Label();
            lblStatus.Text = usuario.Ativo ? "✅ ATIVO" : "❌ INATIVO";
            lblStatus.Location = new Point(480, 8);
            lblStatus.Size = new Size(145, 24);
            lblStatus.Font = new Font("Gagalin", 9F, FontStyle.Bold);
            lblStatus.ForeColor = usuario.Ativo ? Color.Green : Color.Red;
            lblStatus.TextAlign = ContentAlignment.MiddleRight;
            panelNome.Controls.Add(lblStatus);

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
            if (panelCaixa != null) panelCaixa.Visible = false;
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
            if (panelCaixa != null) panelCaixa.Visible = false;
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
            if (panelCaixa != null) panelCaixa.Visible = false;
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
            if (panelCaixa != null) panelCaixa.Visible = false;

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

        private void AbrirDetalhesPedido(Orcamento pedido)
        {
            using (TelaTitulos telaDetalhes = new TelaTitulos(pedido))
            {
                var resultado = telaDetalhes.ShowDialog();

                if (resultado == DialogResult.OK)
                {
                    pedido.Status = "Finalizado";
                    pedidosFinalizados.Add(pedido);

                    // ADICIONAR TRANSAÇÃO NO CAIXA
                    TransacaoCaixa novaTransacao = new TransacaoCaixa
                    {
                        Data = DateTime.Now,
                        Descricao = $"RECEBIDO DE {pedido.Cliente.ToUpper()}",
                        Valor = pedido.TotalGeral,
                        Tipo = "L" // Lucro
                    };

                    transacoesCaixa.Add(novaTransacao);

                    // Remover dos pedidos pendentes
                    pedidos.Remove(pedido);
                    AtualizarPanelTitulos();

                    MessageBox.Show(
                        $"Pedido de {pedido.Cliente} finalizado com sucesso!\n" +
                        $"Valor de {pedido.TotalGeral:C} adicionado ao caixa como Lucro.\n" +
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
            if (panelCaixa != null) panelCaixa.Visible = false;

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
            if (panelCaixa == null) return;

            panelCaixa.BackColor = Color.Transparent;
            panelCaixa.Controls.Clear();

            // Adicionar transações de exemplo
            CarregarTransacoesExemplo();

            // Criar painel principal do caixa
            CriarPainelCaixaPrincipal();
        }

        private void CarregarTransacoesExemplo()
        {
            // Não carrega exemplos - inicia vazio
            transacoesCaixa.Clear();
        }

        private void CriarPainelCaixaPrincipal()
        {
            Panel containerPrincipal = new Panel();
            containerPrincipal.Name = "containerCaixaPrincipal";
            containerPrincipal.Location = new Point(20, 20);
            containerPrincipal.Size = new Size(740, 540);
            containerPrincipal.BackColor = Color.FromArgb(239, 212, 172);
            containerPrincipal.BorderStyle = BorderStyle.FixedSingle;
            panelCaixa.Controls.Add(containerPrincipal);

            // Cabeçalho
            Label lblAbrirCaixa = new Label();
            lblAbrirCaixa.Text = $"ABRIR CAIXA    {dataAberturaCaixa:dd/MM/yyyy}";
            lblAbrirCaixa.Location = new Point(20, 15);
            lblAbrirCaixa.Size = new Size(400, 25);
            lblAbrirCaixa.Font = new Font("Arial", 12F, FontStyle.Bold);
            lblAbrirCaixa.ForeColor = Color.FromArgb(57, 27, 1);
            containerPrincipal.Controls.Add(lblAbrirCaixa);

            // Labels de cabeçalho
            Label lblCaixaDiario = new Label();
            lblCaixaDiario.Text = "CAIXA DIÁRIO";
            lblCaixaDiario.Location = new Point(430, 15);
            lblCaixaDiario.Size = new Size(120, 20);
            lblCaixaDiario.Font = new Font("Arial", 9F, FontStyle.Bold);
            lblCaixaDiario.ForeColor = Color.FromArgb(57, 27, 1);
            lblCaixaDiario.TextAlign = ContentAlignment.TopRight;
            containerPrincipal.Controls.Add(lblCaixaDiario);

            Button btnFecharCaixa = new Button();
            btnFecharCaixa.Text = "FECHAR CAIXA";
            btnFecharCaixa.Location = new Point(560, 12);
            btnFecharCaixa.Size = new Size(160, 26);
            btnFecharCaixa.Font = new Font("Arial", 9F, FontStyle.Bold);
            btnFecharCaixa.BackColor = Color.FromArgb(255, 140, 0);
            btnFecharCaixa.ForeColor = Color.White;
            btnFecharCaixa.FlatStyle = FlatStyle.Flat;
            btnFecharCaixa.FlatAppearance.BorderSize = 0;
            btnFecharCaixa.Cursor = Cursors.Hand;
            btnFecharCaixa.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnFecharCaixa.Width, btnFecharCaixa.Height, 15, 15));
            btnFecharCaixa.Click += (s, e) => FecharCaixa();
            containerPrincipal.Controls.Add(btnFecharCaixa);

            // Container de transações
            Panel containerTransacoes = new Panel();
            containerTransacoes.Name = "containerTransacoes";
            containerTransacoes.Location = new Point(20, 55);
            containerTransacoes.Size = new Size(700, 390);
            containerTransacoes.BackColor = Color.White;
            containerTransacoes.BorderStyle = BorderStyle.FixedSingle;
            containerTransacoes.AutoScroll = true;
            containerPrincipal.Controls.Add(containerTransacoes);

            // Atualizar lista de transações
            AtualizarListaTransacoes();

            // Painel inferior
            Panel panelInferior = new Panel();
            panelInferior.Location = new Point(20, 460);
            panelInferior.Size = new Size(700, 60);
            panelInferior.BackColor = Color.Transparent;
            containerPrincipal.Controls.Add(panelInferior);

            // Campos de entrada
            Label lblDigiteLabel = new Label();
            lblDigiteLabel.Text = "DIGITE A DESPESA/LUCRO:";
            lblDigiteLabel.Location = new Point(10, 12);
            lblDigiteLabel.Size = new Size(180, 20);
            lblDigiteLabel.Font = new Font("Arial", 9F, FontStyle.Regular);
            lblDigiteLabel.ForeColor = Color.FromArgb(57, 27, 1);
            panelInferior.Controls.Add(lblDigiteLabel);

            TextBox txtDespesaLucro = new TextBox();
            txtDespesaLucro.Name = "txtDespesaLucro";
            txtDespesaLucro.Location = new Point(10, 35);
            txtDespesaLucro.Size = new Size(230, 25);
            txtDespesaLucro.Font = new Font("Arial", 10F);
            txtDespesaLucro.BorderStyle = BorderStyle.FixedSingle;
            panelInferior.Controls.Add(txtDespesaLucro);

            Label lblValorLabel = new Label();
            lblValorLabel.Text = "VALOR:";
            lblValorLabel.Location = new Point(250, 12);
            lblValorLabel.Size = new Size(60, 20);
            lblValorLabel.Font = new Font("Arial", 9F, FontStyle.Regular);
            lblValorLabel.ForeColor = Color.FromArgb(57, 27, 1);
            panelInferior.Controls.Add(lblValorLabel);

            TextBox txtValor = new TextBox();
            txtValor.Name = "txtValor";
            txtValor.Location = new Point(250, 35);
            txtValor.Size = new Size(150, 25);
            txtValor.Font = new Font("Arial", 10F);
            txtValor.BorderStyle = BorderStyle.FixedSingle;
            panelInferior.Controls.Add(txtValor);

            Label lblDLLabel = new Label();
            lblDLLabel.Text = "D/L";
            lblDLLabel.Location = new Point(410, 12);
            lblDLLabel.Size = new Size(40, 20);
            lblDLLabel.Font = new Font("Arial", 9F, FontStyle.Regular);
            lblDLLabel.ForeColor = Color.FromArgb(57, 27, 1);
            panelInferior.Controls.Add(lblDLLabel);

            ComboBox cmbDL = new ComboBox();
            cmbDL.Name = "cmbDL";
            cmbDL.Location = new Point(410, 35);
            cmbDL.Size = new Size(60, 25);
            cmbDL.Font = new Font("Arial", 10F);
            cmbDL.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDL.Items.AddRange(new object[] { "D", "L" });
            cmbDL.SelectedIndex = 0;
            panelInferior.Controls.Add(cmbDL);

            // Label Total
            Label lblTotal = new Label();
            lblTotal.Text = "TOTAL:";
            lblTotal.Location = new Point(480, 12);
            lblTotal.Size = new Size(80, 20);
            lblTotal.Font = new Font("Arial", 10F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(57, 27, 1);
            lblTotal.TextAlign = ContentAlignment.MiddleRight;
            panelInferior.Controls.Add(lblTotal);

            Label lblValorTotal = new Label();
            lblValorTotal.Name = "lblValorTotal";
            lblValorTotal.Text = CalcularTotalCaixa().ToString("N2");
            lblValorTotal.Location = new Point(565, 12);
            lblValorTotal.Size = new Size(120, 20);
            lblValorTotal.Font = new Font("Arial", 10F, FontStyle.Bold);
            lblValorTotal.ForeColor = Color.FromArgb(57, 27, 1);
            lblValorTotal.TextAlign = ContentAlignment.MiddleRight;
            panelInferior.Controls.Add(lblValorTotal);

            // Botão Adicionar
            Button btnAdicionar = new Button();
            btnAdicionar.Name = "btnAdicionar";
            btnAdicionar.Text = "ADICIONAR";
            btnAdicionar.Location = new Point(480, 35);
            btnAdicionar.Size = new Size(95, 25);
            btnAdicionar.Font = new Font("Arial", 9F, FontStyle.Bold);
            btnAdicionar.BackColor = Color.FromArgb(144, 238, 144);
            btnAdicionar.ForeColor = Color.Black;
            btnAdicionar.FlatStyle = FlatStyle.Flat;
            btnAdicionar.FlatAppearance.BorderSize = 1;
            btnAdicionar.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnAdicionar.Cursor = Cursors.Hand;
            btnAdicionar.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnAdicionar.Width, btnAdicionar.Height, 15, 15));
            btnAdicionar.Click += BtnAdicionarTransacao_Click;
            panelInferior.Controls.Add(btnAdicionar);

            // Botão Gerar Relatório Anual
            Button btnGerarRelatorio = new Button();
            btnGerarRelatorio.Text = "GERAR RELATÓRIO ANUAL";
            btnGerarRelatorio.Location = new Point(580, 35);
            btnGerarRelatorio.Size = new Size(105, 25);
            btnGerarRelatorio.Font = new Font("Arial", 7F, FontStyle.Bold);
            btnGerarRelatorio.BackColor = Color.FromArgb(239, 212, 172);
            btnGerarRelatorio.ForeColor = Color.FromArgb(57, 27, 1);
            btnGerarRelatorio.FlatStyle = FlatStyle.Flat;
            btnGerarRelatorio.FlatAppearance.BorderSize = 2;
            btnGerarRelatorio.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnGerarRelatorio.Cursor = Cursors.Hand;
            btnGerarRelatorio.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnGerarRelatorio.Width, btnGerarRelatorio.Height, 15, 15));
            btnGerarRelatorio.Click += BtnGerarRelatorioAnual_Click;
            panelInferior.Controls.Add(btnGerarRelatorio);
        }

        private void AtualizarListaTransacoes()
        {
            if (panelCaixa == null) return;

            Panel containerPrincipal = panelCaixa.Controls.Find("containerCaixaPrincipal", false).FirstOrDefault() as Panel;
            if (containerPrincipal == null) return;

            Panel containerTransacoes = containerPrincipal.Controls.Find("containerTransacoes", false).FirstOrDefault() as Panel;
            if (containerTransacoes == null) return;

            containerTransacoes.Controls.Clear();

            int yPos = 10;
            foreach (var transacao in transacoesCaixa)
            {
                Label lblTransacao = new Label();
                lblTransacao.Text = $"        {transacao.Descricao.PadRight(50)}  {transacao.Valor.ToString("N2").PadLeft(15)}        {transacao.Tipo}";
                lblTransacao.Location = new Point(10, yPos);
                lblTransacao.Size = new Size(680, 20);
                lblTransacao.Font = new Font("Arial", 9F, FontStyle.Regular);
                lblTransacao.ForeColor = Color.FromArgb(57, 27, 1);
                containerTransacoes.Controls.Add(lblTransacao);

                yPos += 25;
            }

            if (transacoesCaixa.Count == 0)
            {
                Label lblVazio = new Label();
                lblVazio.Text = "Nenhuma transação registrada ainda.";
                lblVazio.Location = new Point(200, 180);
                lblVazio.Size = new Size(300, 30);
                lblVazio.Font = new Font("Arial", 11F, FontStyle.Italic);
                lblVazio.ForeColor = Color.Gray;
                lblVazio.TextAlign = ContentAlignment.MiddleCenter;
                containerTransacoes.Controls.Add(lblVazio);
            }
        }

        private decimal CalcularTotalCaixa()
        {
            decimal total = 0;
            foreach (var transacao in transacoesCaixa)
            {
                if (transacao.Tipo == "L")
                {
                    total += transacao.Valor;
                }
                else if (transacao.Tipo == "D")
                {
                    total -= transacao.Valor;
                }
            }
            return total;
        }

        private void AtualizarTotalCaixa()
        {
            if (panelCaixa == null) return;

            Panel containerPrincipal = panelCaixa.Controls.Find("containerCaixaPrincipal", false).FirstOrDefault() as Panel;
            if (containerPrincipal == null) return;

            Label lblValorTotal = containerPrincipal.Controls.Find("lblValorTotal", true).FirstOrDefault() as Label;
            if (lblValorTotal != null)
            {
                decimal total = CalcularTotalCaixa();
                lblValorTotal.Text = total.ToString("N2");
                lblValorTotal.ForeColor = total >= 0 ? Color.FromArgb(0, 128, 0) : Color.Red;
            }
        }

        private void BtnAdicionarTransacao_Click(object sender, EventArgs e)
        {
            if (panelCaixa == null) return;

            Panel containerPrincipal = panelCaixa.Controls.Find("containerCaixaPrincipal", false).FirstOrDefault() as Panel;
            if (containerPrincipal == null) return;

            TextBox txtDespesaLucro = containerPrincipal.Controls.Find("txtDespesaLucro", true).FirstOrDefault() as TextBox;
            TextBox txtValor = containerPrincipal.Controls.Find("txtValor", true).FirstOrDefault() as TextBox;
            ComboBox cmbDL = containerPrincipal.Controls.Find("cmbDL", true).FirstOrDefault() as ComboBox;

            if (txtDespesaLucro == null || txtValor == null || cmbDL == null) return;

            // Validações
            if (string.IsNullOrWhiteSpace(txtDespesaLucro.Text))
            {
                MessageBox.Show("Por favor, informe a descrição da transação.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDespesaLucro.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtValor.Text))
            {
                MessageBox.Show("Por favor, informe o valor da transação.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtValor.Focus();
                return;
            }

            if (!decimal.TryParse(txtValor.Text.Replace(".", "").Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal valor) || valor <= 0)
            {
                MessageBox.Show("Por favor, informe um valor válido maior que zero.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtValor.Focus();
                return;
            }

            // Adicionar transação
            TransacaoCaixa novaTransacao = new TransacaoCaixa
            {
                Data = DateTime.Now,
                Descricao = txtDespesaLucro.Text.ToUpper(),
                Valor = valor,
                Tipo = cmbDL.SelectedItem.ToString()
            };

            transacoesCaixa.Add(novaTransacao);

            // Limpar campos
            txtDespesaLucro.Clear();
            txtValor.Clear();
            cmbDL.SelectedIndex = 0;

            // Atualizar lista e total
            AtualizarListaTransacoes();
            AtualizarTotalCaixa();

            MessageBox.Show("Transação adicionada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnGerarRelatorioAnual_Click(object sender, EventArgs e)
        {
            // Ocultar painel principal e mostrar painel de seleção de ano
            Panel containerPrincipal = panelCaixa.Controls.Find("containerCaixaPrincipal", false).FirstOrDefault() as Panel;
            if (containerPrincipal != null)
            {
                containerPrincipal.Visible = false;
            }

            // Criar painel de seleção de ano (panelCaixa2)
            CriarPanelSelecaoAno();
        }

        private void CriarPanelSelecaoAno()
        {
            Panel panelSelecaoAno = new Panel();
            panelSelecaoAno.Name = "panelCaixa2";
            panelSelecaoAno.Location = new Point(20, 20);
            panelSelecaoAno.Size = new Size(740, 540);
            panelSelecaoAno.BackColor = Color.FromArgb(239, 212, 172);
            panelSelecaoAno.BorderStyle = BorderStyle.FixedSingle;
            panelCaixa.Controls.Add(panelSelecaoAno);
            panelSelecaoAno.BringToFront();

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "SELECIONE O ANO QUE DESEJA CRIAR O RELATÓRIO";
            lblTitulo.Location = new Point(50, 30);
            lblTitulo.Size = new Size(640, 30);
            lblTitulo.Font = new Font("Arial", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(57, 27, 1);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            panelSelecaoAno.Controls.Add(lblTitulo);

            // Criar botões de anos - primeira linha (2016-2019)
            int xPos = 60;
            int yPos = 100;
            int[] anosLinha1 = { 2016, 2017, 2018, 2019 };

            foreach (int ano in anosLinha1)
            {
                Button btnAno = CriarBotaoAno(ano, xPos, yPos);
                panelSelecaoAno.Controls.Add(btnAno);
                xPos += 160;
            }

            // Segunda linha (2020-2023)
            xPos = 60;
            yPos = 200;
            int[] anosLinha2 = { 2020, 2021, 2022, 2023 };

            foreach (int ano in anosLinha2)
            {
                Button btnAno = CriarBotaoAno(ano, xPos, yPos);
                panelSelecaoAno.Controls.Add(btnAno);
                xPos += 160;
            }

            // Terceira linha (2024-2025)
            xPos = 60;
            yPos = 300;
            int[] anosLinha3 = { 2024, 2025 };

            foreach (int ano in anosLinha3)
            {
                Button btnAno = CriarBotaoAno(ano, xPos, yPos);
                panelSelecaoAno.Controls.Add(btnAno);
                xPos += 160;
            }

            // Botão Voltar (X)
            Button btnVoltar = new Button();
            btnVoltar.Text = "X";
            btnVoltar.Location = new Point(680, 20);
            btnVoltar.Size = new Size(40, 40);
            btnVoltar.Font = new Font("Arial", 16F, FontStyle.Bold);
            btnVoltar.BackColor = Color.FromArgb(239, 212, 172);
            btnVoltar.ForeColor = Color.FromArgb(57, 27, 1);
            btnVoltar.FlatStyle = FlatStyle.Flat;
            btnVoltar.FlatAppearance.BorderSize = 2;
            btnVoltar.FlatAppearance.BorderColor = Color.FromArgb(57, 27, 1);
            btnVoltar.Cursor = Cursors.Hand;
            btnVoltar.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnVoltar.Width, btnVoltar.Height, 20, 20));
            btnVoltar.Click += (s, e) => VoltarParaCaixaPrincipal();
            panelSelecaoAno.Controls.Add(btnVoltar);
        }

        private Button CriarBotaoAno(int ano, int x, int y)
        {
            Button btnAno = new Button();
            btnAno.Text = ano.ToString();
            btnAno.Location = new Point(x, y);
            btnAno.Size = new Size(140, 70);
            btnAno.Font = new Font("Arial", 16F, FontStyle.Bold);
            btnAno.BackColor = Color.FromArgb(198, 143, 86);
            btnAno.ForeColor = Color.FromArgb(57, 27, 1);
            btnAno.FlatStyle = FlatStyle.Flat;
            btnAno.FlatAppearance.BorderSize = 0;
            btnAno.Cursor = Cursors.Hand;
            btnAno.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnAno.Width, btnAno.Height, 20, 20));
            btnAno.Click += (s, e) => GerarRelatorioAno(ano);

            // Efeito hover
            btnAno.MouseEnter += (s, e) =>
            {
                btnAno.BackColor = Color.FromArgb(180, 123, 57);
            };
            btnAno.MouseLeave += (s, e) =>
            {
                btnAno.BackColor = Color.FromArgb(198, 143, 86);
            };

            return btnAno;
        }

        private void GerarRelatorioAno(int ano)
        {
            MessageBox.Show(
                $"Gerando relatório anual para o ano de {ano}...\n\n" +
                $"Esta funcionalidade gerará um relatório completo com todas as transações do caixa do ano selecionado.",
                "Gerar Relatório Anual",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // Voltar para o painel principal
            VoltarParaCaixaPrincipal();
        }

        private void VoltarParaCaixaPrincipal()
        {
            // Remover painel de seleção de ano
            Panel panelSelecaoAno = panelCaixa.Controls.Find("panelCaixa2", false).FirstOrDefault() as Panel;
            if (panelSelecaoAno != null)
            {
                panelCaixa.Controls.Remove(panelSelecaoAno);
                panelSelecaoAno.Dispose();
            }

            // Mostrar painel principal novamente
            Panel containerPrincipal = panelCaixa.Controls.Find("containerCaixaPrincipal", false).FirstOrDefault() as Panel;
            if (containerPrincipal != null)
            {
                containerPrincipal.Visible = true;
                containerPrincipal.BringToFront();
            }
        }

        private void FecharCaixa()
        {
            var result = MessageBox.Show(
                "Tem certeza que deseja fechar o caixa?\n\n" +
                "Esta ação irá finalizar todas as transações do dia.",
                "Fechar Caixa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show(
                    $"Caixa fechado com sucesso!\n\n" +
                    $"Data de abertura: {dataAberturaCaixa:dd/MM/yyyy}\n" +
                    $"Data de fechamento: {DateTime.Now:dd/MM/yyyy HH:mm}",
                    "Caixa Fechado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Voltar para a tela de orçamentos
                btnOrcamento_Click(null, null);
            }
        }

        private void btnCaixa_Click(object sender, EventArgs e)
        {
            if (panelOrcamento != null) panelOrcamento.Visible = false;
            if (panelEstoque != null) panelEstoque.Visible = false;
            if (panelPedidos != null) panelPedidos.Visible = false;
            if (panelCadastro != null) panelCadastro.Visible = false;
            if (panelTitulos != null) panelTitulos.Visible = false;
            if (panelHistorico != null) panelHistorico.Visible = false;

            if (panelCaixa != null)
            {
                panelCaixa.Visible = true;
                panelCaixa.BringToFront();

                // Garantir que apenas o painel principal está visível
                Panel panelSelecaoAno = panelCaixa.Controls.Find("panelCaixa2", false).FirstOrDefault() as Panel;
                if (panelSelecaoAno != null)
                {
                    panelCaixa.Controls.Remove(panelSelecaoAno);
                    panelSelecaoAno.Dispose();
                }

                Panel containerPrincipal = panelCaixa.Controls.Find("containerCaixaPrincipal", false).FirstOrDefault() as Panel;
                if (containerPrincipal != null)
                {
                    containerPrincipal.Visible = true;
                    containerPrincipal.BringToFront();
                }
            }

            ResetarCoresBotoes();
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
            if (pedidosFinalizados.Count > 0) return;

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
            // Verifica se ainda há anos à direita para mostrar
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

            btnClicado.BackColor = Color.FromArgb(144, 238, 144);
            btnClicado.ForeColor = Color.Black;

            MostrarListaPedidosMes(anoSelecionado, mesSelecionado);
        }

        private void MostrarListaPedidosMes(int ano, string mes)
        {
            string[] meses = { "JANEIRO", "FEVEREIRO", "MARÇO", "ABRIL", "MAIO", "JUNHO",
              "JULHO", "AGOSTO", "SETEMBRO", "OUTUBRO", "NOVEMBRO", "DEZEMBRO" };
            int numeroMes = Array.IndexOf(meses, mes) + 1;

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

            using (FormListaHistorico formLista = new FormListaHistorico(pedidosFiltrados, mes, ano))
            {
                formLista.ShowDialog();
            }
        }

        private void BtnBackup_Click(object sender, EventArgs e)
        {
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
            if (panelCaixa != null) panelCaixa.Visible = false;

            if (panelHistorico != null)
            {
                panelHistorico.Visible = true;
                panelHistorico.BringToFront();
            }

            ResetarCoresBotoes();
        }

        #endregion
    }

    // Classe auxiliar para transações do caixa
    public class TransacaoCaixa
    {
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Tipo { get; set; } // "D" para Despesa, "L" para Lucro
    }
}

