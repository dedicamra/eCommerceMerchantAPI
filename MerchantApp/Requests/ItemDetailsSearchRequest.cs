using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Requests
{
    public class ItemDetailsSearchRequest
    {
        [Required]
        public int BranchId { get; set; }
        public string SearchTerm { get; set; }
        public string ItemCode{ get; set; }
        public int ItemCategoryId { get; set; }

        public bool? Active { get; set; }
    }
}
