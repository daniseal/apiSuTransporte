using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class EstadoEntrega
{
    public int IdEstadoEntrega { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<DetalleGuium> DetalleGuia { get; set; } = new List<DetalleGuium>();
}
