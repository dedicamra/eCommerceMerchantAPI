using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Models
{
    public class ItemBranch
    {
        public int Id { get; set; }

        public int BranchId { get; set; }
        public Branches Branches { get; set; }

        public string BranchName { get; set; }

        public int ItemId { get; set; }
        public Items Item { get; set; }

        public string ItemCode { get; set; }

        public DateTime DateOfAdding { get; set; }

        public bool Active { get; set; }

    }
}
