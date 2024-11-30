using api_A3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarefasAPI.Models;

namespace TarefasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly DatabaseConnection _databaseConnection;
 
        public TarefaController(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
 
        [HttpGet]
        [Authorize]
        //após finalizar, adicionar o authorize nos demais métodos
        public async Task<IEnumerable<Tarefas>> GetTarefas()
        {
                var sql = "SELECT * FROM tbTarefa";
                return  await _databaseConnection.QueryAsync<Tarefas>(sql);
                // AWAIT = APENAS PARA MÉTODOS ASSINCRONOS 
        }
 
        [HttpPost]
        public async Task<IActionResult> CreateTarefa(Tarefas tarefa)
        {
            var sql = "INSERT INTO tbTarefa ( nomeTarefa, dataConclusaoTarefa, descricaoTarefa, idUsuario, idCategoria)" +
                      "VALUES (@nomeTarefa, @dataConclusaoTarefa, @descricaoTarefa, @idUsuario, @idCategoria)";
            var result = await _databaseConnection.ExecuteAsync(sql, tarefa);
            return Ok(new { RowsAffected = result });
        }

        [HttpPut]
       public async Task<IActionResult> UpdateTarefa(Tarefas tarefa){
            var sql = @"UPDATE tbTarefa SET nomeTarefa = @nomeTarefa, dataConclusaoTarefa = @dataConclusaoTarefa, descricaoTarefa = @descricaoTarefa, idUsuario = @idUsuario, idCategoria = @idCategoria 
                      WHERE idTarefa = @idTarefa";
             var result = await _databaseConnection.ExecuteAsync(sql, tarefa);
             return Ok(new { RowsAffected = result });
       }

       [HttpDelete]
       public async Task<IActionResult> DeleteTarefa(int id){
            var sql = @"DELETE tbTarefa WHERE idTarefa = @idTarefa";
             var result = await _databaseConnection.ExecuteAsync(sql, new {
                idTarefa = id
             });
             return Ok(new { RowsAffected = result });
       }
    }
}