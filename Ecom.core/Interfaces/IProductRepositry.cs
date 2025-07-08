using Ecom.core.Entity.Product;
using Ecom.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.infrastructure.Data.DTO;
using Ecom.core.Sharing;

namespace Ecom.core.Interfaces
{
    public interface IProductRepositry : IGenericRepositry<Product>
    {
        // for futuer
        Task<ReturnProductDto> GetAllAsync(ProductParams productParams);
        Task<bool> AddAsync(AddProductDTO addProductDTO);

        Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO);

        Task DeleteAsync(Product product);
    }
}
