using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Orders
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Users Customer { get; set; }
        public int? CouponId { get; set; }
        public Coupons Coupon { get; set; }
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public string ShippingAddress { get; set; }
        public double totalPrice { get; set; }
        public double finalPrice { get; set; }
        public DateTime orderDate { get; set; }
        public bool Completed { get; set; }
    }
}
