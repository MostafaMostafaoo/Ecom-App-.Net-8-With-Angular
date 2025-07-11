using AutoMapper;
using Ecom.core.Entity;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repostories.Services;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
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
        private readonly IConnectionMultiplexer redis;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken token;

        public ICategoryRepositry CategoryRepositry { get; private set; }
        public IPhotoRepositry PhotoRepositry { get; private set; }
        public IProductRepositry ProductRepositry { get; private set; }


        public ICustomerBasketRepository CustomerBasket { get; private set; }

        public IAuth Auth { get; private set; }

        public UnitOfWork(AppDbcontext context, IMapper mapper, IImageManagementService imageManagementService
                , IConnectionMultiplexer redis, UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken token)
        {
            this._context = context;
            this._mapper = mapper;
            this._imageManagementService = imageManagementService;
            this.redis = redis;
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.token = token;

            ProductRepositry = new ProductRepositry(_context, _imageManagementService, mapper: _mapper);
            CategoryRepositry = new CategoryRepositry(_context);
            PhotoRepositry = new PhotoRepositry(_context);
            CustomerBasket = new CustomerBasketRepository(redis);
            Auth = new AuthRepositry(userManager, emailService, signInManager,token,context);
            
        }
    }
}
