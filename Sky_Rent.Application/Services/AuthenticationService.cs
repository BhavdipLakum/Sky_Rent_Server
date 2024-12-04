using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sky_Rent.Application.DTOs;
using Sky_Rent.Domain.Entities;
using Sky_Rent.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sky_Rent.Application.Services
{
    public class AuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> RegisterUserAsync(UserForRegistrationDTO registrationDto)
        {
            if (await _userRepository.UserExistsAsync(registrationDto.Email))
            {
                throw new Exception("User already exists.");
            }

            var user = new User
            {
                Username = registrationDto.Username,
                Email = registrationDto.Email,
                MobileNo = registrationDto.MobileNo
            };

            var createdUser = await _userRepository.RegisterAsync(user, registrationDto.Password);
            return createdUser.Id.ToString();
        }

        public async Task<string> LoginUserAsync(UserForLoginDTO loginDto)
        {
            var user = await _userRepository.GetByEmailOrMobileAsync(loginDto.EmailOrMobile);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid credentials.");
            }

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
