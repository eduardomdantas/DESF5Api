using System.ComponentModel.DataAnnotations;

namespace DESF5Api.Models
{
    public class Pedido
    {
        [Key]
        public long Id { get; set; }

        public long ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime DataPedido { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; } = DateTime.UtcNow;
        public decimal ValorTotal => Itens?.Sum(i => i.PrecoUnitario * i.Quantidade) ?? 0.00M;
        public IEnumerable<PedidoItem> Itens { get; set; }
    }
}