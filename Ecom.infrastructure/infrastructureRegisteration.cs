using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repostories;
using Ecom.infrastructure.Repostories.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure
{
    public static class infrastructureRegisteration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            //services.AddTransient
            //services.AddScoped
            //services.AddSingleton
            services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));
            // services.AddScoped<ICategoryRepositry, CategoryRepositry>();
            //services.AddScoped<IProductRepositry, ProductRepositry>();
            // services.AddScoped<IPhotoRepositry, PhotoRepositry>();

            // Apply unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IImageManagementService, ImageManagementService>();
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(root: Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
         
          
            //Apply DbContext
            services.AddDbContext<AppDbcontext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CS"));
            });

            return services;
        }

        // ✅ نضيف الدالة المساعدة داخل نفس الكلاس
        private static bool IsRunningInEfCoreDesignTime()
        {
            return Environment.GetCommandLineArgs().Any(arg => arg.Contains("ef", StringComparison.OrdinalIgnoreCase));
        }

    }



}
