using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Requests
{
    public class UserUpdateRequest
    {
        [Required]
        [RegularExpression(@"[A-Za-z0-9_]+", ErrorMessage = "Username may only contain letters, numbers and underscores.")]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
