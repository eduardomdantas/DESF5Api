using System.ComponentModel.DataAnnotations;

namespace DESF5Api.Models
{
    public class PedidoItem
    {
        [Key]
        public long Id { get; set; }

        public long PedidoId { get; set; }
        public Pedido Pedido { get; set; }
        public long ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}