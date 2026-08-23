namespace apiSuTransporte.DTOs
{
    public class DetalleGuiaDTO
    {
        public int IdDetalleGuia { get; set; }
        public int IdEntrega { get; set; }
        public int IdEstadoEntrega { get; set; }
        public DateTime FechaEstado { get; set; }
        public string? Observaciones { get; set; }
    }
}