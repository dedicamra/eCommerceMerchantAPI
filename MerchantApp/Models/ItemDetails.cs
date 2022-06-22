using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Models
{
    public class ItemDetails
    {
        public int Id { get; set; }
        public int ItemBranchId { get; set; }
        public ItemBranch ItemBranch { get; set; }
        public string ItemCode { get; set; }


        public int SizeId { get; set; }
        public Size Size { get; set; }
        public string SizeValue { get; set; }

        public int Quantity { get; set; }

        public DateTime DateOfChanging { get; set; }

        public bool Active { get; set; }

    }
}
