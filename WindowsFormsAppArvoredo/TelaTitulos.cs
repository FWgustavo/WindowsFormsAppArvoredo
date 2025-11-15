using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class TelaTitulos : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
           int nLeft, int nTop, int nRight, int nBottom,
           int nWidthEllipse, int nHeightEllipse);

        private Orcamento pedidoSelecionado;
        private DataGridView dgvProdutos;
        private TextBox txtSemJuros;
        private TextBox txtDescontos;
        private TextBox txtAcrescimos;
        private TextBox txtTotalVista;

        public TelaTitulos(Orcamento pedido)
        {
            InitializeComponent();
            pedidoSelecionado = pedido;
        }

        private void TelaTitulos_Load(object sender, EventArgs e)
        {
            // Carregar logo
            try
            {
                pictureBox1.Image = Properties.Resources.telalogo;
            }
            catch { }

            // Configurar data
            lbldata.Text = pedidoSelecionado.DataEmissao.ToString("dd/MM/yyyy");

            // Carregar dados do cliente
            txtCliente.Text = pedidoSelecionado.Cliente;
            txtEndereco.Text = pedidoSelecionado.Endereco;
            txtCEP.Text = pedidoSelecionado.CEP;
            txtBairro.Text = pedidoSelecionado.Bairro;
            txtCidade.Text = pedidoSelecionado.Cidade;
            txtUF.Text = pedidoSelecionado.UF;
            txtCPF.Text = pedidoSelecionado.CPF_CNPJ;
            txtTEL.Text = pedidoSelecionado.Telefone;
            txtNumero.Text = pedidoSelecionado.Numero ?? "";
            txtVendedor.Text = pedidoSelecionado.Vendedor;
            txtFormaPagamento.Text = pedidoSelecionado.FormaPagamento ?? "Dinheiro";

            // Criar DataGridView para produtos
            CriarGridProdutos();

            // Criar seção de totais
            CriarSecaoTotais();

            // Aplicar bordas arredondadas ao botão
            btnFinalizarPedido.Region = Region.FromHrgn(CreateRoundRectRgn(
                0, 0, btnFinalizarPedido.Width, btnFinalizarPedido.Height, 20, 20));
        }


        private void CriarGridProdutos()
        {
            dgvProdutos = new DataGridView();
            dgvProdutos.Location = new Point(50, 360);
            dgvProdutos.Size = new Size(980, 200);
            dgvProdutos.AllowUserToAddRows = false;
            dgvProdutos.AllowUserToDeleteRows = false;
            dgvProdutos.ReadOnly = true;
            dgvProdutos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProdutos.MultiSelect = false;
            dgvProdutos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProdutos.BackgroundColor = Color.White;

            dgvProdutos.Columns.Add("SEQ", "SEQ");
            dgvProdutos.Columns.Add("DESCRICAO", "DESCRIÇÃO");
            dgvProdutos.Columns.Add("UNI", "UNI.");
            dgvProdutos.Columns.Add("QTD", "QTD.");
            dgvProdutos.Columns.Add("VLR_UNI", "VLR. UNI.");
            dgvProdutos.Columns.Add("VLR_TOTAL", "VLR. TOTAL");

            dgvProdutos.Columns["SEQ"].Width = 60;
            dgvProdutos.Columns["UNI"].Width = 80;
            dgvProdutos.Columns["QTD"].Width = 80;
            dgvProdutos.Columns["VLR_UNI"].Width = 100;
            dgvProdutos.Columns["VLR_TOTAL"].Width = 120;

            // Adicionar produtos
            foreach (var item in pedidoSelecionado.Itens)
            {
                dgvProdutos.Rows.Add(
                    item.Sequencia,
                    item.Descricao,
                    item.Unidade,
                    item.Quantidade.ToString("F2"),
                    item.ValorUnitario.ToString("C2"),
                    item.ValorTotal.ToString("C2")
                );
            }

            this.Controls.Add(dgvProdutos);
        }

        private void CriarSecaoTotais()
        {
            // Label TOTAIS
            Label lblTotais = new Label();
            lblTotais.Text = "TOTAIS";
            lblTotais.Location = new Point(500, 580);
            lblTotais.Size = new Size(100, 25);
            lblTotais.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold);
            lblTotais.BorderStyle = BorderStyle.FixedSingle;
            lblTotais.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTotais);

            // 4x Sem Juros
            Label lbl4xSemJuros = new Label();
            lbl4xSemJuros.Text = "4X SEM JUROS:";
            lbl4xSemJuros.Location = new Point(100, 620);
            lbl4xSemJuros.Size = new Size(120, 20);
            lbl4xSemJuros.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lbl4xSemJuros);

            txtSemJuros = new TextBox();
            txtSemJuros.Location = new Point(230, 618);
            txtSemJuros.Size = new Size(150, 22);
            txtSemJuros.ReadOnly = true;
            txtSemJuros.BackColor = Color.LightGray;
            decimal valorParcela = pedidoSelecionado.TotalGeral / 4;
            txtSemJuros.Text = $"4x de {valorParcela:C2}";
            this.Controls.Add(txtSemJuros);

            // Descontos
            Label lblDescontos = new Label();
            lblDescontos.Text = "DESCONTOS:";
            lblDescontos.Location = new Point(400, 620);
            lblDescontos.Size = new Size(100, 20);
            lblDescontos.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lblDescontos);

            txtDescontos = new TextBox();
            txtDescontos.Location = new Point(510, 618);
            txtDescontos.Size = new Size(150, 22);
            txtDescontos.ReadOnly = true;
            txtDescontos.BackColor = Color.LightGray;
            txtDescontos.Text = pedidoSelecionado.Desconto.ToString("C2");
            this.Controls.Add(txtDescontos);

            // Acréscimos
            Label lblAcrescimos = new Label();
            lblAcrescimos.Text = "ACRÉSCIMOS:";
            lblAcrescimos.Location = new Point(680, 620);
            lblAcrescimos.Size = new Size(100, 20);
            lblAcrescimos.Font = new Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(lblAcrescimos);

            txtAcrescimos = new TextBox();
            txtAcrescimos.Location = new Point(790, 618);
            txtAcrescimos.Size = new Size(150, 22);
            txtAcrescimos.ReadOnly = true;
            txtAcrescimos.BackColor = Color.LightGray;
            txtAcrescimos.Text = pedidoSelecionado.Acrescimo.ToString("C2");
            this.Controls.Add(txtAcrescimos);

            // Total à Vista
            Label lblTotalVista = new Label();
            lblTotalVista.Text = "TOTAL À VISTA:";
            lblTotalVista.Location = new Point(400, 680);
            lblTotalVista.Size = new Size(120, 20);
            lblTotalVista.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.Controls.Add(lblTotalVista);

            txtTotalVista = new TextBox();
            txtTotalVista.Location = new Point(520, 678);
            txtTotalVista.Size = new Size(150, 22);
            txtTotalVista.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            txtTotalVista.ReadOnly = true;
            txtTotalVista.BackColor = Color.LightGray;
            txtTotalVista.ForeColor = Color.Green;
            txtTotalVista.Text = pedidoSelecionado.TotalGeral.ToString("C2");
            this.Controls.Add(txtTotalVista);
        }

        private void BtnFinalizarPedido_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                $"Confirmar finalização do pedido?\n\n" +
                $"Cliente: {pedidoSelecionado.Cliente}\n" +
                $"Total: {pedidoSelecionado.TotalGeral:C}\n" +
                $"Forma de Pagamento: {pedidoSelecionado.FormaPagamento}\n\n" +
                $"O pedido será marcado como finalizado e salvo no histórico.",
                "Finalizar Pedido",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                // Marcar pedido como finalizado
                pedidoSelecionado.Status = "Finalizado";
                pedidoSelecionado.DataEmissao = DateTime.Now; // Atualiza para data atual

                MessageBox.Show(
                    $"Pedido finalizado com sucesso!\n\n" +
                    $"O pedido foi salvo no histórico de {pedidoSelecionado.DataEmissao:MMMM/yyyy}.",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Retornar OK para remover da lista de pendentes
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}