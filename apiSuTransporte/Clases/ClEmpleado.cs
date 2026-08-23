using apiSuTransporte.DTOs;
using apiSuTransporte.Models;
using Microsoft.EntityFrameworkCore;

namespace apiSuTransporte.Clases
{
    public class ClEmpleado
    {
        private readonly BdSuTransporteContext _context;

        public ClEmpleado(BdSuTransporteContext context)
        {
            _context = context;
        }

        public async Task<List<object>> Listar()
        {
            var datos = await (
                from e in _context.Set<Empleado>()
                join td in _context.Set<TipoDocumento>() on e.IdTipoDocumento equals td.IdTipoDocumento

                join de in _context.Set<DirEmpleado>() on e.IdEmpleado equals de.IdEmpleado into dirTemp
                from de in dirTemp.DefaultIfEmpty()

                join c in _context.Set<Ciudad>() on de.IdCiudad equals c.IdCiudad into ciudadTemp
                from c in ciudadTemp.DefaultIfEmpty()

                join te in _context.Set<TelEmpleado>() on e.IdEmpleado equals te.IdEmpleado into telTemp
                from te in telTemp.DefaultIfEmpty()

                where e.Activo == true

                select new
                {
                    e.IdEmpleado,
                    Documento = td.Nombre + " " + e.NumeroDocumento,
                    e.Nombres,
                    e.Apellidos,
                    e.Cargo,
                    Ciudad = c != null ? c.Nombre : "",
                    Telefono = te != null ? te.NumeroTelefono : "",
                    Activo = e.Activo == true ? "Activo" : "Inactivo"
                }
            ).ToListAsync<object>();

            return datos;
        }

        public async Task<object?> Buscar(int id)
        {
            var dato = await (
                from e in _context.Set<Empleado>()
                join de in _context.Set<DirEmpleado>() on e.IdEmpleado equals de.IdEmpleado into dirTemp
                from de in dirTemp.DefaultIfEmpty()

                join te in _context.Set<TelEmpleado>() on e.IdEmpleado equals te.IdEmpleado into telTemp
                from te in telTemp.DefaultIfEmpty()

                where e.IdEmpleado == id

                select new
                {
                    e.IdEmpleado,
                    e.IdTipoDocumento,
                    e.NumeroDocumento,
                    e.Nombres,
                    e.Apellidos,
                    e.Cargo,
                    IdCiudad = de != null ? de.IdCiudad : 0,
                    Direccion = de != null ? de.Direccion : "",
                    IdTipoTelefono = te != null ? te.IdTipoTelefono : 0,
                    NumeroTelefono = te != null ? te.NumeroTelefono : "",
                    e.Activo
                }
            ).FirstOrDefaultAsync();

            return dato;
        }

        public async Task<string> Insertar(EmpleadoDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                bool existe = await _context.Set<Empleado>()
                    .AnyAsync(e =>
                        e.IdTipoDocumento == dto.IdTipoDocumento &&
                        e.NumeroDocumento == dto.NumeroDocumento);

                if (existe)
                    return "Ya existe un empleado con ese documento";

                Empleado empleado = new Empleado
                {
                    IdTipoDocumento = dto.IdTipoDocumento,
                    NumeroDocumento = dto.NumeroDocumento,
                    Nombres = dto.Nombres,
                    Apellidos = dto.Apellidos,
                    Cargo = dto.Cargo,
                    Activo = true
                };

                _context.Set<Empleado>().Add(empleado);
                await _context.SaveChangesAsync();

                DirEmpleado direccion = new DirEmpleado
                {
                    IdEmpleado = empleado.IdEmpleado,
                    IdCiudad = dto.IdCiudad,
                    Direccion = dto.Direccion,
                    Activo = true
                };

                _context.Set<DirEmpleado>().Add(direccion);

                TelEmpleado telefono = new TelEmpleado
                {
                    IdEmpleado = empleado.IdEmpleado,
                    IdTipoTelefono = dto.IdTipoTelefono,
                    NumeroTelefono = dto.NumeroTelefono,
                    Activo = true
                };

                _context.Set<TelEmpleado>().Add(telefono);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Empleado registrado correctamente";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                string errorReal = ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;

                return "Error al registrar empleado: " + errorReal;
            }
        }

        public async Task<string> Modificar(int id, EmpleadoDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var empleado = await _context.Set<Empleado>().FindAsync(id);

                if (empleado == null)
                    return "No se encontró el empleado";

                bool existe = await _context.Set<Empleado>()
                    .AnyAsync(e =>
                        e.IdTipoDocumento == dto.IdTipoDocumento &&
                        e.NumeroDocumento == dto.NumeroDocumento &&
                        e.IdEmpleado != id);

                if (existe)
                    return "Ya existe otro empleado con ese documento";

                empleado.IdTipoDocumento = dto.IdTipoDocumento;
                empleado.NumeroDocumento = dto.NumeroDocumento;
                empleado.Nombres = dto.Nombres;
                empleado.Apellidos = dto.Apellidos;
                empleado.Cargo = dto.Cargo;
                empleado.Activo = dto.Activo;

                var direccion = await _context.Set<DirEmpleado>()
                    .FirstOrDefaultAsync(d => d.IdEmpleado == id);

                if (direccion == null)
                {
                    direccion = new DirEmpleado
                    {
                        IdEmpleado = id,
                        IdCiudad = dto.IdCiudad,
                        Direccion = dto.Direccion,
                        Activo = true
                    };

                    _context.Set<DirEmpleado>().Add(direccion);
                }
                else
                {
                    direccion.IdCiudad = dto.IdCiudad;
                    direccion.Direccion = dto.Direccion;
                    direccion.Activo = true;
                }

                var telefono = await _context.Set<TelEmpleado>()
                    .FirstOrDefaultAsync(t => t.IdEmpleado == id);

                if (telefono == null)
                {
                    telefono = new TelEmpleado
                    {
                        IdEmpleado = id,
                        IdTipoTelefono = dto.IdTipoTelefono,
                        NumeroTelefono = dto.NumeroTelefono,
                        Activo = true
                    };

                    _context.Set<TelEmpleado>().Add(telefono);
                }
                else
                {
                    telefono.IdTipoTelefono = dto.IdTipoTelefono;
                    telefono.NumeroTelefono = dto.NumeroTelefono;
                    telefono.Activo = true;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Empleado modificado correctamente";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                string errorReal = ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;

                return "Error al modificar empleado: " + errorReal;
            }
        }

        public async Task<string> Eliminar(int id)
        {
            var empleado = await _context.Set<Empleado>().FindAsync(id);

            if (empleado == null)
                return "No se encontró el empleado";

            empleado.Activo = false;

            var direcciones = await _context.Set<DirEmpleado>()
                .Where(d => d.IdEmpleado == id)
                .ToListAsync();

            foreach (var item in direcciones)
            {
                item.Activo = false;
            }

            var telefonos = await _context.Set<TelEmpleado>()
                .Where(t => t.IdEmpleado == id)
                .ToListAsync();

            foreach (var item in telefonos)
            {
                item.Activo = false;
            }

            await _context.SaveChangesAsync();

            return "Empleado eliminado correctamente";
        }
    }
}