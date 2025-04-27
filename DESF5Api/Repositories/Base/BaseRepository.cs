using Dapper;
using DESF5Api.Services.Observer;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DESF5Api.Repositories.Base
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly string _connectionString;
        protected readonly ISubject _subject;
        protected abstract string TableName { get; }

        protected BaseRepository(IConfiguration configuration, ISubject subject)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
            _subject = subject ?? throw new ArgumentNullException(nameof(subject));
        }

        protected IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public virtual async Task<IEnumerable<T>> ListarTodos()
        {
            _subject.Notify($"Listando todos os registros de {typeof(T).Name}");
            using var connection = CreateConnection();
            return await connection.QueryAsync<T>($"SELECT * FROM {TableName}");
        }

        public virtual async Task<T?> BuscarPorId(long id)
        {
            _subject.Notify($"Buscando {typeof(T).Name} por ID: {id}");
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(
                $"SELECT * FROM {TableName} WHERE Id = @Id", new { Id = id });
        }

        public virtual async Task<IEnumerable<T>> BuscarPorNome(string nome)
        {
            _subject.Notify($"Buscando {typeof(T).Name} por nome: {nome}");
            using var connection = CreateConnection();
            return await connection.QueryAsync<T>(
                $"SELECT * FROM {TableName} WHERE Nome LIKE '%' + @Nome + '%'",
                new { Nome = nome });
        }

        public virtual async Task<T> Inserir(T entity)
        {
            _subject.Notify($"Inserindo novo registro de {typeof(T).Name}");
            using var connection = CreateConnection();
            var id = await connection.ExecuteScalarAsync<int>(
                $"INSERT INTO {TableName} ({GetInsertColumns()}) VALUES ({GetInsertValues()}) RETURNING Id;",
                entity);

            // Carrega o ID gerado na base de dados
            typeof(T).GetProperty("Id")?.SetValue(entity, id);
            return entity;
        }

        public virtual async Task<T> Atualizar(T entity, long Id)
        {
            _subject.Notify($"Atualizando registro de {typeof(T).Name}. Id: {Id}");
            using var connection = CreateConnection();
            var id = await connection.ExecuteScalarAsync<int>(
                $"UPDATE {TableName} SET {GetUpdateColumnsWithValues()} WHERE Id = {Id};",
                entity);

            return entity;
        }

        public virtual async Task<bool> Deletar(long id)
        {
            _subject.Notify($"Deletando {typeof(T).Name} com ID: {id}");
            using var connection = CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TableName} WHERE Id = @Id", new { Id = id });
            return affectedRows > 0;
        }

        public virtual async Task<int> Contar()
        {
            _subject.Notify($"Contando registros de {typeof(T).Name}");
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM {TableName}");
        }

        protected abstract string GetInsertColumns();

        protected abstract string GetInsertValues();

        protected abstract string GetUpdateColumns();

        protected abstract string GetUpdateColumnsWithValues();
    }
}