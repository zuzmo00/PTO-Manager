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
        public Task<string> RemoveDeparment(Guid id, int reszlegId);
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

        public async Task<string> RemoveDeparment(Guid id, int reszlegId)
        {
            var admin = await _context.Administrators.FindAsync(id);
            if (admin == null)
            {
                throw new Exception("Admin not found");
            }
            var user = await _context.Users.FindAsync(admin.SzemelyId);
            foreach (var reszleg in user.Ugyintezo)
            {
                if (reszleg.ReszlegId == reszlegId)
                {
                    user.Ugyintezo.Remove(reszleg);
                    break;
                }
            }
            _context.Users.Update(user);
            if (user.Ugyintezo.Count == 0)
            {
                user.Role = Roles.User;
                _context.Administrators.Remove(admin);
                _context.Users.Update(user);
            }
            await _context.SaveChangesAsync();
            return "Admin removed successfully";
        }
    }
}
