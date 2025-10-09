using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PTO_Manager.Entities.Enums;

namespace PTO_Manager.Entities
{
    public class User
    {
        [Required,Key]
        public Guid Id { get; set; }= Guid.NewGuid();
        public string Nev { get; set; }
        public int Torzsszam { get; set; }
        
        [ForeignKey("Reszleg")]
        public int ReszlegId { get; set; }
        public Department Reszleg { get; set; }
        
        public Roles Role { get; set; }=Roles.User;
        public string Email { get; set; }
        public string Jelszo { get; set; }
        
        public RemainingDay FennmaradoNapok { get; set; }
        public List<Admin>? Ugyintezo { get; set; }
        public List<Request>? Kerelmek { get; set; }
    }
}
