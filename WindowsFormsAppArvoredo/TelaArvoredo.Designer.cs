using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    partial class TelaArvoredo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMenu = new System.Windows.Forms.Panel();
            this.btnEstoque = new System.Windows.Forms.Button();
            this.panelEspacamento3 = new System.Windows.Forms.Panel();
            this.btnOrcamento = new System.Windows.Forms.Button();
            this.panelEspacamento2 = new System.Windows.Forms.Panel();
            this.btnPedidos = new System.Windows.Forms.Button();
            this.panelEspacamento1 = new System.Windows.Forms.Panel();
            this.btnTitulos = new System.Windows.Forms.Button();
            this.panelLogo = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelDegrade = new System.Windows.Forms.Panel();
            this.btnSair = new System.Windows.Forms.Button();
            this.btnCaixa = new System.Windows.Forms.Button();
            this.btnCadastro = new System.Windows.Forms.Button();
            this.btnHistorico = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelCadastro = new System.Windows.Forms.Panel();
            this.panelPedidos = new System.Windows.Forms.Panel();
            this.panelOrcamento = new System.Windows.Forms.Panel();
            this.panelTitulos = new System.Windows.Forms.Panel();
            this.listViewOrcamentos = new System.Windows.Forms.ListView();
            this.lblOrcamentosPendentes = new System.Windows.Forms.Label();
            this.btnNewOrc = new System.Windows.Forms.Button();
            this.panelEstoque = new System.Windows.Forms.Panel();
            this.lblProdutosBaixoEstoque = new System.Windows.Forms.Label();
            this.listViewEstoque = new System.Windows.Forms.ListView();
            this.lblControleEstoque = new System.Windows.Forms.Label();
            this.btnRelatorioEstoque = new System.Windows.Forms.Button();
            this.btnAtualizarEstoque = new System.Windows.Forms.Button();
            this.btnNovoProduto = new System.Windows.Forms.Button();
            this.panelHistorico = new System.Windows.Forms.Panel();
            this.panelMenu.SuspendLayout();
            this.panelLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelDegrade.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelOrcamento.SuspendLayout();
            this.panelEstoque.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.Transparent;
            this.panelMenu.Controls.Add(this.btnEstoque);
            this.panelMenu.Controls.Add(this.panelEspacamento3);
            this.panelMenu.Controls.Add(this.btnOrcamento);
            this.panelMenu.Controls.Add(this.panelEspacamento2);
            this.panelMenu.Controls.Add(this.btnPedidos);
            this.panelMenu.Controls.Add(this.panelEspacamento1);
            this.panelMenu.Controls.Add(this.btnTitulos);
            this.panelMenu.Controls.Add(this.panelLogo);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(300, 661);
            this.panelMenu.TabIndex = 0;
            // 
            // btnEstoque
            // 
            this.btnEstoque.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.btnEstoque.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnEstoque.FlatAppearance.BorderSize = 0;
            this.btnEstoque.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEstoque.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEstoque.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.btnEstoque.Location = new System.Drawing.Point(0, 542);
            this.btnEstoque.Name = "btnEstoque";
            this.btnEstoque.Size = new System.Drawing.Size(300, 84);
            this.btnEstoque.TabIndex = 4;
            this.btnEstoque.Text = "Estoque";
            this.btnEstoque.UseVisualStyleBackColor = false;
            // 
            // panelEspacamento3
            // 
            this.panelEspacamento3.BackColor = System.Drawing.Color.Transparent;
            this.panelEspacamento3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEspacamento3.Location = new System.Drawing.Point(0, 512);
            this.panelEspacamento3.Name = "panelEspacamento3";
            this.panelEspacamento3.Size = new System.Drawing.Size(300, 30);
            this.panelEspacamento3.TabIndex = 7;
            // 
            // btnOrcamento
            // 
            this.btnOrcamento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.btnOrcamento.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOrcamento.FlatAppearance.BorderSize = 0;
            this.btnOrcamento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOrcamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOrcamento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.btnOrcamento.Location = new System.Drawing.Point(0, 428);
            this.btnOrcamento.Name = "btnOrcamento";
            this.btnOrcamento.Size = new System.Drawing.Size(300, 84);
            this.btnOrcamento.TabIndex = 3;
            this.btnOrcamento.Text = "Orçamentos";
            this.btnOrcamento.UseVisualStyleBackColor = false;
            this.btnOrcamento.Click += new System.EventHandler(this.btnOrcamento_Click);
            // 
            // panelEspacamento2
            // 
            this.panelEspacamento2.BackColor = System.Drawing.Color.Transparent;
            this.panelEspacamento2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEspacamento2.Location = new System.Drawing.Point(0, 398);
            this.panelEspacamento2.Name = "panelEspacamento2";
            this.panelEspacamento2.Size = new System.Drawing.Size(300, 30);
            this.panelEspacamento2.TabIndex = 6;
            // 
            // btnPedidos
            // 
            this.btnPedidos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.btnPedidos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPedidos.FlatAppearance.BorderSize = 0;
            this.btnPedidos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPedidos.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPedidos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.btnPedidos.Location = new System.Drawing.Point(0, 314);
            this.btnPedidos.Name = "btnPedidos";
            this.btnPedidos.Size = new System.Drawing.Size(300, 84);
            this.btnPedidos.TabIndex = 2;
            this.btnPedidos.Text = "Pedidos";
            this.btnPedidos.UseVisualStyleBackColor = false;
            // 
            // panelEspacamento1
            // 
            this.panelEspacamento1.BackColor = System.Drawing.Color.Transparent;
            this.panelEspacamento1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEspacamento1.Location = new System.Drawing.Point(0, 284);
            this.panelEspacamento1.Name = "panelEspacamento1";
            this.panelEspacamento1.Size = new System.Drawing.Size(300, 30);
            this.panelEspacamento1.TabIndex = 5;
            // 
            // btnTitulos
            // 
            this.btnTitulos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.btnTitulos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTitulos.FlatAppearance.BorderSize = 0;
            this.btnTitulos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTitulos.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTitulos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.btnTitulos.Location = new System.Drawing.Point(0, 200);
            this.btnTitulos.Name = "btnTitulos";
            this.btnTitulos.Size = new System.Drawing.Size(300, 84);
            this.btnTitulos.TabIndex = 1;
            this.btnTitulos.Text = "Títulos Pendentes";
            this.btnTitulos.UseVisualStyleBackColor = false;
            // 
            // panelLogo
            // 
            this.panelLogo.BackColor = System.Drawing.Color.Transparent;
            this.panelLogo.Controls.Add(this.pictureBox1);
            this.panelLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogo.Location = new System.Drawing.Point(0, 0);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.Size = new System.Drawing.Size(300, 200);
            this.panelLogo.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::WindowsFormsAppArvoredo.Properties.Resources.logo2;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(300, 200);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panelDegrade
            // 
            this.panelDegrade.BackColor = System.Drawing.Color.Transparent;
            this.panelDegrade.Controls.Add(this.btnSair);
            this.panelDegrade.Controls.Add(this.btnCaixa);
            this.panelDegrade.Controls.Add(this.btnCadastro);
            this.panelDegrade.Controls.Add(this.btnHistorico);
            this.panelDegrade.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDegrade.Location = new System.Drawing.Point(300, 0);
            this.panelDegrade.Name = "panelDegrade";
            this.panelDegrade.Size = new System.Drawing.Size(784, 70);
            this.panelDegrade.TabIndex = 1;
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.btnSair.FlatAppearance.BorderSize = 0;
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSair.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.btnSair.Location = new System.Drawing.Point(557, 21);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(170, 34);
            this.btnSair.TabIndex = 8;
            this.btnSair.Text = "Sair";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // btnCaixa
            // 
            this.btnCaixa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.btnCaixa.FlatAppearance.BorderSize = 0;
            this.btnCaixa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaixa.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCaixa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.btnCaixa.Location = new System.Drawing.Point(381, 21);
            this.btnCaixa.Name = "btnCaixa";
            this.btnCaixa.Size = new System.Drawing.Size(170, 34);
            this.btnCaixa.TabIndex = 7;
            this.btnCaixa.Text = "Caixa";
            this.btnCaixa.UseVisualStyleBackColor = false;
            // 
            // btnCadastro
            // 
            this.btnCadastro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.btnCadastro.FlatAppearance.BorderSize = 0;
            this.btnCadastro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCadastro.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCadastro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.btnCadastro.Location = new System.Drawing.Point(216, 21);
            this.btnCadastro.Name = "btnCadastro";
            this.btnCadastro.Size = new System.Drawing.Size(159, 34);
            this.btnCadastro.TabIndex = 6;
            this.btnCadastro.Text = "Cadastro";
            this.btnCadastro.UseVisualStyleBackColor = false;
            // 
            // btnHistorico
            // 
            this.btnHistorico.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.btnHistorico.FlatAppearance.BorderSize = 0;
            this.btnHistorico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistorico.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHistorico.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.btnHistorico.Location = new System.Drawing.Point(45, 21);
            this.btnHistorico.Name = "btnHistorico";
            this.btnHistorico.Size = new System.Drawing.Size(165, 34);
            this.btnHistorico.TabIndex = 5;
            this.btnHistorico.Text = "Histórico";
            this.btnHistorico.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.panel1.Location = new System.Drawing.Point(297, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(3, 661);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.panel2.Controls.Add(this.panelCadastro);
            this.panel2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(300, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(784, 3);
            this.panel2.TabIndex = 3;
            // 
            // panelCadastro
            // 
            this.panelCadastro.Location = new System.Drawing.Point(3, 3);
            this.panelCadastro.Name = "panelCadastro";
            this.panelCadastro.Size = new System.Drawing.Size(778, 585);
            this.panelCadastro.TabIndex = 3;
            // 
            // panelPedidos
            // 
            this.panelPedidos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPedidos.AutoScroll = true;
            this.panelPedidos.BackColor = System.Drawing.Color.Transparent;
            this.panelPedidos.Location = new System.Drawing.Point(301, 74);
            this.panelPedidos.Name = "panelPedidos";
            this.panelPedidos.Size = new System.Drawing.Size(783, 587);
            this.panelPedidos.TabIndex = 6;
            // 
            // panelOrcamento
            // 
            this.panelOrcamento.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelOrcamento.BackColor = System.Drawing.Color.Transparent;
            this.panelOrcamento.Controls.Add(this.panelTitulos);
            this.panelOrcamento.Controls.Add(this.listViewOrcamentos);
            this.panelOrcamento.Controls.Add(this.lblOrcamentosPendentes);
            this.panelOrcamento.Controls.Add(this.btnNewOrc);
            this.panelOrcamento.Location = new System.Drawing.Point(301, 74);
            this.panelOrcamento.Name = "panelOrcamento";
            this.panelOrcamento.Size = new System.Drawing.Size(783, 587);
            this.panelOrcamento.TabIndex = 4;
            // 
            // panelTitulos
            // 
            this.panelTitulos.Location = new System.Drawing.Point(2, 0);
            this.panelTitulos.Name = "panelTitulos";
            this.panelTitulos.Size = new System.Drawing.Size(777, 581);
            this.panelTitulos.TabIndex = 3;
            // 
            // listViewOrcamentos
            // 
            this.listViewOrcamentos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewOrcamentos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.listViewOrcamentos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewOrcamentos.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewOrcamentos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.listViewOrcamentos.FullRowSelect = true;
            this.listViewOrcamentos.GridLines = true;
            this.listViewOrcamentos.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewOrcamentos.HideSelection = false;
            this.listViewOrcamentos.Location = new System.Drawing.Point(22, 130);
            this.listViewOrcamentos.MultiSelect = false;
            this.listViewOrcamentos.Name = "listViewOrcamentos";
            this.listViewOrcamentos.Size = new System.Drawing.Size(720, 400);
            this.listViewOrcamentos.TabIndex = 2;
            this.listViewOrcamentos.UseCompatibleStateImageBehavior = false;
            this.listViewOrcamentos.View = System.Windows.Forms.View.Details;
            // 
            // lblOrcamentosPendentes
            // 
            this.lblOrcamentosPendentes.AutoSize = true;
            this.lblOrcamentosPendentes.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrcamentosPendentes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.lblOrcamentosPendentes.Location = new System.Drawing.Point(22, 80);
            this.lblOrcamentosPendentes.Name = "lblOrcamentosPendentes";
            this.lblOrcamentosPendentes.Size = new System.Drawing.Size(330, 26);
            this.lblOrcamentosPendentes.TabIndex = 1;
            this.lblOrcamentosPendentes.Text = "ORÇAMENTOS PENDENTES";
            // 
            // btnNewOrc
            // 
            this.btnNewOrc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(238)))), ((int)(((byte)(144)))));
            this.btnNewOrc.FlatAppearance.BorderSize = 0;
            this.btnNewOrc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewOrc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewOrc.ForeColor = System.Drawing.Color.Black;
            this.btnNewOrc.Location = new System.Drawing.Point(22, 23);
            this.btnNewOrc.Name = "btnNewOrc";
            this.btnNewOrc.Size = new System.Drawing.Size(130, 30);
            this.btnNewOrc.TabIndex = 0;
            this.btnNewOrc.Text = "NOVO ORÇAMENTO";
            this.btnNewOrc.UseVisualStyleBackColor = false;
            this.btnNewOrc.Click += new System.EventHandler(this.btnNewOrc_Click);
            // 
            // panelEstoque
            // 
            this.panelEstoque.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEstoque.BackColor = System.Drawing.Color.Transparent;
            this.panelEstoque.Controls.Add(this.lblProdutosBaixoEstoque);
            this.panelEstoque.Controls.Add(this.listViewEstoque);
            this.panelEstoque.Controls.Add(this.lblControleEstoque);
            this.panelEstoque.Controls.Add(this.btnRelatorioEstoque);
            this.panelEstoque.Controls.Add(this.btnAtualizarEstoque);
            this.panelEstoque.Controls.Add(this.btnNovoProduto);
            this.panelEstoque.ForeColor = System.Drawing.Color.Transparent;
            this.panelEstoque.Location = new System.Drawing.Point(301, 74);
            this.panelEstoque.Name = "panelEstoque";
            this.panelEstoque.Size = new System.Drawing.Size(783, 587);
            this.panelEstoque.TabIndex = 5;
            // 
            // lblProdutosBaixoEstoque
            // 
            this.lblProdutosBaixoEstoque.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProdutosBaixoEstoque.AutoSize = true;
            this.lblProdutosBaixoEstoque.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProdutosBaixoEstoque.ForeColor = System.Drawing.Color.Red;
            this.lblProdutosBaixoEstoque.Location = new System.Drawing.Point(480, 80);
            this.lblProdutosBaixoEstoque.Name = "lblProdutosBaixoEstoque";
            this.lblProdutosBaixoEstoque.Size = new System.Drawing.Size(223, 17);
            this.lblProdutosBaixoEstoque.TabIndex = 8;
            this.lblProdutosBaixoEstoque.Text = "⚠️ Produtos com estoque baixo: 0";
            // 
            // listViewEstoque
            // 
            this.listViewEstoque.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewEstoque.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.listViewEstoque.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewEstoque.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewEstoque.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.listViewEstoque.FullRowSelect = true;
            this.listViewEstoque.GridLines = true;
            this.listViewEstoque.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewEstoque.HideSelection = false;
            this.listViewEstoque.Location = new System.Drawing.Point(22, 130);
            this.listViewEstoque.MultiSelect = false;
            this.listViewEstoque.Name = "listViewEstoque";
            this.listViewEstoque.Size = new System.Drawing.Size(720, 400);
            this.listViewEstoque.TabIndex = 6;
            this.listViewEstoque.UseCompatibleStateImageBehavior = false;
            this.listViewEstoque.View = System.Windows.Forms.View.Details;
            // 
            // lblControleEstoque
            // 
            this.lblControleEstoque.AutoSize = true;
            this.lblControleEstoque.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblControleEstoque.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.lblControleEstoque.Location = new System.Drawing.Point(22, 80);
            this.lblControleEstoque.Name = "lblControleEstoque";
            this.lblControleEstoque.Size = new System.Drawing.Size(302, 26);
            this.lblControleEstoque.TabIndex = 5;
            this.lblControleEstoque.Text = "CONTROLE DE ESTOQUE";
            // 
            // btnRelatorioEstoque
            // 
            this.btnRelatorioEstoque.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(218)))), ((int)(((byte)(185)))));
            this.btnRelatorioEstoque.FlatAppearance.BorderSize = 0;
            this.btnRelatorioEstoque.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelatorioEstoque.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRelatorioEstoque.ForeColor = System.Drawing.Color.Black;
            this.btnRelatorioEstoque.Location = new System.Drawing.Point(288, 23);
            this.btnRelatorioEstoque.Name = "btnRelatorioEstoque";
            this.btnRelatorioEstoque.Size = new System.Drawing.Size(120, 30);
            this.btnRelatorioEstoque.TabIndex = 7;
            this.btnRelatorioEstoque.Text = "RELATÓRIO";
            this.btnRelatorioEstoque.UseVisualStyleBackColor = false;
            // 
            // btnAtualizarEstoque
            // 
            this.btnAtualizarEstoque.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(216)))), ((int)(((byte)(230)))));
            this.btnAtualizarEstoque.FlatAppearance.BorderSize = 0;
            this.btnAtualizarEstoque.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAtualizarEstoque.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAtualizarEstoque.ForeColor = System.Drawing.Color.Black;
            this.btnAtualizarEstoque.Location = new System.Drawing.Point(155, 23);
            this.btnAtualizarEstoque.Name = "btnAtualizarEstoque";
            this.btnAtualizarEstoque.Size = new System.Drawing.Size(120, 30);
            this.btnAtualizarEstoque.TabIndex = 4;
            this.btnAtualizarEstoque.Text = "ATUALIZAR";
            this.btnAtualizarEstoque.UseVisualStyleBackColor = false;
            // 
            // btnNovoProduto
            // 
            this.btnNovoProduto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(238)))), ((int)(((byte)(144)))));
            this.btnNovoProduto.FlatAppearance.BorderSize = 0;
            this.btnNovoProduto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNovoProduto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNovoProduto.ForeColor = System.Drawing.Color.Black;
            this.btnNovoProduto.Location = new System.Drawing.Point(22, 23);
            this.btnNovoProduto.Name = "btnNovoProduto";
            this.btnNovoProduto.Size = new System.Drawing.Size(120, 30);
            this.btnNovoProduto.TabIndex = 3;
            this.btnNovoProduto.Text = "NOVO PRODUTO";
            this.btnNovoProduto.UseVisualStyleBackColor = false;
            // 
            // panelHistorico
            // 
            this.panelHistorico.BackColor = System.Drawing.Color.Transparent;
            this.panelHistorico.Location = new System.Drawing.Point(303, 73);
            this.panelHistorico.Name = "panelHistorico";
            this.panelHistorico.Size = new System.Drawing.Size(777, 585);
            this.panelHistorico.TabIndex = 0;
            // 
            // TelaArvoredo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 661);
            this.ControlBox = false;
            this.Controls.Add(this.panelHistorico);
            this.Controls.Add(this.panelOrcamento);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelDegrade);
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.panelPedidos);
            this.Controls.Add(this.panelEstoque);
            this.Name = "TelaArvoredo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TelaArvoredo";
            this.Load += new System.EventHandler(this.TelaArvoredo_Load);
            this.panelMenu.ResumeLayout(false);
            this.panelLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelDegrade.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panelOrcamento.ResumeLayout(false);
            this.panelOrcamento.PerformLayout();
            this.panelEstoque.ResumeLayout(false);
            this.panelEstoque.PerformLayout();
            this.ResumeLayout(false);

        }
        

        #endregion

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Panel panelLogo;
        private System.Windows.Forms.Button btnEstoque;
        private System.Windows.Forms.Panel panelEspacamento3;
        private System.Windows.Forms.Button btnOrcamento;
        private System.Windows.Forms.Panel panelEspacamento2;
        private System.Windows.Forms.Button btnPedidos;
        private System.Windows.Forms.Panel panelEspacamento1;
        private System.Windows.Forms.Button btnTitulos;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelDegrade;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Button btnCaixa;
        private System.Windows.Forms.Button btnCadastro;
        private System.Windows.Forms.Button btnHistorico;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelOrcamento;
        private System.Windows.Forms.Button btnNewOrc;
        private System.Windows.Forms.ListView listViewOrcamentos;
        private System.Windows.Forms.Label lblOrcamentosPendentes;
        private System.Windows.Forms.Panel panelEstoque;
        private System.Windows.Forms.ListView listViewEstoque;
        private System.Windows.Forms.Label lblControleEstoque;
        private System.Windows.Forms.Button btnNovoProduto;
        private System.Windows.Forms.Button btnAtualizarEstoque;
        private System.Windows.Forms.Button btnRelatorioEstoque;
        private System.Windows.Forms.Label lblProdutosBaixoEstoque;
        private System.Windows.Forms.Panel panelPedidos;
        private System.Windows.Forms.Panel panelCadastro;
        private Panel panelTitulos;
        private Panel panelHistorico;
    }
}