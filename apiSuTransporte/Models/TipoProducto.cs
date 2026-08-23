using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class TipoProducto
{
    public int IdTipoProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<Entrega> Entregas { get; set; } = new List<Entrega>();
}
