using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MerchantApp.Requests
{
    public class ItemBranchInsertRequest
    {
        [Required]
        public int BranchId { get; set; }
        [Required]
        public int ItemId { get; set; }

        //public string ItemCode { get; set; }

        //[NotMapped]
        //public IFormFile ImageFile{ get; set; }
    }
}