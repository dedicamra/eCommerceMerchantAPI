using MerchantApp.Models;
using MerchantApp.Requests;


namespace MerchantApp.Services
{
    public interface IUserService
    {
        AuthenticatedUser SignUpAdmin(AdminInsertRequest request);
        AuthenticatedUser SignUpMerchant(MerchantInsertRequest request);

        AuthenticatedUser SignIn(SignInRequest request);
       

        void Delete(int id);
        void ForgotPasswordMail(ForgotPasswordRequest request, string origin);
        void ForgotPasswordPhoneNumber(ForgotPasswordPhoneNumberRequest request);
        void ResetPassword(ResetPasswordRequest request);

        UserProfile MyProfile(int id);
        UserProfile MyProfile(string username);
        //Models.UsersMerchant GetCurrentUser();
    }
}
