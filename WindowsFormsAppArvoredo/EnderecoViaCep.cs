namespace WindowsFormsAppArvoredo
{
    /// <summary>
    /// Modelo de dados retornado pela API ViaCEP
    /// </summary>
    public class EnderecoViaCep
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string ibge { get; set; }
        public string gia { get; set; }
        public string ddd { get; set; }
        public string siafi { get; set; }

        // Propriedade auxiliar para indicar erro
        public bool erro { get; set; }
    }
}
