using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestion.Web.Data;
using Gestion.Web.Models;

namespace Gestion.Web.Controllers
{
    [Route("api/[controller]")] // La ruta será: api/productosapi
    [ApiController]
    public class ProductosApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductosApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. OBTENER TODOS LOS PRODUCTOS (GET: api/productosapi)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            // Incluimos las tareas para que la App móvil pueda verlas también
            return await _context.Productos
                                 .Include(p => p.Tareas)
                                 .ToListAsync();
        }

        // POST: api/productosapi (Crear Producto)
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProductos", new { id = producto.Id }, producto);
        }

        // PUT: api/productosapi/:id (Actualizar Producto)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id) return BadRequest();

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Productos.Any(e => e.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/productosapi/:id (Borrar Producto)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 2. CREAR TAREA DESDE LA APP (POST: api/productosapi/tarea)
        // La App móvil enviará una tarea y aquí la guardamos
        [HttpPost("tarea")]
        public async Task<ActionResult<Tarea>> PostTarea(Tarea tarea)
        {
            // Validamos que el producto exista
            var producto = await _context.Productos.FindAsync(tarea.ProductoId);
            if (producto == null)
            {
                return BadRequest("El producto asociado no existe.");
            }

            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();

            return Ok(tarea); // Respondemos con la tarea creada
        }

        // 3. ACTUALIZAR TAREA (Completar) (PUT: api/productosapi/tarea/:id)
        [HttpPut("tarea/{id}")]
        public async Task<IActionResult> PutTarea(int id, Tarea tarea)
        {
            if (id != tarea.Id) return BadRequest();

            _context.Entry(tarea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tareas.Any(e => e.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // 4. ELIMINAR TAREA (DELETE: api/productosapi/tarea/:id)
        [HttpDelete("tarea/{id}")]
        public async Task<IActionResult> DeleteTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null) return NotFound();

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}