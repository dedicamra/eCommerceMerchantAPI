using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Requests
{
    public class BranchInsertRequest
    {
        
        [Required]
        public string Name { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public string Adress { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
