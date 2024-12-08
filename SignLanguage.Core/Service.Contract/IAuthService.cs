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
    }
}
