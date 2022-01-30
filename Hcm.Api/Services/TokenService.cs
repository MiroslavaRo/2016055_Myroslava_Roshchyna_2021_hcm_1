using Hcm.Api.Dto;
using Hcm.Core.Database;
using Hcm.Database.Models;
using Hcm.Database.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hcm.Api.Services
{
    public class TokenService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly PasswordService _passwordService;
        private readonly IUserRepository _userRepository;

        public TokenService(
            IEmployeeRepository employeeRepository,
            PasswordService passwordService,
            IUserRepository userRepository)
        {
            _employeeRepository = employeeRepository;
            _passwordService = passwordService;
            _userRepository = userRepository;
        }

        public async Task<TokenResultDto> IssueTokenAsync(
            string username, 
            string password,
            int? role)
        {
            var dbUser = await _userRepository.GetByUsernameAndRoleAsync(
                username, (Roles)role);

            if (dbUser is null)
            {
                throw new DomainException(
                    "Invalid username or password");
            }

            if (!_passwordService.AreEqual(dbUser.Password, password))
            {
                throw new DomainException(
                    "Invalid username or password");
            }

            var employeeId = string.Empty;
            if (dbUser.Role == Roles.Employee)
            {
                employeeId = await _employeeRepository.Query()
                    .Where(e => e.UserId == dbUser.Id)
                    .Select(e => e.Id)
                    .FirstOrDefaultAsync();
            }

            var claims = new[] {
                new Claim(ClaimTypes.Role, dbUser.Role.ToString()),
                new Claim("id", dbUser.Id),
                new Claim(nameof(employeeId), employeeId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, dbUser.Username),
                new Claim(ClaimTypes.Email, dbUser.Email),
                new Claim(ClaimTypes.MobilePhone, dbUser.Phone)
            };

            var expireAt = DateTime.UtcNow.AddHours(24);

            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("SUPER SECRET KEY 1234"));

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateJwtSecurityToken(
                issuer: "hcm.api",
                audience: "hcm",
                subject: new ClaimsIdentity(
                    claims, 
                        JwtBearerDefaults.AuthenticationScheme, 
                            ClaimTypes.NameIdentifier, 
                                ClaimTypes.Role),
                notBefore: DateTime.UtcNow,
                expires: expireAt,
                issuedAt: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    signingKey, 
                        SecurityAlgorithms.HmacSha256));

            return new TokenResultDto
            {
                Schema = JwtBearerDefaults.AuthenticationScheme,
                Token = tokenHandler.WriteToken(token),
                ValidTo = expireAt
            };
        }
    }
}
