using DESF5Api.Models;

namespace DESF5Api.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> BuscarPorNomeUsuario(string nomeUsuario);
    }
}