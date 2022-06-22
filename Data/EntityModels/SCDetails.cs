using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class SCDetails
    {
        public int Id { get; set; }
        public int quantity { get; set; }
        public int itemDetailsId { get; set; }
        public float ItemPrice { get; set; }
        public float TotalPrice { get; set; }
        public ItemDetails ItemDetails { get; set; }
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
