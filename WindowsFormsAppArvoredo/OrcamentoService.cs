using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WindowsFormsAppArvoredo
{
    /// <summary>
    /// Serviço para gerenciar orçamentos via API
    /// </summary>
    public static class OrcamentoService
    {
        /// <summary>
        /// Cria um novo orçamento na API
        /// </summary>
        public static async Task<OrcamentoAPIResponse> CriarOrcamentoAsync(Orcamento orcamento, int usuarioId)
        {
            try
            {
                var orcamentoAPI = new OrcamentoAPICreate
                {
                    descricao = $"Orçamento - {orcamento.Cliente}",
                    usuarioId = usuarioId,
                    clienteId = null, // Pode ser implementado se tiver cadastro de clientes
                    nome = orcamento.Cliente,
                    cpf = orcamento.CPF_CNPJ,
                    cep = orcamento.CEP,
                    cidade = orcamento.Cidade,
                    estado = orcamento.UF,
                    bairro = orcamento.Bairro,
                    rua = orcamento.Endereco,
                    numero = orcamento.Numero,
                    telefone = orcamento.Telefone,
                    forma = orcamento.FormaPagamento,
                    valorTotal = (double)orcamento.TotalGeral,
                    orcamentoE = orcamento.Itens.Select(item => new OrcamentoEAPICreate
                    {
                        produtoId = item.ProdutoOrigem?.Id,
                        quantidade = (int)item.Quantidade,
                        valorVenda = (double)item.ValorUnitario,
                        valorTotal = (double)item.ValorTotal
                    }).ToList()
                };

                var response = await ApiClient.PostAsync<OrcamentoAPICreate, OrcamentoAPIResponse>(
                    "/orcamentos",
                    orcamentoAPI
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar orçamento: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca todos os orçamentos
        /// </summary>
        public static async Task<List<OrcamentoAPIResponse>> BuscarOrcamentosAsync(string filtroNome = null)
        {
            try
            {
                string endpoint = "/orcamentos";

                if (!string.IsNullOrEmpty(filtroNome))
                {
                    endpoint += $"?nome={Uri.EscapeDataString(filtroNome)}";
                }

                var response = await ApiClient.GetAsync<List<OrcamentoAPIResponse>>(endpoint);
                return response ?? new List<OrcamentoAPIResponse>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar orçamentos: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca um orçamento específico por ID
        /// </summary>
        public static async Task<OrcamentoAPIResponse> BuscarOrcamentoPorIdAsync(int id)
        {
            try
            {
                var response = await ApiClient.GetAsync<OrcamentoAPIResponse>($"/orcamentos/{id}");
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar orçamento: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza um orçamento existente
        /// </summary>
        public static async Task<OrcamentoAPIResponse> AtualizarOrcamentoAsync(int id, Orcamento orcamento)
        {
            try
            {
                var orcamentoAPI = new OrcamentoAPIUpdate
                {
                    descricao = $"Orçamento - {orcamento.Cliente}",
                    nome = orcamento.Cliente,
                    cpf = orcamento.CPF_CNPJ,
                    cep = orcamento.CEP,
                    cidade = orcamento.Cidade,
                    estado = orcamento.UF,
                    bairro = orcamento.Bairro,
                    rua = orcamento.Endereco,
                    numero = orcamento.Numero,
                    telefone = orcamento.Telefone,
                    forma = orcamento.FormaPagamento,
                    valorTotal = (double)orcamento.TotalGeral
                };

                var response = await ApiClient.PutAsync<OrcamentoAPIUpdate, OrcamentoAPIResponse>(
                    $"/orcamentos/{id}",
                    orcamentoAPI
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar orçamento: {ex.Message}");
            }
        }

        /// <summary>
        /// Deleta um orçamento
        /// </summary>
        public static async Task<bool> DeletarOrcamentoAsync(int id)
        {
            try
            {
                return await ApiClient.DeleteAsync($"/orcamentos/{id}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar orçamento: {ex.Message}");
            }
        }

        /// <summary>
        /// Converte orçamento para venda (confirma o orçamento)
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
        /// Adiciona itens a um orçamento existente
        /// </summary>
        public static async Task<OrcamentoAPIResponse> AdicionarItensOrcamentoAsync(int orcamentoId, List<ItemOrcamento> itens)
        {
            try
            {
                var itensAPI = new OrcamentoAddItens
                {
                    orcamentoE = itens.Select(item => new OrcamentoEAPICreate
                    {
                        produtoId = item.ProdutoOrigem?.Id,
                        quantidade = (int)item.Quantidade,
                        valorVenda = (double)item.ValorUnitario,
                        valorTotal = (double)item.ValorTotal
                    }).ToList()
                };

                var response = await ApiClient.PostAsync<OrcamentoAddItens, OrcamentoAPIResponse>(
                    $"/orcamentos/{orcamentoId}/orcamentosE",
                    itensAPI
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar itens ao orçamento: {ex.Message}");
            }
        }

        /// <summary>
        /// Carrega todos os orçamentos da API
        /// </summary>
        public static async Task<List<Orcamento>> CarregarOrcamentosAsync()
        {
            try
            {
                var orcamentosAPI = await BuscarOrcamentosAsync();
                return orcamentosAPI.Select(o => ConverterParaOrcamentoLocal(o)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar orçamentos: {ex.Message}");
            }
        }

        /// <summary>
        /// Converte OrcamentoAPIResponse para Orcamento local
        /// </summary>
        public static Orcamento ConverterParaOrcamentoLocal(OrcamentoAPIResponse apiResponse)
        {
            var orcamento = new Orcamento
            {
                Id = apiResponse.id,
                Cliente = apiResponse.nome ?? "",
                CPF_CNPJ = apiResponse.cpf ?? "",
                Endereco = apiResponse.rua ?? "",
                Numero = apiResponse.numero ?? "",
                Bairro = apiResponse.bairro ?? "",
                CEP = apiResponse.cep ?? "",
                Cidade = apiResponse.cidade ?? "",
                UF = apiResponse.estado ?? "",
                Telefone = apiResponse.telefone ?? "",
                FormaPagamento = apiResponse.forma ?? "Dinheiro",
                DataEmissao = apiResponse.dataCriacao,
                SubTotal = (decimal)apiResponse.valorTotal,
                Desconto = 0,
                Acrescimo = 0,
                TotalGeral = (decimal)apiResponse.valorTotal,
                Status = "Pendente",
                Vendedor = apiResponse.usuario?.nome ?? "Sistema"
            };

            // Converte itens
            if (apiResponse.orcamentoE != null)
            {
                int sequencia = 1;
                foreach (var itemAPI in apiResponse.orcamentoE)
                {
                    var item = new ItemOrcamento
                    {
                        Sequencia = sequencia++,
                        Descricao = itemAPI.produto?.nome ??
                                   itemAPI.peca?.nome ??
                                   itemAPI.estoqueMadeira?.madeira?.nome ?? "Produto",
                        Unidade = itemAPI.produto?.unidade ?? "un",
                        Quantidade = itemAPI.quantidade ?? 0,
                        ValorUnitario = (decimal)(itemAPI.valorVenda ?? 0),
                        ValorTotal = (decimal)itemAPI.valorTotal
                    };

                    // Mantém referência ao produto se disponível
                    if (itemAPI.produto != null)
                    {
                        item.ProdutoOrigem = new Produto
                        {
                            Id = itemAPI.produto.id,
                            Descricao = itemAPI.produto.nome,
                            Unidade = itemAPI.produto.unidade,
                            ValorUnitario = (decimal)itemAPI.produto.valor
                        };
                    }

                    orcamento.Itens.Add(item);
                }
            }

            return orcamento;
        }
    }

    #region Modelos da API de Orçamentos

    public class OrcamentoAPICreate
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
        public List<OrcamentoEAPICreate> orcamentoE { get; set; }
    }

    public class OrcamentoAPIUpdate
    {
        public string descricao { get; set; }
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
    }

    public class OrcamentoEAPICreate
    {
        public int? estoqueMadeiraId { get; set; }
        public int? produtoId { get; set; }
        public string pecaId { get; set; }
        public int quantidade { get; set; }
        public double valorVenda { get; set; }
        public double valorTotal { get; set; }
    }

    public class OrcamentoAddItens
    {
        public List<OrcamentoEAPICreate> orcamentoE { get; set; }
    }

    public class OrcamentoAPIResponse
    {
        public int id { get; set; }
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
        public DateTime dataCriacao { get; set; }
        public double valorTotal { get; set; }
        public UsuarioAPI usuario { get; set; }
        public ClienteAPI cliente { get; set; }
        public List<OrcamentoEAPI> orcamentoE { get; set; }
    }

    public class OrcamentoEAPI
    {
        public int id { get; set; }
        public int orcamentoId { get; set; }
        public int? estoqueMadeiraId { get; set; }
        public int? produtoId { get; set; }
        public string pecaId { get; set; }
        public int? quantidade { get; set; }
        public double? valorVenda { get; set; }
        public double valorTotal { get; set; }
        public DateTime dataCriacao { get; set; }
        public ProdutoAPI produto { get; set; }
        public PecaAPI peca { get; set; }
        public EstoqueMadeiraAPI estoqueMadeira { get; set; }
    }

    public class PecaAPI
    {
        public string id { get; set; }
        public string nome { get; set; }
        public double valor { get; set; }
        public string unidade { get; set; }
    }

    public class EstoqueMadeiraAPI
    {
        public int id { get; set; }
        public int madeiraId { get; set; }
        public int tamanhoId { get; set; }
        public int quantidade { get; set; }
        public MadeiraAPI madeira { get; set; }
        public TamanhoAPI tamanho { get; set; }
    }

    public class VendaFromOrcamentoCreate
    {
        public int usuarioId { get; set; }
        public bool pago { get; set; }
    }

    public class VendaAPIResponse
    {
        public int id { get; set; }
        public string descricao { get; set; }
        public int usuarioId { get; set; }
        public int? clienteId { get; set; }
        public double valorTotal { get; set; }
        public DateTime dataCriacao { get; set; }
        public DateTime? dataPagamento { get; set; }
        public bool pago { get; set; }
    }

    #endregion
}