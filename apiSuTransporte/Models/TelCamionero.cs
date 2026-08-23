using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class TelCamionero
{
    public int IdTelCamionero { get; set; }

    public int IdCamionero { get; set; }

    public int IdTipoTelefono { get; set; }

    public string NumeroTelefono { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual Camionero IdCamioneroNavigation { get; set; } = null!;

    public virtual TipoTelefono IdTipoTelefonoNavigation { get; set; } = null!;
}
