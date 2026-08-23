using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public int IdTipoDocumento { get; set; }

    public string NumeroDocumento { get; set; } = null!;

    public string NombreCompleto { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<DirCliente> DirClientes { get; set; } = new List<DirCliente>();

    public virtual TipoDocumento IdTipoDocumentoNavigation { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();

    public virtual ICollection<TelCliente> TelClientes { get; set; } = new List<TelCliente>();
}
