using DESF5Api.Models;
using DESF5Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DESF5Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClienteController(IClienteService service) : ControllerBase
    {

        /// <summary>
        /// Lista todos os clientes
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> ListarTodos()
        {
            var clientes = await service.ListarTodos();
            return Ok(clientes);
        }

        /// <summary>
        /// Busca um cliente por ID
        /// </summary>
        /// <param name="id">ID do cliente</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> BuscarPorId(int id)
        {
            var cliente = await service.BuscarPorId(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        /// <summary>
        /// Busca clientes por nome
        /// </summary>
        /// <param name="nome">Nome ou parte do nome</param>
        [HttpGet("buscar-por-nome/{nome}")]
        public async Task<ActionResult<IEnumerable<Cliente>>> BuscarPorNome(string nome)
        {
            var clientes = await service.BuscarPorNome(nome);
            return Ok(clientes);
        }

        /// <summary>
        /// Cria um novo cliente
        /// </summary>
        /// <param name="cliente">Dados do cliente</param>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<Cliente>> Criar(Cliente cliente)
        {
            var novoCliente = await service.Criar(cliente);
            return CreatedAtAction(nameof(BuscarPorId), new { id = novoCliente.Id }, novoCliente);
        }

        /// <summary>
        /// Atualizar um cliente
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <param name="cliente">Dados atualizados do cliente</param>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Atualizar(long id, Cliente cliente)
        {
            if (id != cliente.Id)
                return BadRequest("ID na URL não corresponde ao ID do cliente");

            try
            {
                await service.Atualizar(cliente);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deleta um cliente
        /// </summary>
        /// <param name="id">ID do cliente</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var result = await service.Deletar(id);
            if (!result) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Retorna a quantidade total de clientes
        /// </summary>
        [HttpGet("contar")]
        public async Task<ActionResult<int>> Contar()
        {
            var total = await service.Contar();
            return Ok(total);
        }
    }
}