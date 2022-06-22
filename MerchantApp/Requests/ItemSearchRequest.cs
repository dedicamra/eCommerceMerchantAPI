using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Requests
{
    public class ItemSearchRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ItemCategoryId { get; set; }
        public int? BrandId { get; set; }
        public int? GenderId { get; set; }
        public bool? Active { get; set; }
    }
}
