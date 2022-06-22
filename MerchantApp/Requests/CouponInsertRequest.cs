using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Requests
{
    public class CouponInsertRequest
    {

        //public int UserId { get; set; }

        public string Code { get; set; }
        public string Details { get; set; }
        public float Discount { get; set; }
        //public DateTime CreatedOn { get; set; }
        public DateTime ValidUntil { get; set; }

        public bool Active { get; set; }
    }
}
