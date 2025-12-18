using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

namespace Gestion.Web.Models
{
    public class Producto
    {
        // Clave primaria
        public int Id { get; set; }

        // Nombre (obligatorio)
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [Display(Name = "Nombre del Producto")]
        public string Nombre { get; set; } = string.Empty;

        // Precio (mayor que 0)
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Precio { get; set; }

        // Stock (entero no negativo)
        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        // Categoría (opcional)
        [StringLength(50)]
        public string? Categoria { get; set; }

        // Lista de tareas asociadas a este producto (Modelo: 1:N )
        public ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
    }
}
