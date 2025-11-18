namespace WindowsFormsAppArvoredo
{
    partial class FormNovoProduto
    {
        /// <summary>
        /// Variável necessária do designer.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNovoProduto));
            this.lblNome = new System.Windows.Forms.Label();
            this.lblTipo = new System.Windows.Forms.Label();
            this.lblQuantidade = new System.Windows.Forms.Label();
            this.lblQuantidadeMinima = new System.Windows.Forms.Label();
            this.lblUnidade = new System.Windows.Forms.Label();
            this.lblPreco = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.numQuantidade = new System.Windows.Forms.NumericUpDown();
            this.numQuantidadeMinima = new System.Windows.Forms.NumericUpDown();
            this.txtUnidade = new System.Windows.Forms.TextBox();
            this.numPreco = new System.Windows.Forms.NumericUpDown();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbMadeira = new System.Windows.Forms.ComboBox();
            this.cmbTamanho = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantidade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantidadeMinima)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPreco)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Location = new System.Drawing.Point(22, 20);
            this.lblNome.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(58, 13);
            this.lblNome.TabIndex = 0;
            this.lblNome.Text = "Descrição:";
            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Location = new System.Drawing.Point(22, 49);
            this.lblTipo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(31, 13);
            this.lblTipo.TabIndex = 2;
            this.lblTipo.Text = "Tipo:";
            // 
            // lblQuantidade
            // 
            this.lblQuantidade.AutoSize = true;
            this.lblQuantidade.Location = new System.Drawing.Point(22, 105);
            this.lblQuantidade.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQuantidade.Name = "lblQuantidade";
            this.lblQuantidade.Size = new System.Drawing.Size(65, 13);
            this.lblQuantidade.TabIndex = 4;
            this.lblQuantidade.Text = "Quantidade:";
            // 
            // lblQuantidadeMinima
            // 
            this.lblQuantidadeMinima.AutoSize = true;
            this.lblQuantidadeMinima.Location = new System.Drawing.Point(22, 134);
            this.lblQuantidadeMinima.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQuantidadeMinima.Name = "lblQuantidadeMinima";
            this.lblQuantidadeMinima.Size = new System.Drawing.Size(103, 13);
            this.lblQuantidadeMinima.TabIndex = 6;
            this.lblQuantidadeMinima.Text = "Quantidade Mínima:";
            // 
            // lblUnidade
            // 
            this.lblUnidade.AutoSize = true;
            this.lblUnidade.Location = new System.Drawing.Point(22, 163);
            this.lblUnidade.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUnidade.Name = "lblUnidade";
            this.lblUnidade.Size = new System.Drawing.Size(50, 13);
            this.lblUnidade.TabIndex = 8;
            this.lblUnidade.Text = "Unidade:";
            // 
            // lblPreco
            // 
            this.lblPreco.AutoSize = true;
            this.lblPreco.Location = new System.Drawing.Point(22, 193);
            this.lblPreco.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPreco.Name = "lblPreco";
            this.lblPreco.Size = new System.Drawing.Size(77, 13);
            this.lblPreco.TabIndex = 10;
            this.lblPreco.Text = "Preço Unitário:";
            // 
            // txtNome
            // 
            this.txtNome.Location = new System.Drawing.Point(135, 18);
            this.txtNome.Margin = new System.Windows.Forms.Padding(2);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(188, 20);
            this.txtNome.TabIndex = 1;
            // 
            // numQuantidade
            // 
            this.numQuantidade.Location = new System.Drawing.Point(135, 104);
            this.numQuantidade.Margin = new System.Windows.Forms.Padding(2);
            this.numQuantidade.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numQuantidade.Name = "numQuantidade";
            this.numQuantidade.Size = new System.Drawing.Size(90, 20);
            this.numQuantidade.TabIndex = 5;
            // 
            // numQuantidadeMinima
            // 
            this.numQuantidadeMinima.Location = new System.Drawing.Point(135, 133);
            this.numQuantidadeMinima.Margin = new System.Windows.Forms.Padding(2);
            this.numQuantidadeMinima.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numQuantidadeMinima.Name = "numQuantidadeMinima";
            this.numQuantidadeMinima.Size = new System.Drawing.Size(90, 20);
            this.numQuantidadeMinima.TabIndex = 7;
            // 
            // txtUnidade
            // 
            this.txtUnidade.Location = new System.Drawing.Point(135, 162);
            this.txtUnidade.Margin = new System.Windows.Forms.Padding(2);
            this.txtUnidade.Name = "txtUnidade";
            this.txtUnidade.Size = new System.Drawing.Size(91, 20);
            this.txtUnidade.TabIndex = 9;
            // 
            // numPreco
            // 
            this.numPreco.DecimalPlaces = 2;
            this.numPreco.Location = new System.Drawing.Point(135, 191);
            this.numPreco.Margin = new System.Windows.Forms.Padding(2);
            this.numPreco.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numPreco.Name = "numPreco";
            this.numPreco.Size = new System.Drawing.Size(90, 20);
            this.numPreco.TabIndex = 11;
            // 
            // btnSalvar
            // 
            this.btnSalvar.BackColor = System.Drawing.Color.ForestGreen;
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(135, 369);
            this.btnSalvar.Margin = new System.Windows.Forms.Padding(2);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(75, 23);
            this.btnSalvar.TabIndex = 12;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.Firebrick;
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(248, 369);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 13;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Tamanho:";
            // 
            // cmbMadeira
            // 
            this.cmbMadeira.FormattingEnabled = true;
            this.cmbMadeira.Location = new System.Drawing.Point(135, 49);
            this.cmbMadeira.Name = "cmbMadeira";
            this.cmbMadeira.Size = new System.Drawing.Size(121, 21);
            this.cmbMadeira.TabIndex = 16;
            // 
            // cmbTamanho
            // 
            this.cmbTamanho.FormattingEnabled = true;
            this.cmbTamanho.Location = new System.Drawing.Point(135, 79);
            this.cmbTamanho.Name = "cmbTamanho";
            this.cmbTamanho.Size = new System.Drawing.Size(121, 21);
            this.cmbTamanho.TabIndex = 17;
            // 
            // FormNovoProduto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 413);
            this.Controls.Add(this.cmbTamanho);
            this.Controls.Add(this.cmbMadeira);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblNome);
            this.Controls.Add(this.txtNome);
            this.Controls.Add(this.lblTipo);
            this.Controls.Add(this.lblQuantidade);
            this.Controls.Add(this.numQuantidade);
            this.Controls.Add(this.lblQuantidadeMinima);
            this.Controls.Add(this.numQuantidadeMinima);
            this.Controls.Add(this.lblUnidade);
            this.Controls.Add(this.txtUnidade);
            this.Controls.Add(this.lblPreco);
            this.Controls.Add(this.numPreco);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.btnCancelar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormNovoProduto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cadastro de Produto";
            this.Load += new System.EventHandler(this.FormNovoProduto_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numQuantidade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantidadeMinima)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPreco)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Declaração de Controles

        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.Label lblQuantidade;
        private System.Windows.Forms.Label lblQuantidadeMinima;
        private System.Windows.Forms.Label lblUnidade;
        private System.Windows.Forms.Label lblPreco;

        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.NumericUpDown numQuantidade;
        private System.Windows.Forms.NumericUpDown numQuantidadeMinima;
        private System.Windows.Forms.TextBox txtUnidade;
        private System.Windows.Forms.NumericUpDown numPreco;

        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnCancelar;

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbMadeira;
        private System.Windows.Forms.ComboBox cmbTamanho;
    }
}
