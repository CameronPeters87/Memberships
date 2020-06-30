using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Memberships.Models
{
    public class UserViewModel
    {
        [DisplayName("User Id")]
        public int Id { get; set; }
        [Required]
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("First Name")]
        [StringLength(30, ErrorMessage = "The {0} must be atleast {1} long", MinimumLength = 2)]
        public string FirstName { get; set; }
        [DisplayName("Password")]
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}