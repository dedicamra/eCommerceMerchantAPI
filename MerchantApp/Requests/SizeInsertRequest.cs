using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Requests
{
    public class SizeInsertRequest
    {
        [Required]
        public string SizeValue { get; set; }
    }
}
