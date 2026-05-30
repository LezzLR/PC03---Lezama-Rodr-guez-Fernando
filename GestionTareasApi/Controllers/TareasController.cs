using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionTareasApi.Data;
using GestionTareasApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace GestionTareasApi.Controllers
{
    [ApiController]
    [Route("api/tareas")]
    public class TareasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TareasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/tareas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas()
        {
            var tareas = await _context.Tareas.ToListAsync();
            return Ok(tareas);
        }

        // GET: api/tareas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarea>> GetTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);

            if (tarea == null)
            {
                return NotFound(new { mensaje = $"No se encontró la tarea con ID {id}." });
            }

            return Ok(tarea);
        }

        // POST: api/tareas
        [HttpPost]
        public async Task<ActionResult<Tarea>> PostTarea(Tarea tarea)
        {
            // Forzar que la fecha de creación sea la actual
            tarea.FechaCreacion = DateTime.UtcNow;

            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTarea), new { id = tarea.Id }, tarea);
        }

        // PUT: api/tareas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarea(int id, Tarea tarea)
        {
            if (id != tarea.Id && tarea.Id != 0)
            {
                return BadRequest(new { mensaje = "El ID de la tarea en el cuerpo no coincide con el ID de la URL." });
            }

            var existingTarea = await _context.Tareas.FindAsync(id);
            if (existingTarea == null)
            {
                return NotFound(new { mensaje = $"No se encontró la tarea con ID {id}." });
            }

            // Actualizar campos
            existingTarea.Titulo = tarea.Titulo;
            existingTarea.Descripcion = tarea.Descripcion;
            existingTarea.Estado = tarea.Estado;
            existingTarea.Prioridad = tarea.Prioridad;
            existingTarea.FechaVencimiento = tarea.FechaVencimiento;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TareaExists(id))
                {
                    return NotFound(new { mensaje = $"No se encontró la tarea con ID {id}." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/tareas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
            {
                return NotFound(new { mensaje = $"No se encontró la tarea con ID {id}." });
            }

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(e => e.Id == id);
        }
    }
}
