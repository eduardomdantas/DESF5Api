using DESF5Api.Models;
using DESF5Api.Repositories;
using FluentValidation;

namespace DESF5Api.Services
{
    public class PedidoService(
        IPedidoRepository repository,
        IClienteRepository clienteRepository,
        IValidator<Pedido> validator,
        IValidator<PedidoItem> itemValidator) : IPedidoService
    {
        public async Task<IEnumerable<Pedido>> ListarTodos() => await repository.ListarTodos();

        public async Task<Pedido?> BuscarPorId(int id, int? clienteId = null)
        {
            var pedido = await repository.BuscarPorId(id);

            // Proteção para que um cliente não acesse pedido de outro
            if (clienteId.HasValue && pedido?.ClienteId != clienteId)
                throw new UnauthorizedAccessException("Acesso não autorizado a este pedido");

            return pedido;
        }

        public async Task<IEnumerable<Pedido>> ListarPorCliente(int clienteId) =>
            await repository.ListarPorCliente(clienteId);

        public async Task<Pedido> Criar(Pedido pedido)
        {
            // Verifica se o cliente existe
            var cliente = await clienteRepository.BuscarPorId(pedido.ClienteId);
            if (cliente == null)
                throw new KeyNotFoundException("Cliente não encontrado");

            await validator.ValidateAndThrowAsync(pedido);
            return await repository.Inserir(pedido);
        }

        public async Task<bool> Deletar(int id, int? clienteId = null)
        {
            if (clienteId.HasValue)
            {
                var pedido = await repository.BuscarPorId(id);
                if (pedido?.ClienteId != clienteId)
                    throw new UnauthorizedAccessException("Acesso não autorizado a este pedido");
            }

            return await repository.Deletar(id);
        }

        public async Task<int> Contar() => await repository.Contar();

        public async Task<PedidoItem> AdicionarItem(long pedidoId, PedidoItem item)
        {
            item.PedidoId = pedidoId;
            await itemValidator.ValidateAndThrowAsync(item);
            return await repository.AdicionarItem(item);
        }

        public async Task<bool> RemoverItem(long itemId)
        {
            return await repository.RemoverItem(itemId);
        }
    }
}