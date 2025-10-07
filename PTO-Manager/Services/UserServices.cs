using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PTO_Manager.Services
{
    public interface IUserServices
    {
        Task<string> Login(LoginInputDto loginInputDto);
        Task<string> GenerateToken(Szemelyek user);
        Task<ClaimsIdentity> GetClaimsIdentity(Szemelyek user);
    }
    public class UserServices : IUserServices
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public UserServices(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GenerateToken(Szemelyek user)
        {
            var id = await GetClaimsIdentity(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var exp = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiresInMinutes"]));
            var token = new JwtSecurityToken(_configuration["JwtSettings:Issuer"], _configuration["JwtSettings:Audience"], id.Claims, expires: exp, signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public Task<ClaimsIdentity> GetClaimsIdentity(Szemelyek user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nev),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            return Task.FromResult(new ClaimsIdentity(claims, "Token"));
        }


        public async Task<string> Login(LoginInputDto userLoginDto)
        {
            var user = await _dbContext.Szemelyek.FirstOrDefaultAsync(x => x.Email == userLoginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginDto.Jelszo, user.Jelszo))
            {
                throw new UnauthorizedAccessException("Wrong email or password");
            }
            return await GenerateToken(user);
        }

    }
}