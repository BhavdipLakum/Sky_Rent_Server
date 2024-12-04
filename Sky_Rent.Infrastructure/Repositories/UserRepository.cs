using Microsoft.EntityFrameworkCore;
using Sky_Rent.Domain.Entities;
using Sky_Rent.Domain.Interfaces;
using Sky_Rent.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sky_Rent.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailOrMobileAsync(string emailOrMobile)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == emailOrMobile || u.MobileNo == emailOrMobile);
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password); // Hash password
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UserExistsAsync(string emailOrMobile)
        {
            return await _context.Users.AnyAsync(u => u.Email == emailOrMobile || u.MobileNo == emailOrMobile);
        }
    }
}
