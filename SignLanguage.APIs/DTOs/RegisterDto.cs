﻿using System.ComponentModel.DataAnnotations;

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
        [RegularExpression(@"^(?=.* [A-Za-z])(?=.*\d)(?=.* [@$!%*? &])[A-Za-z\d@$!%*? &]{8,}$")]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"^(?=.* [A-Za-z])(?=.*\d)(?=.* [@$!%*? &])[A-Za-z\d@$!%*? &]{8,}$")]
        public string RePassword { get; set; }
    }
}
