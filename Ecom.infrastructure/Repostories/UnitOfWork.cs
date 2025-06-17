using AutoMapper;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repostories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbcontext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagementService _imageManagementService;
        public ICategoryRepositry CategoryRepositry { get; private set; }
        public IPhotoRepositry PhotoRepositry { get; private set; }
        public IProductRepositry ProductRepositry { get; private set; }

        public UnitOfWork(AppDbcontext context, IMapper mapper, IImageManagementService imageManagementService)
        {
            this._context = context;
            this._mapper = mapper;
            this._imageManagementService = imageManagementService;


            ProductRepositry = new ProductRepositry(_context, _imageManagementService, mapper: _mapper);
            CategoryRepositry = new CategoryRepositry(_context);
            PhotoRepositry = new PhotoRepositry(_context);
           
        }
    }
}
