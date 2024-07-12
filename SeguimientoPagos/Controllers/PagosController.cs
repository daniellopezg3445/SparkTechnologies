using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeguimientoPagos.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeguimientoPagos.Controllers
{
    public class PagosController : Controller
    {
        private readonly PagosUsuarioContext _context;

        // Constructor del controlador
        public PagosController(PagosUsuarioContext context)
        {
            _context = context;
        }

        // Muestra los detalles de un pago específico
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.FirstOrDefaultAsync(m => m.Id == id);

            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // Muestra la lista de pagos
        public async Task<IActionResult> Index(int? buscar)
        {
            var filtro = from pago in _context.Pagos select pago;

            if (buscar.HasValue)
            {
                filtro = filtro.Where(s => s.Folio == buscar.Value);
            }

            return View(await filtro.ToListAsync());
        }


        // Muestra el formulario de creación de un nuevo pago
        public IActionResult Create()
        {
            return View();
        }

        // Maneja el envío del formulario de creación de un nuevo pago
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Folio,Descripcion,Fecha,Cantidad")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pago);
        }

        // Muestra el formulario de edición de un pago existente
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            return View(pago);
        }

        // Maneja el envío del formulario de edición de un pago existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Folio,Descripcion,Fecha,Cantidad")] Pago pago)
        {
            if (id != pago.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pago);
        }

        // Muestra el formulario de confirmación de eliminación de un pago
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // Maneja la eliminación de un pago
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Método auxiliar para verificar si existe un pago
        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.Id == id);
        }

        // Muestra la confirmación para eliminar múltiples pagos
        public async Task<IActionResult> DeleteMultipleConfirmation(List<int> selectedIds)
        {
            if (selectedIds == null || selectedIds.Count == 0)
            {
                TempData["Message"] = "Por favor, selecciona al menos un pago para eliminar.";
                return RedirectToAction(nameof(Index));
            }

            var pagosToDelete = await _context.Pagos
                                            .Where(p => selectedIds.Contains(p.Id))
                                            .ToListAsync();

            if (pagosToDelete == null || pagosToDelete.Count == 0)
            {
                TempData["Message"] = "No se encontraron pagos seleccionados para eliminar.";
                return RedirectToAction(nameof(Index));
            }

            return View(pagosToDelete);
        }

        // Maneja la eliminación de múltiples pagos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMultiple(List<int> selectedIds)
        {
            if (selectedIds == null || selectedIds.Count == 0)
            {
                TempData["Message"] = "No se encontraron pagos seleccionados para eliminar.";
                return RedirectToAction(nameof(Index));
            }

            var pagosToDelete = await _context.Pagos
                                            .Where(p => selectedIds.Contains(p.Id))
                                            .ToListAsync();

            if (pagosToDelete == null || pagosToDelete.Count == 0)
            {
                TempData["Message"] = "No se encontraron pagos seleccionados para eliminar.";
                return RedirectToAction(nameof(Index));
            }

            _context.Pagos.RemoveRange(pagosToDelete);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




        // Acción para descargar los pagos
        public async Task<IActionResult> Download()
        {
            var pagos = await _context.Pagos.ToListAsync();

            var builder = new StringBuilder();
            builder.AppendLine("Folio,Descripcion,Fecha,Cantidad");

            foreach (var pago in pagos)
            {
                builder.AppendLine($"{pago.Folio},{pago.Descripcion},{pago.Fecha},{pago.Cantidad}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "pagos.csv");
        }
    }


}
