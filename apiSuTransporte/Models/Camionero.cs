using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class Camionero
{
    public int IdCamionero { get; set; }

    public int IdTipoDocumento { get; set; }

    public string NumeroDocumento { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public decimal Salario { get; set; }

    public string CategoriaPase { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<DirCamionero> DirCamioneros { get; set; } = new List<DirCamionero>();

    public virtual ICollection<GuiaDespacho> GuiaDespachos { get; set; } = new List<GuiaDespacho>();

    public virtual TipoDocumento IdTipoDocumentoNavigation { get; set; } = null!;

    public virtual ICollection<TelCamionero> TelCamioneros { get; set; } = new List<TelCamionero>();
}
