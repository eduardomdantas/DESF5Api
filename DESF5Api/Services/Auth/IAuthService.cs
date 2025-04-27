using DESF5Api.Models.Auth;

namespace DESF5Api.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
    }
}