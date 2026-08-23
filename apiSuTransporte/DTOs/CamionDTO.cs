namespace apiSuTransporte.DTOs
{
    public class CamionDTO
    {
        public int IdCamion { get; set; }
        public int IdMarca { get; set; }
        public int IdSede { get; set; }
        public int IdTipoCamion { get; set; }

        public string Modelo { get; set; } = null!;
        public string NumeroMotor { get; set; } = null!;
        public string NumeroChasis { get; set; } = null!;
        public string Placa { get; set; } = null!;
        public string? Potencia { get; set; }
        public string Estado { get; set; } = null!;
        public bool Activo { get; set; } = true;
    }
}