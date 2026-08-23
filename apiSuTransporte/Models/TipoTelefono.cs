using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class TipoTelefono
{
    public int IdTipoTelefono { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<TelCamionero> TelCamioneros { get; set; } = new List<TelCamionero>();

    public virtual ICollection<TelCliente> TelClientes { get; set; } = new List<TelCliente>();

    public virtual ICollection<TelEmpleado> TelEmpleados { get; set; } = new List<TelEmpleado>();
}
