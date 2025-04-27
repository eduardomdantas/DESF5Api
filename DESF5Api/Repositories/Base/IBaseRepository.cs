namespace DESF5Api.Repositories.Base
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> ListarTodos();

        Task<T?> BuscarPorId(long id);

        Task<IEnumerable<T>> BuscarPorNome(string nome);

        Task<T> Inserir(T entity);

        Task<T> Atualizar(T entity, long id);

        Task<bool> Deletar(long id);

        Task<int> Contar();
    }
}