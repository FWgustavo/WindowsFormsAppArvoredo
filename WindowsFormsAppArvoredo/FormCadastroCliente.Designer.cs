namespace WindowsFormsAppArvoredo
{
    partial class FormCadastroCliente
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelPrincipal = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.lblNome = new System.Windows.Forms.Label();
            this.txtCpfCnpj = new System.Windows.Forms.TextBox();
            this.lblCpfCnpj = new System.Windows.Forms.Label();
            this.txtTelefone = new System.Windows.Forms.TextBox();
            this.lblTelefone = new System.Windows.Forms.Label();
            this.txtCep = new System.Windows.Forms.TextBox();
            this.lblCep = new System.Windows.Forms.Label();
            this.txtEndereco = new System.Windows.Forms.TextBox();
            this.lblEndereco = new System.Windows.Forms.Label();
            this.txtBairro = new System.Windows.Forms.TextBox();
            this.lblBairro = new System.Windows.Forms.Label();
            this.btnExcluir = new System.Windows.Forms.Button();
            this.btnCadastrar = new System.Windows.Forms.Button();
            this.panelPrincipal.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPrincipal
            // 
            this.panelPrincipal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.panelPrincipal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPrincipal.Controls.Add(this.btnCadastrar);
            this.panelPrincipal.Controls.Add(this.btnExcluir);
            this.panelPrincipal.Controls.Add(this.txtBairro);
            this.panelPrincipal.Controls.Add(this.lblBairro);
            this.panelPrincipal.Controls.Add(this.txtEndereco);
            this.panelPrincipal.Controls.Add(this.lblEndereco);
            this.panelPrincipal.Controls.Add(this.txtCep);
            this.panelPrincipal.Controls.Add(this.lblCep);
            this.panelPrincipal.Controls.Add(this.txtTelefone);
            this.panelPrincipal.Controls.Add(this.lblTelefone);
            this.panelPrincipal.Controls.Add(this.txtCpfCnpj);
            this.panelPrincipal.Controls.Add(this.lblCpfCnpj);
            this.panelPrincipal.Controls.Add(this.txtNome);
            this.panelPrincipal.Controls.Add(this.lblNome);
            this.panelPrincipal.Controls.Add(this.lblTitulo);
            this.panelPrincipal.Location = new System.Drawing.Point(20, 20);
            this.panelPrincipal.Name = "panelPrincipal";
            this.panelPrincipal.Size = new System.Drawing.Size(700, 480);
            this.panelPrincipal.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.lblTitulo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTitulo.Font = new System.Drawing.Font("Gagalin", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.lblTitulo.Location = new System.Drawing.Point(50, 30);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(600, 45);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "CADASTRAR CLIENTE";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Font = new System.Drawing.Font("Gagalin", 11F, System.Drawing.FontStyle.Bold);
            this.lblNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.lblNome.Location = new System.Drawing.Point(70, 110);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(60, 18);
            this.lblNome.TabIndex = 1;
            this.lblNome.Text = "NOME:";
            // 
            // txtNome
            // 
            this.txtNome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNome.Font = new System.Drawing.Font("Gagalin", 10F);
            this.txtNome.Location = new System.Drawing.Point(73, 130);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(560, 24);
            this.txtNome.TabIndex = 2;
            // 
            // lblCpfCnpj
            // 
            this.lblCpfCnpj.AutoSize = true;
            this.lblCpfCnpj.Font = new System.Drawing.Font("Gagalin", 11F, System.Drawing.FontStyle.Bold);
            this.lblCpfCnpj.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.lblCpfCnpj.Location = new System.Drawing.Point(70, 165);
            this.lblCpfCnpj.Name = "lblCpfCnpj";
            this.lblCpfCnpj.Size = new System.Drawing.Size(93, 18);
            this.lblCpfCnpj.TabIndex = 3;
            this.lblCpfCnpj.Text = "CPF/CNPJ:";
            // 
            // txtCpfCnpj
            // 
            this.txtCpfCnpj.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCpfCnpj.Font = new System.Drawing.Font("Gagalin", 10F);
            this.txtCpfCnpj.Location = new System.Drawing.Point(73, 185);
            this.txtCpfCnpj.Name = "txtCpfCnpj";
            this.txtCpfCnpj.Size = new System.Drawing.Size(560, 24);
            this.txtCpfCnpj.TabIndex = 4;
            // 
            // lblTelefone
            // 
            this.lblTelefone.AutoSize = true;
            this.lblTelefone.Font = new System.Drawing.Font("Gagalin", 11F, System.Drawing.FontStyle.Bold);
            this.lblTelefone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.lblTelefone.Location = new System.Drawing.Point(70, 220);
            this.lblTelefone.Name = "lblTelefone";
            this.lblTelefone.Size = new System.Drawing.Size(134, 18);
            this.lblTelefone.TabIndex = 5;
            this.lblTelefone.Text = "TELEFONE/CELL:";
            // 
            // txtTelefone
            // 
            this.txtTelefone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTelefone.Font = new System.Drawing.Font("Gagalin", 10F);
            this.txtTelefone.Location = new System.Drawing.Point(73, 240);
            this.txtTelefone.Name = "txtTelefone";
            this.txtTelefone.Size = new System.Drawing.Size(560, 24);
            this.txtTelefone.TabIndex = 6;
            // 
            // lblCep
            // 
            this.lblCep.AutoSize = true;
            this.lblCep.Font = new System.Drawing.Font("Gagalin", 11F, System.Drawing.FontStyle.Bold);
            this.lblCep.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.lblCep.Location = new System.Drawing.Point(70, 275);
            this.lblCep.Name = "lblCep";
            this.lblCep.Size = new System.Drawing.Size(130, 18);
            this.lblCep.TabIndex = 7;
            this.lblCep.Text = "CEP/MUNICÍPIO:";
            // 
            // txtCep
            // 
            this.txtCep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCep.Font = new System.Drawing.Font("Gagalin", 10F);
            this.txtCep.Location = new System.Drawing.Point(73, 295);
            this.txtCep.Name = "txtCep";
            this.txtCep.Size = new System.Drawing.Size(560, 24);
            this.txtCep.TabIndex = 8;
            // 
            // lblEndereco
            // 
            this.lblEndereco.AutoSize = true;
            this.lblEndereco.Font = new System.Drawing.Font("Gagalin", 11F, System.Drawing.FontStyle.Bold);
            this.lblEndereco.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.lblEndereco.Location = new System.Drawing.Point(70, 330);
            this.lblEndereco.Name = "lblEndereco";
            this.lblEndereco.Size = new System.Drawing.Size(95, 18);
            this.lblEndereco.TabIndex = 9;
            this.lblEndereco.Text = "ENDEREÇO:";
            // 
            // txtEndereco
            // 
            this.txtEndereco.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEndereco.Font = new System.Drawing.Font("Gagalin", 10F);
            this.txtEndereco.Location = new System.Drawing.Point(73, 350);
            this.txtEndereco.Name = "txtEndereco";
            this.txtEndereco.Size = new System.Drawing.Size(560, 24);
            this.txtEndereco.TabIndex = 10;
            // 
            // lblBairro
            // 
            this.lblBairro.AutoSize = true;
            this.lblBairro.Font = new System.Drawing.Font("Gagalin", 11F, System.Drawing.FontStyle.Bold);
            this.lblBairro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.lblBairro.Location = new System.Drawing.Point(70, 385);
            this.lblBairro.Name = "lblBairro";
            this.lblBairro.Size = new System.Drawing.Size(73, 18);
            this.lblBairro.TabIndex = 11;
            this.lblBairro.Text = "BAIRRO:";
            // 
            // txtBairro
            // 
            this.txtBairro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBairro.Font = new System.Drawing.Font("Gagalin", 10F);
            this.txtBairro.Location = new System.Drawing.Point(73, 405);
            this.txtBairro.Name = "txtBairro";
            this.txtBairro.Size = new System.Drawing.Size(560, 24);
            this.txtBairro.TabIndex = 12;
            // 
            // btnExcluir
            // 
            this.btnExcluir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnExcluir.FlatAppearance.BorderSize = 0;
            this.btnExcluir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcluir.Font = new System.Drawing.Font("Gagalin", 12F, System.Drawing.FontStyle.Bold);
            this.btnExcluir.ForeColor = System.Drawing.Color.White;
            this.btnExcluir.Location = new System.Drawing.Point(73, 440);
            this.btnExcluir.Name = "btnExcluir";
            this.btnExcluir.Size = new System.Drawing.Size(200, 35);
            this.btnExcluir.TabIndex = 13;
            this.btnExcluir.Text = "EXCLUIR";
            this.btnExcluir.UseVisualStyleBackColor = false;
            this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // btnCadastrar
            // 
            this.btnCadastrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(238)))), ((int)(((byte)(144)))));
            this.btnCadastrar.FlatAppearance.BorderSize = 0;
            this.btnCadastrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCadastrar.Font = new System.Drawing.Font("Gagalin", 12F, System.Drawing.FontStyle.Bold);
            this.btnCadastrar.ForeColor = System.Drawing.Color.Black;
            this.btnCadastrar.Location = new System.Drawing.Point(433, 440);
            this.btnCadastrar.Name = "btnCadastrar";
            this.btnCadastrar.Size = new System.Drawing.Size(200, 35);
            this.btnCadastrar.TabIndex = 14;
            this.btnCadastrar.Text = "CADASTRAR";
            this.btnCadastrar.UseVisualStyleBackColor = false;
            this.btnCadastrar.Click += new System.EventHandler(this.btnCadastrar_Click);
            // 
            // FormCadastroCliente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(230)))), ((int)(((byte)(194)))));
            this.ClientSize = new System.Drawing.Size(744, 521);
            this.Controls.Add(this.panelPrincipal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCadastroCliente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cadastro de Cliente";
            this.panelPrincipal.ResumeLayout(false);
            this.panelPrincipal.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelPrincipal;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Label lblCpfCnpj;
        private System.Windows.Forms.TextBox txtCpfCnpj;
        private System.Windows.Forms.Label lblTelefone;
        private System.Windows.Forms.TextBox txtTelefone;
        private System.Windows.Forms.Label lblCep;
        private System.Windows.Forms.TextBox txtCep;
        private System.Windows.Forms.Label lblEndereco;
        private System.Windows.Forms.TextBox txtEndereco;
        private System.Windows.Forms.Label lblBairro;
        private System.Windows.Forms.TextBox txtBairro;
        private System.Windows.Forms.Button btnExcluir;
        private System.Windows.Forms.Button btnCadastrar;
    }
}