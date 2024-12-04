using Sky_Rent.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sky_Rent.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmailOrMobileAsync(string emailOrMobile);
        Task<User> RegisterAsync(User user, string password);
        Task<bool> UserExistsAsync(string emailOrMobile);
    }
}
