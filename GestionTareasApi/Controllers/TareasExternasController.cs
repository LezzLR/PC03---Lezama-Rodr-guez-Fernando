using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using GestionTareasApi.Services;
using GestionTareasApi.Models;

namespace GestionTareasApi.Controllers
{
    [ApiController]
    [Route("api/tareas-externas")]
    public class TareasExternasController : ControllerBase
    {
        private readonly TareasExternasService _tareasExternasService;

        public TareasExternasController(TareasExternasService tareasExternasService)
        {
            _tareasExternasService = tareasExternasService;
        }

        // GET: api/tareas-externas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TareaExternaDto>>> GetTareasExternas()
        {
            try
            {
                var todos = await _tareasExternasService.GetTodosAsync();
                
                var dtos = new List<TareaExternaDto>();
                foreach (var todo in todos)
                {
                    dtos.Add(new TareaExternaDto
                    {
                        ExternalId = todo.Id,
                        Titulo = todo.Title,
                        Completado = todo.Completed
                    });
                }

                return Ok(dtos);
            }
            catch (HttpRequestException ex)
            {
                // Error de comunicación con la API externa (502 Bad Gateway)
                return StatusCode(StatusCodes.Status502BadGateway, new 
                { 
                    mensaje = "Error al intentar comunicarse con el proveedor externo de tareas.",
                    detalle = ex.Message 
                });
            }
            catch (Exception ex)
            {
                // Error general / Servicio no disponible (503 Service Unavailable)
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new 
                { 
                    mensaje = "El servicio de tareas externas no está disponible temporalmente.",
                    detalle = ex.Message 
                });
            }
        }

        // GET: api/tareas-externas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TareaExternaDto>> GetTareaExterna(int id)
        {
            try
            {
                var todo = await _tareasExternasService.GetTodoByIdAsync(id);

                if (todo == null)
                {
                    return NotFound(new { mensaje = $"No se encontró la tarea externa con el ID {id}." });
                }

                var dto = new TareaExternaDto
                {
                    ExternalId = todo.Id,
                    Titulo = todo.Title,
                    Completado = todo.Completed
                };

                return Ok(dto);
            }
            catch (HttpRequestException ex)
            {
                // Error de comunicación con la API externa (502 Bad Gateway)
                return StatusCode(StatusCodes.Status502BadGateway, new 
                { 
                    mensaje = "Error al intentar comunicarse con el proveedor externo de tareas.",
                    detalle = ex.Message 
                });
            }
            catch (Exception ex)
            {
                // Error general / Servicio no disponible (503 Service Unavailable)
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new 
                { 
                    mensaje = "El servicio de tareas externas no está disponible temporalmente.",
                    detalle = ex.Message 
                });
            }
        }
    }
}
