using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class DirEmpleado
{
    public int IdDirEmpleado { get; set; }

    public int IdEmpleado { get; set; }

    public int IdCiudad { get; set; }

    public string Direccion { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual Ciudad IdCiudadNavigation { get; set; } = null!;

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;
}
