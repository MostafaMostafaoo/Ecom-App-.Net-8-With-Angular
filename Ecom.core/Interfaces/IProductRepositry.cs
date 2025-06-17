using Ecom.core.Entity.Product;
using Ecom.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.infrastructure.Data.DTO;

namespace Ecom.core.Interfaces
{
    public interface IProductRepositry : IGenericRepositry<Product>
    {
        // for futuer

        Task<bool> AddAsync(AddProductDTO addProductDTO);

        Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO);
    }
}
