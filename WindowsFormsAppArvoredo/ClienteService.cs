using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WindowsFormsAppArvoredo
{
    /// <summary>
    /// Serviço para integração com a API de Clientes
    /// </summary>
    public static class ClienteService
    {
        /// <summary>
        /// Carrega todos os clientes da API
        /// </summary>
        public static async Task<List<Cliente>> CarregarClientesAsync()
        {
            try
            {
                var clientesAPI = await ApiClient.GetAsync<List<ClienteAPI>>("/clientes");
                var clientes = new List<Cliente>();

                if (clientesAPI != null)
                {
                    foreach (var clienteAPI in clientesAPI)
                    {
                        clientes.Add(ConverterDeAPI(clienteAPI));
                    }
                }

                return clientes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar clientes: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca cliente por ID
        /// </summary>
        public static async Task<Cliente> BuscarClientePorIdAsync(int id)
        {
            try
            {
                var clienteAPI = await ApiClient.GetAsync<ClienteAPI>($"/clientes/{id}");
                return clienteAPI != null ? ConverterDeAPI(clienteAPI) : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Cria um novo cliente na API
        /// </summary>
        public static async Task<Cliente> CriarClienteAsync(Cliente cliente)
        {
            try
            {
                var clienteCreate = new ClienteAPICreate
                {
                    nome = cliente.Nome,
                    email = cliente.Email,
                    cpf = cliente.CpfCnpj,
                    telefone = cliente.Telefone,
                    cep = cliente.Cep,
                    cidade = cliente.Municipio,
                    estado = "SP", // Valor padrão
                    bairro = cliente.Bairro,
                    rua = cliente.Endereco,
                    numero = cliente.Numero
                };

                var clienteAPI = await ApiClient.PostAsync<ClienteAPICreate, ClienteAPI>(
                    "/clientes",
                    clienteCreate
                );

                return ConverterDeAPI(clienteAPI);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza um cliente existente
        /// </summary>
        public static async Task<Cliente> AtualizarClienteAsync(Cliente cliente)
        {
            try
            {
                var clienteUpdate = new ClienteAPICreate
                {
                    nome = cliente.Nome,
                    email = cliente.Email,
                    cpf = cliente.CpfCnpj,
                    telefone = cliente.Telefone,
                    cep = cliente.Cep,
                    cidade = cliente.Municipio,
                    estado = "SP",
                    bairro = cliente.Bairro,
                    rua = cliente.Endereco,
                    numero = cliente.Numero
                };

                var clienteAPI = await ApiClient.PutAsync<ClienteAPICreate, ClienteAPI>(
                    $"/clientes/{cliente.Id}",
                    clienteUpdate
                );

                return ConverterDeAPI(clienteAPI);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Deleta um cliente
        /// </summary>
        public static async Task<bool> DeletarClienteAsync(int id)
        {
            try
            {
                return await ApiClient.DeleteAsync($"/clientes/{id}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Converte ClienteAPI para Cliente
        /// </summary>
        private static Cliente ConverterDeAPI(ClienteAPI clienteAPI)
        {
            return new Cliente
            {
                Id = clienteAPI.id,
                Nome = clienteAPI.nome ?? "",
                Email = clienteAPI.email ?? "",
                CpfCnpj = clienteAPI.cpf ?? "",
                Telefone = clienteAPI.telefone ?? "",
                Cep = clienteAPI.cep ?? "",
                Municipio = clienteAPI.cidade ?? "",
                Endereco = clienteAPI.rua ?? "",
                Bairro = clienteAPI.bairro ?? "",
                Numero = clienteAPI.numero ?? "",
                DataCadastro = DateTime.Now
            };
        }
    }

    // Classes para API de Clientes
    public class ClienteAPI
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string cpf { get; set; }
        public string telefone { get; set; }
        public string cep { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string bairro { get; set; }
        public string rua { get; set; }
        public string numero { get; set; }
    }

    public class ClienteAPICreate
    {
        public string nome { get; set; }
        public string email { get; set; }
        public string cpf { get; set; }
        public string telefone { get; set; }
        public string cep { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string bairro { get; set; }
        public string rua { get; set; }
        public string numero { get; set; }
    }
}