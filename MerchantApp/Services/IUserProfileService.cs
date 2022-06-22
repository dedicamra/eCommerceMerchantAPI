using MerchantApp.Models;
using MerchantApp.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface IUserProfileService
    {
        UserProfile EditProfile(UserUpdateRequest request);
    }
}
