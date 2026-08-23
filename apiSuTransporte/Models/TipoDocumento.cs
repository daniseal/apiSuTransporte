using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class TipoDocumento
{
    public int IdTipoDocumento { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<Camionero> Camioneros { get; set; } = new List<Camionero>();

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
