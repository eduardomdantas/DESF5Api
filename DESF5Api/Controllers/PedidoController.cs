using DESF5Api.Models;
using DESF5Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DESF5Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PedidoController(IPedidoService service) : ControllerBase
    {
        private int? GetClienteId()
        {
            // Em uma aplicação real, você teria um sistema de roles mais complexo
            var isAdmin = User.IsInRole("Admin");
            if (isAdmin) return null;

            var clienteIdClaim = User.FindFirst("ClienteId")?.Value;
            if (int.TryParse(clienteIdClaim, out var clienteId))
                return clienteId;

            return null;
        }

        /// <summary>
        /// Lista todos os pedidos (apenas admin)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Pedido>>> ListarTodos()
        {
            var pedidos = await service.ListarTodos();
            return Ok(pedidos);
        }

        /// <summary>
        /// Busca um pedido por ID
        /// </summary>
        /// <param name="id">ID do pedido</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> BuscarPorId(int id)
        {
            var clienteId = GetClienteId();
            var pedido = await service.BuscarPorId(id, clienteId);
            if (pedido == null) return NotFound();
            return Ok(pedido);
        }

        /// <summary>
        /// Lista pedidos de um cliente específico
        /// </summary>
        /// <param name="clienteId">ID do cliente</param>
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<Pedido>>> ListarPorCliente(int clienteId)
        {
            var currentClienteId = GetClienteId();
            if (currentClienteId.HasValue && currentClienteId != clienteId)
                return Forbid();

            var pedidos = await service.ListarPorCliente(clienteId);
            return Ok(pedidos);
        }

        /// <summary>
        /// Cria um novo pedido
        /// </summary>
        /// <param name="pedido">Dados do pedido</param>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<Pedido>> Criar(Pedido pedido)
        {
            var clienteId = GetClienteId();
            if (clienteId.HasValue)
                pedido.ClienteId = clienteId.Value;

            var novoPedido = await service.Criar(pedido);
            return CreatedAtAction(nameof(BuscarPorId), new { id = novoPedido.Id }, novoPedido);
        }

        /// <summary>
        /// Deleta um pedido
        /// </summary>
        /// <param name="id">ID do pedido</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var clienteId = GetClienteId();
            var result = await service.Deletar(id, clienteId);
            if (!result) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Retorna a quantidade total de pedidos (apenas admin)
        /// </summary>
        [HttpGet("contar")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> Contar()
        {
            return await service.Contar();
        }

        /// <summary>
        /// Adiciona o item do pedido (apenas admin)
        /// </summary>
        [HttpPost("{pedidoId}/itens")]
        public async Task<ActionResult<PedidoItem>> AdicionarItem(long pedidoId, PedidoItem item)
        {
            var novoItem = await service.AdicionarItem(pedidoId, item);
            return Ok(novoItem);
        }

        /// <summary>
        /// Remove o item do pedido (apenas admin)
        /// </summary>
        [HttpDelete("itens/{itemId}")]
        public async Task<IActionResult> RemoverItem(long itemId)
        {
            var result = await service.RemoverItem(itemId);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}