using api_A3.Data;
using Microsoft.AspNetCore.Mvc;
using TarefasAPI.Models;

namespace TarefasAPI.Models
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly DatabaseConnection _databaseConnection;
 
        public UsuarioController(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
 
        [HttpGet]
        //[Authorize]//
        //após finalizar, adicionar o authorize nos demais métodos
        public async Task<IEnumerable<Usuarios>> GetUsuarios()
        {
                var sql = "SELECT * FROM tbUsuario";
                return  await _databaseConnection.QueryAsync<Usuarios>(sql);
                // AWAIT = APENAS PARA MÉTODOS ASSINCRONOS 
        }
 
        [HttpPost]
        public async Task<IActionResult> CreateUsuarios(Usuarios usuarios)
        {
            var sql = "INSERT INTO tbUsuario ( nomeUsuario, emailUsuario, senhaUsuario)" +
                      "VALUES (@nomeUsuario, @emailUsuario, @senhaUsuario)";
            var result = await _databaseConnection.ExecuteAsync(sql, usuarios);
            return Ok(new { RowsAffected = result });
        }

        [HttpPut]
       public async Task<IActionResult> UpdateUsuario(Usuarios usuarios){
            var sql = @"UPDATE tbUsuario ( nomeUsuario, emailUsuario, senhaUsuario)" +
                      "VALUES (@nomeUsuario, @emailUsuario, @senhaUsuario)";
             var result = await _databaseConnection.ExecuteAsync(sql, usuarios);
             return Ok(new { RowsAffected = result });
       }

       [HttpDelete]
       public async Task<IActionResult> DeleteCategoria(int id){
            var sql = @"DELETE tbUsuario WHERE idUsuario = @idUsuario";
             var result = await _databaseConnection.ExecuteAsync(sql, new {
                idCategoria = id
             });
             return Ok(new { RowsAffected = result });
       }
    }
}