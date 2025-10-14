using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTO_Manager.Entities
{
    public class Admin
    {
        [Required,Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        
       public bool CanRequest { get; set; }
       public bool CanDecide { get; set; }
       public bool CanRevoke { get; set; }
    }
}
