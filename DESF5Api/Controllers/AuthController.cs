using DESF5Api.Models.Auth;
using DESF5Api.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DESF5Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {

        /// <summary>
        /// Realiza o login e retorna um token JWT
        /// </summary>
        /// <param name="request">Credenciais de login</param>
        /// <returns>Token JWT</returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var response = await authService.Login(request);
            return Ok(response);
        }
    }
}