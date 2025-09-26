namespace WindowsFormsAppArvoredo
{
    partial class FormMenuCadastro
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos.
        /// </summary>
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnCadastroClientes = new System.Windows.Forms.Button();
            this.btnCadastroFornecedores = new System.Windows.Forms.Button();
            this.btnCadastroProdutos = new System.Windows.Forms.Button();
            this.btnCadastroUsuarios = new System.Windows.Forms.Button();
            this.btnFechar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Gagalin", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(57, 27, 1);
            this.lblTitulo.Location = new System.Drawing.Point(80, 30);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(240, 26);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "MENU DE CADASTROS";
            // 
            // btnCadastroClientes
            // 
            this.btnCadastroClientes.BackColor = System.Drawing.Color.FromArgb(144, 238, 144);
            this.btnCadastroClientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCadastroClientes.Font = new System.Drawing.Font("Gagalin", 12F);
            this.btnCadastroClientes.Location = new System.Drawing.Point(50, 90);
            this.btnCadastroClientes.Name = "btnCadastroClientes";
            this.btnCadastroClientes.Size = new System.Drawing.Size(300, 50);
            this.btnCadastroClientes.TabIndex = 1;
            this.btnCadastroClientes.Text = "👥 CADASTRO DE CLIENTES";
            this.btnCadastroClientes.UseVisualStyleBackColor = false;
            this.btnCadastroClientes.Click += new System.EventHandler(this.btnCadastroClientes_Click);
            // 
            // btnCadastroFornecedores
            // 
            this.btnCadastroFornecedores.BackColor = System.Drawing.Color.FromArgb(173, 216, 230);
            this.btnCadastroFornecedores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCadastroFornecedores.Font = new System.Drawing.Font("Gagalin", 12F);
            this.btnCadastroFornecedores.Location = new System.Drawing.Point(50, 160);
            this.btnCadastroFornecedores.Name = "btnCadastroFornecedores";
            this.btnCadastroFornecedores.Size = new System.Drawing.Size(300, 50);
            this.btnCadastroFornecedores.TabIndex = 2;
            this.btnCadastroFornecedores.Text = "🏭 CADASTRO DE FORNECEDORES";
            this.btnCadastroFornecedores.UseVisualStyleBackColor = false;
            this.btnCadastroFornecedores.Click += new System.EventHandler(this.btnCadastroFornecedores_Click);
            // 
            // btnCadastroProdutos
            // 
            this.btnCadastroProdutos.BackColor = System.Drawing.Color.FromArgb(255, 218, 185);
            this.btnCadastroProdutos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCadastroProdutos.Font = new System.Drawing.Font("Gagalin", 12F);
            this.btnCadastroProdutos.Location = new System.Drawing.Point(50, 230);
            this.btnCadastroProdutos.Name = "btnCadastroProdutos";
            this.btnCadastroProdutos.Size = new System.Drawing.Size(300, 50);
            this.btnCadastroProdutos.TabIndex = 3;
            this.btnCadastroProdutos.Text = "📦 CADASTRO DE PRODUTOS";
            this.btnCadastroProdutos.UseVisualStyleBackColor = false;
            this.btnCadastroProdutos.Click += new System.EventHandler(this.btnCadastroProdutos_Click);
            // 
            // btnCadastroUsuarios
            // 
            this.btnCadastroUsuarios.BackColor = System.Drawing.Color.FromArgb(221, 160, 221);
            this.btnCadastroUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCadastroUsuarios.Font = new System.Drawing.Font("Gagalin", 12F);
            this.btnCadastroUsuarios.Location = new System.Drawing.Point(50, 300);
            this.btnCadastroUsuarios.Name = "btnCadastroUsuarios";
            this.btnCadastroUsuarios.Size = new System.Drawing.Size(300, 50);
            this.btnCadastroUsuarios.TabIndex = 4;
            this.btnCadastroUsuarios.Text = "👤 CADASTRO DE USUÁRIOS";
            this.btnCadastroUsuarios.UseVisualStyleBackColor = false;
            this.btnCadastroUsuarios.Click += new System.EventHandler(this.btnCadastroUsuarios_Click);
            // 
            // btnFechar
            // 
            this.btnFechar.BackColor = System.Drawing.Color.FromArgb(239, 212, 172);
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Font = new System.Drawing.Font("Gagalin", 10F);
            this.btnFechar.Location = new System.Drawing.Point(275, 380);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(75, 30);
            this.btnFechar.TabIndex = 5;
            this.btnFechar.Text = "Fechar";
            this.btnFechar.UseVisualStyleBackColor = false;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // FormMenuCadastro
            // 
            this.BackColor = System.Drawing.Color.FromArgb(250, 230, 194);
            this.ClientSize = new System.Drawing.Size(400, 430);
            this.Controls.Add(this.btnFechar);
            this.Controls.Add(this.btnCadastroUsuarios);
            this.Controls.Add(this.btnCadastroProdutos);
            this.Controls.Add(this.btnCadastroFornecedores);
            this.Controls.Add(this.btnCadastroClientes);
            this.Controls.Add(this.lblTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMenuCadastro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Menu de Cadastros";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnCadastroClientes;
        private System.Windows.Forms.Button btnCadastroFornecedores;
        private System.Windows.Forms.Button btnCadastroProdutos;
        private System.Windows.Forms.Button btnCadastroUsuarios;
        private System.Windows.Forms.Button btnFechar;
    }
}
