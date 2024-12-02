using api_A3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using TarefasAPI.Models; 

namespace TarefasAPI.Controllers
{
    // Define que esta classe é um controlador de API
    [ApiController]
    // Define a rota base para as ações deste controlador
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        // Variável para gerenciar a conexão com o banco de dados
        private readonly DatabaseConnection _databaseConnection;

        // Construtor para injetar a dependência da conexão com o banco de dados
        public TarefaController(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        // Endpoint para listar todas as tarefas
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<Tarefas>> GetTarefas()
        {
            // SQL para buscar todas as tarefas no banco de dados
            var sql = "SELECT * FROM tbTarefa";
            // Retorna os resultados da consulta
            return await _databaseConnection.QueryAsync<Tarefas>(sql);
        }

        // Endpoint para criar uma nova tarefa
        [HttpPost]
        public async Task<IActionResult> CreateTarefa([FromBody] Tarefas tarefa)
        {
            // Validação para garantir que os dados da tarefa sejam válidos
            if (tarefa == null || string.IsNullOrEmpty(tarefa.nomeTarefa))
            {
                return BadRequest("Os dados da tarefa são inválidos."); // Retorna um erro 400
            }

            // SQL para inserir uma nova tarefa no banco de dados
            var sql = @"INSERT INTO tbTarefa 
                        (nomeTarefa, dataConclusaoTarefa, descricaoTarefa, idUsuario, idCategoria)
                        VALUES 
                        (@nomeTarefa, @dataConclusaoTarefa, @descricaoTarefa, @idUsuario, @idCategoria)";

            try
            {
                // Executa o comando SQL e retorna o número de linhas afetadas
                var result = await _databaseConnection.ExecuteAsync(sql, tarefa);
                // Retorna uma mensagem de sucesso e o número de linhas afetadas
                return Ok(new { Message = "Tarefa criada com sucesso!", RowsAffected = result });
            }
            catch (Exception ex)
            {
                // Retorna um erro 500 em caso de falha no servidor
                return StatusCode(500, new { Message = "Erro ao criar a tarefa.", Error = ex.Message });
            }
        }

        // Endpoint para atualizar uma tarefa existente
        [HttpPut]
        public async Task<IActionResult> UpdateTarefa([FromBody] Tarefas tarefa)
        {
            // SQL para atualizar os dados de uma tarefa no banco de dados
            var sql = @"UPDATE tbTarefa 
                        SET nomeTarefa = @nomeTarefa, 
                            dataConclusaoTarefa = @dataConclusaoTarefa, 
                            descricaoTarefa = @descricaoTarefa, 
                            completaTarefa = @completaTarefa, 
                            idUsuario = @idUsuario, 
                            idCategoria = @idCategoria
                        WHERE idTarefa = @idTarefa";

            // Executa o comando SQL e retorna o número de linhas afetadas
            var result = await _databaseConnection.ExecuteAsync(sql, tarefa);
            // Retorna uma resposta com o número de linhas afetadas
            return Ok(new { RowsAffected = result });
        }

        // Endpoint para marcar uma tarefa como concluída
        [HttpPatch("{id}/Concluir")]
        public async Task<IActionResult> ConcluirTarefa(int id)
        {
            // SQL para marcar a tarefa como concluída no banco de dados
            var sql = @"UPDATE tbTarefa SET completaTarefa = 1 WHERE idTarefa = @idTarefa";
            // Executa o comando SQL e retorna o número de linhas afetadas
            var result = await _databaseConnection.ExecuteAsync(sql, new { idTarefa = id });
            // Retorna uma resposta com o número de linhas afetadas
            return Ok(new { RowsAffected = result });
        }

        // Endpoint para excluir uma tarefa
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarefa(int id)
        {
            // SQL para excluir a tarefa do banco de dados
            var sql = @"DELETE FROM tbTarefa WHERE idTarefa = @idTarefa";
            // Executa o comando SQL e retorna o número de linhas afetadas
            var result = await _databaseConnection.ExecuteAsync(sql, new { idTarefa = id });
            // Retorna uma resposta com o número de linhas afetadas
            return Ok(new { RowsAffected = result });
        }
    }
}
