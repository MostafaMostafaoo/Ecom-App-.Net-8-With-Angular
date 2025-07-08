using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.DTO
{

    public record LogInDTO
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
    public record RegisterDTO : LogInDTO
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; } // ✅ أضف هذا


    }

    public record RestPasswordDTO: LogInDTO
    {
        
        public string Token { get; set; }
  
    }

    public record ActiveAccountDTO
    {
        public string Email { get; set; }

        public string Token { get; set; }
 
    }
}
