using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTO
{
    public class ProductDTOUpdateRequest : IValidatableObject
    {
        [Range(1, 9999, ErrorMessage = "Estoque deve estar entre 1 e 9999")]
        public float Stock { get; set; }
        public DateTime DateRegistration { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(DateRegistration.Date <= DateTime.Now) 
            {
                yield return new ValidationResult("A data deve ser maior que a data atual",
                    new[] { nameof(this.DateRegistration) });
            }
        }
    }
}
