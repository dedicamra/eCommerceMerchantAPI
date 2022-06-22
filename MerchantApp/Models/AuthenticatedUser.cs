using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Models
{
    public class AuthenticatedUser
    {
        public string Token { get; set; }
        public string Username { get; set; }
    }
}
