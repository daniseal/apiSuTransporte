using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class Servicio
{
    public int IdServicio { get; set; }

    public int IdCliente { get; set; }

    public int IdEmpleadoRecibe { get; set; }

    public int IdDirClienteOrigen { get; set; }

    public int IdEstadoServicio { get; set; }

    public DateTime FechaServicio { get; set; }

    public decimal CostoServicio { get; set; }

    public string? Observaciones { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Entrega> Entregas { get; set; } = new List<Entrega>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual DirCliente IdDirClienteOrigenNavigation { get; set; } = null!;

    public virtual Empleado IdEmpleadoRecibeNavigation { get; set; } = null!;

    public virtual EstadoServicio IdEstadoServicioNavigation { get; set; } = null!;
}
