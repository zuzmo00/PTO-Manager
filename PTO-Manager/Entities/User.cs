using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PTO_Manager.Entities.Enums;

namespace PTO_Manager.Entities
{
    public class User
    {
        [Required,Key]
        public Guid Id { get; set; }= Guid.NewGuid();
        public string Name { get; set; }
        public int Employeeid { get; set; }
        
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        
        public Roles Role { get; set; }=Roles.User;
        public string Email { get; set; }
        public string Password { get; set; }
        
        public RemainingDay RemainingDay { get; set; }
        public List<Admin> AdminRoles { get; set; }
        public List<Request>? Requests { get; set; }
    }
}
