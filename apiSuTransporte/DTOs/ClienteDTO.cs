using System.ComponentModel.DataAnnotations;

namespace apiSuTransporte.DTOs
{
    public class ClienteDTO
    {
        public int IdCliente { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de documento")]
        public int IdTipoDocumento { get; set; }

        [Required(ErrorMessage = "El número de documento es obligatorio")]
        public string NumeroDocumento { get; set; } = null!;

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        public string NombreCompleto { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una ciudad")]
        public int IdCiudad { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de teléfono")]
        public int IdTipoTelefono { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio")]
        public string NumeroTelefono { get; set; } = null!;

        public bool Activo { get; set; } = true;
    }
}