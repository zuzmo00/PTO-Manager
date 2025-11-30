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
        public Task<string> CreateAdmin(CreateAdminInputDTO createAdminInputDto);
        public Task<string> ChangePermissions(PermissionUpdateDto permissionUpdateDto);
        public Task<List<PermissionGetDto>> GetPermissionsForUser(GetpermissionInputDto getpermissionInputDto);
        public Task<string> AddDepartment(Guid id, int departmentId);
        public Task<string> RemovePriviligeByDeparment(RemoveAdminPriviligeInputDto removeDto);

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
            var admin = await _context.Administrators.FirstOrDefaultAsync(x => x.UserId == permissionUpdateDto.UserId && x.DepartmentId == permissionUpdateDto.DepartmentId);
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

        public async Task<string> CreateAdmin(CreateAdminInputDTO createAdminInputDTO)
        {
            var user = await _context.Users
                .Include(c => c.AdminRoles)
                .ThenInclude(l=>l.Department)
                .FirstOrDefaultAsync(k => k.Id == createAdminInputDTO.id) ?? throw new Exception("User not found");
            if(user.Role==Roles.Administrator && user.AdminRoles.Any(x=>x.Department.DepartmentName==createAdminInputDTO.departmentName))
            {
                throw new Exception("User is already an admin");
            }
            var department = await _context.Department.FirstOrDefaultAsync(x=>x.DepartmentName==createAdminInputDTO.departmentName) ?? throw new Exception("Department not found");
            
            var admin=new Admin
            {
                UserId = user.Id,
                DepartmentId = department.Id,
                CanRequest = createAdminInputDTO.CanRequest,
                CanRevoke = createAdminInputDTO.CanRevoke,
                CanDecide = createAdminInputDTO.CanDecide,
            };
            await _context.Administrators.AddAsync(admin);
            user.Role = Roles.Administrator;
            user.AdminRoles ??= [];
            user.AdminRoles.Add(admin);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return "Admin created successfully";
        }

        public async Task<string> RemovePriviligeByDeparment(RemoveAdminPriviligeInputDto removeDto)
        {
            var user = await _context.Users
                           .Include(u => u.AdminRoles)
                           .ThenInclude(l => l.Department)
                           .FirstOrDefaultAsync(u => u.Id == removeDto.id)
                       ?? throw new Exception("User not found");

            var roleToRemove = user.AdminRoles
                .FirstOrDefault(r => r.Department.DepartmentName == removeDto.departmentName);

            if (roleToRemove != null)
                _context.Administrators.Remove(roleToRemove);
            
            if (user.AdminRoles.Count == 0)
            {
                user.Role = Roles.User;
            }

            await _context.SaveChangesAsync();
            return "Admin removed successfully";
        }


        public async Task<List<PermissionGetDto>> GetPermissionsForUser(GetpermissionInputDto getpermissionInputDto)
        {
            var tempUser = await _context.Users
                .Include(k => k.AdminRoles)
                .ThenInclude(l => l.Department)
                .FirstOrDefaultAsync(c => c.Id.ToString() == getpermissionInputDto.userId) ?? throw new Exception("User not found");
            
            return _mapper.Map<List<PermissionGetDto>>(tempUser.AdminRoles);
        }
    }
}
