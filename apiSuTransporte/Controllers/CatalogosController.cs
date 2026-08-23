using apiSuTransporte.DTOs;
using apiSuTransporte.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiSuTransporte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogosController : ControllerBase
    {
        private readonly BdSuTransporteContext _context;

        public CatalogosController(BdSuTransporteContext context)
        {
            _context = context;
        }

        [HttpGet("Marcas")]
        public async Task<IActionResult> Marcas()
        {
            var datos = await _context.Set<Marca>()
                .Select(x => new
                {
                    idMarca = x.IdMarca,
                    nombre = x.Nombre,
                    activo = x.Activo
                })
                .ToListAsync();

            return Ok(datos);
        }

        [HttpGet("Marcas/{id}")]
        public async Task<IActionResult> BuscarMarca(int id)
        {
            var dato = await _context.Set<Marca>()
                .Where(x => x.IdMarca == id)
                .Select(x => new
                {
                    idMarca = x.IdMarca,
                    nombre = x.Nombre,
                    activo = x.Activo
                })
                .FirstOrDefaultAsync();

            if (dato == null)
                return NotFound(new { mensaje = "No se encontró la marca" });

            return Ok(dato);
        }

        [HttpPost("Marcas")]
        public async Task<IActionResult> InsertarMarca([FromBody] CatalogoDTO dto)
        {
            bool existe = await _context.Set<Marca>()
                .AnyAsync(x => x.Nombre == dto.Nombre);

            if (existe)
                return BadRequest(new { mensaje = "Ya existe una marca con ese nombre" });

            Marca marca = new Marca
            {
                Nombre = dto.Nombre,
                Activo = true
            };

            _context.Set<Marca>().Add(marca);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Marca registrada correctamente" });
        }

        [HttpPut("Marcas/{id}")]
        public async Task<IActionResult> ModificarMarca(int id, [FromBody] CatalogoDTO dto)
        {
            var marca = await _context.Set<Marca>().FindAsync(id);

            if (marca == null)
                return NotFound(new { mensaje = "No se encontró la marca" });

            bool existe = await _context.Set<Marca>()
                .AnyAsync(x => x.Nombre == dto.Nombre && x.IdMarca != id);

            if (existe)
                return BadRequest(new { mensaje = "Ya existe otra marca con ese nombre" });

            marca.Nombre = dto.Nombre;
            marca.Activo = dto.Activo;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Marca modificada correctamente" });
        }

        [HttpDelete("Marcas/{id}")]
        public async Task<IActionResult> EliminarMarca(int id)
        {
            var marca = await _context.Set<Marca>().FindAsync(id);

            if (marca == null)
                return NotFound(new { mensaje = "No se encontró la marca" });

            marca.Activo = false;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Marca eliminada correctamente" });
        }

        [HttpGet("TiposCamion")]
        public async Task<IActionResult> TiposCamion()
        {
            var datos = await _context.Set<TipoCamion>()
                .Select(x => new
                {
                    idTipoCamion = x.IdTipoCamion,
                    nombre = x.Nombre,
                    activo = x.Activo
                })
                .ToListAsync();

            return Ok(datos);
        }

        [HttpGet("TiposCamion/{id}")]
        public async Task<IActionResult> BuscarTipoCamion(int id)
        {
            var dato = await _context.Set<TipoCamion>()
                .Where(x => x.IdTipoCamion == id)
                .Select(x => new
                {
                    idTipoCamion = x.IdTipoCamion,
                    nombre = x.Nombre,
                    activo = x.Activo
                })
                .FirstOrDefaultAsync();

            if (dato == null)
                return NotFound(new { mensaje = "No se encontró el tipo de camión" });

            return Ok(dato);
        }

        [HttpPost("TiposCamion")]
        public async Task<IActionResult> InsertarTipoCamion([FromBody] CatalogoDTO dto)
        {
            bool existe = await _context.Set<TipoCamion>()
                .AnyAsync(x => x.Nombre == dto.Nombre);

            if (existe)
                return BadRequest(new { mensaje = "Ya existe un tipo de camión con ese nombre" });

            TipoCamion tipo = new TipoCamion
            {
                Nombre = dto.Nombre,
                Activo = true
            };

            _context.Set<TipoCamion>().Add(tipo);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Tipo de camión registrado correctamente" });
        }

        [HttpPut("TiposCamion/{id}")]
        public async Task<IActionResult> ModificarTipoCamion(int id, [FromBody] CatalogoDTO dto)
        {
            var tipo = await _context.Set<TipoCamion>().FindAsync(id);

            if (tipo == null)
                return NotFound(new { mensaje = "No se encontró el tipo de camión" });

            bool existe = await _context.Set<TipoCamion>()
                .AnyAsync(x => x.Nombre == dto.Nombre && x.IdTipoCamion != id);

            if (existe)
                return BadRequest(new { mensaje = "Ya existe otro tipo de camión con ese nombre" });

            tipo.Nombre = dto.Nombre;
            tipo.Activo = dto.Activo;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Tipo de camión modificado correctamente" });
        }

        [HttpDelete("TiposCamion/{id}")]
        public async Task<IActionResult> EliminarTipoCamion(int id)
        {
            var tipo = await _context.Set<TipoCamion>().FindAsync(id);

            if (tipo == null)
                return NotFound(new { mensaje = "No se encontró el tipo de camión" });

            tipo.Activo = false;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Tipo de camión eliminado correctamente" });
        }

        [HttpGet("Sedes")]
        public async Task<IActionResult> Sedes()
        {
            var datos = await _context.Set<Sede>()
                .Select(x => new
                {
                    idSede = x.IdSede,
                    nombre = x.Nombre,
                    activo = x.Activo
                })
                .ToListAsync();

            return Ok(datos);
        }

        [HttpGet("Sedes/{id}")]
        public async Task<IActionResult> BuscarSede(int id)
        {
            var dato = await _context.Set<Sede>()
                .Where(x => x.IdSede == id)
                .Select(x => new
                {
                    idSede = x.IdSede,
                    nombre = x.Nombre,
                    activo = x.Activo
                })
                .FirstOrDefaultAsync();

            if (dato == null)
                return NotFound(new { mensaje = "No se encontró la sede" });

            return Ok(dato);
        }

        [HttpPost("Sedes")]
        public async Task<IActionResult> InsertarSede([FromBody] CatalogoDTO dto)
        {
            bool existe = await _context.Set<Sede>()
                .AnyAsync(x => x.Nombre == dto.Nombre);

            if (existe)
                return BadRequest(new { mensaje = "Ya existe una sede con ese nombre" });

            Sede sede = new Sede
            {
                Nombre = dto.Nombre,
                Activo = true
            };

            _context.Set<Sede>().Add(sede);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Sede registrada correctamente" });
        }

        [HttpPut("Sedes/{id}")]
        public async Task<IActionResult> ModificarSede(int id, [FromBody] CatalogoDTO dto)
        {
            var sede = await _context.Set<Sede>().FindAsync(id);

            if (sede == null)
                return NotFound(new { mensaje = "No se encontró la sede" });

            bool existe = await _context.Set<Sede>()
                .AnyAsync(x => x.Nombre == dto.Nombre && x.IdSede != id);

            if (existe)
                return BadRequest(new { mensaje = "Ya existe otra sede con ese nombre" });

            sede.Nombre = dto.Nombre;
            sede.Activo = dto.Activo;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Sede modificada correctamente" });
        }

        [HttpDelete("Sedes/{id}")]
        public async Task<IActionResult> EliminarSede(int id)
        {
            var sede = await _context.Set<Sede>().FindAsync(id);

            if (sede == null)
                return NotFound(new { mensaje = "No se encontró la sede" });

            sede.Activo = false;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Sede eliminada correctamente" });
        }

        [HttpGet("Departamentos")]
        public async Task<IActionResult> Departamentos()
        {
            var datos = await _context.Set<Departamento>()
                .Where(x => x.Activo == true)
                .Select(x => new
                {
                    id = x.IdDepartamento,
                    texto = x.Nombre
                })
                .ToListAsync();

            return Ok(datos);
        }

        [HttpGet("Ciudades")]
        public async Task<IActionResult> Ciudades()
        {
            var datos = await (
                from c in _context.Set<Ciudad>()
                join d in _context.Set<Departamento>() on c.IdDepartamento equals d.IdDepartamento
                select new
                {
                    idCiudad = c.IdCiudad,
                    idDepartamento = c.IdDepartamento,
                    departamento = d.Nombre,
                    nombre = c.Nombre,
                    activo = c.Activo
                }
            ).ToListAsync();

            return Ok(datos);
        }

        [HttpGet("Ciudades/{id}")]
        public async Task<IActionResult> BuscarCiudad(int id)
        {
            var dato = await _context.Set<Ciudad>()
                .Where(x => x.IdCiudad == id)
                .Select(x => new
                {
                    idCiudad = x.IdCiudad,
                    idDepartamento = x.IdDepartamento,
                    nombre = x.Nombre,
                    activo = x.Activo
                })
                .FirstOrDefaultAsync();

            if (dato == null)
                return NotFound(new { mensaje = "No se encontró la ciudad" });

            return Ok(dato);
        }

        [HttpPost("Ciudades")]
        public async Task<IActionResult> InsertarCiudad([FromBody] CatalogoCiudadDTO dto)
        {
            bool existeDepartamento = await _context.Set<Departamento>()
                .AnyAsync(x => x.IdDepartamento == dto.IdDepartamento && x.Activo == true);

            if (!existeDepartamento)
                return BadRequest(new { mensaje = "El departamento seleccionado no existe o está inactivo" });

            bool existe = await _context.Set<Ciudad>()
                .AnyAsync(x => x.Nombre == dto.Nombre && x.IdDepartamento == dto.IdDepartamento);

            if (existe)
                return BadRequest(new { mensaje = "Ya existe esa ciudad en el departamento seleccionado" });

            Ciudad ciudad = new Ciudad
            {
                IdDepartamento = dto.IdDepartamento,
                Nombre = dto.Nombre,
                Activo = true
            };

            _context.Set<Ciudad>().Add(ciudad);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Ciudad registrada correctamente" });
        }

        [HttpPut("Ciudades/{id}")]
        public async Task<IActionResult> ModificarCiudad(int id, [FromBody] CatalogoCiudadDTO dto)
        {
            var ciudad = await _context.Set<Ciudad>().FindAsync(id);

            if (ciudad == null)
                return NotFound(new { mensaje = "No se encontró la ciudad" });

            bool existeDepartamento = await _context.Set<Departamento>()
                .AnyAsync(x => x.IdDepartamento == dto.IdDepartamento && x.Activo == true);

            if (!existeDepartamento)
                return BadRequest(new { mensaje = "El departamento seleccionado no existe o está inactivo" });

            bool existe = await _context.Set<Ciudad>()
                .AnyAsync(x =>
                    x.Nombre == dto.Nombre &&
                    x.IdDepartamento == dto.IdDepartamento &&
                    x.IdCiudad != id);

            if (existe)
                return BadRequest(new { mensaje = "Ya existe otra ciudad con ese nombre en el departamento seleccionado" });

            ciudad.IdDepartamento = dto.IdDepartamento;
            ciudad.Nombre = dto.Nombre;
            ciudad.Activo = dto.Activo;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Ciudad modificada correctamente" });
        }

        [HttpDelete("Ciudades/{id}")]
        public async Task<IActionResult> EliminarCiudad(int id)
        {
            var ciudad = await _context.Set<Ciudad>().FindAsync(id);

            if (ciudad == null)
                return NotFound(new { mensaje = "No se encontró la ciudad" });

            ciudad.Activo = false;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Ciudad eliminada correctamente" });
        }
    }
}