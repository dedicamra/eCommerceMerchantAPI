using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class ItemBranch
    {
        public int Id { get; set; }

        public int? BranchId { get; set; }
        public Branches Branch { get; set; }
        public string BranchName { get; set; }

        public int? ItemId { get; set; }
        public Items Item { get; set; }

        public string ItemCode { get; set; }

        public DateTime DateOfAdding { get; set; }

        public bool Active { get; set; } 

    }
}
