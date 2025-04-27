using DESF5Api.Models;
using DESF5Api.Repositories.Base;
using DESF5Api.Services.Observer;

namespace DESF5Api.Repositories
{
    public class ClienteRepository(IConfiguration configuration, ISubject subject) : BaseRepository<Cliente>(configuration, subject), IClienteRepository
    {
        protected override string TableName => "Clientes";

        protected override string GetInsertColumns() =>
            "Nome, CPF, Email, DataCadastro";

        protected override string GetInsertValues()
        {
            var columns = GetInsertColumns().Split(", ");
            var valuesWithAt = columns.Select(column => $"@{column}");
            return string.Join(", ", valuesWithAt);
        }

        protected override string GetUpdateColumns() =>
            "Nome, CPF, Email, DataAtualizacao";

        protected override string GetUpdateColumnsWithValues()
        {
            var columns = GetInsertColumns().Split(", ");
            var valuesWithAt = columns.Select(column => $"{column}=@{column}");
            return string.Join(", ", valuesWithAt);
        }
    }
}