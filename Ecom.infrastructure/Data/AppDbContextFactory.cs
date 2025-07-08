

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbcontext>
    {
        public AppDbcontext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbcontext>();

           
            optionsBuilder.UseSqlServer(connectionString: "Server=DESKTOP-5C5HTFB;Database=Ecom11;Trusted_Connection=true;TrustServerCertificate=true");

            return new AppDbcontext(optionsBuilder.Options);
        }
    }
}
