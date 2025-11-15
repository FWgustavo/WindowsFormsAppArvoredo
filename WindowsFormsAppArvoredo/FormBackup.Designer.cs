using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsAppArvoredo
{
    partial class FormBackup
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
            this.txtNomeBackup = new System.Windows.Forms.TextBox();
            this.chkCompactar = new System.Windows.Forms.CheckBox();
            this.panelListaArquivos = new System.Windows.Forms.Panel();
            this.btnRestaurarBackup = new System.Windows.Forms.Button();
            this.btnExcluirBackup = new System.Windows.Forms.Button();
            this.btnAbrirPasta = new System.Windows.Forms.Button();
            this.btnCriarArquivo = new System.Windows.Forms.Button();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.lblTituloCriar = new System.Windows.Forms.Label();
            this.lblNomeBackup = new System.Windows.Forms.Label();
            this.lblTituloAcoes = new System.Windows.Forms.Label();
            this.lblListaArquivos = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtNomeBackup
            // 
            this.txtNomeBackup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNomeBackup.Font = new System.Drawing.Font("Arial", 10F);
            this.txtNomeBackup.Location = new System.Drawing.Point(50, 110);
            this.txtNomeBackup.Name = "txtNomeBackup";
            this.txtNomeBackup.Size = new System.Drawing.Size(300, 25);
            this.txtNomeBackup.TabIndex = 0;
            // 
            // chkCompactar
            // 
            this.chkCompactar.BackColor = System.Drawing.Color.Transparent;
            this.chkCompactar.Checked = true;
            this.chkCompactar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCompactar.Font = new System.Drawing.Font("Gagalin", 9F, System.Drawing.FontStyle.Regular);
            this.chkCompactar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.chkCompactar.Location = new System.Drawing.Point(50, 145);
            this.chkCompactar.Name = "chkCompactar";
            this.chkCompactar.Size = new System.Drawing.Size(250, 25);
            this.chkCompactar.TabIndex = 1;
            this.chkCompactar.Text = "COMPACTAR ARQUIVO";
            this.chkCompactar.UseVisualStyleBackColor = false;
            // 
            // panelListaArquivos
            // 
            this.panelListaArquivos.AutoScroll = true;
            this.panelListaArquivos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.panelListaArquivos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelListaArquivos.Location = new System.Drawing.Point(35, 305);
            this.panelListaArquivos.Name = "panelListaArquivos";
            this.panelListaArquivos.Size = new System.Drawing.Size(630, 180);
            this.panelListaArquivos.TabIndex = 2;
            // 
            // btnRestaurarBackup
            // 
            this.btnRestaurarBackup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(216)))), ((int)(((byte)(230)))));
            this.btnRestaurarBackup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestaurarBackup.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.btnRestaurarBackup.FlatAppearance.BorderSize = 2;
            this.btnRestaurarBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestaurarBackup.Font = new System.Drawing.Font("Gagalin", 9F, System.Drawing.FontStyle.Bold);
            this.btnRestaurarBackup.ForeColor = System.Drawing.Color.Black;
            this.btnRestaurarBackup.Location = new System.Drawing.Point(400, 155);
            this.btnRestaurarBackup.Name = "btnRestaurarBackup";
            this.btnRestaurarBackup.Size = new System.Drawing.Size(250, 30);
            this.btnRestaurarBackup.TabIndex = 3;
            this.btnRestaurarBackup.Text = "RESTAURAR BACKUP";
            this.btnRestaurarBackup.UseVisualStyleBackColor = false;
            this.btnRestaurarBackup.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, 250, 30, 15, 15));
            this.btnRestaurarBackup.Click += new System.EventHandler(this.BtnRestaurarBackup_Click);
            // 
            // btnExcluirBackup
            // 
            this.btnExcluirBackup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(182)))), ((int)(((byte)(193)))));
            this.btnExcluirBackup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExcluirBackup.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.btnExcluirBackup.FlatAppearance.BorderSize = 2;
            this.btnExcluirBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcluirBackup.Font = new System.Drawing.Font("Gagalin", 9F, System.Drawing.FontStyle.Bold);
            this.btnExcluirBackup.ForeColor = System.Drawing.Color.Black;
            this.btnExcluirBackup.Location = new System.Drawing.Point(400, 195);
            this.btnExcluirBackup.Name = "btnExcluirBackup";
            this.btnExcluirBackup.Size = new System.Drawing.Size(250, 30);
            this.btnExcluirBackup.TabIndex = 4;
            this.btnExcluirBackup.Text = "EXCLUIR BACKUP";
            this.btnExcluirBackup.UseVisualStyleBackColor = false;
            this.btnExcluirBackup.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, 250, 30, 15, 15));
            this.btnExcluirBackup.Click += new System.EventHandler(this.BtnExcluirBackup_Click);
            // 
            // btnAbrirPasta
            // 
            this.btnAbrirPasta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(218)))), ((int)(((byte)(185)))));
            this.btnAbrirPasta.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbrirPasta.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.btnAbrirPasta.FlatAppearance.BorderSize = 2;
            this.btnAbrirPasta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrirPasta.Font = new System.Drawing.Font("Gagalin", 9F, System.Drawing.FontStyle.Bold);
            this.btnAbrirPasta.ForeColor = System.Drawing.Color.Black;
            this.btnAbrirPasta.Location = new System.Drawing.Point(400, 235);
            this.btnAbrirPasta.Name = "btnAbrirPasta";
            this.btnAbrirPasta.Size = new System.Drawing.Size(250, 30);
            this.btnAbrirPasta.TabIndex = 5;
            this.btnAbrirPasta.Text = "ABRIR PASTA";
            this.btnAbrirPasta.UseVisualStyleBackColor = false;
            this.btnAbrirPasta.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, 250, 30, 15, 15));
            this.btnAbrirPasta.Click += new System.EventHandler(this.BtnAbrirPasta_Click);
            // 
            // btnCriarArquivo
            // 
            this.btnCriarArquivo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(238)))), ((int)(((byte)(144)))));
            this.btnCriarArquivo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCriarArquivo.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.btnCriarArquivo.FlatAppearance.BorderSize = 2;
            this.btnCriarArquivo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCriarArquivo.Font = new System.Drawing.Font("Gagalin", 10F, System.Drawing.FontStyle.Bold);
            this.btnCriarArquivo.ForeColor = System.Drawing.Color.Black;
            this.btnCriarArquivo.Location = new System.Drawing.Point(400, 105);
            this.btnCriarArquivo.Name = "btnCriarArquivo";
            this.btnCriarArquivo.Size = new System.Drawing.Size(250, 40);
            this.btnCriarArquivo.TabIndex = 6;
            this.btnCriarArquivo.Text = "CRIAR ARQUIVO";
            this.btnCriarArquivo.UseVisualStyleBackColor = false;
            this.btnCriarArquivo.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, 250, 40, 20, 20));
            this.btnCriarArquivo.Click += new System.EventHandler(this.BtnCriarArquivo_Click);
            // 
            // btnVoltar
            // 
            this.btnVoltar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.btnVoltar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVoltar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.btnVoltar.FlatAppearance.BorderSize = 2;
            this.btnVoltar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVoltar.Font = new System.Drawing.Font("Gagalin", 10F, System.Drawing.FontStyle.Bold);
            this.btnVoltar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.btnVoltar.Location = new System.Drawing.Point(275, 500);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(150, 35);
            this.btnVoltar.TabIndex = 7;
            this.btnVoltar.Text = "VOLTAR";
            this.btnVoltar.UseVisualStyleBackColor = false;
            this.btnVoltar.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, 150, 35, 20, 20));
            this.btnVoltar.Click += new System.EventHandler(this.BtnVoltar_Click);
            // 
            // lblTituloCriar
            // 
            this.lblTituloCriar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.lblTituloCriar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTituloCriar.Font = new System.Drawing.Font("Gagalin", 14F, System.Drawing.FontStyle.Bold);
            this.lblTituloCriar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.lblTituloCriar.Location = new System.Drawing.Point(50, 30);
            this.lblTituloCriar.Name = "lblTituloCriar";
            this.lblTituloCriar.Size = new System.Drawing.Size(300, 35);
            this.lblTituloCriar.TabIndex = 8;
            this.lblTituloCriar.Text = "CRIAR NOVO BACKUP";
            this.lblTituloCriar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTituloCriar.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, 300, 35, 20, 20));
            // 
            // lblNomeBackup
            // 
            this.lblNomeBackup.Font = new System.Drawing.Font("Gagalin", 9F, System.Drawing.FontStyle.Regular);
            this.lblNomeBackup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.lblNomeBackup.Location = new System.Drawing.Point(50, 85);
            this.lblNomeBackup.Name = "lblNomeBackup";
            this.lblNomeBackup.Size = new System.Drawing.Size(300, 20);
            this.lblNomeBackup.TabIndex = 9;
            this.lblNomeBackup.Text = "NOME DO BACKUP (OPCIONAL):";
            // 
            // lblTituloAcoes
            // 
            this.lblTituloAcoes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.lblTituloAcoes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTituloAcoes.Font = new System.Drawing.Font("Gagalin", 14F, System.Drawing.FontStyle.Bold);
            this.lblTituloAcoes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.lblTituloAcoes.Location = new System.Drawing.Point(400, 30);
            this.lblTituloAcoes.Name = "lblTituloAcoes";
            this.lblTituloAcoes.Size = new System.Drawing.Size(250, 35);
            this.lblTituloAcoes.TabIndex = 10;
            this.lblTituloAcoes.Text = "AÇÕES";
            this.lblTituloAcoes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTituloAcoes.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, 250, 35, 20, 20));
            // 
            // lblListaArquivos
            // 
            this.lblListaArquivos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(212)))), ((int)(((byte)(172)))));
            this.lblListaArquivos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblListaArquivos.Font = new System.Drawing.Font("Gagalin", 7F, System.Drawing.FontStyle.Bold);
            this.lblListaArquivos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(27)))), ((int)(((byte)(1)))));
            this.lblListaArquivos.Location = new System.Drawing.Point(35, 280);
            this.lblListaArquivos.Name = "lblListaArquivos";
            this.lblListaArquivos.Size = new System.Drawing.Size(630, 25);
            this.lblListaArquivos.TabIndex = 11;
            this.lblListaArquivos.Text = "NOME DO ARQUIVO          DATA/HORA          TAMANHO     COMPACTADO     EXCLUIR";
            this.lblListaArquivos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(230)))), ((int)(((byte)(194)))));
            this.ClientSize = new System.Drawing.Size(700, 570);
            this.Controls.Add(this.lblListaArquivos);
            this.Controls.Add(this.lblTituloAcoes);
            this.Controls.Add(this.lblNomeBackup);
            this.Controls.Add(this.lblTituloCriar);
            this.Controls.Add(this.btnVoltar);
            this.Controls.Add(this.btnCriarArquivo);
            this.Controls.Add(this.btnAbrirPasta);
            this.Controls.Add(this.btnExcluirBackup);
            this.Controls.Add(this.btnRestaurarBackup);
            this.Controls.Add(this.panelListaArquivos);
            this.Controls.Add(this.chkCompactar);
            this.Controls.Add(this.txtNomeBackup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormBackup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormBackup";
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, 700, 570, 30, 30));
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormBackup_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtNomeBackup;
        private System.Windows.Forms.CheckBox chkCompactar;
        private System.Windows.Forms.Panel panelListaArquivos;
        private System.Windows.Forms.Button btnRestaurarBackup;
        private System.Windows.Forms.Button btnExcluirBackup;
        private System.Windows.Forms.Button btnAbrirPasta;
        private System.Windows.Forms.Button btnCriarArquivo;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.Label lblTituloCriar;
        private System.Windows.Forms.Label lblNomeBackup;
        private System.Windows.Forms.Label lblTituloAcoes;
        private System.Windows.Forms.Label lblListaArquivos;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
           int nLeft, int nTop, int nRight, int nBottom,
           int nWidthEllipse, int nHeightEllipse);
    }
}