using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class Camion
{
    public int IdCamion { get; set; }

    public int IdMarca { get; set; }

    public int IdSede { get; set; }

    public string Modelo { get; set; } = null!;

    public string NumeroMotor { get; set; } = null!;

    public string NumeroChasis { get; set; } = null!;

    public string Placa { get; set; } = null!;

    public string? Potencia { get; set; }

    public string Estado { get; set; } = null!;

    public bool Activo { get; set; }

    public int IdTipoCamion { get; set; }

    public virtual ICollection<GuiaDespacho> GuiaDespachos { get; set; } = new List<GuiaDespacho>();

    public virtual Marca IdMarcaNavigation { get; set; } = null!;

    public virtual Sede IdSedeNavigation { get; set; } = null!;

    public virtual TipoCamion IdTipoCamionNavigation { get; set; } = null!;
}
