using Ecom.core.DTO;
using Ecom.core.Entity;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.core.Sharing;
using Ecom.infrastructure.Repostories.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Ecom.infrastructure.Repostories
{
    public class AuthRepositry : IAuth
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken generateToken;
        public AuthRepositry(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken generateToken)
        {
            this.userManager   = userManager;
            this.emailService  = emailService;
            this.signInManager = signInManager;
            this.generateToken = generateToken;
        }

        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                return null;
            }
            if (await userManager.FindByNameAsync(registerDTO.UserName) is not null)
            {
                return "This UserName Is Already Registerd";
            }
            if (await userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return "This Email Is Already Registerd";
            }

            AppUser user = new AppUser()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,
                DisplayName = registerDTO.DisplayName

            };

             var result = await userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded is not true)
            {
                return result.Errors.ToList()[0].Description;
            }


            /*
            try
            {
                var result = await userManager.CreateAsync(user, registerDTO.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(separator: ", ", result.Errors.Select(e => e.Description));
                    return $"Registration failed: {errors}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception during user creation: {ex.Message} - {ex.InnerException?.Message}";
            };
            */



            // Send Active Email
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmail(user.Email, token, "active", "ActiveEmail", "Please active your email, click on button to active");
            return "done";
        }

        public async Task SendEmail(string email, string code, string component, string subject, string message)
        {
            var result = new EmailDTO(email,
                "mostafahomekk@gmail.com",
                subject, EmailStringBody.send(email, code, component, message));

            await emailService.SendEmail(result);
        }


        public async Task<string> LogInAsync(LogInDTO login)
        {
            if (login == null)
            {
                return null;
            }
            var finduser = await userManager.FindByEmailAsync(login.Email);
            if (!finduser.EmailConfirmed)
            {
                string token = await userManager.GenerateEmailConfirmationTokenAsync(finduser);

                await SendEmail(finduser.Email, token, "active", "ActiveEmail", "Please active your email, click on button to active");
                return "Please confirme your email first , we have send activat to your E-mail";
            }
            var result = await signInManager.CheckPasswordSignInAsync(finduser, login.Password, true);
            if (result.Succeeded)
            {
                return generateToken.GetAndCreateToken(finduser);
            }

            return "please ckeck your email and password , something your wrong";
        }

        public async Task<bool> SendEmailforForgetPassword(string email)
        {
            var findUser = await userManager.FindByEmailAsync(email);
            if (findUser is null)
            {
                return false;
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "Reset-Password", "Rest Password", "click on button to Reset your passwrd");
            return true;
        }

        public async Task<string> ResetPassword(RestPasswordDTO restPassword)
        {
            var findUser = await userManager.FindByEmailAsync(restPassword.Email);
            if (findUser is null)
            {
                return null;
            }

            string decodedToken = Uri.UnescapeDataString(restPassword.Token);

            var result = await userManager.ResetPasswordAsync(findUser, decodedToken, restPassword.Password);
            if (result.Succeeded)
            {
                return "done";
            }

            return result.Errors.ToList()[0].Description;

        }


        public async Task<bool> ActiveAccount(ActiveAccountDTO accountDTO)
        {
            var findUser = await userManager.FindByEmailAsync(accountDTO.Email);
            if (findUser is null)
            {
                return false;
            }

            string decodedToken = Uri.UnescapeDataString(accountDTO.Token);
            var reslt = await userManager.ConfirmEmailAsync(findUser, decodedToken);
            if (reslt.Succeeded)
            {
                return true;
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "active", "ActiveEmail", "Please active your email, click on button to active");
            return false;
        }

    }


}
       
