using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sky_Rent.Application.DTOs
{
    public class UserForLoginDTO
    {
        public string EmailOrMobile { get; set; }
        public string Password { get; set; }
    }
}
