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
            this.lblNome = new System.Windows.Forms.Label();
            this.lblTipo = new System.Windows.Forms.Label();
            this.lblQuantidade = new System.Windows.Forms.Label();
            this.lblQuantidadeMinima = new System.Windows.Forms.Label();
            this.lblUnidade = new System.Windows.Forms.Label();
            this.lblPreco = new System.Windows.Forms.Label();

            this.txtNome = new System.Windows.Forms.TextBox();
            this.txtTipo = new System.Windows.Forms.TextBox();
            this.numQuantidade = new System.Windows.Forms.NumericUpDown();
            this.numQuantidadeMinima = new System.Windows.Forms.NumericUpDown();
            this.txtUnidade = new System.Windows.Forms.TextBox();
            this.numPreco = new System.Windows.Forms.NumericUpDown();

            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.numQuantidade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantidadeMinima)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPreco)).BeginInit();
            this.SuspendLayout();

            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Location = new System.Drawing.Point(30, 30);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(108, 20);
            this.lblNome.TabIndex = 0;
            this.lblNome.Text = "Descrição:";

            // 
            // txtNome
            // 
            this.txtNome.Location = new System.Drawing.Point(180, 27);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(250, 27);
            this.txtNome.TabIndex = 1;

            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Location = new System.Drawing.Point(30, 75);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(46, 20);
            this.lblTipo.TabIndex = 2;
            this.lblTipo.Text = "Tipo:";

            // 
            // txtTipo
            // 
            this.txtTipo.Location = new System.Drawing.Point(180, 72);
            this.txtTipo.Name = "txtTipo";
            this.txtTipo.Size = new System.Drawing.Size(250, 27);
            this.txtTipo.TabIndex = 3;

            // 
            // lblQuantidade
            // 
            this.lblQuantidade.AutoSize = true;
            this.lblQuantidade.Location = new System.Drawing.Point(30, 120);
            this.lblQuantidade.Name = "lblQuantidade";
            this.lblQuantidade.Size = new System.Drawing.Size(98, 20);
            this.lblQuantidade.TabIndex = 4;
            this.lblQuantidade.Text = "Quantidade:";

            // 
            // numQuantidade
            // 
            this.numQuantidade.Location = new System.Drawing.Point(180, 118);
            this.numQuantidade.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numQuantidade.Name = "numQuantidade";
            this.numQuantidade.Size = new System.Drawing.Size(120, 27);
            this.numQuantidade.TabIndex = 5;

            // 
            // lblQuantidadeMinima
            // 
            this.lblQuantidadeMinima.AutoSize = true;
            this.lblQuantidadeMinima.Location = new System.Drawing.Point(30, 165);
            this.lblQuantidadeMinima.Name = "lblQuantidadeMinima";
            this.lblQuantidadeMinima.Size = new System.Drawing.Size(144, 20);
            this.lblQuantidadeMinima.TabIndex = 6;
            this.lblQuantidadeMinima.Text = "Quantidade Mínima:";

            // 
            // numQuantidadeMinima
            // 
            this.numQuantidadeMinima.Location = new System.Drawing.Point(180, 163);
            this.numQuantidadeMinima.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numQuantidadeMinima.Name = "numQuantidadeMinima";
            this.numQuantidadeMinima.Size = new System.Drawing.Size(120, 27);
            this.numQuantidadeMinima.TabIndex = 7;

            // 
            // lblUnidade
            // 
            this.lblUnidade.AutoSize = true;
            this.lblUnidade.Location = new System.Drawing.Point(30, 210);
            this.lblUnidade.Name = "lblUnidade";
            this.lblUnidade.Size = new System.Drawing.Size(73, 20);
            this.lblUnidade.TabIndex = 8;
            this.lblUnidade.Text = "Unidade:";

            // 
            // txtUnidade
            // 
            this.txtUnidade.Location = new System.Drawing.Point(180, 207);
            this.txtUnidade.Name = "txtUnidade";
            this.txtUnidade.Size = new System.Drawing.Size(120, 27);
            this.txtUnidade.TabIndex = 9;

            // 
            // lblPreco
            // 
            this.lblPreco.AutoSize = true;
            this.lblPreco.Location = new System.Drawing.Point(30, 255);
            this.lblPreco.Name = "lblPreco";
            this.lblPreco.Size = new System.Drawing.Size(103, 20);
            this.lblPreco.TabIndex = 10;
            this.lblPreco.Text = "Preço Unitário:";

            // 
            // numPreco
            // 
            this.numPreco.DecimalPlaces = 2;
            this.numPreco.Location = new System.Drawing.Point(180, 253);
            this.numPreco.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numPreco.Name = "numPreco";
            this.numPreco.Size = new System.Drawing.Size(120, 27);
            this.numPreco.TabIndex = 11;

            // 
            // btnSalvar
            // 
            this.btnSalvar.BackColor = System.Drawing.Color.ForestGreen;
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(180, 310);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(100, 35);
            this.btnSalvar.TabIndex = 12;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);

            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.Firebrick;
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(330, 310);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 35);
            this.btnCancelar.TabIndex = 13;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            // 
            // FormNovoProduto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 380);
            this.Controls.Add(this.lblNome);
            this.Controls.Add(this.txtNome);
            this.Controls.Add(this.lblTipo);
            this.Controls.Add(this.txtTipo);
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
            this.Name = "FormNovoProduto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cadastro de Produto";

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
        private System.Windows.Forms.TextBox txtTipo;
        private System.Windows.Forms.NumericUpDown numQuantidade;
        private System.Windows.Forms.NumericUpDown numQuantidadeMinima;
        private System.Windows.Forms.TextBox txtUnidade;
        private System.Windows.Forms.NumericUpDown numPreco;

        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnCancelar;

        #endregion
    }
}
