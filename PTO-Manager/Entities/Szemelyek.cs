using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PTO_Manager.Entities.Enums;

namespace PTO_Manager.Entities
{
    public class Szemelyek
    {
        [Required,Key]
        public Guid Id { get; set; }= Guid.NewGuid();
        public string Nev { get; set; }
        public int Torzsszam { get; set; }
        
        [ForeignKey("Reszleg")]
        public int ReszlegId { get; set; }
        public Reszleg Reszleg { get; set; }
        
        public Roles Role { get; set; }
        public string Email { get; set; }
        public string Jelszo { get; set; }
        
        public FennmaradoNapok FennmaradoNapok { get; set; }
        public List<Ugyintezok>? Ugyintezo { get; set; }
        public List<Kerelmek>? Kerelmek { get; set; }
    }
}
