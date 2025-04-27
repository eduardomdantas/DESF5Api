using System.ComponentModel.DataAnnotations;

namespace DESF5Api.Models
{
    public class Usuario
    {
        [Key]
        public long Id { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "Admin";
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
    }
}