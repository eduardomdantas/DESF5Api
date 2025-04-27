using DESF5Api.Models;
using DESF5Api.Repositories.Base;
using DESF5Api.Services.Observer;

namespace DESF5Api.Repositories
{
    public class ProdutoRepository(IConfiguration configuration, ISubject subject) : BaseRepository<Produto>(configuration, subject), IProdutoRepository
    {
        protected override string TableName => "Produtos";

        protected override string GetInsertColumns() =>
            "Nome, Descricao, Preco";

        protected override string GetInsertValues()
        {
            var columns = GetInsertColumns().Split(", ");
            var valuesWithAt = columns.Select(column => $"@{column}");
            return string.Join(", ", valuesWithAt);
        }

        protected override string GetUpdateColumns() =>
            "Nome, Descricao, Preco, DataAtualizacao";

        protected override string GetUpdateColumnsWithValues()
        {
            var columns = GetInsertColumns().Split(", ");
            var valuesWithAt = columns.Select(column => $"{column}=@{column}");
            return string.Join(", ", valuesWithAt);
        }
    }
}