using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Requests
{
    public class ItemBranchSearchRequest
    {
        public int BranchId { get; set; }
        public string SearchByItemCode { get; set; }
        public DateTime? FirstDate { get; set; }
        public DateTime? SecondDate { get; set; }
        public int ItemCategoryId { get; set; }
        public int GenderId { get; set; }
        public int BrandId { get; set; }

    }
}
