namespace apiSuTransporte.DTOs
{
    public class GuiaDespachoDTO
    {
        public int IdGuiaDespacho { get; set; }
        public DateTime FechaDistribucion { get; set; }
        public int IdCamionero { get; set; }
        public int IdCamion { get; set; }
        public int IdEmpleadoDespacha { get; set; }
        public int? IdEmpleadoRecibe { get; set; }
        public int IdEstadoGuia { get; set; }
        public int IdCiudadDestino { get; set; }
        public int CantidadEntregas { get; set; }
        public decimal CostoTotal { get; set; }
        public decimal ValorTotal { get; set; }
        public string? Observaciones { get; set; }
        public bool Activo { get; set; } = true;

        public List<DetalleGuiaDTO> Detalles { get; set; } = new List<DetalleGuiaDTO>();
    }
}