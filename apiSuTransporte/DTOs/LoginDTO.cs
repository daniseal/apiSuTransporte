using System.ComponentModel.DataAnnotations;

namespace apiSuTransporte.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Usuario { get; set; } = null!;

        [Required(ErrorMessage = "La clave es obligatoria")]
        public string Clave { get; set; } = null!;
    }
}