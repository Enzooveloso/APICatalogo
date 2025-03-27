using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTO
{
    public class RegisterModelDTO
    {
        [Required(ErrorMessage = "User name is required")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Pasword is required")]
        public string? Password { get; set; }
    }
}
