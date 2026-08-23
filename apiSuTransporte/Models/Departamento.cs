using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class Departamento
{
    public int IdDepartamento { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<Ciudad> Ciudads { get; set; } = new List<Ciudad>();
}
