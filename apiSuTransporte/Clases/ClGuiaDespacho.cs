using apiSuTransporte.DTOs;
using apiSuTransporte.Models;
using Microsoft.EntityFrameworkCore;

namespace apiSuTransporte.Clases
{
    // Clase operativa encargada de manejar la lógica de la Guía de Despacho.
    // Aquí se consultan, registran y eliminan guías junto con sus detalles.
    public class ClGuiaDespacho
    {
        // Contexto de Entity Framework.
        // Permite acceder a las tablas de SQL Server.
        private readonly BdSuTransporteContext _context;

        // Constructor de la clase.
        // Recibe el contexto de base de datos desde el controlador.
        public ClGuiaDespacho(BdSuTransporteContext context)
        {
            _context = context;
        }

        // Método Listar.
        // Sirve para consultar todas las guías de despacho activas.
        // Une varias tablas para mostrar información entendible:
        // camionero, camión, empleado que despacha y estado de la guía.
        public async Task<List<object>> Listar()
        {
            var datos = await (
                from g in _context.Set<GuiaDespacho>()
                join ca in _context.Set<Camionero>() on g.IdCamionero equals ca.IdCamionero
                join cm in _context.Set<Camion>() on g.IdCamion equals cm.IdCamion
                join tc in _context.Set<TipoCamion>() on cm.IdTipoCamion equals tc.IdTipoCamion
                join ed in _context.Set<Empleado>() on g.IdEmpleadoDespacha equals ed.IdEmpleado
                join eg in _context.Set<EstadoGuium>() on g.IdEstadoGuia equals eg.IdEstadoGuia
                join ci in _context.Set<Ciudad>() on g.IdCiudadDestino equals ci.IdCiudad
                where g.Activo == true
                select new
                {
                    g.IdGuiaDespacho,
                    g.FechaDistribucion,
                    Camionero = ca.Nombres + " " + ca.Apellidos,
                    Camion = cm.Placa + " - " + tc.Nombre,
                    EmpleadoDespacha = ed.Nombres + " " + ed.Apellidos,
                    EstadoGuia = eg.Nombre,
                    g.IdCiudadDestino,
                    CiudadDestino = ci.Nombre,
                    g.CantidadEntregas,
                    g.CostoTotal,
                    g.ValorTotal,
                    g.Observaciones,
                    g.Activo
                }
            ).ToListAsync<object>();

            return datos;
        }

        // Método Buscar.
        // Sirve para consultar una guía específica por su IdGuiaDespacho.
        // Devuelve dos partes:
        // 1. Los datos principales de la guía.
        // 2. Los detalles o entregas asociadas a esa guía.
        public async Task<object?> Buscar(int id)
        {
            // Consulta la información principal de la guía.
            var guia = await _context.Set<GuiaDespacho>()
                .Where(g => g.IdGuiaDespacho == id)
                .Select(g => new
                {
                    g.IdGuiaDespacho,
                    g.FechaDistribucion,
                    g.IdCamionero,
                    g.IdCamion,
                    g.IdEmpleadoDespacha,
                    g.IdEmpleadoRecibe,
                    g.IdEstadoGuia,
                    g.IdCiudadDestino,
                    g.CantidadEntregas,
                    g.CostoTotal,
                    g.ValorTotal,
                    g.Observaciones,
                    g.Activo
                })
                .FirstOrDefaultAsync();

            // Si la guía no existe, retorna null.
            if (guia == null)
                return null;

            // Consulta los detalles de la guía.
            // Cada detalle representa una entrega asociada.
            var detalles = await (
                from d in _context.Set<DetalleGuium>()

                // Relación con Entrega para mostrar el destinatario.
                join e in _context.Set<Entrega>()
                    on d.IdEntrega equals e.IdEntrega

                // Relación con EstadoEntrega para mostrar el estado de la entrega.
                join ee in _context.Set<EstadoEntrega>()
                    on d.IdEstadoEntrega equals ee.IdEstadoEntrega

                // Solo se consultan los detalles activos de la guía seleccionada.
                where d.IdGuiaDespacho == id && d.Activo == true

                select new
                {
                    d.IdDetalleGuia,
                    d.IdEntrega,
                    Entrega = "Entrega #" + e.IdEntrega + " - " + e.NombreDestinatario,
                    d.IdEstadoEntrega,
                    EstadoEntrega = ee.Nombre,
                    d.FechaEstado,
                    d.Observaciones
                }
            ).ToListAsync();

            // Retorna un objeto con la guía y sus detalles.
            return new
            {
                guia,
                detalles
            };
        }

        // Método Insertar.
        // Sirve para registrar una nueva guía de despacho junto con sus detalles.
        // Usa transacción para asegurar que si falla algo, no se guarde información incompleta.
        public async Task<string> Insertar(GuiaDespachoDTO dto)
        {
            // Inicia una transacción en la base de datos.
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Valida que la guía tenga mínimo una entrega en el detalle.
                if (dto.Detalles == null || dto.Detalles.Count == 0)
                    return "Debe agregar mínimo un detalle a la guía";

                // Valida que el camionero exista y esté activo.
                bool existeCamionero = await _context.Set<Camionero>()
                    .AnyAsync(x => x.IdCamionero == dto.IdCamionero && x.Activo == true);

                if (!existeCamionero)
                    return "El camionero seleccionado no existe o está inactivo";

                // Valida que el camión exista y esté activo.
                bool existeCamion = await _context.Set<Camion>()
                    .AnyAsync(x => x.IdCamion == dto.IdCamion && x.Activo == true);

                if (!existeCamion)
                    return "El camión seleccionado no existe o está inactivo";

                // Valida que el empleado que despacha exista y esté activo.
                bool existeEmpleadoDespacha = await _context.Set<Empleado>()
                    .AnyAsync(x => x.IdEmpleado == dto.IdEmpleadoDespacha && x.Activo == true);

                if (!existeEmpleadoDespacha)
                    return "El empleado que despacha no existe o está inactivo";

                // Valida que el estado de la guía exista y esté activo.
                bool existeEstadoGuia = await _context.Set<EstadoGuium>()
                    .AnyAsync(x => x.IdEstadoGuia == dto.IdEstadoGuia && x.Activo == true);

                if (!existeEstadoGuia)
                    return "El estado de guía seleccionado no existe o está inactivo";

      

                // Recorre cada detalle enviado desde el formulario.
                foreach (var item in dto.Detalles)
                {
                    // Valida que la entrega exista y esté activa.
                    bool existeEntrega = await _context.Set<Entrega>()
                        .AnyAsync(x => x.IdEntrega == item.IdEntrega && x.Activo == true);

                    if (!existeEntrega)
                        return "La entrega seleccionada no existe o está inactiva";

                    // Valida que la entrega no esté asociada a otra guía activa.
                    // Esto evita repetir una misma entrega en varias guías.
                    bool entregaYaUsada = await _context.Set<DetalleGuium>()
                        .AnyAsync(x => x.IdEntrega == item.IdEntrega && x.Activo == true);

                    if (entregaYaUsada)
                        return "La entrega #" + item.IdEntrega + " ya está asociada a otra guía de despacho";

                    // Valida que el estado de entrega exista y esté activo.
                    bool existeEstadoEntrega = await _context.Set<EstadoEntrega>()
                        .AnyAsync(x => x.IdEstadoEntrega == item.IdEstadoEntrega && x.Activo == true);

                    if (!existeEstadoEntrega)
                        return "El estado de entrega seleccionado no existe o está inactivo";

                    bool existeCiudadDestino = await _context.Set<Ciudad>()
                       .AnyAsync(x => x.IdCiudad == dto.IdCiudadDestino && x.Activo == true);

                    if (!existeCiudadDestino)
                        return "La ciudad destino seleccionada no existe o está inactiva";

                }

                // Crea la guía de despacho principal.
                GuiaDespacho guia = new GuiaDespacho
                {
                    FechaDistribucion = dto.FechaDistribucion,
                    IdCamionero = dto.IdCamionero,
                    IdCamion = dto.IdCamion,
                    IdEmpleadoDespacha = dto.IdEmpleadoDespacha,
                    IdEmpleadoRecibe = dto.IdEmpleadoRecibe,
                    IdEstadoGuia = dto.IdEstadoGuia,
                    IdCiudadDestino = dto.IdCiudadDestino,
                    CantidadEntregas = dto.Detalles.Count,
                    CostoTotal = dto.CostoTotal,
                    ValorTotal = dto.ValorTotal,
                    Observaciones = dto.Observaciones,
                    Activo = true
                };

                // Agrega la guía al contexto.
                _context.Set<GuiaDespacho>().Add(guia);

                // Guarda primero la guía para obtener el IdGuiaDespacho generado.
                await _context.SaveChangesAsync();

                // Recorre los detalles enviados desde el formulario.
                foreach (var item in dto.Detalles)
                {
                    // Crea cada detalle asociado a la guía recién creada.
                    DetalleGuium detalle = new DetalleGuium
                    {
                        IdGuiaDespacho = guia.IdGuiaDespacho,
                        IdEntrega = item.IdEntrega,
                        IdEstadoEntrega = item.IdEstadoEntrega,
                        FechaEstado = item.FechaEstado,
                        Observaciones = item.Observaciones,
                        Activo = true
                    };

                    // Agrega el detalle al contexto.
                    _context.Set<DetalleGuium>().Add(detalle);
                }

                // Guarda todos los detalles.
                await _context.SaveChangesAsync();

                // Confirma la transacción.
                await transaction.CommitAsync();

                return "Guía de despacho registrada correctamente";
            }
            catch (Exception ex)
            {
                // Si ocurre un error, deshace todos los cambios.
                await transaction.RollbackAsync();

                // Obtiene el error real si existe una excepción interna.
                string errorReal = ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;

                return "Error al registrar la guía: " + errorReal;
            }
        }

        // Método Eliminar.
        // Realiza eliminación lógica de la guía de despacho.
        // No borra físicamente el registro, solo cambia Activo a false.
        // También desactiva todos los detalles asociados a esa guía.
        public async Task<string> Eliminar(int id)
        {
            // Busca la guía por su llave primaria.
            var guia = await _context.Set<GuiaDespacho>().FindAsync(id);

            // Si no existe, retorna mensaje de error.
            if (guia == null)
                return "No se encontró la guía";

            // Marca la guía como inactiva.
            guia.Activo = false;

            // Busca todos los detalles asociados a la guía.
            var detalles = await _context.Set<DetalleGuium>()
                .Where(d => d.IdGuiaDespacho == id)
                .ToListAsync();

            // Marca cada detalle como inactivo.
            foreach (var detalle in detalles)
            {
                detalle.Activo = false;
            }

            // Guarda los cambios en SQL Server.
            await _context.SaveChangesAsync();

            return "Guía eliminada correctamente";
        }
    }
}