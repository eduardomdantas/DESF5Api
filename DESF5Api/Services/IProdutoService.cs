using DESF5Api.Models;

namespace DESF5Api.Services
{
    public interface IProdutoService
    {
        Task<IEnumerable<Produto>> ListarTodos();

        Task<Produto?> BuscarPorId(int id);

        Task<IEnumerable<Produto>> BuscarPorNome(string nome);

        Task<Produto> Criar(Produto produto);

        Task<Produto> Atualizar(int id, Produto produto);

        Task<bool> Deletar(int id);

        Task<int> Contar();
    }
}