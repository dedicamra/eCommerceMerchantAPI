using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Models
{
    public class Items
    {
        public int Id { get; set; }

        public int ItemCategoryId { get; set; }
        public ItemCategory ItemCategory { get; set; }
        public string CategoryName { get; set; }

        public int BrandId { get; set; }
        public Brands Brand { get; set; }
        public string BrandName { get; set; }

        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public string GenderName { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public float StartPrice { get; set; }
        public int Discount { get; set; }
        public byte[] ImageByteArray { get; set; }
        public string ImageName { get; set; }

        public bool Active { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public string  ImageSource { get; set; }

    }
}
