using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public string CardNumber { get; set; }
        public string FullName{ get; set; }
        public string CVV { get; set; }
    }
}
