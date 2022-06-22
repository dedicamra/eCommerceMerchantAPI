using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public bool Ordered { get; set; }
        public int CustomerId { get; set; }
        public Users Customer { get; set; }
    }
}
