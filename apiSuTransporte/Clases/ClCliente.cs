using apiSuTransporte.DTOs;
using apiSuTransporte.Models;
using Microsoft.EntityFrameworkCore;

namespace apiSuTransporte.Clases
{
    // Clase operativa encargada de manejar la lógica del módulo Cliente.
    // Aquí se realizan las operaciones de consultar, insertar, modificar y eliminar clientes.
    public class ClCliente
    {
        // Contexto de Entity Framework.
        // Permite acceder a las tablas de la base de datos bdSuTransporte.
        private readonly BdSuTransporteContext _context;

        // Constructor de la clase.
        // Recibe el contexto de base de datos desde el controlador.
        public ClCliente(BdSuTransporteContext context)
        {
            _context = context;
        }

        // Método Listar.
        // Consulta todos los clientes activos.
        // También trae información relacionada como tipo de documento, ciudad, dirección y teléfono.
        public async Task<List<object>> Listar()
        {
            var datos = await (
                from c in _context.Set<Cliente>()

                // Relación con TipoDocumento para mostrar el nombre del tipo de documento.
                join td in _context.Set<TipoDocumento>()
                    on c.IdTipoDocumento equals td.IdTipoDocumento

                // Relación con DirCliente.
                // Se usa left join para que el cliente aparezca aunque no tenga dirección registrada.
                join dc in _context.Set<DirCliente>()
                    on c.IdCliente equals dc.IdCliente into dirTemp
                from dc in dirTemp.DefaultIfEmpty()

                    // Relación con Ciudad.
                    // Se usa left join porque puede no existir dirección.
                join ci in _context.Set<Ciudad>()
                    on dc.IdCiudad equals ci.IdCiudad into ciudadTemp
                from ci in ciudadTemp.DefaultIfEmpty()

                    // Relación con TelCliente.
                    // Se usa left join para que el cliente aparezca aunque no tenga teléfono registrado.
                join tc in _context.Set<TelCliente>()
                    on c.IdCliente equals tc.IdCliente into telTemp
                from tc in telTemp.DefaultIfEmpty()

                    // Solo se listan los clientes activos.
                where c.Activo == true

                // Se retorna un objeto personalizado para mostrar datos claros en el DataTable.
                select new
                {
                    c.IdCliente,
                    TipoDocumento = td.Nombre,
                    c.NumeroDocumento,
                    c.NombreCompleto,
                    Ciudad = ci != null ? ci.Nombre : "",
                    Direccion = dc != null ? dc.Direccion : "",
                    Telefono = tc != null ? tc.NumeroTelefono : "",
                    c.Activo
                }
            ).ToListAsync<object>();

            return datos;
        }

        // Método Buscar.
        // Consulta un cliente específico por su IdCliente.
        // Se usa para cargar los datos en el formulario cuando se selecciona un cliente.
        public async Task<object?> Buscar(int id)
        {
            var dato = await (
                from c in _context.Set<Cliente>()

                // Relación con TipoDocumento.
                join td in _context.Set<TipoDocumento>()
                    on c.IdTipoDocumento equals td.IdTipoDocumento

                // Relación con DirCliente.
                join dc in _context.Set<DirCliente>()
                    on c.IdCliente equals dc.IdCliente into dirTemp
                from dc in dirTemp.DefaultIfEmpty()

                    // Relación con TelCliente.
                join tc in _context.Set<TelCliente>()
                    on c.IdCliente equals tc.IdCliente into telTemp
                from tc in telTemp.DefaultIfEmpty()

                    // Filtro por el cliente seleccionado.
                where c.IdCliente == id

                // Se devuelven los IDs necesarios para llenar los combos del formulario.
                select new
                {
                    c.IdCliente,
                    c.IdTipoDocumento,
                    c.NumeroDocumento,
                    c.NombreCompleto,
                    IdCiudad = dc != null ? dc.IdCiudad : 0,
                    Direccion = dc != null ? dc.Direccion : "",
                    IdTipoTelefono = tc != null ? tc.IdTipoTelefono : 0,
                    NumeroTelefono = tc != null ? tc.NumeroTelefono : "",
                    c.Activo
                }
            ).FirstOrDefaultAsync();

            return dato;
        }

        // Método Insertar.
        // Registra un nuevo cliente junto con su dirección y teléfono.
        // Usa una transacción para asegurar que todo se guarde correctamente.
        public async Task<string> Insertar(ClienteDTO dto)
        {
            // Inicia una transacción.
            // Si falla cliente, dirección o teléfono, no se guarda nada incompleto.
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Valida si ya existe un cliente con el mismo tipo y número de documento.
                bool existe = await _context.Set<Cliente>()
                    .AnyAsync(c =>
                        c.IdTipoDocumento == dto.IdTipoDocumento &&
                        c.NumeroDocumento == dto.NumeroDocumento);

                if (existe)
                    return "Ya existe un cliente con ese documento";

                // Crea el registro principal del cliente.
                Cliente cliente = new Cliente
                {
                    IdTipoDocumento = dto.IdTipoDocumento,
                    NumeroDocumento = dto.NumeroDocumento,
                    NombreCompleto = dto.NombreCompleto,
                    Activo = true
                };

                // Agrega el cliente al contexto.
                _context.Set<Cliente>().Add(cliente);

                // Guarda primero el cliente para obtener el IdCliente generado.
                await _context.SaveChangesAsync();

                // Crea la dirección asociada al cliente recién creado.
                DirCliente direccion = new DirCliente
                {
                    IdCliente = cliente.IdCliente,
                    IdCiudad = dto.IdCiudad,
                    Direccion = dto.Direccion,
                    Activo = true
                };

                // Agrega la dirección al contexto.
                _context.Set<DirCliente>().Add(direccion);

                // Crea el teléfono asociado al cliente.
                TelCliente telefono = new TelCliente
                {
                    IdCliente = cliente.IdCliente,
                    IdTipoTelefono = dto.IdTipoTelefono,
                    NumeroTelefono = dto.NumeroTelefono,
                    Activo = true
                };

                // Agrega el teléfono al contexto.
                _context.Set<TelCliente>().Add(telefono);

                // Guarda dirección y teléfono en la base de datos.
                await _context.SaveChangesAsync();

                // Confirma la transacción.
                await transaction.CommitAsync();

                return "Cliente registrado correctamente";
            }
            catch (Exception ex)
            {
                // Si ocurre un error, deshace todo lo realizado en la transacción.
                await transaction.RollbackAsync();

                return "Error al registrar cliente: " + ex.Message;
            }
        }

        // Método Modificar.
        // Actualiza los datos del cliente, su dirección y su teléfono.
        // También usa transacción para evitar guardar datos incompletos.
        public async Task<string> Modificar(int id, ClienteDTO dto)
        {
            // Inicia una transacción.
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Busca el cliente por su llave primaria.
                var cliente = await _context.Set<Cliente>().FindAsync(id);

                if (cliente == null)
                    return "No se encontró el cliente";

                // Valida que no exista otro cliente con el mismo documento.
                bool existe = await _context.Set<Cliente>()
                    .AnyAsync(c =>
                        c.IdTipoDocumento == dto.IdTipoDocumento &&
                        c.NumeroDocumento == dto.NumeroDocumento &&
                        c.IdCliente != id);

                if (existe)
                    return "Ya existe otro cliente con ese documento";

                // Actualiza los datos principales del cliente.
                cliente.IdTipoDocumento = dto.IdTipoDocumento;
                cliente.NumeroDocumento = dto.NumeroDocumento;
                cliente.NombreCompleto = dto.NombreCompleto;
                cliente.Activo = dto.Activo;

                // Busca la dirección actual del cliente.
                var direccion = await _context.Set<DirCliente>()
                    .FirstOrDefaultAsync(d => d.IdCliente == id);

                // Si el cliente no tiene dirección, se crea una nueva.
                if (direccion == null)
                {
                    direccion = new DirCliente
                    {
                        IdCliente = id,
                        IdCiudad = dto.IdCiudad,
                        Direccion = dto.Direccion,
                        Activo = true
                    };

                    _context.Set<DirCliente>().Add(direccion);
                }
                else
                {
                    // Si ya existe, se actualiza.
                    direccion.IdCiudad = dto.IdCiudad;
                    direccion.Direccion = dto.Direccion;
                    direccion.Activo = true;
                }

                // Busca el teléfono actual del cliente.
                var telefono = await _context.Set<TelCliente>()
                    .FirstOrDefaultAsync(t => t.IdCliente == id);

                // Si el cliente no tiene teléfono, se crea uno nuevo.
                if (telefono == null)
                {
                    telefono = new TelCliente
                    {
                        IdCliente = id,
                        IdTipoTelefono = dto.IdTipoTelefono,
                        NumeroTelefono = dto.NumeroTelefono,
                        Activo = true
                    };

                    _context.Set<TelCliente>().Add(telefono);
                }
                else
                {
                    // Si ya existe, se actualiza.
                    telefono.IdTipoTelefono = dto.IdTipoTelefono;
                    telefono.NumeroTelefono = dto.NumeroTelefono;
                    telefono.Activo = true;
                }

                // Guarda todos los cambios realizados.
                await _context.SaveChangesAsync();

                // Confirma la transacción.
                await transaction.CommitAsync();

                return "Cliente modificado correctamente";
            }
            catch (Exception ex)
            {
                // Si ocurre un error, deshace los cambios realizados.
                await transaction.RollbackAsync();

                return "Error al modificar cliente: " + ex.Message;
            }
        }

        // Método Eliminar.
        // Realiza eliminación lógica del cliente.
        // No borra físicamente los datos, solo cambia Activo a false.
        public async Task<string> Eliminar(int id)
        {
            // Busca el cliente por su IdCliente.
            var cliente = await _context.Set<Cliente>().FindAsync(id);

            if (cliente == null)
                return "No se encontró el cliente";

            // Marca el cliente como inactivo.
            cliente.Activo = false;

            // Busca todas las direcciones asociadas al cliente.
            var direcciones = await _context.Set<DirCliente>()
                .Where(d => d.IdCliente == id)
                .ToListAsync();

            // Marca las direcciones como inactivas.
            foreach (var item in direcciones)
            {
                item.Activo = false;
            }

            // Busca todos los teléfonos asociados al cliente.
            var telefonos = await _context.Set<TelCliente>()
                .Where(t => t.IdCliente == id)
                .ToListAsync();

            // Marca los teléfonos como inactivos.
            foreach (var item in telefonos)
            {
                item.Activo = false;
            }

            // Guarda los cambios en SQL Server.
            await _context.SaveChangesAsync();

            return "Cliente eliminado correctamente";
        }
    }
}