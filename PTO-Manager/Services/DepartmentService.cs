using AutoMapper;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;

namespace PTO_Manager.Services
{
    public interface IDepartmentService
    {
        public Task<int> CreateDepartment(CreateDepartmentDto departmentName);
        public Task<int> RemoveDepartment(int id);
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
                ReszlegNev = departmentName.ReszlegNev
            };
            await _context.Department.AddAsync(newDepartment);
            await _context.SaveChangesAsync();
            return newDepartment.Id;
        }
        public async Task<int> RemoveDepartment(int id)
        {
            var department = _context.Department.FindAsync(id);
            if (department == null)
            {
                throw new Exception("Department not found");
            }
            _context.Department.Remove(department.Result);
            await _context.SaveChangesAsync();
            return id;
        }
    }
}
