using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class EstadoGuium
{
    public int IdEstadoGuia { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<GuiaDespacho> GuiaDespachos { get; set; } = new List<GuiaDespacho>();
}
