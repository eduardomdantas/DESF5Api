using System.ComponentModel.DataAnnotations;

namespace DESF5Api.Models
{
    public class Cliente
    {
        [Key]
        public long Id { get; set; }

        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
    }
}