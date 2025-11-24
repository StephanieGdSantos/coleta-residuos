using coleta_residuos.Models;

namespace coleta_residuos.Services.Impl
{
    public class AuthService : IAuthService
    {
        private List<UserModel> _users = new List<UserModel>
        {
            new UserModel { UserId = 1, Username = "operador01", Password = "pass123", Role = "operador" },
            new UserModel { UserId = 3, Username = "gerente01", Password = "pass123", Role = "gerente" },
            new UserModel { UserId = 8, Username = "analista01", Password = "pass123", Role = "analista" },
            new UserModel { UserId = 9, Username = "visitante01", Password = "pass123", Role = "visitante" }
        };

        public UserModel Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
