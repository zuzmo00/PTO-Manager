using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;
using SzabadsagKezeloWebApp.Services;

namespace PTO_Manager.Services
{
    public interface IDepartmentService
    {
        public Task<string> CreateDepartment(CreateDepartmentDto departmentName);
        public Task<string> RemoveDepartment(DepartmentRemoveDto departmentRemoveDto);
        public Task<List<DepartmentGetDto>> GetDepartmentsForManage();
        public Task<List<string>> GetDepartments();
        public Task<List<string>> GetDepartmentsForDecide();
    }
    public class DepartmentService : IDepartmentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAktualisFelhasznaloService _aktualisFelhasznaloService;
        public DepartmentService (AppDbContext context, IMapper mapper, IAktualisFelhasznaloService aktualisFelhasznaloService)
        {
            _context = context;
            _mapper = mapper;
            _aktualisFelhasznaloService = aktualisFelhasznaloService;
        }

        public async Task<string> CreateDepartment(CreateDepartmentDto departmentName)
        {
            var newDepartment = new Department
            {
                DepartmentName = departmentName.DepartmentName
            };
            await _context.Department.AddAsync(newDepartment);
            await _context.SaveChangesAsync();
            return $"Department added successfully with id: {newDepartment.Id}";
        }
        
        public async Task<string> RemoveDepartment(DepartmentRemoveDto departmentRemoveDto)
        {
            var department = await _context.Department.FirstOrDefaultAsync(k=>k.Id == departmentRemoveDto.id) ?? throw new Exception("Department not found");
            
            _context.Department.Remove(department);
            await _context.SaveChangesAsync();
            return "Department removed successfully";
        }
        
        
        public async Task<List<string>> GetDepartments()
        {
            var department = await _context.Department.Select(c => c.DepartmentName).ToListAsync();
            return department;
        }
        
        public async Task<List<string>> GetDepartmentsForDecide()
        {
            var userObj = await _context.Users
                .Include(k => k.AdminRoles)
                .ThenInclude(k=>k.Department)
                .FirstOrDefaultAsync(l => l.Id.ToString() == _aktualisFelhasznaloService.UserId) ?? throw new Exception("Administrator not found");
            
            var departments = userObj.AdminRoles.Select(k => k.Department.DepartmentName).ToList();
            
            return departments;
        }

        
        public async Task<List<DepartmentGetDto>> GetDepartmentsForManage()
        {
            var department = await _context.Department
                .Include(k=>k.Admins)
                .ToListAsync();
            return _mapper.Map<List<DepartmentGetDto>>(department);
        }
    }
}
