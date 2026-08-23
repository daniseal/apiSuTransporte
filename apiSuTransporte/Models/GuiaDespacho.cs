using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class GuiaDespacho
{
    public int IdGuiaDespacho { get; set; }

    public DateTime FechaDistribucion { get; set; }

    public int IdCamionero { get; set; }

    public int IdCamion { get; set; }

    public int IdEmpleadoDespacha { get; set; }

    public int? IdEmpleadoRecibe { get; set; }

    public int IdEstadoGuia { get; set; }

    public int CantidadEntregas { get; set; }

    public decimal CostoTotal { get; set; }

    public decimal ValorTotal { get; set; }

    public string? Observaciones { get; set; }

    public bool Activo { get; set; }

    public int IdCiudadDestino { get; set; }

    public virtual ICollection<DetalleGuium> DetalleGuia { get; set; } = new List<DetalleGuium>();

    public virtual Camion IdCamionNavigation { get; set; } = null!;

    public virtual Camionero IdCamioneroNavigation { get; set; } = null!;

    public virtual Ciudad IdCiudadDestinoNavigation { get; set; } = null!;

    public virtual Empleado IdEmpleadoDespachaNavigation { get; set; } = null!;

    public virtual Empleado? IdEmpleadoRecibeNavigation { get; set; }

    public virtual EstadoGuium IdEstadoGuiaNavigation { get; set; } = null!;
}
