using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Models
{
    public class Coupons
    {
        public int Id { get; set; }

        public int UsersMerchantId { get; set; }
        //public UsersMerchant UserMerchant { get; set; }

        public string Code { get; set; }
        public string Details { get; set; }
        public float Discount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ValidUntil { get; set; }

        public bool Active { get; set; }
    }
}
