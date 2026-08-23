using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class DetalleGuium
{
    public int IdDetalleGuia { get; set; }

    public int IdGuiaDespacho { get; set; }

    public int IdEntrega { get; set; }

    public int IdEstadoEntrega { get; set; }

    public DateTime FechaEstado { get; set; }

    public string? Observaciones { get; set; }

    public bool Activo { get; set; }

    public virtual Entrega IdEntregaNavigation { get; set; } = null!;

    public virtual EstadoEntrega IdEstadoEntregaNavigation { get; set; } = null!;

    public virtual GuiaDespacho IdGuiaDespachoNavigation { get; set; } = null!;
}
