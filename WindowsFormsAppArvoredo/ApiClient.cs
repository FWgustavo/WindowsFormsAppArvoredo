// ============================================
// ARQUIVO: WindowsFormsAppArvoredo/ApiClient.cs
// ============================================

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsAppArvoredo
{
    public class ApiClient
    {
        private static readonly HttpClient client = new HttpClient();
        private static string baseUrl = "https://arvoredoapi.vercel.app";
        private static string apiKey = "68e553e6f1c4fffd11c95840";

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

            if (client.DefaultRequestHeaders.Contains("x-api-key"))
                client.DefaultRequestHeaders.Remove("x-api-key");

            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // Teste de conexão
        public static async Task<bool> TestarConexaoAsync()
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}/");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao testar conexão: {ex.Message}");
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

        // POST genérico com DEBUG
        public static async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                // DEBUG request
                Debug.WriteLine($"\n{"=".PadRight(60, '=')}");
                Debug.WriteLine($"📤 POST REQUEST: {endpoint}");
                Debug.WriteLine($"URL Completa: {baseUrl}{endpoint}");
                Debug.WriteLine($"Headers: x-api-key={apiKey}");
                Debug.WriteLine($"JSON Enviado:\n{FormatarJSON(json)}");
                Debug.WriteLine($"{"=".PadRight(60, '=')}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{baseUrl}{endpoint}", content);

                var responseContent = await response.Content.ReadAsStringAsync();

                // DEBUG response
                Debug.WriteLine($"\n{"=".PadRight(60, '=')}");
                Debug.WriteLine($"📥 RESPOSTA: {(int)response.StatusCode} {response.StatusCode}");
                Debug.WriteLine($"JSON Recebido:\n{FormatarJSON(responseContent)}");
                Debug.WriteLine($"{"=".PadRight(60, '=')}\n");

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        dynamic errorResponse = JsonConvert.DeserializeObject(responseContent);
                        string errorMessage = errorResponse?.message ?? errorResponse?.error ?? responseContent;

                        throw new Exception($"Erro {(int)response.StatusCode}: {errorMessage}");
                    }
                    catch
                    {
                        throw new Exception($"Erro na requisição POST: HTTP {(int)response.StatusCode}");
                    }
                }

                return JsonConvert.DeserializeObject<TResponse>(responseContent);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"❌ HTTP EXCEPTION: {ex.Message}");
                throw new Exception($"Erro na requisição POST: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ EXCEPTION: {ex.Message}");
                throw new Exception($"Erro ao processar resposta: {ex.Message}");
            }
        }

        // PUT genérico com DEBUG
        public static async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                Debug.WriteLine($"\n{"=".PadRight(60, '=')}");
                Debug.WriteLine($"📤 PUT REQUEST: {endpoint}");
                Debug.WriteLine($"JSON Enviado:\n{FormatarJSON(json)}");
                Debug.WriteLine($"{"=".PadRight(60, '=')}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{baseUrl}{endpoint}", content);

                var responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"📥 RESPOSTA: {(int)response.StatusCode}");
                Debug.WriteLine($"JSON:\n{FormatarJSON(responseContent)}");

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Erro na requisição PUT: HTTP {(int)response.StatusCode}");

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

        // Helpers
        public static string ObterUrlBase() => baseUrl;

        public static bool TemApiKey() => !string.IsNullOrEmpty(apiKey);

        // FORMATADOR DE JSON PARA DEBUG
        private static string FormatarJSON(string json)
        {
            try
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(json);
                return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }
            catch
            {
                return json;
            }
        }
    }
}
