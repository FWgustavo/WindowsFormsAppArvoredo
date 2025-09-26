using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WindowsFormsAppArvoredo
{
    /// <summary>
    /// Serviço de consulta ao ViaCEP com fallback em caso de falha
    /// </summary>
    public static class ViaCepService
    {
        private static readonly HttpClient httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        /// <summary>
        /// Consulta um CEP na API ViaCEP
        /// </summary>
        public static async Task<EnderecoViaCep> ConsultarCepAsync(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8 || !cep.All(char.IsDigit))
                return new EnderecoViaCep { erro = true };

            string url = $"https://viacep.com.br/ws/{cep}/json/";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return null;

                string json = await response.Content.ReadAsStringAsync();
                var endereco = JsonSerializer.Deserialize<EnderecoViaCep>(json);

                if (endereco == null || endereco.localidade == null)
                    return new EnderecoViaCep { erro = true };

                return endereco;
            }
            catch
            {
                return null; // erro de conexão
            }
        }

        /// <summary>
        /// Consulta o CEP e tenta novamente em caso de falha (fallback)
        /// </summary>
        public static async Task<EnderecoViaCep> ConsultarCepComFallbackAsync(string cep)
        {
            var endereco = await ConsultarCepAsync(cep);

            if (endereco == null)
            {
                await Task.Delay(1000);
                endereco = await ConsultarCepAsync(cep);
            }

            return endereco;
        }
    }
}
