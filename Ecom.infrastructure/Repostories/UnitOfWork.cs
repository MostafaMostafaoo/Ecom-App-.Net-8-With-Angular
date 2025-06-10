using Ecom.core.Interfaces;
using Ecom.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repostories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbcontext _context;

        public ICategoryRepositry CategoryRepositry { get; }

        public IPhotoRepositry PhotoRepositry { get; }

        public IProductRepositry ProductRepositry { get; }

        public UnitOfWork(AppDbcontext context)
        {
            this._context = context;
            CategoryRepositry = new CategoryRepositry(_context);
            PhotoRepositry = new PhotoRepositry(_context);

        }
    }
}
