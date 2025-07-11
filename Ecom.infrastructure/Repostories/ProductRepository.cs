﻿using AutoMapper;
using Ecom.core.Entity.Product;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.core.Sharing;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Data.DTO;
using Ecom.infrastructure.Repostories.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repostories
{
    public class ProductRepositry : GenericRepositry<Product>, IProductRepositry
    {
        private readonly AppDbcontext context;
        private readonly IImageManagementService imageManagementService;
        private readonly IMapper mapper;

        public ProductRepositry(AppDbcontext context,IImageManagementService imageManagementService, IMapper mapper) : base(context)
        {
            this.context = context;
            this.imageManagementService = imageManagementService;
            this.mapper = mapper;
        }


        public async Task<ReturnProductDto> GetAllAsync(ProductParams productParams)
        {
            var query = context.Products
                .Include(m => m.Category)
                .Include(m => m.Photos)
                .AsNoTracking();

            //filtering by word

            if (!string.IsNullOrEmpty(productParams.Search))
            {
                var searchWords = productParams.Search.Split(' ');
                query = query.Where(m => searchWords.All(word =>
                    m.Name.ToLower().Contains(word.ToLower()) ||
                    m.Description.ToLower().Contains(word.ToLower())
                ));
            }



            if (productParams.CategoryId.HasValue)
                query = query.Where(m => m.CategoryId == productParams.CategoryId);
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                query = productParams.Sort switch
                {
                    "PriceAce" => query.OrderBy(m => m.NewPrice),
                    "PriceDce" => query.OrderByDescending(m => m.NewPrice),
                    _ => query.OrderBy(m => m.NewPrice),
                };
            }

            ReturnProductDto returnProductDto = new ReturnProductDto();
            returnProductDto.TotalCount = query.Count();

            query = query.Skip((productParams.pageSize) * (productParams.PageNumber - 1)).Take(productParams.pageSize);
            returnProductDto.products = mapper.Map<List<ProductDTO>>(query);
            return returnProductDto;
        }
        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            if (productDTO == null) return false;
            var product = mapper.Map<Product>(productDTO);
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            var ImagePath = await imageManagementService.AddImageAsync(productDTO.Photo, productDTO.Name);

            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id,
            }).ToList();
            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;

        }

  
        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO)
        {
            if (updateProductDTO is null)
            {
                return false;
            }
            var FindProduct = await context.Products.Include(m => m.Category)
                .Include(m => m.Photos)
                .FirstOrDefaultAsync(m => m.Id == updateProductDTO.Id);

            if (FindProduct is null)
            {
                return false;
            }
            mapper.Map(updateProductDTO, FindProduct);

            var FindPhoto = await context.Photos.Where(m => m.ProductId == updateProductDTO.Id).ToListAsync();

            foreach (var item in FindPhoto)
            {
                imageManagementService.DeleteImageAsync(item.ImageName);

            }

            context.Photos.RemoveRange(FindPhoto);

            var ImagePath = await imageManagementService.AddImageAsync(updateProductDTO.Photo, updateProductDTO.Name);

            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = updateProductDTO.Id,
            }).ToList();
            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true; 
        }

        public async Task DeleteAsync(Product product)
        {
            var photo = await context.Photos.Where(x => x.ProductId == product.Id)
                .ToListAsync();
            foreach (var item in photo)
            {
                imageManagementService.DeleteImageAsync(item.ImageName);
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

    }
}
