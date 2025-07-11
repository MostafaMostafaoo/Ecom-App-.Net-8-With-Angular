using AutoMapper;
using Ecom.Api.Helper;
using Ecom.core.DTO;
using Ecom.core.Entity;
using Ecom.core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecom.Api.Controllers
{
   // [Route("api/[controller]")]
   // [ApiController]
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-address-for-user")]

        public async Task<IActionResult> getAddress()
        {
            var address = await work.Auth.getUserAddress(User.FindFirst(ClaimTypes.Email).Value);
            var result = mapper.Map<ShipAddressDTO>(address);
            return Ok(result);
        }


        [HttpGet("IsUserAuth")]
        public async Task<IActionResult> IsUserAuth()
        {
            return User.Identity.IsAuthenticated ? Ok() : BadRequest();
        }


        [Authorize]
        [HttpPut("update-address")]
        public async Task<IActionResult>updateAddress(ShipAddressDTO addressDTO)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var address = mapper.Map<Address>(addressDTO);
            var result = await work.Auth.UpdateAddress(email, address);
            return result ? Ok() : BadRequest();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> register(RegisterDTO registerDTO)
        {
            string resut = await work.Auth.RegisterAsync(registerDTO);
            if (resut !="done")
            {
                return BadRequest(new ResponseAPI(400, resut));
            }
            return Ok(new ResponseAPI(200, resut));
        }

        [HttpPost("Login")]

        public async Task<IActionResult> login(LogInDTO logInDTO)
        {
            var result = await work.Auth.LogInAsync(logInDTO);
            if (result.StartsWith("please"))
            {
                return BadRequest(new ResponseAPI(400, result));
            }

            Response.Cookies.Append("token", result, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Domain = "localhost",
                Expires = DateTime.Now.AddDays(1),
                IsEssential = true,
                //SameSite = SameSiteMode.Strict,
                SameSite = SameSiteMode.None, // ✅ مهم جدًا

            });

            return Ok(new ResponseAPI(200));
        }

        [HttpPost ("active-account")]

        public async Task<IActionResult> active(ActiveAccountDTO accountDTO)
        {
            var result = await work.Auth.ActiveAccount(accountDTO);
            return result ? Ok(value: new ResponseAPI(200)) : BadRequest(new ResponseAPI(400));
        }

        [HttpGet("send-email-forget-password")]
        public async Task<IActionResult> forget(string email)
        {
            var result = await work.Auth.SendEmailforForgetPassword(email);
            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(400));

        }


        [HttpPost("reset-password")]

        public async Task<IActionResult> reset(RestPasswordDTO restPasswordDTO)
        {
            var result = await work.Auth.ResetPassword(restPasswordDTO);

            if (result == "done")
            {
                return Ok(new ResponseAPI(200));
            }
            return BadRequest(new ResponseAPI(400));
        }



    }
}
