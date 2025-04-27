using Dapper;
using DESF5Api.Models;
using DESF5Api.Repositories.Base;
using DESF5Api.Services.Observer;

namespace DESF5Api.Repositories
{
    public class PedidoRepository(IConfiguration configuration, ISubject subject) : BaseRepository<Pedido>(configuration, subject), IPedidoRepository
    {
        protected override string TableName => "Pedidos";

        protected override string GetInsertColumns() =>
            "ClienteId, DataPedido, ValorTotal";

        protected override string GetInsertValues()
        {
            var columns = GetInsertColumns().Split(", ");
            var valuesWithAt = columns.Select(column => $"@{column}");
            return string.Join(", ", valuesWithAt);
        }

        protected override string GetUpdateColumns() =>
            "DataAtualizacao";

        protected override string GetUpdateColumnsWithValues()
        {
            var columns = GetInsertColumns().Split(", ");
            var valuesWithAt = columns.Select(column => $"{column}=@{column}");
            return string.Join(", ", valuesWithAt);
        }

        public override async Task<Pedido?> BuscarPorId(long id)
        {
            var pedido = await base.BuscarPorId(id);
            if (pedido != null)
                pedido.Itens = await CarregarItens(pedido.Id);

            return pedido;
        }

        public async Task<IEnumerable<Pedido>> ListarPorCliente(long clienteId)
        {
            _subject.Notify($"Listando pedidos do cliente ID: {clienteId}");

            using var connection = CreateConnection();

            var pedidos = await connection.QueryAsync<Pedido>(
                "SELECT * FROM Pedidos WHERE ClienteId = @ClienteId",
                new { ClienteId = clienteId });

            // Carrega os itens de cada pedido
            foreach (var pedido in pedidos)
            {
                pedido.Itens = await CarregarItens(pedido.Id);
            }

            return pedidos;
        }

        private async Task<IEnumerable<PedidoItem>> CarregarItens(long pedidoId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<PedidoItem>(
                "SELECT * FROM PedidoItens WHERE PedidoId = @PedidoId",
                new { PedidoId = pedidoId });
        }

        public override async Task<Pedido> Inserir(Pedido entity)
        {
            using var connection = CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Insere o pedido
                var pedidoId = await connection.ExecuteScalarAsync<long>(
                    @"INSERT INTO Pedidos (ClienteId, DataPedido)
                      VALUES (@ClienteId, @DataPedido)
                      RETURNING Id;",
                    entity, transaction);

                // Insere os itens
                foreach (var item in entity.Itens)
                {
                    item.PedidoId = pedidoId;
                    await AdicionarItem(item);
                }

                transaction.Commit();
                entity.Id = pedidoId;
                return entity;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<PedidoItem> AdicionarItem(PedidoItem entity)
        {
            using var connection = CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var pedidoItemId = await connection.ExecuteAsync(
                        @"INSERT INTO PedidoItens (PedidoId, ProdutoId, Quantidade, PrecoUnitario)
                  VALUES (@PedidoId, @ProdutoId, @Quantidade, @PrecoUnitario)
                      RETURNING Id;",
                        entity, transaction);

                transaction.Commit();
                entity.Id = pedidoItemId;
                return entity;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> RemoverItem(long pedidoItemId)
        {
            using var connection = CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var rowsAffected = await connection.ExecuteAsync(
                        @"DELETE FROM PedidoItens WHERE Id = @Id;",
                        new { Id = pedidoItemId }, transaction);

                transaction.Commit();
                return rowsAffected > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}