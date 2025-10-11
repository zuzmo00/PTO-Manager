using AutoMapper;
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
        Task<string> GenerateToken(User user);
        Task<ClaimsIdentity> GetClaimsIdentity(User user);
        Task<Guid> Register(UserRegisterDto userRegisterDto);
    }
    public class UserServices : IUserServices
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper mapper;
        public UserServices(AppDbContext dbContext,IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            this.mapper = mapper;


        }

        public async Task<string> GenerateToken(User user)
        {
            var id = await GetClaimsIdentity(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var exp = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiresInMinutes"]));
            var token = new JwtSecurityToken(_configuration["JwtSettings:Issuer"], _configuration["JwtSettings:Audience"], id.Claims, expires: exp, signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public Task<ClaimsIdentity> GetClaimsIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nev),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.ReszlegId.ToString()), // This will be used to send the department id
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            return Task.FromResult(new ClaimsIdentity(claims, "Token"));
        }


        public async Task<string> Login(LoginInputDto userLoginDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == userLoginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginDto.Jelszo, user.Jelszo))
            {
                throw new UnauthorizedAccessException("Wrong email or password");
            }
            return await GenerateToken(user);
        }

        public async Task<Guid> Register(UserRegisterDto userRegisterDto)
        {
            var email = await _dbContext.Users.FirstOrDefaultAsync(x=>x.Email==userRegisterDto.Email);
            if (email != null)
            {
                throw new Exception("User already in use");
            }
            var user = mapper.Map<User>(userRegisterDto);
            user.FennmaradoNapok = new RemainingDay
            {
                OsszeesSzab = userRegisterDto.FennmaradoNapok,
            };
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user.Id;
        }
    }
}