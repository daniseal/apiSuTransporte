using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class Marca
{
    public int IdMarca { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Camion> Camions { get; set; } = new List<Camion>();
}
