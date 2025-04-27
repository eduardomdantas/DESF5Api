using DESF5Api.Models.Auth;
using DESF5Api.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DESF5Api.Services.Auth
{
    public class AuthService(IConfiguration configuration, IUsuarioRepository usuarioRepository) : IAuthService
    {
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var usuario = await usuarioRepository.BuscarPorNomeUsuario(request.Username);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
                throw new UnauthorizedAccessException("Usuário e/ou senha inválidos");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(key))
                throw new InvalidOperationException("JWT key is not configured.");

            var keyBytes = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                        new(ClaimTypes.Name, usuario.Username),
                        new(ClaimTypes.Role, usuario.Role)
                ]),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new LoginResponse
            {
                Token = tokenHandler.WriteToken(token),
                ExpiresIn = tokenDescriptor.Expires.Value
            };
        }
    }
}