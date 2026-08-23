using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class EstadoServicio
{
    public int IdEstadoServicio { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
