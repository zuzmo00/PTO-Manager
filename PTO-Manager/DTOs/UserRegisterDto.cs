using PTO_Manager.Entities.Enums;
using PTO_Manager.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.DTOs
{
    public class UserRegisterDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public int Employeeid { get; set; }
        public string DepartmentName { get; set; }
        public string Password { get; set; }
        public int AllHoliday { get; set; }
    }
    
    public class AdminCreateDto
    {

        public Guid UserId { get; set; }
        
        public int DepartmentId { get; set; }

        public bool CanRequest { get; set; }
        public bool CanDecide { get; set; }
        public bool CanRevoke { get; set; }
    }
}
