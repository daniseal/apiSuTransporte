using apiSuTransporte.Clases;
using apiSuTransporte.DTOs;
using apiSuTransporte.Models;
using Microsoft.AspNetCore.Mvc;

namespace apiSuTransporte.Controllers
{
    // Define la ruta base del controlador.
    // Como la clase se llama LoginController, la ruta base será:
    // api/Login
    [Route("api/[controller]")]

    // Indica que esta clase funciona como controlador de una Web API.
    // Permite recibir y responder peticiones HTTP.
    [ApiController]
    public class LoginController : ControllerBase
    {
        // Contexto de Entity Framework.
        // Permite conectarse con la base de datos bdSuTransporte.
        private readonly BdSuTransporteContext _context;

        // Constructor del controlador.
        // Recibe el contexto de la base de datos mediante inyección de dependencias.
        public LoginController(BdSuTransporteContext context)
        {
            _context = context;
        }

        // Método POST para validar el inicio de sesión.
        // Ruta completa:
        // POST api/Login/Validar
        //
        // Este método recibe desde el frontend un objeto JSON con usuario y clave.
        // Ejemplo:
        // {
        //     "usuario": "admin",
        //     "clave": "12345"
        // }
        [HttpPost("Validar")]
        public async Task<IActionResult> Validar([FromBody] LoginDTO login)
        {
            try
            {
                // Se crea un objeto de la clase operativa ClLogin.
                // Esta clase contiene la lógica para validar usuario y clave.
                ClLogin clase = new ClLogin(_context);

                // Se llama al método Validar de la clase ClLogin.
                // Este método consulta la tabla UsuarioSistema.
                var resultado = await clase.Validar(login);

                // Devuelve el resultado en formato JSON.
                // Si el usuario existe, devuelve correcto = true.
                // Si no existe, devuelve correcto = false.
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                // Si ocurre un error durante la validación,
                // devuelve una respuesta 400 Bad Request con el mensaje del error.
                return BadRequest(new
                {
                    correcto = false,
                    mensaje = ex.Message
                });
            }
        }
    }
}