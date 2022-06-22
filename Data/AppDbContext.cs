using Data.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Data
{
        public class AppDbcontext : DbContext
        {
            public AppDbcontext([NotNullAttribute] DbContextOptions<AppDbcontext> options) : base(options)
            {

            }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Brands> Brands { get; set; }
        public DbSet<ItemCategory> ItemCategory { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<ItemBranch> ItemBranch { get; set; }
        public DbSet<ItemDetails> ItemDetails { get; set; }
        //public DbSet<Color> Color{ get;set; }
        public DbSet<City> City { get; set; }
        public DbSet<Branches> Branches { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<Coupons> Coupons { get; set; }
        public DbSet<SCDetails> SCDetails { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<UsersMerchant> UsersMerchants { get; set; }
        public DbSet<PaymentMethod> Paymentmethod { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
    }
}
