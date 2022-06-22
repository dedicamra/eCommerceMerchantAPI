using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class UsersMerchant
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public Roles Role { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateRegistered { get; set; }
        public DateTime LastLogin { get; set; }
        public int? BranchId { get; set; }
        public Branches Branch { get; set; }
        public bool Active { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
