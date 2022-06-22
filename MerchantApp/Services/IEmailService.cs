using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html, string from = null);
        Task SendEmailAsync(string email, string subject, string body);
    }
}
