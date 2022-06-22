using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface IClient
    {
        bool IsInitialized { get; set; }
        bool CanSendSms { get; }
        bool FromNumberRequired { get; }
        //void Init();
        Task<IResponse> SendSmsAsync(string to, string msg);
        string ToString();
    }
}
