﻿using System;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class FormNovoProduto : Form
    {
        public Produto ProdutoCriado { get; private set; }

        public FormNovoProduto()
        {
            InitializeComponent();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescricao.Text) || cbUnidade.SelectedItem == null)
            {
                MessageBox.Show("Preencha todos os campos obrigatórios!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ProdutoCriado = new Produto
            {
                Descricao = txtDescricao.Text,
                Unidade = cbUnidade.SelectedItem.ToString(),                                       
                Quantidade = numQuantidade.Value,
                ValorUnitario = numValorUnitario.Value
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}