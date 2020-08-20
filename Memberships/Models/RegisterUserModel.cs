using System.ComponentModel.DataAnnotations;

namespace Memberships.Models
{
    public class RegisterUserModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "The name cannot be more than 30 characters long", MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string Password { get; set; }
        [Required]
        public bool AcceptUserAgreement { get; set; }

    }
}