using Dapper;
using DESF5Api.Models;
using System.Data;

namespace DESF5Api.Repositories
{
    public class UsuarioRepository(DapperContext context) : IUsuarioRepository
    {
        private readonly IDbConnection _connection = context.CreateConnection();

        public async Task<Usuario?> BuscarPorNomeUsuario(string nomeUsuario)
        {
            return await _connection.QueryFirstOrDefaultAsync<Usuario>(
                "SELECT * FROM Usuarios WHERE Username = @NomeUsuario",
                new { NomeUsuario = nomeUsuario });
        }
    }
}