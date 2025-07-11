﻿using AutoMapper;
using Ecom.Api.Helper;
using Ecom.core.Interfaces;
using Ecom.core.Sharing;
using Ecom.infrastructure.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{

    public class ProductController : BaseController
    {
        public ProductController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> get([FromQuery]ProductParams productParams)
        {
            try
            {
                var Product = await work.ProductRepositry
                    .GetAllAsync(productParams);

               // var totalCount = await work.ProductRepositry.CountAsync();
                return Ok(new Pagination<ProductDTO>(productParams.PageNumber, productParams.pageSize ,Product.TotalCount, Product.products));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> getById(int id)
        {
            try
            {
                var product = await work.ProductRepositry.GetByIdAsync(id,
                    x => x.Category, x => x.Photos);
                var result = mapper.Map<ProductDTO>(product);
                if (product is null) return BadRequest(new ResponseAPI(400));
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost ("Add-Product")]

        public async Task<IActionResult> add(AddProductDTO productDTO)
        {
            try
            {
                await work.ProductRepositry.AddAsync(productDTO);
                return Ok(new  ResponseAPI(400));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

        [HttpPut( "Update-Product")]
        public async Task<IActionResult> Update(UpdateProductDTO updateProductDTO)
        {
            try
            {
                await work.ProductRepositry.UpdateAsync(updateProductDTO);
                return Ok(new ResponseAPI(400));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(statusCode: 400, ex.Message));
            }
        }


        [HttpDelete ("Delete-product/{id}")]

        public async Task<IActionResult> delete(int id)
        {
            try
            {
                var product = await work.ProductRepositry.GetByIdAsync(id, x => x.Photos, x => x.Category);

                await work.ProductRepositry.DeleteAsync(product);
                return Ok(new ResponseAPI(400));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(statusCode: 400, ex.Message));
            }
        }
    }
}
