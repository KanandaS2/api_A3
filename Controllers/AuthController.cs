using api_A3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TarefasAPI.Models;

namespace api_A3.Controllers
{
    [ApiController] // Declara que esta classe é um controlador de API.
    [Route("api/[controller]")] // Define a rota base para o controlador como "api/Auth".
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration; // Armazena a configuração da aplicação, como variáveis de ambiente.
        private readonly DatabaseConnection _databaseConnection; // Representa a conexão com o banco de dados.

        // Construtor que injeta as dependências de configuração e conexão com o banco de dados.
        public AuthController(IConfiguration configuration, DatabaseConnection databaseConnection)
        {
            _configuration = configuration; // Atribui a configuração à variável local.
            _databaseConnection = databaseConnection; // Atribui a conexão com o banco à variável local.
        }

        [HttpPost("login")] // Define um endpoint POST em "api/Auth/login".
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Consulta SQL para buscar o usuário no banco de dados com base no email fornecido.
            var sql = "SELECT * FROM tbUsuario WHERE emailUsuario = @email";

            // Executa a consulta usando o objeto de conexão com o banco e atribui os resultados a 'ret'.
            var ret = await _databaseConnection.QueryAsync<Usuarios>(sql, new {
                email = request.Email // Passa o email do request como parâmetro para a consulta.
            });

            // Obtém o primeiro usuário da lista retornada (ou null se a lista estiver vazia).
            var user = ret.FirstOrDefault();

            // Verifica se o usuário foi encontrado.
            if (user != null)
            {
                // Compara o email e a senha fornecidos com os do usuário encontrado.
                if (request.Email == user.emailUsuario && request.Password == user.senhaUsuario)
                {
                    // Cria um manipulador para tokens JWT.
                    var tokenHandler = new JwtSecurityTokenHandler();

                    // Gera uma chave de segurança usando a chave secreta configurada.
                    var key = Encoding.ASCII.GetBytes(_configuration["secretkey:YourSecretKeyHere"]);

                    // Define as propriedades do token JWT, como as claims e o tempo de expiração.
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, request.Email) }), // Adiciona a claim do email.
                        Expires = DateTime.UtcNow.AddHours(1), // Define o tempo de expiração do token para 1 hora.
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Define a chave e o algoritmo de assinatura.
                    };

                    // Cria o token usando as configurações definidas.
                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    // Retorna o token gerado ao cliente.
                    return Ok(new { Token = tokenHandler.WriteToken(token) });
                }
                else
                {
                    // Retorna uma mensagem indicando que o email ou a senha estão incorretos.
                    return Ok("Email/Senha inválido.");
                }
            }
            else
            {
                // Retorna uma mensagem indicando que o usuário não foi encontrado no banco de dados.
                return Ok("Usuário não existe no banco de dados.");
            }
        }
    }

    // Classe que representa a estrutura do request de login.
    public class LoginRequest
    {
        public string Email { get; set; } // Propriedade para armazenar o email do usuário.
        public string Password { get; set; } // Propriedade para armazenar a senha do usuário.
    }
}
