using System.ComponentModel.DataAnnotations;

namespace SignLanguage.APIs.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z][A-Za-z\d@$!%?&]{5,}$", ErrorMessage ="Invalid Pasword")]
        public string Password { get; set; }
    }
}
