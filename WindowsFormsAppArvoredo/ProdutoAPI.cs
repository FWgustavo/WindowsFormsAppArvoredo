// Adicione este arquivo: ProdutoAPI.cs
// Coloque na pasta do projeto WindowsFormsAppArvoredo

using System;
using System.Collections.Generic;

namespace WindowsFormsAppArvoredo
{
    /// <summary>
    /// Modelo de produto retornado pela API
    /// </summary>
    public class ProdutoAPI
    {
        public int id { get; set; }
        public string nome { get; set; }
        public int? madeiraId { get; set; }
        public int? tamanhoId { get; set; }
        public double valor { get; set; }
        public string unidade { get; set; }
        public int quantidade { get; set; }
        public int quantidadeMin { get; set; }
        public bool ativo { get; set; }
        public bool acabando { get; set; }
        public int? fornecedorId { get; set; }
    }

    /// <summary>
    /// Modelo para criação/atualização de produto na API
    /// </summary>
    public class ProdutoAPICreate
    {
        public string nome { get; set; }
        public int? madeiraId { get; set; }
        public int? tamanhoId { get; set; }
        public double valor { get; set; }
        public string unidade { get; set; }
        public int quantidade { get; set; }
        public int quantidadeMin { get; set; }
        public bool ativo { get; set; }
        public int? fornecedorId { get; set; }
    }

    /// <summary>
    /// Modelo de madeira da API
    /// </summary>
    public class MadeiraAPI
    {
        public int id { get; set; }
        public string nome { get; set; }
        public bool ativo { get; set; }
        public int? fornecedorId { get; set; }
    }

    /// <summary>
    /// Modelo de tamanho da API
    /// </summary>
    public class TamanhoAPI
    {
        public int id { get; set; }
        public string nome { get; set; }
        public bool ativo { get; set; }
    }
}