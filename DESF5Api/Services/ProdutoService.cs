using DESF5Api.Models;
using DESF5Api.Repositories;
using FluentValidation;

namespace DESF5Api.Services
{
    public class ProdutoService(IProdutoRepository repository, IValidator<Produto> validator) : IProdutoService
    {
        public async Task<IEnumerable<Produto>> ListarTodos() => await repository.ListarTodos();

        public async Task<Produto?> BuscarPorId(int id) => await repository.BuscarPorId(id);

        public async Task<IEnumerable<Produto>> BuscarPorNome(string nome) => await repository.BuscarPorNome(nome);

        public async Task<Produto> Criar(Produto produto)
        {
            await validator.ValidateAndThrowAsync(produto);
            return await repository.Inserir(produto);
        }

        public async Task<Produto> Atualizar(int id, Produto produto)
        {
            var existing = await repository.BuscarPorId(id) ?? throw new KeyNotFoundException("Produto não encontrado");
            existing.Nome = produto.Nome;
            existing.Descricao = produto.Descricao;
            existing.Preco = produto.Preco;

            await validator.ValidateAndThrowAsync(existing);
            await repository.Inserir(existing);
            return existing;
        }

        public async Task<bool> Deletar(int id) => await repository.Deletar(id);

        public async Task<int> Contar() => await repository.Contar();
    }
}