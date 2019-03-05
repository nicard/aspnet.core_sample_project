using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Alura.ListaLeitura.Seguranca;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Alura.ListaLeitura.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public LoginController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> TokenLogin(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, false, false);
                if (result.Succeeded)
                {
                    var direitos = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, model.Login),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    var chave = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("teste-chave-validade-api-jwt-token"));
                    var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: "WebApp",
                        audience: "Postman",
                        claims: direitos,
                        signingCredentials: credenciais,
                        expires: DateTime.Now.AddMinutes(30)
                    );


                    string sToken = new JwtSecurityTokenHandler().WriteToken(token);

                    return Ok(sToken);
                }
                ModelState.AddModelError(String.Empty, "Erro na autenticação");
                return Unauthorized();
            }
            return BadRequest();
        }
    }
}