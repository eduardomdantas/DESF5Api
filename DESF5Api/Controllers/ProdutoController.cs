using DESF5Api.Models;
using DESF5Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DESF5Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController(IProdutoService service) : ControllerBase
    {

        /// <summary>
        /// Lista todos os produtos
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Produto>>> ListarTodos()
        {
            var produtos = await service.ListarTodos();
            return Ok(produtos);
        }

        /// <summary>
        /// Busca um produto por ID
        /// </summary>
        /// <param name="id">ID do produto</param>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Produto>> BuscarPorId(int id)
        {
            var produto = await service.BuscarPorId(id);
            if (produto == null) return NotFound();
            return Ok(produto);
        }

        /// <summary>
        /// Busca produtos por nome
        /// </summary>
        /// <param name="nome">Nome ou parte do nome</param>
        [HttpGet("buscar-por-nome/{nome}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Produto>>> BuscarPorNome(string nome)
        {
            var produtos = await service.BuscarPorNome(nome);
            return Ok(produtos);
        }

        /// <summary>
        /// Cria um novo produto
        /// </summary>
        /// <param name="produto">Dados do produto</param>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(201)]
        public async Task<ActionResult<Produto>> Criar(Produto produto)
        {
            var novoProduto = await service.Criar(produto);
            return CreatedAtAction(nameof(BuscarPorId), new { id = novoProduto.Id }, novoProduto);
        }

        /// <summary>
        /// Atualiza um produto existente
        /// </summary>
        /// <param name="id">ID do produto</param>
        /// <param name="produto">Dados atualizados do produto</param>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Atualizar(int id, Produto produto)
        {
            if (id != produto.Id) return BadRequest();

            try
            {
                await service.Atualizar(id, produto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Deleta um produto
        /// </summary>
        /// <param name="id">ID do produto</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deletar(int id)
        {
            var result = await service.Deletar(id);
            if (!result) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Retorna a quantidade total de produtos
        /// </summary>
        [HttpGet("contar")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> Contar()
        {
            var total = await service.Contar();
            return Ok(total);
        }
    }
}