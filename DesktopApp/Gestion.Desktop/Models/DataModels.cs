namespace Gestion.Desktop.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? Categoria { get; set; }
        public List<Tarea> Tareas { get; set; } = new List<Tarea>();
    }

    public class Tarea
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public bool EstaCompletada { get; set; }
        public int ProductoId { get; set; }
    }
}