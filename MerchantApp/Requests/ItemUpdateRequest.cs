using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Requests
{
    public class ItemUpdateRequest
    {
        [Required]
        public int ItemCategoryId { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float StartPrice { get; set; }
        [Required]
        public float Discount { get; set; }
        [Required]
        public int GenderId { get; set; }

        public string ImageName { get; set; }
        
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
