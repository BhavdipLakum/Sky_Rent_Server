using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sky_Rent.Application.DTOs
{
   
    public class UserForRegistrationDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
