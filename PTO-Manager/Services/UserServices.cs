using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;
using PTO_Manager.Entities.Enums;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SzabadsagKezeloWebApp.Services;
using RemainingDay = PTO_Manager.Entities.RemainingDay;

namespace PTO_Manager.Services
{
    public interface IUserServices
    {
        Task<LoginReturnDto> Login(LoginInputDto loginInputDto);
        Task<string> GenerateToken(User user);
        Task<ClaimsIdentity> GetClaimsIdentity(User user);
        Task<Guid> Register(UserRegisterDto userRegisterDto);
        Task<RemainingDayGetDto> RemainingDaysGet();
    }

    public class UserServices : IUserServices
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IAktualisFelhasznaloService _aktualisFelhasznaloService;

        public UserServices(AppDbContext dbContext, IMapper mapper, IConfiguration configuration, IAktualisFelhasznaloService aktualisFelhasznaloService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _mapper = mapper;
            _aktualisFelhasznaloService = aktualisFelhasznaloService;
        }

        public async Task<string> GenerateToken(User user)
        {
            var id = await GetClaimsIdentity(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var exp = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtSettings:ExpiresInMinutes"]));
            var exp = DateTime.Now.AddDays(30);
            var token = new JwtSecurityToken(_configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"], id.Claims, expires: exp, signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<ClaimsIdentity> GetClaimsIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nev),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.ReszlegId.ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            return Task.FromResult(new ClaimsIdentity(claims, "Token"));
        }


        public async Task<LoginReturnDto> Login(LoginInputDto userLoginDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == userLoginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginDto.Jelszo, user.Jelszo))
            {
                throw new UnauthorizedAccessException("Wrong email or password");
            }

            List<AdminPrivilegesDto> adminPrivilegesList = new List<AdminPrivilegesDto>();
            if (user.Role == Roles.Administrator)
            {
                var admin = await _dbContext.Administrators.Where(x => x.SzemelyId == user.Id).ToListAsync();
                foreach (var item in admin)
                {
                    AdminPrivilegesDto adminPrivilegesDto = new AdminPrivilegesDto
                    {
                        kerhet = item.Kerhet,
                        visszavonhat = item.Visszavonhat,
                        biralhat = item.Biralhat,
                        reszlegId = item.ReszlegId,
                    };
                    adminPrivilegesList.Add(adminPrivilegesDto);
                }

                LoginReturnDto loginReturnDto = new LoginReturnDto
                {
                    token = await GenerateToken(user),
                    adminPrivileges = adminPrivilegesList
                };
                return loginReturnDto;
            }

            return new LoginReturnDto
            {
                token = await GenerateToken(user),
                adminPrivileges = null
            };
        }

        public async Task<Guid> Register(UserRegisterDto userRegisterDto)
        {
            var email = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == userRegisterDto.Email);
            if (email != null)
            {
                throw new Exception("User already in use");
            }

            var user = _mapper.Map<User>(userRegisterDto);
            user.FennmaradoNapok = new RemainingDay
            {
                OsszesSzab = userRegisterDto.FennmaradoNapok,
            };
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user.Id;
        }

        public async Task<RemainingDayGetDto> RemainingDaysGet()
        {
            var remainingEntity =
                await _dbContext.Remaining.FirstOrDefaultAsync(c =>
                    c.SzemelyId.ToString() == _aktualisFelhasznaloService.UserId) ?? throw new Exception("User with the given parameters, not found in the database");
            return _mapper.Map<RemainingDayGetDto>(remainingEntity);
        }
    }
}