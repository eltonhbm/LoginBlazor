using LoginBlazor.Models;

namespace LoginBlazor.Data
{
    public interface IUserService
    {
        User ValidarUsuario(string usuario, string senha);
    }
}
