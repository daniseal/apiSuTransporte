using apiSuTransporte.DTOs;
using apiSuTransporte.Models;
using Microsoft.EntityFrameworkCore;

namespace apiSuTransporte.Clases
{
    // Clase operativa encargada de manejar la lógica del inicio de sesión.
    // Esta clase valida el usuario y la clave contra la tabla UsuarioSistema.
    public class ClLogin
    {
        // Contexto de Entity Framework.
        // Permite acceder a las tablas de la base de datos bdSuTransporte.
        private readonly BdSuTransporteContext _context;

        // Constructor de la clase.
        // Recibe el contexto de la base de datos desde el controlador LoginController.
        public ClLogin(BdSuTransporteContext context)
        {
            _context = context;
        }

        // Método Validar.
        // Sirve para verificar si el usuario y la clave enviados desde el formulario login
        // existen en la tabla UsuarioSistema y si el usuario está activo.
        public async Task<object> Validar(LoginDTO login)
        {
            // Busca en la tabla UsuarioSistema un registro que coincida con:
            // usuario, clave y estado activo.
            var usuario = await _context.Set<UsuarioSistema>()
                .FirstOrDefaultAsync(u =>
                    u.Usuario == login.Usuario &&
                    u.Clave == login.Clave &&
                    u.Estado == true);

            // Si no encuentra el usuario, devuelve un objeto indicando que el ingreso falló.
            if (usuario == null)
            {
                return new
                {
                    correcto = false,
                    mensaje = "Usuario o clave incorrectos"
                };
            }

            // Si el usuario existe, busca el empleado asociado a ese usuario.
            // Esto sirve para mostrar el nombre del empleado logueado en el sistema.
            var empleado = await _context.Set<Empleado>()
                .FirstOrDefaultAsync(e => e.IdEmpleado == usuario.IdEmpleado);

            // Devuelve la información del usuario validado.
            // Estos datos se usan en el frontend para crear variables de sesión.
            return new
            {
                correcto = true,
                mensaje = "Ingreso correcto",
                idUsuario = usuario.IdUsuario,
                idEmpleado = usuario.IdEmpleado,
                usuario = usuario.Usuario,

                // Si encuentra el empleado, devuelve nombres y apellidos.
                // Si no lo encuentra, devuelve el nombre de usuario.
                nombreEmpleado = empleado != null
                    ? empleado.Nombres + " " + empleado.Apellidos
                    : usuario.Usuario
            };
        }
    }
}