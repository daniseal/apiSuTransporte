using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class TelCliente
{
    public int IdTelCliente { get; set; }

    public int IdCliente { get; set; }

    public int IdTipoTelefono { get; set; }

    public string NumeroTelefono { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual TipoTelefono IdTipoTelefonoNavigation { get; set; } = null!;
}
