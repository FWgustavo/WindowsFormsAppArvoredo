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

        private List<Orcamento> orcamentos = new List<Orcamento>();

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
        private void btnOrcamento_Click(object sender, EventArgs e)
        {
            // Tornar o painel de orçamentos visível
            panelOrcamento.Visible = true;
            panelOrcamento.BringToFront();

            // Opcional: Destacar o botão ativo
            ResetarCoresBotoes();
            btnOrcamento.BackColor = Color.FromArgb(198, 143, 86); // Cor mais escura para indicar seleção
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
            btnTitulos.BackColor = Color.FromArgb(198, 143, 86);
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
            btnPedidos.BackColor = Color.FromArgb(198, 143, 86);
        }

        private void btnEstoque_Click(object sender, EventArgs e)
        {
            // Esconder painel atual
            panelOrcamento.Visible = false;

            // Aqui você criaria e mostraria o painel de estoque
            // panelEstoque.Visible = true;
            // panelEstoque.BringToFront();

            // Destacar botão ativo
            ResetarCoresBotoes();
            btnEstoque.BackColor = Color.FromArgb(198, 143, 86);
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close(); // Fecha a aplicação
        }
    }
}