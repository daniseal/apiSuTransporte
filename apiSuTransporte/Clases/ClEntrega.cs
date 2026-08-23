using apiSuTransporte.DTOs;
using apiSuTransporte.Models;
using Microsoft.EntityFrameworkCore;

namespace apiSuTransporte.Clases
{
    public class ClEntrega
    {
        private readonly BdSuTransporteContext _context;

        public ClEntrega(BdSuTransporteContext context)
        {
            _context = context;
        }

        public async Task<List<object>> Listar()
        {
            var datos = await (
                from e in _context.Set<Entrega>()
                join s in _context.Set<Servicio>() on e.IdServicio equals s.IdServicio
                join tp in _context.Set<TipoProducto>() on e.IdTipoProducto equals tp.IdTipoProducto
                join c in _context.Set<Ciudad>() on e.IdCiudadDestino equals c.IdCiudad
                where e.Activo == true
                select new
                {
                    e.IdEntrega,
                    e.IdServicio,
                    Servicio = "Servicio #" + s.IdServicio,
                    e.IdTipoProducto,
                    TipoProducto = tp.Nombre,
                    e.IdCiudadDestino,
                    CiudadDestino = c.Nombre,
                    e.NombreDestinatario,
                    e.ApellidoDestinatario,
                    e.TelefonoDestinatario,
                    e.DireccionDestino,
                    e.Contenido,
                    e.Cantidad,
                    e.Dimensiones,
                    e.PesoKg,
                    e.Costo,
                    e.ValorDeclarado,
                    e.Observaciones,
                    Activo = e.Activo == true ? "Activo" : "Inactivo"
                }
            ).ToListAsync<object>();

            return datos;
        }

        public async Task<object?> Buscar(int id)
        {
            var dato = await (
                from e in _context.Set<Entrega>()
                where e.IdEntrega == id
                select new
                {
                    e.IdEntrega,
                    e.IdServicio,
                    e.IdTipoProducto,
                    e.IdCiudadDestino,
                    e.NombreDestinatario,
                    e.ApellidoDestinatario,
                    e.TelefonoDestinatario,
                    e.DireccionDestino,
                    e.Contenido,
                    e.Cantidad,
                    e.Dimensiones,
                    e.PesoKg,
                    e.Costo,
                    e.ValorDeclarado,
                    e.Observaciones,
                    e.Activo
                }
            ).FirstOrDefaultAsync();

            return dato;
        }

        public async Task<string> Insertar(EntregaDTO dto)
        {
            bool existeServicio = await _context.Set<Servicio>()
                .AnyAsync(s => s.IdServicio == dto.IdServicio && s.Activo == true);

            if (!existeServicio)
                return "El servicio seleccionado no existe o está inactivo";

            bool existeTipoProducto = await _context.Set<TipoProducto>()
                .AnyAsync(tp => tp.IdTipoProducto == dto.IdTipoProducto && tp.Activo == true);

            if (!existeTipoProducto)
                return "El tipo de producto seleccionado no existe o está inactivo";

            bool existeCiudad = await _context.Set<Ciudad>()
                .AnyAsync(c => c.IdCiudad == dto.IdCiudadDestino && c.Activo == true);

            if (!existeCiudad)
                return "La ciudad destino seleccionada no existe o está inactiva";

            Entrega entrega = new Entrega
            {
                IdServicio = dto.IdServicio,
                IdTipoProducto = dto.IdTipoProducto,
                IdCiudadDestino = dto.IdCiudadDestino,
                NombreDestinatario = dto.NombreDestinatario,
                ApellidoDestinatario = dto.ApellidoDestinatario,
                TelefonoDestinatario = dto.TelefonoDestinatario,
                DireccionDestino = dto.DireccionDestino,
                Contenido = dto.Contenido,
                Cantidad = dto.Cantidad,
                Dimensiones = dto.Dimensiones,
                PesoKg = dto.PesoKg,
                Costo = dto.Costo,
                ValorDeclarado = dto.ValorDeclarado,
                Observaciones = dto.Observaciones,
                Activo = true
            };

            _context.Set<Entrega>().Add(entrega);
            await _context.SaveChangesAsync();

            return "Entrega registrada correctamente";
        }

        public async Task<string> Modificar(int id, EntregaDTO dto)
        {
            var entrega = await _context.Set<Entrega>().FindAsync(id);

            if (entrega == null)
                return "No se encontró la entrega";

            bool existeServicio = await _context.Set<Servicio>()
                .AnyAsync(s => s.IdServicio == dto.IdServicio && s.Activo == true);

            if (!existeServicio)
                return "El servicio seleccionado no existe o está inactivo";

            bool existeTipoProducto = await _context.Set<TipoProducto>()
                .AnyAsync(tp => tp.IdTipoProducto == dto.IdTipoProducto && tp.Activo == true);

            if (!existeTipoProducto)
                return "El tipo de producto seleccionado no existe o está inactivo";

            bool existeCiudad = await _context.Set<Ciudad>()
                .AnyAsync(c => c.IdCiudad == dto.IdCiudadDestino && c.Activo == true);

            if (!existeCiudad)
                return "La ciudad destino seleccionada no existe o está inactiva";

            entrega.IdServicio = dto.IdServicio;
            entrega.IdTipoProducto = dto.IdTipoProducto;
            entrega.IdCiudadDestino = dto.IdCiudadDestino;
            entrega.NombreDestinatario = dto.NombreDestinatario;
            entrega.ApellidoDestinatario = dto.ApellidoDestinatario;
            entrega.TelefonoDestinatario = dto.TelefonoDestinatario;
            entrega.DireccionDestino = dto.DireccionDestino;
            entrega.Contenido = dto.Contenido;
            entrega.Cantidad = dto.Cantidad;
            entrega.Dimensiones = dto.Dimensiones;
            entrega.PesoKg = dto.PesoKg;
            entrega.Costo = dto.Costo;
            entrega.ValorDeclarado = dto.ValorDeclarado;
            entrega.Observaciones = dto.Observaciones;
            entrega.Activo = dto.Activo;

            await _context.SaveChangesAsync();

            return "Entrega modificada correctamente";
        }

        public async Task<string> Eliminar(int id)
        {
            var entrega = await _context.Set<Entrega>().FindAsync(id);

            if (entrega == null)
                return "No se encontró la entrega";

            entrega.Activo = false;

            await _context.SaveChangesAsync();

            return "Entrega eliminada correctamente";
        }
    }
}