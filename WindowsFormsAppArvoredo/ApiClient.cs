using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public class ApiClient
    {
        private static readonly HttpClient client = new HttpClient();
        private static string baseUrl = "https://arvoredoapi.vercel.app"; // Substitua pela URL da sua API
        private static string apiKey = "68e553e6f1c4fffd11c95840"; // Será configurado pelo usuário

        static ApiClient()
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        }

        // Configurar a URL base da API
        public static void ConfigurarUrlBase(string url)
        {
            baseUrl = url.TrimEnd('/');
        }

        // Configurar a chave de API
        public static void ConfigurarApiKey(string key)
        {
            apiKey = key;

            // Remove header anterior se existir
            if (client.DefaultRequestHeaders.Contains("x-api-key"))
            {
                client.DefaultRequestHeaders.Remove("x-api-key");
            }

            // Adiciona novo header
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // Teste de conexão com a API
        public static async Task<bool> TestarConexaoAsync()
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}/");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao testar conexão: {ex.Message}");
                return false;
            }
        }

        // GET genérico
        public static async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}{endpoint}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro na requisição GET: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao processar resposta: {ex.Message}");
            }
        }

        // POST genérico
        public static async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{baseUrl}{endpoint}", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responseContent);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro na requisição POST: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao processar resposta: {ex.Message}");
            }
        }

        // PUT genérico
        public static async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{baseUrl}{endpoint}", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responseContent);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro na requisição PUT: {ex.Message}");
            }
        }

        // DELETE genérico
        public static async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await client.DeleteAsync($"{baseUrl}{endpoint}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na requisição DELETE: {ex.Message}");
            }
        }

        // Método auxiliar para obter informações da API
        public static string ObterUrlBase()
        {
            return baseUrl;
        }

        public static bool TemApiKey()
        {
            return !string.IsNullOrEmpty(apiKey);
        }
    }
}