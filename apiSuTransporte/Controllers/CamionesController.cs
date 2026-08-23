using apiSuTransporte.Clases;
using apiSuTransporte.DTOs;
using apiSuTransporte.Models;
using Microsoft.AspNetCore.Mvc;

namespace apiSuTransporte.Controllers
{
    // Define la ruta base del controlador.
    // Como el controlador se llama CamionesController,
    // la ruta principal será: api/Camiones
    [Route("api/[controller]")]

    // Indica que esta clase es un controlador de Web API.
    // Permite recibir peticiones HTTP como GET, POST, PUT y DELETE.
    [ApiController]
    public class CamionesController : ControllerBase
    {
        // Contexto de Entity Framework.
        // Permite conectarse a la base de datos bdSuTransporte.
        private readonly BdSuTransporteContext _context;

        // Constructor del controlador.
        // Recibe el contexto de la base de datos mediante inyección de dependencias.
        public CamionesController(BdSuTransporteContext context)
        {
            _context = context;
        }

        // Método GET general.
        // Ruta: GET api/Camiones
        // Sirve para listar todos los camiones activos.
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Se crea un objeto de la clase operativa ClCamion.
            // Esta clase contiene la lógica de negocio del módulo Camión.
            ClCamion clase = new ClCamion(_context);

            // Se llama al método Listar para obtener todos los camiones activos.
            var datos = await clase.Listar();

            // Devuelve la lista de camiones en formato JSON con respuesta 200 OK.
            return Ok(datos);
        }

        // Método GET por ID.
        // Ruta: GET api/Camiones/{id}
        // Sirve para consultar un camión específico por su IdCamion.
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            // Se crea el objeto de la clase operativa.
            ClCamion clase = new ClCamion(_context);

            // Se busca el camión por el ID recibido en la URL.
            var dato = await clase.Buscar(id);

            // Si no se encuentra el camión, devuelve respuesta 404 Not Found.
            if (dato == null)
                return NotFound("No se encontró el camión");

            // Si lo encuentra, devuelve el camión en formato JSON.
            return Ok(dato);
        }

        // Método POST.
        // Ruta: POST api/Camiones
        // Sirve para registrar un nuevo camión.
        // Recibe los datos desde el frontend en formato JSON usando CamionDTO.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CamionDTO dto)
        {
            try
            {
                // Se crea el objeto de la clase operativa.
                ClCamion clase = new ClCamion(_context);

                // Se llama al método Insertar para guardar el nuevo camión.
                string mensaje = await clase.Insertar(dto);

                // Devuelve el mensaje de resultado en formato JSON.
                return Ok(new
                {
                    mensaje
                });
            }
            catch (Exception ex)
            {
                // Si ocurre un error, devuelve respuesta 400 Bad Request.
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }

        // Método PUT.
        // Ruta: PUT api/Camiones/{id}
        // Sirve para modificar un camión existente.
        // Recibe el ID por la URL y los nuevos datos en el cuerpo de la petición.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CamionDTO dto)
        {
            try
            {
                // Se crea el objeto de la clase operativa.
                ClCamion clase = new ClCamion(_context);

                // Se llama al método Modificar para actualizar el camión.
                string mensaje = await clase.Modificar(id, dto);

                // Si la clase devuelve que no encontró el camión,
                // el controlador responde con 404 Not Found.
                if (mensaje.Contains("No se encontró"))
                {
                    return NotFound(new
                    {
                        mensaje
                    });
                }

                // Si existe otro camión con la misma matrícula, motor o chasis,
                // se responde con 400 Bad Request.
                if (mensaje.Contains("Ya existe"))
                {
                    return BadRequest(new
                    {
                        mensaje
                    });
                }

                // Si todo sale bien, devuelve respuesta 200 OK.
                return Ok(new
                {
                    mensaje
                });
            }
            catch (Exception ex)
            {
                // Si ocurre un error de SQL Server o Entity Framework,
                // se muestra el error interno si existe.
                return BadRequest(new
                {
                    mensaje = ex.InnerException != null ? ex.InnerException.Message : ex.Message
                });
            }
        }

        // Método DELETE.
        // Ruta: DELETE api/Camiones/{id}
        // Sirve para eliminar un camión.
        // En este proyecto se maneja como eliminación lógica:
        // no borra físicamente, solo cambia Activo a false.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Se crea el objeto de la clase operativa.
                ClCamion clase = new ClCamion(_context);

                // Se llama al método Eliminar para desactivar el camión.
                string mensaje = await clase.Eliminar(id);

                // Devuelve el mensaje de resultado.
                return Ok(new
                {
                    mensaje
                });
            }
            catch (Exception ex)
            {
                // Si ocurre un error, devuelve respuesta 400 Bad Request.
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }
    }
}