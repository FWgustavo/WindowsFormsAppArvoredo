// ============================================
// ARQUIVO: OrcamentoService.cs
// ============================================

using Newtonsoft.Json;
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
        // ---------------------------------------------------------
        // CRIAR ORÇAMENTO
        // ---------------------------------------------------------
        public static async Task<OrcamentoAPIResponse> CriarOrcamentoAsync(Orcamento orcamento, int usuarioId)
        {
            try
            {
                if (orcamento == null)
                    throw new ArgumentNullException(nameof(orcamento), "Orçamento não pode ser nulo");

                if (string.IsNullOrWhiteSpace(orcamento.Cliente))
                    throw new ArgumentException("Nome do cliente é obrigatório");

                if (orcamento.Itens == null || orcamento.Itens.Count == 0)
                    throw new ArgumentException("Orçamento deve conter pelo menos um item");

                // Converter itens para API
                List<OrcamentoEAPICreate> itensAPI = new List<OrcamentoEAPICreate>();

                foreach (var item in orcamento.Itens)
                {
                    itensAPI.Add(new OrcamentoEAPICreate
                    {
                        produtoId = item.ProdutoOrigem?.Id,
                        quantidade = (int)item.Quantidade,
                        valorVenda = (double)item.ValorUnitario,
                        valorTotal = (double)item.ValorTotal
                    });
                }

                var orcamentoAPI = new OrcamentoAPICreate
                {
                    descricao = $"Orçamento - {orcamento.Cliente}",
                    usuarioId = usuarioId,
                    clienteId = null,
                    nome = orcamento.Cliente,
                    cpf = orcamento.CPF_CNPJ ?? "",
                    cep = orcamento.CEP ?? "",
                    cidade = orcamento.Cidade ?? "",
                    estado = orcamento.UF ?? "",
                    bairro = orcamento.Bairro ?? "",
                    rua = orcamento.Endereco ?? "",
                    numero = orcamento.Numero ?? "",
                    telefone = orcamento.Telefone ?? "",
                    forma = orcamento.FormaPagamento ?? "Dinheiro",
                    valorTotal = (double)orcamento.TotalGeral,
                    orcamentoE = itensAPI
                };

                var response = await ApiClient.PostAsync<OrcamentoAPICreate, OrcamentoAPIResponse>(
                    "/orcamentos", orcamentoAPI);

                if (response == null)
                    throw new Exception("Falha ao receber resposta da API ao criar orçamento");

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar orçamento: {ex.Message}");
            }
        }

        // ---------------------------------------------------------
        // BUSCAR ORÇAMENTOS
        // ---------------------------------------------------------
        public static async Task<List<OrcamentoAPIResponse>> BuscarOrcamentosAsync(string filtroNome = null)
        {
            try
            {
                string endpoint = "/orcamentos";

                if (!string.IsNullOrEmpty(filtroNome))
                {
                    endpoint += $"?nome={Uri.EscapeDataString(filtroNome)}";
                }

                return await ApiClient.GetAsync<List<OrcamentoAPIResponse>>(endpoint)
                       ?? new List<OrcamentoAPIResponse>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar orçamentos: {ex.Message}");
            }
        }

        // ---------------------------------------------------------
        // BUSCAR POR ID
        // ---------------------------------------------------------
        public static async Task<OrcamentoAPIResponse> BuscarOrcamentoPorIdAsync(int id)
        {
            try
            {
                return await ApiClient.GetAsync<OrcamentoAPIResponse>($"/orcamentos/{id}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar orçamento: {ex.Message}");
            }
        }

        // ---------------------------------------------------------
        // ATUALIZAR
        // ---------------------------------------------------------
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

                return await ApiClient.PutAsync<OrcamentoAPIUpdate, OrcamentoAPIResponse>(
                    $"/orcamentos/{id}", orcamentoAPI);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar orçamento: {ex.Message}");
            }
        }

        // ---------------------------------------------------------
        // DELETAR
        // ---------------------------------------------------------
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

        // ---------------------------------------------------------
        // CONVERTER PARA VENDA
        // ---------------------------------------------------------
        // ============================================
        // SUBSTITUIR o método ConverterOrcamentoParaVendaAsync no OrcamentoService.cs
        // ============================================

        public static async Task<VendaAPIResponse> ConverterOrcamentoParaVendaAsync(int orcamentoId, int usuarioId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[CONVERTER] Convertendo orçamento #{orcamentoId} para venda...");

                // Tenta com forma de pagamento padrão
                var vendaCreate = new VendaFromOrcamentoCreate
                {
                    usuarioId = usuarioId,
                    pago = false,
                    forma = "Dinheiro"
                };

                var response = await ApiClient.PostAsync<VendaFromOrcamentoCreate, VendaAPIResponse>(
                    $"/vendas/from-orcamento/{orcamentoId}",
                    vendaCreate
                );

                if (response != null && response.id > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[CONVERTER] ✓ Venda criada com sucesso: ID #{response.id}");
                    return response;
                }

                throw new Exception("Resposta inválida da API ao converter orçamento.");
            }
            catch (Exception ex)
            {
                // Verifica se o erro é "já foi convertido" - isso significa sucesso anterior
                if (ex.Message.Contains("ja foi convertido") ||
                    ex.Message.Contains("já foi convertido") ||
                    ex.Message.Contains("Orcamento ja foi convertido"))
                {
                    System.Diagnostics.Debug.WriteLine($"[CONVERTER] ⚠ Orçamento #{orcamentoId} já foi convertido anteriormente");

                    // Retorna um objeto indicando que a conversão já ocorreu
                    // O ID será 0 mas o chamador deve tratar isso
                    return new VendaAPIResponse
                    {
                        id = orcamentoId, // Usa o mesmo ID como referência
                        pago = false,
                        dataCriacao = DateTime.Now
                    };
                }

                System.Diagnostics.Debug.WriteLine($"[CONVERTER] ❌ Erro: {ex.Message}");
                throw new Exception($"Erro ao converter orçamento para venda: {ex.Message}");
            }
        }



        // Método helper para normalizar forma de 

        // Método auxiliar para remover acentos
        private static string RemoverAcentos(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            string normalizado = texto.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (char c in normalizado)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) !=
                    System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        private static string NormalizarFormaPagamento(string forma)
        {
            if (string.IsNullOrWhiteSpace(forma))
                return "Dinheiro";

            // Mapeamento para valores aceitos pela API
            switch (forma.ToLower().Trim())
            {
                case "débito":
                case "debito":
                    return "Débito";

                case "crédito":
                case "credito":
                    return "Crédito";

                case "pix":
                    return "PIX";

                case "dinheiro":
                default:
                    return "Dinheiro";
            }
        }

        // ---------------------------------------------------------
        // ADICIONAR ITENS
        // ---------------------------------------------------------
        public static async Task<OrcamentoAPIResponse> AdicionarItensOrcamentoAsync(int orcamentoId, List<ItemOrcamento> itens)
        {
            try
            {
                var itensAPI = new OrcamentoAddItens
                {
                    orcamentoE = itens.Select(i => new OrcamentoEAPICreate
                    {
                        produtoId = i.ProdutoOrigem?.Id,
                        quantidade = (int)i.Quantidade,
                        valorVenda = (double)i.ValorUnitario,
                        valorTotal = (double)i.ValorTotal
                    }).ToList()
                };

                return await ApiClient.PostAsync<OrcamentoAddItens, OrcamentoAPIResponse>(
                    $"/orcamentos/{orcamentoId}/orcamentosE",
                    itensAPI
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar itens: {ex.Message}");
            }
        }

        public static async Task<List<Orcamento>> CarregarOrcamentosAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("\n[CARREGAR] Iniciando carregamento de orçamentos...");

                // Busca todos os orçamentos da API
                var orcamentosAPI = await BuscarOrcamentosAsync();

                if (orcamentosAPI == null || orcamentosAPI.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("[CARREGAR] Nenhum orçamento encontrado na API");
                    return new List<Orcamento>();
                }

                System.Diagnostics.Debug.WriteLine($"[CARREGAR] {orcamentosAPI.Count} orçamento(s) encontrado(s)");

                // Converte para formato local
                var orcamentosLocais = new List<Orcamento>();

                foreach (var orcAPI in orcamentosAPI)
                {
                    try
                    {
                        var orcLocal = ConverterParaOrcamentoLocal(orcAPI);
                        orcamentosLocais.Add(orcLocal);
                        System.Diagnostics.Debug.WriteLine($"[CARREGAR] ✓ Orçamento #{orcAPI.id} - {orcAPI.nome}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[CARREGAR] ❌ Erro ao converter orçamento #{orcAPI.id}: {ex.Message}");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"[CARREGAR] ✓ {orcamentosLocais.Count} orçamento(s) carregado(s) com sucesso\n");

                return orcamentosLocais;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CARREGAR] ❌ Erro ao carregar orçamentos: {ex.Message}\n");
                throw new Exception($"Erro ao carregar orçamentos: {ex.Message}");
            }
        }

        public static Orcamento ConverterParaOrcamentoLocal(OrcamentoAPIResponse apiResponse)
        {
            try
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

                // Converte itens do orçamento
                if (apiResponse.orcamentoE != null && apiResponse.orcamentoE.Count > 0)
                {
                    int sequencia = 1;
                    foreach (var itemAPI in apiResponse.orcamentoE)
                    {
                        try
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
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[CONVERTER] ⚠️ Erro ao converter item: {ex.Message}");
                        }
                    }
                }

                return orcamento;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CONVERTER] ❌ Erro ao converter orçamento: {ex.Message}");
                throw new Exception($"Erro ao converter orçamento: {ex.Message}");
            }
        }

        public static async Task<bool> ExcluirOrcamentoAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID do orçamento inválido.");

                System.Diagnostics.Debug.WriteLine($"[API] Solicitando exclusão do orçamento #{id}...");

                bool sucesso = await ApiClient.DeleteAsync($"/orcamentos/{id}");

                if (!sucesso)
                    System.Diagnostics.Debug.WriteLine($"[API] ⚠ A API não confirmou a exclusão do orçamento #{id}");

                return sucesso;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir orçamento #{id}: {ex.Message}");
            }
        }

        private static string FormaPagamentoFixa()
        {
            // 🔒 Sempre retorno a forma fixa desejada:
            return "Dinheiro";
        }


        // ========================================================
        // MODELOS — ÚNICA DEFINIÇÃO (SEM DUPLICAÇÃO)
        // ========================================================

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
            public string forma { get; set; } // ← CAMPO OBRIGATÓRIO
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
    }
}

