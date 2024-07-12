using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SeguimientoPagos.Models
{
    // Atributo de validación personalizada para verificar la unicidad del folio
    public class UniqueFolioAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (PagosUsuarioContext)validationContext.GetService(typeof(PagosUsuarioContext));
            var entity = context.Pagos.SingleOrDefault(e => e.Folio == (int?)value);

            if (entity != null)
            {
                return new ValidationResult("El folio ya existe.");
            }

            return ValidationResult.Success;
        }
    }

    public partial class Pago
    {
        public int Id { get; set; }

        // Propiedad Folio con validaciones
        [Required(ErrorMessage = "El folio es obligatorio.")]
        [UniqueFolio(ErrorMessage = "El folio ya existe.")]
        public int Folio { get; set; }

        // Propiedad Descripcion con validación
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public required string Descripcion { get; set; }

        // Propiedad Fecha con validación
        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        // Propiedad Cantidad con validación
        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        public int Cantidad { get; set; }
    }
}
