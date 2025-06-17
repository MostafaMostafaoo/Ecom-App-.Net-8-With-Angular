﻿using AutoMapper;
using Ecom.Api.Helper;
using Ecom.core.Entity.Product;
using Ecom.core.Interfaces;
using Ecom.infrastructure.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
  
    public class CategoryController : BaseController
    {
        public CategoryController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]

        public async Task<IActionResult> get()
        {
            try
            {
             var category = await work.CategoryRepositry.GetAllAsync();
                if (category is null)
                    return BadRequest(new  ResponseAPI(400));
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> getbyId(int id)
        {
            try {
                var category = await work.CategoryRepositry.GetByIdAsync(id);
                if (category is null) return BadRequest(new ResponseAPI(400, $"not found category id: {id}"));
                return Ok(category);                    
                    
             }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost ("add-category")]
        public async Task<IActionResult> add(CategoryDTO categoryDTO)
        {
            try {

                var category = mapper.Map<Category>(categoryDTO);


                await work.CategoryRepositry.AddAsync(category);
                return Ok(new ResponseAPI(200 , "Item has been added"));
            }
            catch (Exception ex){
                return BadRequest(ex.Message);
            }
        }


        [HttpPut ("update-category")]
        public async Task<IActionResult> update(UpdateCategoryDTO categoryDTO)
        {
            try
            {
                var category = mapper.Map<Category>(categoryDTO);
                await work.CategoryRepositry.UpdateAsync(category);
                return Ok(new ResponseAPI(200, "Item has been updated"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                await work.CategoryRepositry.DeleteAsync(id);
                return Ok(new ResponseAPI(200, "Item has been deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
