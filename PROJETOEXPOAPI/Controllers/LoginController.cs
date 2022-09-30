using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using PROJETOEXPOAPI.Interfaces;
using PROJETOEXPOAPI.Models;
using PROJETOEXPOAPI.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PROJETOEXPOAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioRepository _iUsuarioRepository;

        public LoginController(IUsuarioRepository iUsuarioRepository)
        {
            _iUsuarioRepository = iUsuarioRepository;
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel dadosLogin)
        {
            try
            {
                Usuario usuarioBuscado = _iUsuarioRepository.Login(dadosLogin.email, dadosLogin.senha);

                if (usuarioBuscado == null)
                {
                    return Unauthorized(new { msg = "Email e/ou Senha incorretos" });
                }

                var minhasClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, usuarioBuscado.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, usuarioBuscado.Id.ToString()),
                    new Claim(ClaimTypes.Role, usuarioBuscado.Tipo)
                };

                var chave = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("arquivo-chave-autenticacao"));

                var credencials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

                var meuToken = new JwtSecurityToken(
                    issuer: "projeto.webapi",
                    audience: "projeto.webapi",
                    claims: minhasClaims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials :credencials
                    );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(meuToken) });
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
    }
}
