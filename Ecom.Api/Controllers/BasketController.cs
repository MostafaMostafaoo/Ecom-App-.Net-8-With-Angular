﻿using AutoMapper;
using Ecom.Api.Helper;
using Ecom.core.Entity;
using Ecom.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{

    public class BasketController : BaseController
    {
        public BasketController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-basket-item/{id}")]

        public async Task<IActionResult> get (string id)
        {
            var result = await work.CustomerBasket.GetBasketAsync(id);
            if (result is null)
            {
                return Ok(new CustomerBasket());
            }
            return Ok(value: result);
        }

        [HttpPost("update-basket")]

        public async Task<IActionResult> add (CustomerBasket basket)
        {
            var _basket = await work.CustomerBasket.UpdateBasketAsync(basket);
            return Ok(basket);
        }

        [HttpDelete("delete-basket-item/{id}")]

        public async Task<IActionResult> delete (string id)
        {
            var result = await work.CustomerBasket.DeleteBasketAsync(id);

            return result ? Ok(new ResponseAPI(200, "Item Deleted")) : BadRequest(new ResponseAPI(400)); 
        }
    }
}
