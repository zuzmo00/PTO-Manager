using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;
using PTO_Manager.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTO_Manager.Services
{
    public interface IAdminService
    {
        public Task<string> CreateAdmin(Guid id, int departmentId);
        public Task<string> RemoveDeparment(Guid id, int departmentId);
        public Task<string> ChangePermissions(PermissionUpdateDto permissionUpdateDto);
        public Task<string> AddDepartment(Guid id, int departmentId);
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

        public async Task<string> AddDepartment(Guid id, int departmentId)
        {
            var adminInTable = await _context.Administrators.FirstOrDefaultAsync(x=>x.UserId==id);
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Id==id);
            if (adminInTable == null)
            {
                throw new Exception("Admin not found");
            }
            Admin admin = new Admin
            {
                UserId = id,
                DepartmentId = departmentId,
                CanRequest = true,
                CanRevoke = true,
                CanDecide = true,
            };
            await _context.Administrators.AddAsync(admin);
            user.AdminRoles ??= new List<Admin>();
            user.AdminRoles.Add(admin);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return "Department added successfully";
        }

        public async Task<string> ChangePermissions(PermissionUpdateDto permissionUpdateDto)
        {
            var admin = await _context.Administrators.FirstOrDefaultAsync(x => x.UserId == permissionUpdateDto.Id && x.DepartmentId == permissionUpdateDto.DepartmentId);
            if (admin == null)
            {
                throw new Exception("Admin not found");
            }
            admin.CanRequest = permissionUpdateDto.CanRequest;
            admin.CanRevoke = permissionUpdateDto.CanRevoke;
            admin.CanDecide = permissionUpdateDto.CanDecide;
            _context.Administrators.Update(admin);
            await _context.SaveChangesAsync();
            return "Permissions changed successfully";
        }

        public async Task<string> CreateAdmin(Guid id, int departmentId)
        {
            var user = await _context.Users.FindAsync(id);
            if(user.Role==Roles.Administrator)
            {
                throw new Exception("User is already an admin");
            }
            var admin=new Admin
            {
                UserId = user.Id,
                DepartmentId = departmentId,
                CanRequest = true,
                CanRevoke = true,
                CanDecide = true,
            };
            await _context.Administrators.AddAsync(admin);
            user.Role = Roles.Administrator;
            user.AdminRoles ??= [];
            user.AdminRoles.Add(admin);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return "Admin created successfully";
        }

        public async Task<string> RemoveDeparment(Guid id, int departmentId)
        {
            var admin = await _context.Administrators.FindAsync(id);
            if (admin == null)
            {
                throw new Exception("Admin not found");
            }
            var user = await _context.Users.FindAsync(admin.UserId);
            foreach (var reszleg in user.AdminRoles)
            {
                if (reszleg.DepartmentId == departmentId)
                {
                    user.AdminRoles.Remove(reszleg);
                    break;
                }
            }
            _context.Users.Update(user);
            if (user.AdminRoles.Count == 0)
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
