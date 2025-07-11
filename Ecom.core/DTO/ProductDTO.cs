﻿using Ecom.core.Entity.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data.DTO
{
    public class ProductDTO
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }

        public virtual List<PhotoDTO> Photos { get; set; }
        public string CategoryName { get; set; }

    }

    public record ReturnProductDto()
    {
        public List<ProductDTO> products { get; set; }

        public int TotalCount { get; set; }
    }

    public record PhotoDTO
    {
        public string imageName { get; set; }
        public int ProductId { get; set; }

    }

    public record AddProductDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }

        public int CategoryId { get; set; }

        public IFormFileCollection Photo { get; set; } 
    }

    public record UpdateProductDTO : AddProductDTO
    {
        public int Id { get; set; }
    }
}
