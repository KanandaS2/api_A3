using api_A3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TarefasAPI.Models;

namespace api_A3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration; //compila o arquivo de configurações
        private readonly DatabaseConnection _databaseConnection;
 
        public AuthController(IConfiguration configuration, DatabaseConnection databaseConnection)
        {
            _configuration = configuration;
            _databaseConnection = databaseConnection;
        }
 
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            //busca pelo usuário    

            var sql = "SELECT * FROM tbUsuario WHERE emailUsuario = @email";
            var ret = await _databaseConnection.QueryAsync<Usuarios>(sql, new{
                email = request.Email
            });
            var user = ret.FirstOrDefault();
            //fim da busca pelo usuário

            if(user != null){
                if (request.Email == user.emailUsuario && request.Password == user.senhaUsuario)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_configuration["secretkey:YourSecretKeyHere"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, request.Email) }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return Ok(new { Token = tokenHandler.WriteToken(token) });
                }else{
                    return Ok("Email/Senha inválido.");
                }
            }else{
                 return Ok("Usuário não existe no banco de dados.");
            }
        }
    }
 
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
 