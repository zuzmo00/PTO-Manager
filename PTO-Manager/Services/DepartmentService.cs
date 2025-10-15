using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;

namespace PTO_Manager.Services
{
    public interface IDepartmentService
    {
        public Task<int> CreateDepartment(CreateDepartmentDto departmentName);
        public Task<int> RemoveDepartment(int id);
        public Task<List<string>> GetDepartments();
    }
    public class DepartmentService : IDepartmentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public DepartmentService (AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> CreateDepartment(CreateDepartmentDto departmentName)
        {
            var newDepartment = new Department
            {
                DepartmentName = departmentName.DepartmentName
            };
            await _context.Department.AddAsync(newDepartment);
            await _context.SaveChangesAsync();
            return newDepartment.Id;
        }
        public async Task<int> RemoveDepartment(int id)
        {
            var department = await _context.Department.FindAsync(id);
            if (department == null)
            {
                throw new Exception("Department not found");
            }
            _context.Department.Remove(department);
            await _context.SaveChangesAsync();
            return id;
        }
        public async Task<List<string>> GetDepartments()
        {
            var department = await _context.Department.Select(c => c.DepartmentName).ToListAsync();
            return department;
        }
    }
}
