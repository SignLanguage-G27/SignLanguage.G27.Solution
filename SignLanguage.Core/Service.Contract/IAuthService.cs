using Microsoft.AspNetCore.Identity;
using SignLanguage.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignLanguage.Core.Service.Contract
{
    public interface IAuthService
    {
        Task<string>CreateTokenAsync(AppUser user,UserManager<AppUser> userManager);

        Task<ApiResponse> ForgetPasswordByEmailAsync(string email);

        Task<ApiResponse> ForgetPasswordByTelegramAsync(string phoneNumber);

        Task<dynamic> ResetPasswordAsync(string identifier, string newPassword, bool isEmail);

        Task<dynamic> VerifyResetCodeAsync(string identifier, string resetCode, bool isEmail);
    }
}
