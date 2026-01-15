using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestion.Web.Data;
using Gestion.Web.Models;

namespace Gestion.Web.Controllers
{
    public class TareasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TareasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listar tareas de UN producto específico
        public async Task<IActionResult> Index(int productoId)
        {
            var producto = await _context.Productos
                .Include(p => p.Tareas)
                .FirstOrDefaultAsync(p => p.Id == productoId);

            if (producto == null) return NotFound();

            ViewBag.ProductoNombre = producto.Nombre;
            ViewBag.ProductoId = producto.Id;

            return View(producto.Tareas);
        }

        // Formulario para añadir tarea
        public IActionResult Create(int productoId)
        {
            ViewBag.ProductoId = productoId;
            return View();
        }

        // Guardar la tarea
        [HttpPost]
        public async Task<IActionResult> Create(Tarea tarea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tarea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { productoId = tarea.ProductoId });
            }
            return View(tarea);
        }

        // GET: Tareas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null) return NotFound();

            return View(tarea);
        }

        // POST: Tareas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tarea tarea)
        {
            if (id != tarea.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(tarea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { productoId = tarea.ProductoId });
            }
            return View(tarea);
        }

        // POST: Tareas/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null) return NotFound();

            int pId = tarea.ProductoId;
            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { productoId = pId });
        }
    }
}