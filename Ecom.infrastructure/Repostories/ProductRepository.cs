using Ecom.core.Entity.Product;
using Ecom.core.Interfaces;
using Ecom.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repostories
{
    public class ProductRepositry : GenericRepositry<Product>, IProductRepositry
    {
        public ProductRepositry(AppDbcontext context) : base(context)
        {
        }
    }
}
