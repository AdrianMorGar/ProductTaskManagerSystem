using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public class Tarea
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción de la tarea es obligatoria.")]
        [StringLength(250)]
        [Display(Name = "Descripción de la Tarea")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "¿Completada?")]
        public bool EstaCompletada { get; set; } = false;

        // Clave Foránea (Foreign Key) que enlaza con Producto
        [Display(Name = "Producto Asociado")]
        public int ProductoId { get; set; }

        // Propiedad de navegación: Un Producto para cada Tarea
        public Producto? Producto { get; set; }
    }
}