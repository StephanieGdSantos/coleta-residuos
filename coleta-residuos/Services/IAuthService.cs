using coleta_residuos.Models;

namespace coleta_residuos.Services
{
    public interface IAuthService
    {
        UserModel Authenticate(string username, string password);

    }
}
