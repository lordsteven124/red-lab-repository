using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.API.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Required]
        public bool Estado { get; set; }

        [Required]
        public string UsuarioCreacion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaCreacion { get; set; }

        public string? UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}