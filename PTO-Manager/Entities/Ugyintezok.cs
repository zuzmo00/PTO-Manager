using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTO_Manager.Entities
{
    public class Ugyintezok
    {
        [Required,Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [ForeignKey("Szemely")]
        public Guid SzemelyId { get; set; }
        public Szemelyek Szemely { get; set; }
        
        [ForeignKey("Reszleg")]
        public int ReszlegId { get; set; }
        public Reszleg Reszleg { get; set; }
        
       public bool Kerhet { get; set; }
       public bool Biralhat { get; set; }
       public bool Visszavonhat { get; set; }
    }
}
