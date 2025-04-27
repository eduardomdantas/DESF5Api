using DESF5Api.Models;

namespace DESF5Api.Services
{
    public interface IPedidoService
    {
        Task<IEnumerable<Pedido>> ListarTodos();

        Task<Pedido?> BuscarPorId(int id, int? clienteId = null);

        Task<IEnumerable<Pedido>> ListarPorCliente(int clienteId);

        Task<Pedido> Criar(Pedido pedido);

        Task<bool> Deletar(int id, int? clienteId = null);

        Task<int> Contar();

        Task<PedidoItem> AdicionarItem(long pedidoId, PedidoItem item);

        Task<bool> RemoverItem(long itemId);
    }
}