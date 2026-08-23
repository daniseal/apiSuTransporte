using apiSuTransporte.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiSuTransporte.Controllers
{
    // Controlador usado para llenar listas desplegables o combos del frontend.
    // Todos los métodos devuelven datos en formato JSON con dos campos:
    // id    = valor que se guarda en la base de datos
    // texto = texto visible para el usuario en el combo
    [Route("api/[controller]")]
    [ApiController]
    public class CombosController : ControllerBase
    {
        // Contexto de Entity Framework para consultar la base de datos.
        private readonly BdSuTransporteContext _context;

        // Constructor del controlador.
        // Recibe el contexto de base de datos mediante inyección de dependencias.
        public CombosController(BdSuTransporteContext context)
        {
            _context = context;
        }

        // Método para llenar el combo de marcas de camiones.
        // Ruta: GET api/Combos/Marcas
        [HttpGet("Marcas")]
        public async Task<IActionResult> Marcas()
        {
            var datos = await _context.Set<Marca>()
                .Where(x => x.Activo == true)
                .Select(x => new
                {
                    id = x.IdMarca,
                    texto = x.Nombre
                })
                .ToListAsync();

            return Ok(datos);
        }

        // Método para llenar el combo de sedes.
        // Ruta: GET api/Combos/Sedes
        [HttpGet("Sedes")]
        public async Task<IActionResult> Sedes()
        {
            var datos = await _context.Set<Sede>()
                .Where(x => x.Activo == true)
                .Select(x => new
                {
                    id = x.IdSede,
                    texto = x.Nombre
                })
                .ToListAsync();

            return Ok(datos);
        }

        // Método para llenar el combo de camioneros.
        // Se usa en el formulario de Guía de Despacho.
        // Ruta: GET api/Combos/Camioneros
        [HttpGet("Camioneros")]
        public async Task<IActionResult> Camioneros()
        {
            var datos = await _context.Set<Camionero>()
                .Where(x => x.Activo == true)
                .Select(x => new
                {
                    id = x.IdCamionero,
                    texto = x.Nombres + " " + x.Apellidos
                })
                .ToListAsync();

            return Ok(datos);
        }

        // Método para llenar el combo de camiones.
        // Muestra matrícula y tipo para que el usuario identifique el vehículo.
        // Ruta: GET api/Combos/Camiones
        [HttpGet("Camiones")]
        public async Task<IActionResult> Camiones()
        {
            var datos = await (
                from c in _context.Set<Camion>()
                join tc in _context.Set<TipoCamion>() on c.IdTipoCamion equals tc.IdTipoCamion
                where c.Activo == true
                select new
                {
                    id = c.IdCamion,
                    texto = c.Placa + " - " + tc.Nombre
                }
            ).ToListAsync();

            return Ok(datos);
        }

        // Método para llenar combos de empleados.
        // Se usa para empleado que despacha y empleado que recibe.
        // Ruta: GET api/Combos/Empleados
        [HttpGet("Empleados")]
        public async Task<IActionResult> Empleados()
        {
            var datos = await _context.Set<Empleado>()
                .Where(x => x.Activo == true)
                .Select(x => new
                {
                    id = x.IdEmpleado,
                    texto = x.Nombres + " " + x.Apellidos
                })
                .ToListAsync();

            return Ok(datos);
        }

        // Método para llenar el combo de estados de guía.
        // Ejemplo: Abierta, En ruta, Cerrada, Cancelada.
        // Ruta: GET api/Combos/EstadosGuia
        [HttpGet("EstadosGuia")]
        public async Task<IActionResult> EstadosGuia()
        {
            var datos = await _context.Set<EstadoGuium>()
                .Where(x => x.Activo == true)
                .Select(x => new
                {
                    id = x.IdEstadoGuia,
                    texto = x.Nombre
                })
                .ToListAsync();

            return Ok(datos);
        }

        // Método para llenar el combo de estados de entrega.
        // Ejemplo: Pendiente, En ruta, Entregado, Devuelto.
        // Ruta: GET api/Combos/EstadosEntrega
        [HttpGet("EstadosEntrega")]
        public async Task<IActionResult> EstadosEntrega()
        {
            var datos = await _context.Set<EstadoEntrega>()
                .Where(x => x.Activo == true)
                .Select(x => new
                {
                    id = x.IdEstadoEntrega,
                    texto = x.Nombre
                })
                .ToListAsync();

            return Ok(datos);
        }

        // Método para llenar el combo de todas las entregas activas.
        // Muestra todas las entregas, estén o no asociadas a una guía.
        // Ruta: GET api/Combos/Entregas
        [HttpGet("Entregas")]
        public async Task<IActionResult> Entregas()
        {
            var datos = await _context.Set<Entrega>()
                .Where(x => x.Activo == true)
                .Select(x => new
                {
                    id = x.IdEntrega,
                    texto = "Entrega #" + x.IdEntrega + " - " + x.NombreDestinatario
                })
                .ToListAsync();

            return Ok(datos);
        }

        // Método para llenar el combo de ciudades.
        // Se usa en formularios como Cliente, Empleado, Camionero y Entrega.
        // Ruta: GET api/Combos/Ciudades
        [HttpGet("Ciudades")]
        public async Task<IActionResult> Ciudades()
        {
            var datos = await _context.Set<Ciudad>()
                .Where(x => x.Activo == true)
                .Select(x => new
                {
                    id = x.IdCiudad,
                    texto = x.Nombre
                })
                .ToListAsync();

            return Ok(datos);
        }

        // Método para llenar el combo de tipos de documento.
        // Ejemplo: CC, NIT, CE.
        // Ruta: GET api/Combos/TipoDocumentos
        [HttpGet("TipoDocumentos")]
        public async Task<IActionResult> TipoDocumentos()
        {
            var datos = await _context.Set<TipoDocumento>()
                .Where(x => x.Activo == true)
                .Select(x => new
                {
                    id = x.IdTipoDocumento,
                    texto = x.Nombre
                })
                .ToListAsync();

            return Ok(datos);
        }

        // Método para llenar el combo de tipos de teléfono.
        // Ejemplo: Celular, Fijo, WhatsApp.
        // Ruta: GET api/Combos/TipoTelefonos
        [HttpGet("TipoTelefonos")]
        public async Task<IActionResult> TipoTelefonos()
        {
            var datos = await _context.Set<TipoTelefono>()
                .Where(x => x.Activo == true)
                .Select(x => new
                {
                    id = x.IdTipoTelefono,
                    texto = x.Nombre
                })
                .ToListAsync();

            return Ok(datos);
        }

        // Método para llenar el combo de entregas disponibles.
        // Solo devuelve entregas activas que todavía NO están asociadas
        // a una guía de despacho activa.
        //
        // Esto evita que una misma entrega sea agregada a dos guías diferentes.
        // Ruta: GET api/Combos/EntregasDisponibles
        [HttpGet("EntregasDisponibles")]
        public async Task<IActionResult> EntregasDisponibles()
        {
            // Primero consulta las entregas que ya están usadas
            // en la tabla DetalleGuia y que siguen activas.
            var entregasUsadas = await _context.Set<DetalleGuium>()
                .Where(d => d.Activo == true)
                .Select(d => d.IdEntrega)
                .ToListAsync();

            // Luego consulta las entregas activas que NO estén dentro
            // de la lista de entregas ya usadas.
            var datos = await _context.Set<Entrega>()
                .Where(e => e.Activo == true && !entregasUsadas.Contains(e.IdEntrega))
                .Select(e => new
                {
                    id = e.IdEntrega,
                    texto = "Entrega #" + e.IdEntrega + " - " + e.NombreDestinatario
                })
                .ToListAsync();

            return Ok(datos);
        }

        [HttpGet("TiposCamion")]
        public async Task<IActionResult> TiposCamion()
        {
            var datos = await _context.Set<TipoCamion>()
                .Where(x => x.Activo == true)
                .Select(x => new
                {
                    id = x.IdTipoCamion,
                    texto = x.Nombre
                })
                .ToListAsync();

            return Ok(datos);
        }

        [HttpGet("Servicios")]
        public async Task<IActionResult> Servicios()
        {
            var datos = await (
                from s in _context.Set<Servicio>()
                join c in _context.Set<Cliente>() on s.IdCliente equals c.IdCliente
                where s.Activo == true
                select new
                {
                    id = s.IdServicio,
                    texto = "Servicio #" + s.IdServicio + " - " + c.NombreCompleto
                }
            ).ToListAsync();

            return Ok(datos);
        }

        [HttpGet("TipoProductos")]
        public async Task<IActionResult> TipoProductos()
        {
            var datos = await _context.Set<TipoProducto>()
                .Where(x => x.Activo == true)
                .Select(x => new
                {
                    id = x.IdTipoProducto,
                    texto = x.Nombre
                })
                .ToListAsync();

            return Ok(datos);
        }
    }
}