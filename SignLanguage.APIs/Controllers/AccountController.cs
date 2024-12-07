using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignLanguage.APIs.DTOs;
using SignLanguage.Core.Entities.Identity;

namespace SignLanguage.APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager) 
        {
            _userManager=userManager;
            _signInManager=signInManager;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user==null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded) return Unauthorized();

            return Ok(new UserDto()
            {
                DisplayName=user.DisplayName,
                Email=user.Email,
                Token="Soooooooon !"
            });
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new AppUser()
            {
                DisplayName=model.DisplayName,
                Email=model.Email,
                UserName=model.Email.Split("@")[0],
                PhoneNumber=model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest();

            return Ok(new UserDto()
            {
                DisplayName=user.DisplayName,
                Email=user.Email,
                Token="Soooooon"
            });

        }

        }
}
