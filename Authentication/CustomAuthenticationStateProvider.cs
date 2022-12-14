using LoginBlazor.Data;
using LoginBlazor.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoginBlazor.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime jsRuntime;
        private readonly IUserService usuarioService;

        private User UsuarioEmCache;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime, IUserService usuarioService)
        {
            this.jsRuntime = jsRuntime;
            this.usuarioService = usuarioService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identidade = new ClaimsIdentity();

            if (UsuarioEmCache == null)
            {
                var usuarioJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");

                if (!string.IsNullOrEmpty(usuarioJson))
                {
                    User usuarioSessao = JsonSerializer.Deserialize<User>(usuarioJson);
                    ValidarLogin(usuarioSessao.Usuario, usuarioSessao.Senha);
                }
            }
            else
            {
                identidade = ConfigurarClaimsDoUsuario(UsuarioEmCache);
            }

            ClaimsPrincipal cachedClaimsPrincipal = new ClaimsPrincipal(identidade);
            return await Task.FromResult(new AuthenticationState(cachedClaimsPrincipal));
        }

        public void ValidarLogin(string usuario, string senha)
        {
            Console.WriteLine("Validação de login");

            if (string.IsNullOrEmpty(usuario))
                throw new Exception("Informe o usuário");

            if (string.IsNullOrEmpty(senha))
                throw new Exception("Informe a senha");

            var identidade = new ClaimsIdentity();

            try
            {
                User user = usuarioService.ValidarUsuario(usuario, senha);
                identidade = ConfigurarClaimsDoUsuario(user);
                string dadosSerializados = JsonSerializer.Serialize(user);
                jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", dadosSerializados);
                UsuarioEmCache = user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identidade))));
        }

        public void Logout()
        {
            UsuarioEmCache = null;
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private ClaimsIdentity ConfigurarClaimsDoUsuario(User usuario)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, usuario.Usuario));
            claims.Add(new Claim(ClaimTypes.Role, usuario.Funcao));
            claims.Add(new Claim("Cidade", usuario.Cidade));
            claims.Add(new Claim("Dominio", usuario.Dominio));
            claims.Add(new Claim(ClaimTypes.DateOfBirth, usuario.AnoNascimento.ToString()));
            claims.Add(new Claim("NivelSeguranca", usuario.NivelSeguranca.ToString()));

            ClaimsIdentity identidade = new ClaimsIdentity(claims, "apiauth_type");
            return identidade;
        }
    }
}
