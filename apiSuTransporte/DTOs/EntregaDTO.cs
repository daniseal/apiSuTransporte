namespace apiSuTransporte.DTOs
{
    public class EntregaDTO
    {
        public int IdEntrega { get; set; }
        public int IdServicio { get; set; }
        public int IdTipoProducto { get; set; }
        public int IdCiudadDestino { get; set; }

        public string NombreDestinatario { get; set; } = null!;
        public string? ApellidoDestinatario { get; set; }
        public string TelefonoDestinatario { get; set; } = null!;
        public string DireccionDestino { get; set; } = null!;

        public string Contenido { get; set; } = null!;
        public int Cantidad { get; set; }
        public string? Dimensiones { get; set; }
        public decimal PesoKg { get; set; }
        public decimal Costo { get; set; }
        public decimal ValorDeclarado { get; set; }

        public string? Observaciones { get; set; }
        public bool Activo { get; set; } = true;
    }
}