using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.Entities;

public class Department
{
    [Required,Key]
    public int Id { get; set; }
    public string DepartmentName { get; set; }
    
    public List<User> Users { get; set; }
    public List<Admin> Admins { get; set; }
}