using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WindowsFormsAppArvoredo
{
    /// <summary>
    /// Serviço para integração com a API de Usuários
    /// </summary>
    public static class UsuarioService
    {
        /// <summary>
        /// Carrega todos os usuários da API
        /// </summary>
        public static async Task<List<Usuario>> CarregarUsuariosAsync()
        {
            try
            {
                var usuariosAPI = await ApiClient.GetAsync<List<UsuarioAPI>>("/usuarios");
                var usuarios = new List<Usuario>();

                if (usuariosAPI != null)
                {
                    foreach (var usuarioAPI in usuariosAPI)
                    {
                        usuarios.Add(ConverterDeAPI(usuarioAPI));
                    }
                }

                return usuarios;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar usuários: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca usuário por ID
        /// </summary>
        public static async Task<Usuario> BuscarUsuarioPorIdAsync(int id)
        {
            try
            {
                var usuarioAPI = await ApiClient.GetAsync<UsuarioAPI>($"/usuarios/{id}");
                return usuarioAPI != null ? ConverterDeAPI(usuarioAPI) : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar usuário: {ex.Message}");
            }
        }

        /// <summary>
        /// Cria um novo usuário na API
        /// </summary>
        public static async Task<Usuario> CriarUsuarioAsync(Usuario usuario)
        {
            try
            {
                var usuarioCreate = new UsuarioAPICreate
                {
                    login = usuario.Login,
                    senha = usuario.Senha,
                    nome = usuario.Nome,
                    email = usuario.Email,
                    nivelAcesso = ObterNivelAcesso(usuario.Perfil)
                };

                var usuarioAPI = await ApiClient.PostAsync<UsuarioAPICreate, UsuarioAPI>(
                    "/usuarios",
                    usuarioCreate
                );

                return ConverterDeAPI(usuarioAPI);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar usuário: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        public static async Task<Usuario> AtualizarUsuarioAsync(Usuario usuario)
        {
            try
            {
                var usuarioUpdate = new UsuarioAPIUpdate
                {
                    nome = usuario.Nome,
                    login = usuario.Login,
                    senha = usuario.Senha,
                    email = usuario.Email,
                    nivelAcesso = ObterNivelAcesso(usuario.Perfil)
                };

                var usuarioAPI = await ApiClient.PutAsync<UsuarioAPIUpdate, UsuarioAPI>(
                    $"/usuarios/{usuario.Id}",
                    usuarioUpdate
                );

                return ConverterDeAPI(usuarioAPI);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar usuário: {ex.Message}");
            }
        }

        /// <summary>
        /// Deleta um usuário
        /// </summary>
        public static async Task<bool> DeletarUsuarioAsync(int id)
        {
            try
            {
                return await ApiClient.DeleteAsync($"/usuarios/{id}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar usuário: {ex.Message}");
            }
        }

        /// <summary>
        /// Converte UsuarioAPI para Usuario
        /// </summary>
        private static Usuario ConverterDeAPI(UsuarioAPI usuarioAPI)
        {
            return new Usuario
            {
                Id = usuarioAPI.id,
                Nome = usuarioAPI.nome ?? "",
                Login = usuarioAPI.login ?? "",
                Senha = "******", // Não retorna a senha por segurança
                Email = usuarioAPI.email ?? "",
                Perfil = ObterPerfilTexto(usuarioAPI.nivelAcesso),
                Ativo = usuarioAPI.ativo,
                DataCadastro = usuarioAPI.dataCriacao
            };
        }

        /// <summary>
        /// Converte o perfil do usuário para o nível de acesso da API
        /// </summary>
        private static int ObterNivelAcesso(string perfil)
        {
            switch (perfil?.ToUpper())
            {
                case "ADMIN":
                case "ADMINISTRADOR":
                    return 3;

                case "VENDEDOR":
                    return 2;

                case "USUARIO":
                case "MOBILE":
                default:
                    return 1;
            }
        }

        /// <summary>
        /// Converte o nível de acesso da API para o perfil do usuário
        /// </summary>
        private static string ObterPerfilTexto(int nivelAcesso)
        {
            switch (nivelAcesso)
            {
                case 3:
                    return "Admin";
                case 2:
                    return "Vendedor";
                case 1:
                default:
                    return "Usuario";
            }
        }
    }

    // Classes para API de Usuários
    public class UsuarioAPI
    {
        public int id { get; set; }
        public string login { get; set; }
        public string senha { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public int nivelAcesso { get; set; }
        public bool ativo { get; set; }
        public DateTime dataCriacao { get; set; }
    }

    public class UsuarioAPICreate
    {
        public string login { get; set; }
        public string senha { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public int nivelAcesso { get; set; }
    }

    public class UsuarioAPIUpdate
    {
        public string nome { get; set; }
        public string login { get; set; }
        public string senha { get; set; }
        public string email { get; set; }
        public int nivelAcesso { get; set; }
    }
}