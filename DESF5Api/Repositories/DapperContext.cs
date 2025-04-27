using Dapper;
using Npgsql;
using System.Data;

namespace DESF5Api.Repositories
{
    public class DapperContext(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        public IDbConnection CreateConnection() => new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        public async Task Init()
        {
            using var connection = CreateConnection();
            await InitDatabase();
            await InitTables();
        }

        private async Task InitDatabase()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("DefaultConnection string is not configured");
            }

            using var connection = new NpgsqlConnection(
                connectionString.Replace("Database=desf5db", "Database=postgres"));

            var databaseExists = await connection.ExecuteScalarAsync<int>(
                "SELECT 1 FROM pg_database WHERE datname = 'desf5db';");

            if (databaseExists == 0)
            {
                await connection.ExecuteAsync(
                    "CREATE DATABASE desf5db ENCODING 'UTF8' TEMPLATE template0;");
            }
        }

        private async Task InitTables()
        {
            using var connection = CreateConnection();

            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Clientes (
                    Id BIGSERIAL PRIMARY KEY,
                    Nome VARCHAR(100) NOT NULL,
                    CPF VARCHAR(11) NOT NULL UNIQUE,
                    Email VARCHAR(100) NOT NULL,
                    DataCadastro TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                );

                CREATE TABLE IF NOT EXISTS Produtos (
                    Id BIGSERIAL PRIMARY KEY,
                    Nome VARCHAR(100) NOT NULL,
                    Descricao VARCHAR(500),
                    Preco NUMERIC(18,2) NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Pedidos (
                    Id BIGSERIAL PRIMARY KEY,
                    ClienteId BIGINT NOT NULL REFERENCES Clientes(Id),
                    DataPedido TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                );

                CREATE TABLE IF NOT EXISTS PedidoItens (
                    Id BIGSERIAL PRIMARY KEY,
                    PedidoId BIGINT NOT NULL REFERENCES Pedidos(Id) ON DELETE CASCADE,
                    ProdutoId BIGINT NOT NULL REFERENCES Produtos(Id),
                    Quantidade NUMERIC(18,3) NOT NULL,
                    PrecoUnitario NUMERIC(18,2) NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Usuarios (
                    Id BIGSERIAL PRIMARY KEY,
                    Username VARCHAR(50) NOT NULL UNIQUE,
                    PasswordHash VARCHAR(255) NOT NULL,
                    Role VARCHAR(20) NOT NULL
                );

                INSERT INTO Usuarios (Username, PasswordHash, Role)
                SELECT 'admin', '$2a$12$S7qJ7Q9z4wZ7g8hYQ1Zr3uY9WQ0bKj8XvL7dD1mR3nWv1sV2J5XeK', 'Admin'
                WHERE NOT EXISTS (SELECT 1 FROM Usuarios WHERE Username = 'admin');
            ");
        }
    }
}