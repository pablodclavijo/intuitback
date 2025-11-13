using IntuitBack.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace IntuitBack.IntuitBack.Application.DTOs.Cliente
{
    public class CreateClienteDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Nombres { get; set; } = "";

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(100, ErrorMessage = "El apellido no puede tener más de 100 caracteres.")]
        public string Apellidos { get; set; } = "";

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(CreateClienteDto), nameof(ValidarFechaNacimiento))]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El CUIT es obligatorio.")]
        [RegularExpression(@"^\d{2}-\d{8}-\d{1}$", ErrorMessage = "El CUIT debe tener el formato NN-NNNNNNNN-N.")]
        public string Cuit { get; set; } = "";

        [StringLength(150)]
        public string Domicilio { get; set; } = "";

        [Required(ErrorMessage = "El teléfono celular es obligatorio.")]
        [Phone(ErrorMessage = "El teléfono celular no tiene un formato válido.")]
        public string TelefonoCelular { get; set; } = "";

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        [StringLength(150)]
        public string Email { get; set; } = "";

        public static ValidationResult? ValidarFechaNacimiento(DateTime fecha, ValidationContext context)
        {
            if (fecha > DateTime.Today)
                return new ValidationResult("La fecha de nacimiento no puede ser futura.");
            return ValidationResult.Success;
        }
    }
}
