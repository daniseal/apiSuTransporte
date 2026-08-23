using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class UsuarioSistema
{
    public int IdUsuario { get; set; }

    public int IdEmpleado { get; set; }

    public string Usuario { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;
}
