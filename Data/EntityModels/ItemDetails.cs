using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class ItemDetails
    {
        public int Id { get; set; }

        public int? ItemBranchId { get; set; }
        public ItemBranch ItemBranch { get; set; }
     
        public int? SizeId { get; set; }
        public Size Size { get; set; }
      
        public int Quantity { get; set; }

        public DateTime DateOfChanging { get; set; }

        public bool Active { get; set; }

        //public int? ColorId { get; set; }
        //public Color Color { get; set; }
    }
}
