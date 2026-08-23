namespace apiSuTransporte.DTOs
{
    public class CatalogoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Activo { get; set; } = true;
    }
}