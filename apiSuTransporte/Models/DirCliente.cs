using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class DirCliente
{
    public int IdDirCliente { get; set; }

    public int IdCliente { get; set; }

    public int IdCiudad { get; set; }

    public string Direccion { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual Ciudad IdCiudadNavigation { get; set; } = null!;

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
