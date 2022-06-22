using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Requests
{
    public class ItemDetailsInsertRequest
    {
        [Required]
        public int ItemBranchId { get; set; }
       
        public int SizeId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
