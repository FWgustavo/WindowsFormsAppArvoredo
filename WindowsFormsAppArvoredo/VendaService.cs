using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WindowsFormsAppArvoredo
{
    /// <summary>
    /// Serviço para gerenciar vendas via API
    /// </summary>
    public static class VendaService
    {
        /// <summary>
        /// Cria uma venda diretamente (sem orçamento prévio)
        /// </summary>
        public static async Task<VendaAPIResponse> CriarVendaDiretaAsync(Orcamento orcamento, int usuarioId)
        {
            try
            {
                var vendaAPI = new VendaAPICreate
                {
                    descricao = $"Venda - {orcamento.Cliente}",
                    usuarioId = usuarioId,
                    clienteId = null,
                    nome = orcamento.Cliente,
                    cpf = orcamento.CPF_CNPJ,
                    cep = orcamento.CEP,
                    cidade = orcamento.Cidade,
                    estado = orcamento.UF,
                    bairro = orcamento.Bairro,
                    rua = orcamento.Endereco,
                    numero = orcamento.Numero,
                    telefone = orcamento.Telefone,
                    forma = orcamento.FormaPagamento ?? "Dinheiro",
                    valorTotal = (double)orcamento.TotalGeral,
                    pago = false,
                    vendaE = orcamento.Itens.Select(item => new VendaEAPICreate
                    {
                        produtoId = item.ProdutoOrigem?.Id,
                        quantidade = (int)item.Quantidade,
                        valorVenda = (double)item.ValorUnitario,
                        valorTotal = (double)item.ValorTotal
                    }).ToList()
                };

                var response = await ApiClient.PostAsync<VendaAPICreate, VendaAPIResponse>(
                    "/vendas",
                    vendaAPI
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar venda: {ex.Message}");
            }
        }

        /// <summary>
        /// Converte um orçamento existente em venda
        /// </summary>
        public static async Task<VendaAPIResponse> ConverterOrcamentoParaVendaAsync(int orcamentoId, int usuarioId)
        {
            try
            {
                var vendaCreate = new VendaFromOrcamentoCreate
                {
                    usuarioId = usuarioId,
                    pago = false
                };

                var response = await ApiClient.PostAsync<VendaFromOrcamentoCreate, VendaAPIResponse>(
                    $"/vendas/from-orcamento/{orcamentoId}",
                    vendaCreate
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao converter orçamento para venda: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca todas as vendas
        /// </summary>
        public static async Task<List<VendaAPIResponse>> BuscarVendasAsync(string filtroNome = null)
        {
            try
            {
                string endpoint = "/vendas";

                if (!string.IsNullOrEmpty(filtroNome))
                {
                    endpoint += $"?nome={Uri.EscapeDataString(filtroNome)}";
                }

                var response = await ApiClient.GetAsync<List<VendaAPIResponse>>(endpoint);
                return response ?? new List<VendaAPIResponse>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar vendas: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca uma venda específica por ID
        /// </summary>
        public static async Task<VendaAPIResponse> BuscarVendaPorIdAsync(int id)
        {
            try
            {
                var response = await ApiClient.GetAsync<VendaAPIResponse>($"/vendas/{id}");
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar venda: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza uma venda existente
        /// </summary>
        public static async Task<VendaAPIResponse> AtualizarVendaAsync(int id, VendaAPIUpdate vendaUpdate)
        {
            try
            {
                var response = await ApiClient.PutAsync<VendaAPIUpdate, VendaAPIResponse>(
                    $"/vendas/{id}",
                    vendaUpdate
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar venda: {ex.Message}");
            }
        }

        /// <summary>
        /// Deleta uma venda
        /// </summary>
        public static async Task<bool> DeletarVendaAsync(int id)
        {
            try
            {
                return await ApiClient.DeleteAsync($"/vendas/{id}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar venda: {ex.Message}");
            }
        }
    }

    #region Modelos da API de Vendas

    public class VendaAPICreate
    {
        public string descricao { get; set; }
        public int usuarioId { get; set; }
        public int? clienteId { get; set; }
        public string nome { get; set; }
        public string cpf { get; set; }
        public string cep { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string bairro { get; set; }
        public string rua { get; set; }
        public string numero { get; set; }
        public string telefone { get; set; }
        public string forma { get; set; }
        public double valorTotal { get; set; }
        public bool pago { get; set; }
        public DateTime? dataPagamento { get; set; }
        public List<VendaEAPICreate> vendaE { get; set; }
    }

    public class VendaAPIUpdate
    {
        public string descricao { get; set; }
        public int? clienteId { get; set; }
        public int? usuarioId { get; set; }
        public string nome { get; set; }
        public string cpf { get; set; }
        public string cep { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string bairro { get; set; }
        public string rua { get; set; }
        public string numero { get; set; }
        public string telefone { get; set; }
        public string forma { get; set; }
        public DateTime? dataPagamento { get; set; }
        public bool pago { get; set; }
    }

    public class VendaEAPICreate
    {
        public int? estoqueMadeiraId { get; set; }
        public int? produtoId { get; set; }
        public string pecaId { get; set; }
        public int quantidade { get; set; }
        public double valorVenda { get; set; }
        public double valorTotal { get; set; }
    }

    public class VendaFromOrcamentoCreate
    {
        public int usuarioId { get; set; }
        public bool pago { get; set; }
    }

    #endregion
}