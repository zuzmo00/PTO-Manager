using PTO_Manager.Entities.Enums;

namespace PTO_Manager.DTOs;

public class GetUsersGetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int EmployeeId { get; set; }
    public string DepartmentName { get; set;} 
    public string Role { get; set; }
    public string Email { get; set; }
}
