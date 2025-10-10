using AutoMapper;
using PTO_Manager.Context;
using PTO_Manager.Entities;
using PTO_Manager.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTO_Manager.Services
{
    public interface IAdminService
    {
        public Task<string> CreateAdmin(Guid id, int reszlegId);
    }
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public AdminService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> CreateAdmin(Guid id, int reszlegId)
        {
            var user = await _context.Users.FindAsync(id);
            if(user.Role==Roles.Administrator)
            {
                throw new Exception("User is already an admin");
            }
            var admin=new Admin
            {
                SzemelyId = user.Id,
                ReszlegId = reszlegId,
                Kerhet = true,
                Visszavonhat = true,
                Biralhat = true,
            };
            await _context.Administrators.AddAsync(admin);
            user.Role = Roles.Administrator;
            user.Ugyintezo ??= new List<Admin>();
            user.Ugyintezo.Add(admin);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return "Admin created successfully";
        }

    }
}
