using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Data.EntityModels.ItemCategory, Models.ItemCategory>();
            CreateMap<Data.EntityModels.Brands, Models.Brands>();
            CreateMap<Data.EntityModels.City, Models.City>();
            CreateMap<Data.EntityModels.Branches, Models.Branches>()
                .ForMember(x => x.CityName, opt => opt.MapFrom(o => o.City.Name));
            CreateMap<Data.EntityModels.Gender, Models.Gender>();
            CreateMap<Data.EntityModels.Items, Models.Items>();
            CreateMap<Data.EntityModels.ItemBranch, Models.ItemBranch>()
                .ForMember(x => x.DateOfAdding, opt => opt.MapFrom(o => DateTime.Today)); 
            CreateMap<Data.EntityModels.Size, Models.Size>();
            CreateMap<Data.EntityModels.ItemDetails, Models.ItemDetails>()
                .ForMember(x=>x.SizeValue, opt=>opt.MapFrom(o=>o.Size.SizeValue))
                .ForMember(x=>x.ItemCode, opt=>opt.MapFrom(o=>o.ItemBranch.ItemCode));
            CreateMap<Data.EntityModels.Users, Models.Users>();
            CreateMap<Data.EntityModels.Roles, Models.Roles>();
            CreateMap<Data.EntityModels.Coupons, Models.Coupons>();
            CreateMap<Data.EntityModels.UsersMerchant, Models.UsersMerchant>();
            CreateMap<Data.EntityModels.UsersMerchant, Models.UserProfile>();


            CreateMap<Data.EntityModels.UsersMerchant, Models.UserProfile>().ReverseMap();
            CreateMap<Data.EntityModels.UsersMerchant, Requests.AdminInsertRequest>().ReverseMap();
            CreateMap<Data.EntityModels.UsersMerchant, Requests.MerchantInsertRequest>().ReverseMap();
            CreateMap<Data.EntityModels.UsersMerchant, Requests.UserUpdateRequest>().ReverseMap();
            CreateMap<Data.EntityModels.Coupons, Requests.CouponInsertRequest>().ReverseMap();
            CreateMap<Data.EntityModels.ItemDetails, Requests.ItemDetailsInsertRequest>().ReverseMap()
                .ForMember(x=>x.DateOfChanging, opt=>opt.MapFrom(o=>DateTime.Today));
            CreateMap<Data.EntityModels.Size, Requests.SizeInsertRequest>().ReverseMap();
            CreateMap<Data.EntityModels.ItemBranch, Requests.ItemBranchInsertRequest>().ReverseMap();
            CreateMap<Data.EntityModels.Items, Requests.ItemInsertRequest>().ReverseMap();
            CreateMap<Data.EntityModels.Items, Requests.ItemUpdateRequest>().ReverseMap();
            CreateMap<Data.EntityModels.Gender, Requests.GenderInsertRequest>().ReverseMap();
            CreateMap<Data.EntityModels.Branches, Requests.BranchInsertRequest>().ReverseMap();
            CreateMap<Data.EntityModels.City, Requests.CityInsertRequest>().ReverseMap();
            CreateMap<Data.EntityModels.Brands, Requests.BrandsInsertRequest>().ReverseMap();
            CreateMap<Data.EntityModels.ItemCategory, Requests.ItemCategoryInsertRequest>().ReverseMap();

        }
    }
}
