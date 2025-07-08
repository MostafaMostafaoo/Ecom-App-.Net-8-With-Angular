using Ecom.core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);
        Task<string> LogInAsync(LogInDTO login);
        Task<bool> SendEmailforForgetPassword(string email);
        Task<string> ResetPassword(RestPasswordDTO restPassword);

        Task<bool> ActiveAccount(ActiveAccountDTO accountDTO);
    }
}
