namespace apiSuTransporte.DTOs
{
    public class CatalogoCiudadDTO
    {
        public int IdCiudad { get; set; }
        public int IdDepartamento { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Activo { get; set; } = true;
    }
}