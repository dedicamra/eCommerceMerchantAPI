using AutoMapper;
using Data;
using MerchantApp.Exceptions;
using MerchantApp.Models;
using MerchantApp.Requests;
using MerchantApp.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MerchantApp.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbcontext _db;
        private readonly IMapper _mapper;
        private readonly IExistsInDatabaseService _existsService;
        private readonly IEmailService _emailService;
        private readonly IClient _smsclientService;
        private static Random random = new Random();

        //private readonly Data.EntityModels.UsersMerchant _currentUser;

        public UserService(AppDbcontext db, IMapper mapper, IExistsInDatabaseService existsService, IEmailService emailService, IClient smsclientService)
        {
            _db = db;
            _mapper = mapper;
            _existsService = existsService;
            _emailService = emailService;
            _smsclientService = smsclientService;

            //var username = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            //_currentUser = _db.UsersMerchants.Where(x => x.Username == username).Include(x => x.Role).Include(x => x.Branch).FirstOrDefault();
        }

        public AuthenticatedUser SignUpAdmin(AdminInsertRequest request)
        {
            var checkUser = _db.UsersMerchants.Any(u => u.Username.Equals(request.Username));

            if (CheckUsernameExists(request.Username))
            {
                throw new CustomException("Username is already taken.");
            }
            if (CheckEmailExists(request.Email))
            {
                throw new CustomException("Email is already taken.");
            }
            if (CheckPhoneNumberExists(request.PhoneNumber))
            {
                throw new CustomException("Phone number is already taken.");
            }
            if (request.Password != request.PasswordConfirmation)
            {
                throw new CustomException("Passwords do not match");
            }

            var user = _mapper.Map<Data.EntityModels.UsersMerchant>(request);
            user.PasswordSalt = GenerateSalt();
            user.PasswordHash = GenerateHash(user.PasswordSalt, request.Password);
            user.RoleId = _db.Roles.Where(x => x.Name == "Administrator").FirstOrDefault().Id;
            user.DateRegistered = DateTime.Now;
            user.Active = true;

            _db.UsersMerchants.Add(user);
            _db.SaveChanges();

            return new AuthenticatedUser
            {
                Username = request.Username,
                Token = JWTGenerator.GenerateuserToken(user.Username, user.Role.Name)
            };
        }

        public AuthenticatedUser SignUpMerchant(MerchantInsertRequest request)
        {
            var checkUser = _db.UsersMerchants.Any(u => u.Username.Equals(request.Username));

            if (CheckUsernameExists(request.Username))
            {
                throw new CustomException("Username is already taken.");
            }
            if (CheckEmailExists(request.Email))
            {
                throw new CustomException("Email is already taken.");
            }
            if (CheckPhoneNumberExists(request.PhoneNumber))
            {
                throw new CustomException("Phone number is already taken.");
            }
            if (request.Password != request.PasswordConfirmation)
            {
                throw new CustomException("Passwords do not match");
            }
            if (!_existsService.BranchExists(request.BranchId))
                throw new CustomException("Branch does not exist.");

            var user = _mapper.Map<Data.EntityModels.UsersMerchant>(request);
            user.PasswordSalt = GenerateSalt();
            user.PasswordHash = GenerateHash(user.PasswordSalt, request.Password);
            user.RoleId = _db.Roles.Where(x => x.Name == "Merchant").FirstOrDefault().Id;
            user.DateRegistered = DateTime.Now;
            user.Active = true;

            _db.UsersMerchants.Add(user);
            _db.SaveChanges();



            return new AuthenticatedUser
            {
                Username = request.Username,
                Token = JWTGenerator.GenerateuserToken(user.Username, user.Role.Name)
            };
        }


        public AuthenticatedUser SignIn(SignInRequest request)
        {
            var user = _db.UsersMerchants.Where(x => x.Username == request.Username).Include(x => x.Role).FirstOrDefault();

            if (user == null)
                throw new CustomException("Username of password not valid.");

            var newHash = GenerateHash(user.PasswordSalt, request.Password);
            if (newHash != user.PasswordHash)
                throw new CustomException("Username of password not valid.");

            user.LastLogin = DateTime.Now;

            _db.UsersMerchants.Attach(user);
            _db.UsersMerchants.Update(user);
            _db.SaveChanges();


            return new AuthenticatedUser
            {
                Username = user.Username,
                Token = JWTGenerator.GenerateuserToken(user.Username, user.Role.Name)
            };
        }


        public void Delete(int id)
        {
            var entity = _db.UsersMerchants.Where(x => x.Id == id).FirstOrDefault();
            if (entity != null)
            {
                entity.Active = false;
                _db.UsersMerchants.Attach(entity);
                _db.UsersMerchants.Update(entity);
                _db.SaveChanges();

            }
            else
                throw new CustomException("User not found.");
        }



        public UserProfile MyProfile(int id)
        {
            var entity = _db.UsersMerchants.Where(x => x.Id == id).Include(x => x.Role).Include(x => x.Branch).FirstOrDefault();
            return _mapper.Map<UserProfile>(entity);
        }
        public UserProfile MyProfile(string username)
        {

            var entity = _db.UsersMerchants.Where(x => x.Username.Equals(username)).Include(x => x.Role).Include(x => x.Branch).FirstOrDefault();
            return _mapper.Map<UserProfile>(entity);
        }


        public void ForgotPasswordMail(ForgotPasswordRequest request, string origin)
        {
            var user = _db.UsersMerchants.Where(x => x.Email == request.Email).Include(x => x.Role).FirstOrDefault();
            if (user == null)
                return;
            //throw new CustomException("User not found.");

            //generate reset token
            user.ResetToken = RandomString(8);
            user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(5);
            user.DateUpdated = DateTime.UtcNow;

            _db.UsersMerchants.Attach(user);
            _db.UsersMerchants.Update(user);
            _db.SaveChanges();

            //send email
            SendPasswordResetEmail(user, origin);
        }

        public void ForgotPasswordPhoneNumber(ForgotPasswordPhoneNumberRequest request)
        {
            var user = _db.UsersMerchants.Where(x => x.PhoneNumber == request.PhoneNumber).Include(x => x.Role).FirstOrDefault();
            if (user == null)
                return;
            //throw new CustomException("User not found.");

            //generate reset token
            user.ResetToken = RandomString(8);
            user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(5);
            user.DateUpdated = DateTime.UtcNow;

            _db.UsersMerchants.Attach(user);
            _db.UsersMerchants.Update(user);
            _db.SaveChanges();

            //send email
            sendPasswordResetMessage(user);
        }

        private async void sendPasswordResetMessage(Data.EntityModels.UsersMerchant user)
        {
            //var from = "+19703674418";
            var to = $"+{user.PhoneNumber}";
            var message = $"Please use code bellow to reset your password.  The code will be valid for the next 5 minuts. CODE:{user.ResetToken}";
            await _smsclientService.SendSmsAsync(to, message);
            //throw new NotImplementedException();
        }
        private async void SendPasswordResetEmail(Data.EntityModels.UsersMerchant user, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                //var resetUrl = $"{origin}/account/reset-password?token={user.ResetToken}";
                //message = $@"<p>Please click the link below to reset your password, the link will be valid for 5 minuts:</p>
                //             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
                message = $@"<p>Please use code below to reset your password. The code will be valid for the next 5 minuts.</p>
                             <p>Code: {user.ResetToken}</p>";
            }
            else
            {
                message = $@"<p>Please use code below to reset your password: The code will be valid for the next 5 minuts.</p>
                             <p>Code: {user.ResetToken}</p>";
            }


            await _emailService.SendEmailAsync(
                 email: user.Email,
                 subject: "Reset password",
                 body: $@"<h4>Reset password email</h4>
                         {message}"
             );


        }

        public void ResetPassword(ResetPasswordRequest request)
        {
            var user = _db.UsersMerchants.Where(x => x.ResetToken == request.Code && x.ResetTokenExpires > DateTime.UtcNow).FirstOrDefault();
            if (user == null)
                throw new CustomException("Invalid code");

            if (request.Password != request.PasswordConfirmation)
            {
                throw new CustomException("Passwords do not match");
            }

            user.PasswordSalt = GenerateSalt();
            user.PasswordHash = GenerateHash(user.PasswordSalt, request.Password);
            user.ResetToken = null;
            user.ResetTokenExpires = null;

            _db.UsersMerchants.Update(user);
            _db.SaveChanges();


        }


        public static string GenerateSalt()
        {
            var buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }
        public static string GenerateHash(string salt, string password)
        {
            byte[] src = Convert.FromBase64String(salt);
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] dst = new byte[src.Length + bytes.Length];

            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuwxyz$#!%-ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }



        public bool CheckUsernameExists(string username)
        {
            return _db.UsersMerchants.Any(x => x.Username == username);
        }
        public bool CheckEmailExists(string email)
        {
            return _db.UsersMerchants.Any(x => x.Email == email);
        }
        private bool CheckPhoneNumberExists(string phoneNumber)
        {
            return _db.UsersMerchants.Any(x => x.PhoneNumber == phoneNumber);
        }

    }
}
