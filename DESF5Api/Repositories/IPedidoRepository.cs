using DESF5Api.Models;
using DESF5Api.Repositories.Base;

namespace DESF5Api.Repositories
{
    public interface IPedidoRepository : IBaseRepository<Pedido>
    {
        Task<IEnumerable<Pedido>> ListarPorCliente(long clienteId);
        Task<PedidoItem> AdicionarItem(PedidoItem entity);
        Task<bool> RemoverItem(long pedidoItemId);
    }
}