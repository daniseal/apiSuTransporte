using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class TelEmpleado
{
    public int IdTelEmpleado { get; set; }

    public int IdEmpleado { get; set; }

    public int IdTipoTelefono { get; set; }

    public string NumeroTelefono { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual TipoTelefono IdTipoTelefonoNavigation { get; set; } = null!;
}
