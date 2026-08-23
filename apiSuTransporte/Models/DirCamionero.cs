using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class DirCamionero
{
    public int IdDirCamionero { get; set; }

    public int IdCamionero { get; set; }

    public int IdCiudad { get; set; }

    public string Direccion { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual Camionero IdCamioneroNavigation { get; set; } = null!;

    public virtual Ciudad IdCiudadNavigation { get; set; } = null!;
}
