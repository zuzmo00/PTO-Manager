using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTO_Manager.Entities
{
    public class Admin
    {
        [Required,Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [ForeignKey("Szemely")]
        public Guid SzemelyId { get; set; }
        public User Szemely { get; set; }
        
        [ForeignKey("Reszleg")]
        public int ReszlegId { get; set; }
        public Department Reszleg { get; set; }
        
       public bool Kerhet { get; set; }
       public bool Biralhat { get; set; }
       public bool Visszavonhat { get; set; }
    }
}
