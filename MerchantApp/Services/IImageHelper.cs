using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface IImageHelper
    {
        void SetImageSource(Data.EntityModels.Items result);
        string SaveImage(IFormFile imageFile);
        void DeleteImage(string imageName);
    }
}
