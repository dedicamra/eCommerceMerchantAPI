using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Transactions
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Users Customer { get; set; }
        public int OrderId { get; set; }
        public Orders Orders { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

    }
}
