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

            //   هنا سلسلة الاتصال الخاصة بك
            optionsBuilder.UseSqlServer(connectionString: "Server=DESKTOP-5C5HTFB;Database=Ecom;Trusted_Connection=true;TrustServerCertificate=true");

            return new AppDbcontext(optionsBuilder.Options);
        }
    }
}
