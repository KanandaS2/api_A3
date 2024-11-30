using api_A3.Data;
using Microsoft.AspNetCore.Mvc;
using TarefasAPI.Models;

namespace TarefasAPI.Models
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly DatabaseConnection _databaseConnection;
 
        public CategoriaController(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
 
        [HttpGet]
        //[Authorize]//
        //após finalizar, adicionar o authorize nos demais métodos
        public async Task<IEnumerable<Categoria>> GetCategoria()
        {
                var sql = "SELECT * FROM tbCategoria";
                return  await _databaseConnection.QueryAsync<Categoria>(sql);
                // AWAIT = APENAS PARA MÉTODOS ASSINCRONOS 
        }
 
        [HttpPost]
        public async Task<IActionResult> CreateCategoria(Categoria categoria)
        {
            var sql = "INSERT INTO tbCategoria ( nomeCategoria)" +
                      "VALUES (@nomeCategoria)";
            var result = await _databaseConnection.ExecuteAsync(sql, categoria);
            return Ok(new { RowsAffected = result });
        }

        [HttpPut]
       public async Task<IActionResult> UpdateCategoria(Categoria categoria){
            var sql = @"UPDATE tbCategoria SET nomeCategoria = @nomeCategoria 
                      WHERE idCategoria = @idCategoria";
             var result = await _databaseConnection.ExecuteAsync(sql, categoria);
             return Ok(new { RowsAffected = result });
       }

       [HttpDelete]
       public async Task<IActionResult> DeleteCategoria(int id){
            var sql = @"DELETE tbCategoria WHERE idCategoria = @idCategoria";
             var result = await _databaseConnection.ExecuteAsync(sql, new {
                idCategoria = id
             });
             return Ok(new { RowsAffected = result });
       }
    }
}