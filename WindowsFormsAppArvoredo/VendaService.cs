using Newtonsoft.Json;
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
        // Modelos da API
        public class VendaAPIResponse
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
            public DateTime? dataPagamento { get; set; }
            public double valorTotal { get; set; }
            public bool pago { get; set; }
            public UsuarioAPI usuario { get; set; }
            public ClienteAPI cliente { get; set; }
            public List<VendaEAPI> vendaE { get; set; }
        }

        public class VendaEAPI
        {
            public int id { get; set; }
            public int vendaId { get; set; }
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

        public class TamanhoAPI
        {
            public int id { get; set; }
            public string nome { get; set; }
            public bool ativo { get; set; }
        }

        public class UsuarioAPI
        {
            public int id { get; set; }
            public string nome { get; set; }
            public string login { get; set; }
            public string email { get; set; }
        }

        public class ClienteAPI
        {
            public int id { get; set; }
            public string nome { get; set; }
            public string cpf { get; set; }
        }

        // Modelo para atualização de venda
        public class VendaAPIUpdate
        {
            public string descricao { get; set; }
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
            public string dataPagamento { get; set; }
            public bool pago { get; set; }
        }

        // ---------------------------------------------------------
        // CARREGAR VENDAS DA API
        // ---------------------------------------------------------
        public static async Task<List<VendaAPIResponse>> CarregarVendasAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("\n[VENDA] Iniciando carregamento de vendas...");

                var vendasAPI = await ApiClient.GetAsync<List<VendaAPIResponse>>("/vendas");

                if (vendasAPI == null || vendasAPI.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("[VENDA] Nenhuma venda encontrada na API");
                    return new List<VendaAPIResponse>();
                }

                System.Diagnostics.Debug.WriteLine($"[VENDA] {vendasAPI.Count} venda(s) encontrada(s)");

                return vendasAPI;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VENDA] ❌ Erro ao carregar vendas: {ex.Message}\n");
                throw new Exception($"Erro ao carregar vendas da API: {ex.Message}");
            }
        }

        // ---------------------------------------------------------
        // MARCAR VENDA COMO PAGA
        // ---------------------------------------------------------
        public static async Task<VendaAPIResponse> MarcarComoPagaAsync(int vendaId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[VENDA] Marcando venda #{vendaId} como paga...");

                var vendaUpdate = new VendaAPIUpdate
                {
                    pago = true,
                    dataPagamento = DateTime.Now.ToString("yyyy-MM-dd")
                };

                var response = await ApiClient.PutAsync<VendaAPIUpdate, VendaAPIResponse>(
                    $"/vendas/{vendaId}",
                    vendaUpdate
                );

                if (response != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[VENDA] ✓ Venda #{vendaId} marcada como paga");
                }

                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VENDA] ❌ Erro ao marcar venda como paga: {ex.Message}");
                throw new Exception($"Erro ao marcar venda como paga: {ex.Message}");
            }
        }

        // ---------------------------------------------------------
        // ATUALIZAR VENDA
        // ---------------------------------------------------------
        public static async Task<VendaAPIResponse> AtualizarVendaAsync(int vendaId, VendaAPIUpdate vendaUpdate)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[VENDA] Atualizando venda #{vendaId}...");

                var response = await ApiClient.PutAsync<VendaAPIUpdate, VendaAPIResponse>(
                    $"/vendas/{vendaId}",
                    vendaUpdate
                );

                if (response != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[VENDA] ✓ Venda #{vendaId} atualizada com sucesso");
                }

                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VENDA] ❌ Erro ao atualizar venda: {ex.Message}");
                throw new Exception($"Erro ao atualizar venda: {ex.Message}");
            }
        }

        // ---------------------------------------------------------
        // CONVERTER VENDA API PARA ORCAMENTO LOCAL (PEDIDO)
        // ---------------------------------------------------------
        public static Orcamento ConverterVendaParaOrcamento(VendaAPIResponse vendaAPI)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"\n[DEBUG] Convertendo venda #{vendaAPI.id}");
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Total de itens: {vendaAPI.vendaE?.Count ?? 0}");
                var orcamento = new Orcamento
                {
                    Id = vendaAPI.id,
                    Cliente = vendaAPI.nome ?? "",
                    CPF_CNPJ = vendaAPI.cpf ?? "",
                    Endereco = vendaAPI.rua ?? "",
                    Numero = vendaAPI.numero ?? "",
                    Bairro = vendaAPI.bairro ?? "",
                    CEP = vendaAPI.cep ?? "",
                    Cidade = vendaAPI.cidade ?? "",
                    UF = vendaAPI.estado ?? "",
                    Telefone = vendaAPI.telefone ?? "",
                    FormaPagamento = vendaAPI.forma ?? "Dinheiro",
                    DataEmissao = vendaAPI.dataCriacao,
                    SubTotal = (decimal)vendaAPI.valorTotal,
                    Desconto = 0,
                    Acrescimo = 0,
                    TotalGeral = (decimal)vendaAPI.valorTotal,
                    Status = vendaAPI.pago ? "Pago" : "Pendente",
                    Vendedor = vendaAPI.usuario?.nome ?? "Sistema"
                };

                // Converte itens da venda
                if (vendaAPI.vendaE != null && vendaAPI.vendaE.Count > 0)
                {
                    int sequencia = 1;
                    foreach (var itemAPI in vendaAPI.vendaE)
                    {
                        try
                        {
                            System.Diagnostics.Debug.WriteLine($"\n[DEBUG] Item #{sequencia}:");
                            System.Diagnostics.Debug.WriteLine($"  - produto?.nome: {itemAPI.produto?.nome ?? "NULL"}");
                            System.Diagnostics.Debug.WriteLine($"  - peca?.nome: {itemAPI.peca?.nome ?? "NULL"}");
                            System.Diagnostics.Debug.WriteLine($"  - estoqueMadeira?.madeira?.nome: {itemAPI.estoqueMadeira?.madeira?.nome ?? "NULL"}");

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

                            if (itemAPI.produto != null)
                            {
                                item.ProdutoOrigem = new Produto
                                {
                                    Id = itemAPI.produto.id,
                                    Descricao = itemAPI.produto.nome,  // ← GARANTIR QUE ESTÁ USANDO .nome
                                    Unidade = itemAPI.produto.unidade,
                                    ValorUnitario = (decimal)itemAPI.produto.valor
                                };
                            }

                            orcamento.Itens.Add(item);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[VENDA] ⚠️ Erro ao converter item: {ex.Message}");
                        }
                    }
                }

                return orcamento;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VENDA] ❌ Erro ao converter venda: {ex.Message}");
                throw new Exception($"Erro ao converter venda: {ex.Message}");
            }
        }

        public static async Task<List<Orcamento>> BuscarVendasPagasPorMesAnoAsync(int mes, int ano)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"\n[HISTÓRICO] Buscando vendas pagas de {mes:00}/{ano}...");

                // Monta query string com filtros
                string query = $"/vendas?pago=true";

                // Busca todas as vendas pagas
                var vendasAPI = await ApiClient.GetAsync<List<VendaAPIResponse>>(query);

                if (vendasAPI == null || vendasAPI.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("[HISTÓRICO] Nenhuma venda paga encontrada");
                    return new List<Orcamento>();
                }

                // Filtra localmente por mês/ano da data de criação
                var vendasDoMes = vendasAPI
                    .Where(v => v.dataCriacao.Month == mes && v.dataCriacao.Year == ano)
                    .ToList();

                System.Diagnostics.Debug.WriteLine($"[HISTÓRICO] {vendasDoMes.Count} venda(s) paga(s) encontrada(s) em {mes:00}/{ano}");

                // Converte para Orcamento (para usar nas telas existentes)
                var pedidosFinalizados = new List<Orcamento>();

                foreach (var vendaAPI in vendasDoMes)
                {
                    try
                    {
                        var pedido = ConverterVendaParaOrcamento(vendaAPI);
                        pedidosFinalizados.Add(pedido);
                        System.Diagnostics.Debug.WriteLine($"[HISTÓRICO] ✓ Venda #{vendaAPI.id} - {vendaAPI.nome} - {vendaAPI.valorTotal:C}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[HISTÓRICO] ❌ Erro ao converter venda #{vendaAPI.id}: {ex.Message}");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"[HISTÓRICO] ✓ Total carregado: {pedidosFinalizados.Count} venda(s)\n");

                return pedidosFinalizados;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[HISTÓRICO] ❌ Erro ao buscar vendas: {ex.Message}\n");
                throw new Exception($"Erro ao buscar vendas do histórico: {ex.Message}");
            }
        }
        // Adicionar este método dentro da classe VendaService
        private static string ObterDescricaoItem(VendaEAPI itemAPI)
        {
            // Prioridade: Produto > Madeira com Tamanho > Peça > Fallback

            if (itemAPI.produto != null && !string.IsNullOrEmpty(itemAPI.produto.nome))
            {
                return itemAPI.produto.nome;
            }

            if (itemAPI.estoqueMadeira != null)
            {
                string nomeMadeira = itemAPI.estoqueMadeira.madeira?.nome ?? "Madeira";
                string nomeTamanho = itemAPI.estoqueMadeira.tamanho?.nome ?? "";

                if (!string.IsNullOrEmpty(nomeTamanho))
                {
                    return $"{nomeMadeira} {nomeTamanho}";
                }
                return nomeMadeira;
            }

            if (itemAPI.peca != null && !string.IsNullOrEmpty(itemAPI.peca.nome))
            {
                return itemAPI.peca.nome;
            }

            return "Produto sem descrição";
        }
    }
}