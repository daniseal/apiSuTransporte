namespace apiSuTransporte.DTOs
{
    public class EmpleadoDTO
    {
        public int IdEmpleado { get; set; }
        public int IdTipoDocumento { get; set; }
        public string NumeroDocumento { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Cargo { get; set; } = null!;

        public int IdCiudad { get; set; }
        public string Direccion { get; set; } = null!;

        public int IdTipoTelefono { get; set; }
        public string NumeroTelefono { get; set; } = null!;

        public bool Activo { get; set; } = true;
    }
}