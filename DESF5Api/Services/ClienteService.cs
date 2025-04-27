using DESF5Api.Models;
using DESF5Api.Repositories;
using FluentValidation;

namespace DESF5Api.Services
{
    public class ClienteService(IClienteRepository repository, IValidator<Cliente> validator) : IClienteService
    {
        public async Task<IEnumerable<Cliente>> ListarTodos() => await repository.ListarTodos();

        public async Task<Cliente?> BuscarPorId(int id) => await repository.BuscarPorId(id);

        public async Task<IEnumerable<Cliente>> BuscarPorNome(string nome) => await repository.BuscarPorNome(nome);

        public async Task<Cliente> Criar(Cliente cliente)
        {
            await validator.ValidateAndThrowAsync(cliente);
            return await repository.Inserir(cliente);
        }

        public async Task<Cliente> Atualizar(Cliente cliente)
        {
            await validator.ValidateAndThrowAsync(cliente);
            return await repository.Atualizar(cliente, cliente.Id);
        }

        public async Task<bool> Deletar(int id) => await repository.Deletar(id);

        public async Task<int> Contar() => await repository.Contar();
    }
}