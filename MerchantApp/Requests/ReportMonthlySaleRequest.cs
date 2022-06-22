using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Requests
{
    public class ReportMonthlySaleRequest
    {
        [Required]
        public int BranchId { get; set; }
       // //[Required]
       // public int Month { get; set; }
       //// [Required]
       // public int Year { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
    }
}
