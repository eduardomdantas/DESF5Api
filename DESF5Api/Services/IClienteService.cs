using DESF5Api.Models;

namespace DESF5Api.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> ListarTodos();

        Task<Cliente?> BuscarPorId(int id);

        Task<IEnumerable<Cliente>> BuscarPorNome(string nome);

        Task<Cliente> Criar(Cliente cliente);

        Task<Cliente> Atualizar(Cliente cliente);

        Task<bool> Deletar(int id);

        Task<int> Contar();
    }
}