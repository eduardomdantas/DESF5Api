using System.ComponentModel.DataAnnotations;

namespace DESF5Api.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
    }
}