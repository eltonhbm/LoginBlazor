using LoginBlazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoginBlazor.Data.Impl
{
    public class InMemoryUserService : IUserService
    {
        private List<User> usuarios;

        public InMemoryUserService()
        {
            usuarios = new[] {
            new User {
                Cidade = "Jales",
                Dominio = "sistemasbr",
                Senha = "123456",
                Funcao = "admin",
                AnoNascimento = 1990,
                NivelSeguranca = 5,
                Usuario = "elton"
            },
            new User {
                Cidade = "Jales",
                Dominio = "sistemasbr",
                Senha = "123456",
                Funcao = "admin",
                AnoNascimento = 1989,
                NivelSeguranca = 4,
                Usuario = "fernando"
            },
            new User {
                Cidade = "Jales",
                Dominio = "sistemasbr",
                Senha = "123456",
                Funcao = "suporte",
                AnoNascimento = 1999,
                NivelSeguranca = 3,
                Usuario = "jessica"
            },
            new User {
                Cidade = "Turmalina",
                Dominio = "sistemasbr",
                Senha = "123456",
                Funcao = "suporte",
                AnoNascimento = 2001,
                NivelSeguranca = 2,
                Usuario = "bruna"
            },
            new User {
                Cidade = "Jales",
                Dominio = "sistemasbr",
                Senha = "123456",
                Funcao = "vendedor",
                AnoNascimento = 2001,
                NivelSeguranca = 1,
                Usuario = "jose"
            },
            new User {
                Cidade = "Fortaleza",
                Dominio = "parceiro",
                Senha = "123456",
                Funcao = "admin",
                AnoNascimento = 1969,
                NivelSeguranca = 3,
                Usuario = "claudio"
            }
        }.ToList();
        }

        public User ValidarUsuario(string usuario, string senha)
        {
            User primeiroEncontrado = usuarios.FirstOrDefault(user => user.Usuario.Equals(usuario));

            if (primeiroEncontrado == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            if (!primeiroEncontrado.Senha.Equals(senha))
            {
                throw new Exception("Senha incorreta");
            }

            return primeiroEncontrado;
        }
    }
}
