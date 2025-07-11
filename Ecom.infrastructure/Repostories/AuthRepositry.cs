﻿using Ecom.core.DTO;
using Ecom.core.Entity;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.core.Sharing;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repostories.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly AppDbcontext context;
        public AuthRepositry(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken generateToken, AppDbcontext context)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.generateToken = generateToken;
            this.context = context;
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
        /*
        public async Task<bool> UpdateAddress(string email, Address address)
        {
            var findUser = await userManager.FindByEmailAsync(email);
            if (findUser is null)
            {
                return false;
            }
            var Myaddress = await context.Addresses.FirstOrDefaultAsync(m => m.AppUserId == findUser.Id);

            if (Myaddress is null)
            {
                address.AppUserId = findUser.Id;
                await context.Addresses.AddAsync(address);
            }
            else
            {
                address.Id = Myaddress.Id;
                context.Addresses.Update(address);

            }
            await context.SaveChangesAsync();
            return true;
        }
        */

        public async Task<bool> UpdateAddress(string email, Address address)
        {
            var findUser = await userManager.FindByEmailAsync(email);
            if (findUser == null)
            {
                return false;
            }

            var myAddress = await context.Addresses.FirstOrDefaultAsync(m => m.AppUserId == findUser.Id);

            if (myAddress == null)
            {
                // جديد
                address.AppUserId = findUser.Id;
                await context.Addresses.AddAsync(address);
            }
            else
            {
                // تعديل البيانات فقط على العنصر الموجود
                myAddress.FirstName = address.FirstName;
                myAddress.LastName = address.LastName;
                myAddress.street = address.street;
                myAddress.City = address.City;
                myAddress.state = address.state;
                myAddress.ZipCode = address.ZipCode;

                context.Addresses.Update(myAddress);
            }

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Address> getUserAddress(string email)
        {
            var User = await userManager.FindByEmailAsync(email);
            var address = await context.Addresses.FirstOrDefaultAsync(m => m.AppUserId == User.Id);

            return address;
        }
    }


}
       
