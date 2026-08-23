namespace apiSuTransporte.DTOs
{
    public class CamioneroDTO
    {
        public int IdCamionero { get; set; }
        public int IdTipoDocumento { get; set; }
        public string NumeroDocumento { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public decimal Salario { get; set; }
        public string CategoriaPase { get; set; } = null!;

        public int IdCiudad { get; set; }
        public string Direccion { get; set; } = null!;

        public int IdTipoTelefono { get; set; }
        public string NumeroTelefono { get; set; } = null!;

        public bool Activo { get; set; } = true;
    }
}