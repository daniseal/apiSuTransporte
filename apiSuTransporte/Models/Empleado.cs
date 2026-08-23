using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public int IdTipoDocumento { get; set; }

    public string NumeroDocumento { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Cargo { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<DirEmpleado> DirEmpleados { get; set; } = new List<DirEmpleado>();

    public virtual ICollection<GuiaDespacho> GuiaDespachoIdEmpleadoDespachaNavigations { get; set; } = new List<GuiaDespacho>();

    public virtual ICollection<GuiaDespacho> GuiaDespachoIdEmpleadoRecibeNavigations { get; set; } = new List<GuiaDespacho>();

    public virtual TipoDocumento IdTipoDocumentoNavigation { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();

    public virtual ICollection<TelEmpleado> TelEmpleados { get; set; } = new List<TelEmpleado>();

    public virtual UsuarioSistema? UsuarioSistema { get; set; }
}
