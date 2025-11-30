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
        Task<RemainingDayGetDto> RemainingDaysGetByUserid(GetRemainingForUserDto userDto);
        Task<List<GetUsersGetDto>> GetUsersByParams(GetUsersInputDTO userDto);
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
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.DepartmentId.ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            return Task.FromResult(new ClaimsIdentity(claims, "Token"));
        }


        public async Task<LoginReturnDto> Login(LoginInputDto userLoginDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == userLoginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginDto.password, user.Password))
            {
                throw new UnauthorizedAccessException("Wrong email or password");
            }

            List<AdminPrivilegesDto> adminPrivilegesList = new List<AdminPrivilegesDto>();
            if (user.Role == Roles.Administrator)
            {
                var admin = await _dbContext.Administrators.Where(x => x.UserId == user.Id).ToListAsync();
                foreach (var item in admin)
                {
                    AdminPrivilegesDto adminPrivilegesDto = new AdminPrivilegesDto
                    {
                        CanRequest = item.CanRequest,
                        CanRevoke = item.CanRevoke,
                        CanDecide = item.CanDecide,
                        DepartmentId = item.DepartmentId,
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
            user.RemainingDay = new RemainingDay
            {
                AllHoliday = userRegisterDto.AllHoliday,
                RemainingDays = userRegisterDto.AllHoliday,
            };
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user.Id;
        }

        public async Task<RemainingDayGetDto> RemainingDaysGet()
        {
            var remainingEntity =
                await _dbContext.Remaining.FirstOrDefaultAsync(c =>
                    c.UserId.ToString() == _aktualisFelhasznaloService.UserId) ?? throw new Exception("User with the given parameters, not found in the database");
            var temp = _mapper.Map<RemainingDayGetDto>(remainingEntity);
            temp.TimeProportional = (int)Math.Round(((double)temp.AllHoliday /365) * DateTime.Now.DayOfYear);
            return temp;
        }
        
        public async Task<RemainingDayGetDto> RemainingDaysGetByUserid(GetRemainingForUserDto userDto)
        {
            var remainingEntity =
                await _dbContext.Remaining.FirstOrDefaultAsync(c =>
                    c.UserId.ToString() == userDto.userId) ?? throw new Exception("User with the given parameters, not found in the database");
            var temp = _mapper.Map<RemainingDayGetDto>(remainingEntity);
            temp.TimeProportional = (int)Math.Round(((double)temp.AllHoliday /365) * DateTime.Now.DayOfYear);
            return temp;
        }


        public async Task<List<GetUsersGetDto>> GetUsersByParams(GetUsersInputDTO userDto)
        {
            var tempList = _dbContext.Users
                .Include(k => k.Department)
                .Where(c => userDto.DepartmentIds.Contains(c.Department.DepartmentName));
            
            if (!string.IsNullOrEmpty(userDto.inputText))
            {
                tempList = tempList.Where(k => EF.Functions.Like(k.Name, $"%{userDto.inputText}%"));
            }
            
            var returnlist = await tempList.ToListAsync();
            
            return _mapper.Map<List<GetUsersGetDto>>(returnlist);
        }
    }
}