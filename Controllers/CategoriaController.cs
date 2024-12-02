using api_A3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarefasAPI.Models;

namespace TarefasAPI.Models
{
    [ApiController] // Declara que esta classe é um controlador de API.
    [Route("api/[controller]")] // Define a rota base do controlador como "api/Categoria".
    public class CategoriaController : ControllerBase
    {
        private readonly DatabaseConnection _databaseConnection; // Variável para armazenar a conexão com o banco de dados.

        // Construtor que injeta a dependência de conexão com o banco de dados.
        public CategoriaController(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection; // Atribui a conexão à variável local.
        }

        [HttpGet] // Define um endpoint GET para listar categorias.
        public async Task<IEnumerable<Categoria>> GetCategoria()
        {
            // Consulta SQL para buscar todas as categorias na tabela 'tbCategoria'.
            var sql = "SELECT * FROM tbCategoria";
            return await _databaseConnection.QueryAsync<Categoria>(sql); // Executa a consulta de forma assíncrona e retorna os resultados.
            // AWAIT = MÉTODOS ASSÍNCRONOS REQUEREM O USO DE 'AWAIT' PARA ESPERAR A CONCLUSÃO DA TAREFA.
        }

        [HttpPost] // Define um endpoint POST para criar uma nova categoria.
        public async Task<IActionResult> CreateCategoria(Categoria categoria)
        {
            // Comando SQL para inserir uma nova categoria na tabela 'tbCategoria'.
            var sql = "INSERT INTO tbCategoria ( nomeCategoria)" +
                      "VALUES (@nomeCategoria)";
            var result = await _databaseConnection.ExecuteAsync(sql, categoria); // Executa o comando e retorna o número de linhas afetadas.
            return Ok(new { RowsAffected = result }); // Retorna o número de linhas afetadas no banco.
        }

        [HttpPut] // Define um endpoint PUT para atualizar uma categoria existente.
        public async Task<IActionResult> UpdateCategoria(Categoria categoria)
        {
            // Comando SQL para atualizar o nome de uma categoria baseada no ID.
            var sql = @"UPDATE tbCategoria SET nomeCategoria = @nomeCategoria 
                      WHERE idCategoria = @idCategoria";
            var result = await _databaseConnection.ExecuteAsync(sql, categoria); // Executa o comando de atualização.
            return Ok(new { RowsAffected = result }); // Retorna o número de linhas afetadas.
        }

        [HttpDelete] // Define um endpoint DELETE para remover uma categoria.
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            // Comando SQL para deletar uma categoria baseada no ID.
            var sql = @"DELETE tbCategoria WHERE idCategoria = @idCategoria";
            var result = await _databaseConnection.ExecuteAsync(sql, new {
                idCategoria = id // Passa o ID como parâmetro para a consulta.
            });
            return Ok(new { RowsAffected = result }); // Retorna o número de linhas afetadas.
        }
    }
}
