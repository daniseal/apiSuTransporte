using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class Sede
{
    public int IdSede { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<Camion> Camions { get; set; } = new List<Camion>();
}
