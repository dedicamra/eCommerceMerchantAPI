using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Coupons
    {
        public int Id { get; set; }

        public int UsersMerchantId { get; set; }
        public UsersMerchant UserMerchant { get; set; }

        public string Code { get; set; }
        public string Details { get; set; }
        public float Discount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ValidUntil { get; set; }

        public bool Active { get; set; }
    }
}
