﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignLanguage.APIs.DTOs;
using SignLanguage.APIs.Errors;
using SignLanguage.Core.Entities.Identity;
using SignLanguage.Core.Service.Contract;

namespace SignLanguage.APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService  _authService;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IAuthService authService
            ) 
        {
            _userManager=userManager;
            _signInManager=signInManager;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user==null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false );

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                Message="Success",
                DisplayName=user.DisplayName,
                Email=user.Email,
                Token=await _authService.CreateTokenAsync(user,_userManager)
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            
            var user = new AppUser()
            {
                DisplayName=model.DisplayName,
                Email=model.Email,
                UserName=model.Email.Split("@")[0],
                PhoneNumber=model.PhoneNumber
            };

            //var result = await _userManager.CreateAsync(user, model.Password);
            
            // إنشاء كلمة المرور (تشفيرها)
            var passwordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
            user.PasswordHash = passwordHash;

            // تعيين قيمة RePassword بنفس قيمة PasswordHash
            user.RePassword = passwordHash;

            // إضافة المستخدم إلى قاعدة البيانات
            var result = await _userManager.CreateAsync(user);


            if (!result.Succeeded)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Registration failed",
                    Detail = string.Join("; ", result.Errors.Select(e => e.Description))
                });
            }

            user.RePassword=user.PasswordHash;

            return Ok(new UserDto()
            {
                Message="Success",
                DisplayName=user.DisplayName,
                Email=user.Email,
                Token= await _authService.CreateTokenAsync(user, _userManager)
            });

        }

        }
}
