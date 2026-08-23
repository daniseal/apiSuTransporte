using System;
using System.Collections.Generic;

namespace apiSuTransporte.Models;

public partial class Ciudad
{
    public int IdCiudad { get; set; }

    public int IdDepartamento { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<DirCamionero> DirCamioneros { get; set; } = new List<DirCamionero>();

    public virtual ICollection<DirCliente> DirClientes { get; set; } = new List<DirCliente>();

    public virtual ICollection<DirEmpleado> DirEmpleados { get; set; } = new List<DirEmpleado>();

    public virtual ICollection<Entrega> Entregas { get; set; } = new List<Entrega>();

    public virtual ICollection<GuiaDespacho> GuiaDespachos { get; set; } = new List<GuiaDespacho>();

    public virtual Departamento IdDepartamentoNavigation { get; set; } = null!;
}
