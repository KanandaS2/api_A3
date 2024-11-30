using System.Data;
using System.Data.SqlClient;
using Dapper;
 
namespace api_A3.Data
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;
 
        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }
 
        // Método para obter uma conexão com o banco de dados
        public IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
 
        // Exemplo de método para executar uma consulta SELECT
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using (var connection = GetConnection())
            {
                return await connection.QueryAsync<T>(sql, parameters);
            }
        }
 
        // Exemplo de método para executar um comando INSERT, UPDATE ou DELETE
        public async Task<int> ExecuteAsync(string sql, object parameters = null)
        {
            using (var connection = GetConnection())
            {
                return await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}