using apiSuTransporte.Clases;
using apiSuTransporte.DTOs;
using apiSuTransporte.Models;
using Microsoft.AspNetCore.Mvc;

namespace apiSuTransporte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuiaDespachosController : ControllerBase
    {
        private readonly BdSuTransporteContext _context;

        public GuiaDespachosController(BdSuTransporteContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ClGuiaDespacho clase = new ClGuiaDespacho(_context);
            var datos = await clase.Listar();

            return Ok(datos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ClGuiaDespacho clase = new ClGuiaDespacho(_context);
            var dato = await clase.Buscar(id);

            if (dato == null)
                return NotFound("No se encontró la guía de despacho");

            return Ok(dato);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GuiaDespachoDTO dto)
        {
            try
            {
                ClGuiaDespacho clase = new ClGuiaDespacho(_context);
                string mensaje = await clase.Insertar(dto);

                if (mensaje.Contains("Error") ||
                    mensaje.Contains("no existe") ||
                    mensaje.Contains("inactivo") ||
                    mensaje.Contains("ya está asociada") ||
                    mensaje.Contains("Debe agregar"))
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
                ClGuiaDespacho clase = new ClGuiaDespacho(_context);
                string mensaje = await clase.Eliminar(id);

                return Ok(new
                {
                    mensaje
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }
    }
}