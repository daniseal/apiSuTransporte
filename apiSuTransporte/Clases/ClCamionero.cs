using apiSuTransporte.DTOs;
using apiSuTransporte.Models;
using Microsoft.EntityFrameworkCore;

namespace apiSuTransporte.Clases
{
    public class ClCamionero
    {
        private readonly BdSuTransporteContext _context;

        public ClCamionero(BdSuTransporteContext context)
        {
            _context = context;
        }

        public async Task<List<object>> Listar()
        {
            var datos = await (
                from c in _context.Set<Camionero>()
                join td in _context.Set<TipoDocumento>() on c.IdTipoDocumento equals td.IdTipoDocumento

                join dc in _context.Set<DirCamionero>() on c.IdCamionero equals dc.IdCamionero into dirTemp
                from dc in dirTemp.DefaultIfEmpty()

                join ci in _context.Set<Ciudad>() on dc.IdCiudad equals ci.IdCiudad into ciudadTemp
                from ci in ciudadTemp.DefaultIfEmpty()

                join tc in _context.Set<TelCamionero>() on c.IdCamionero equals tc.IdCamionero into telTemp
                from tc in telTemp.DefaultIfEmpty()

                where c.Activo == true

                select new
                {
                    c.IdCamionero,
                    Documento = td.Nombre + " " + c.NumeroDocumento,
                    c.Nombres,
                    c.Apellidos,
                    c.Salario,
                    c.CategoriaPase,
                    Ciudad = ci != null ? ci.Nombre : "",
                    Telefono = tc != null ? tc.NumeroTelefono : "",
                    Activo = c.Activo == true ? "Activo" : "Inactivo"
                }
            ).ToListAsync<object>();

            return datos;
        }

        public async Task<object?> Buscar(int id)
        {
            var dato = await (
                from c in _context.Set<Camionero>()

                join dc in _context.Set<DirCamionero>() on c.IdCamionero equals dc.IdCamionero into dirTemp
                from dc in dirTemp.DefaultIfEmpty()

                join tc in _context.Set<TelCamionero>() on c.IdCamionero equals tc.IdCamionero into telTemp
                from tc in telTemp.DefaultIfEmpty()

                where c.IdCamionero == id

                select new
                {
                    c.IdCamionero,
                    c.IdTipoDocumento,
                    c.NumeroDocumento,
                    c.Nombres,
                    c.Apellidos,
                    c.Salario,
                    c.CategoriaPase,
                    IdCiudad = dc != null ? dc.IdCiudad : 0,
                    Direccion = dc != null ? dc.Direccion : "",
                    IdTipoTelefono = tc != null ? tc.IdTipoTelefono : 0,
                    NumeroTelefono = tc != null ? tc.NumeroTelefono : "",
                    c.Activo
                }
            ).FirstOrDefaultAsync();

            return dato;
        }

        public async Task<string> Insertar(CamioneroDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                bool existeDocumento = await _context.Set<Camionero>()
                    .AnyAsync(c =>
                        c.IdTipoDocumento == dto.IdTipoDocumento &&
                        c.NumeroDocumento == dto.NumeroDocumento);

                if (existeDocumento)
                    return "Ya existe un camionero con ese documento";

                Camionero camionero = new Camionero
                {
                    IdTipoDocumento = dto.IdTipoDocumento,
                    NumeroDocumento = dto.NumeroDocumento,
                    Nombres = dto.Nombres,
                    Apellidos = dto.Apellidos,
                    Salario = dto.Salario,
                    CategoriaPase = dto.CategoriaPase,
                    Activo = true
                };

                _context.Set<Camionero>().Add(camionero);
                await _context.SaveChangesAsync();

                DirCamionero direccion = new DirCamionero
                {
                    IdCamionero = camionero.IdCamionero,
                    IdCiudad = dto.IdCiudad,
                    Direccion = dto.Direccion,
                    Activo = true
                };

                _context.Set<DirCamionero>().Add(direccion);

                TelCamionero telefono = new TelCamionero
                {
                    IdCamionero = camionero.IdCamionero,
                    IdTipoTelefono = dto.IdTipoTelefono,
                    NumeroTelefono = dto.NumeroTelefono,
                    Activo = true
                };

                _context.Set<TelCamionero>().Add(telefono);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Camionero registrado correctamente";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                string errorReal = ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;

                return "Error al registrar camionero: " + errorReal;
            }
        }

        public async Task<string> Modificar(int id, CamioneroDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var camionero = await _context.Set<Camionero>().FindAsync(id);

                if (camionero == null)
                    return "No se encontró el camionero";

                bool existeDocumento = await _context.Set<Camionero>()
                    .AnyAsync(c =>
                        c.IdTipoDocumento == dto.IdTipoDocumento &&
                        c.NumeroDocumento == dto.NumeroDocumento &&
                        c.IdCamionero != id);

                if (existeDocumento)
                    return "Ya existe otro camionero con ese documento";

                camionero.IdTipoDocumento = dto.IdTipoDocumento;
                camionero.NumeroDocumento = dto.NumeroDocumento;
                camionero.Nombres = dto.Nombres;
                camionero.Apellidos = dto.Apellidos;
                camionero.Salario = dto.Salario;
                camionero.CategoriaPase = dto.CategoriaPase;
                camionero.Activo = dto.Activo;

                var direccion = await _context.Set<DirCamionero>()
                    .FirstOrDefaultAsync(d => d.IdCamionero == id);

                if (direccion == null)
                {
                    direccion = new DirCamionero
                    {
                        IdCamionero = id,
                        IdCiudad = dto.IdCiudad,
                        Direccion = dto.Direccion,
                        Activo = true
                    };

                    _context.Set<DirCamionero>().Add(direccion);
                }
                else
                {
                    direccion.IdCiudad = dto.IdCiudad;
                    direccion.Direccion = dto.Direccion;
                    direccion.Activo = true;
                }

                var telefono = await _context.Set<TelCamionero>()
                    .FirstOrDefaultAsync(t => t.IdCamionero == id);

                if (telefono == null)
                {
                    telefono = new TelCamionero
                    {
                        IdCamionero = id,
                        IdTipoTelefono = dto.IdTipoTelefono,
                        NumeroTelefono = dto.NumeroTelefono,
                        Activo = true
                    };

                    _context.Set<TelCamionero>().Add(telefono);
                }
                else
                {
                    telefono.IdTipoTelefono = dto.IdTipoTelefono;
                    telefono.NumeroTelefono = dto.NumeroTelefono;
                    telefono.Activo = true;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Camionero modificado correctamente";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                string errorReal = ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;

                return "Error al modificar camionero: " + errorReal;
            }
        }

        public async Task<string> Eliminar(int id)
        {
            var camionero = await _context.Set<Camionero>().FindAsync(id);

            if (camionero == null)
                return "No se encontró el camionero";

            camionero.Activo = false;

            var direcciones = await _context.Set<DirCamionero>()
                .Where(d => d.IdCamionero == id)
                .ToListAsync();

            foreach (var item in direcciones)
            {
                item.Activo = false;
            }

            var telefonos = await _context.Set<TelCamionero>()
                .Where(t => t.IdCamionero == id)
                .ToListAsync();

            foreach (var item in telefonos)
            {
                item.Activo = false;
            }

            await _context.SaveChangesAsync();

            return "Camionero eliminado correctamente";
        }
    }
}