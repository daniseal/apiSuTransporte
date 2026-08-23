using apiSuTransporte.Clases;
using apiSuTransporte.DTOs;
using apiSuTransporte.Models;
using Microsoft.AspNetCore.Mvc;

namespace apiSuTransporte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntregasController : ControllerBase
    {
        private readonly BdSuTransporteContext _context;

        public EntregasController(BdSuTransporteContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ClEntrega clase = new ClEntrega(_context);
            var datos = await clase.Listar();

            return Ok(datos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ClEntrega clase = new ClEntrega(_context);
            var dato = await clase.Buscar(id);

            if (dato == null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró la entrega"
                });
            }

            return Ok(dato);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EntregaDTO dto)
        {
            try
            {
                ClEntrega clase = new ClEntrega(_context);
                string mensaje = await clase.Insertar(dto);

                if (mensaje.Contains("no existe") || mensaje.Contains("inactivo"))
                {
                    return BadRequest(new
                    {
                        mensaje
                    });
                }

                return Ok(new
                {
                    mensaje
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.InnerException != null ? ex.InnerException.Message : ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EntregaDTO dto)
        {
            try
            {
                ClEntrega clase = new ClEntrega(_context);
                string mensaje = await clase.Modificar(id, dto);

                if (mensaje.Contains("No se encontró"))
                {
                    return NotFound(new
                    {
                        mensaje
                    });
                }

                if (mensaje.Contains("no existe") || mensaje.Contains("inactivo"))
                {
                    return BadRequest(new
                    {
                        mensaje
                    });
                }

                return Ok(new
                {
                    mensaje
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.InnerException != null ? ex.InnerException.Message : ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ClEntrega clase = new ClEntrega(_context);
                string mensaje = await clase.Eliminar(id);

                if (mensaje.Contains("No se encontró"))
                {
                    return NotFound(new
                    {
                        mensaje
                    });
                }

                return Ok(new
                {
                    mensaje
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.InnerException != null ? ex.InnerException.Message : ex.Message
                });
            }
        }
    }
}