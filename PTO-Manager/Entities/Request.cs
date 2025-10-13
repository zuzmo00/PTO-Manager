using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PTO_Manager.Entities.Enums;

namespace PTO_Manager.Entities
{
 
    public class Request
    {
        [Required,Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        public SzabStatusz Statusz { get; set; }=SzabStatusz.Fuggoben;
        public DateOnly Date { get; set; }
        public Guid KerelemSzam { get; set; } //Ez autogen volt, de kiszedtem, mert csak problémtá okozna valszeg a tobb napos szaboknal
        public SzabadsagTipus Tipus { get; set; } //Ezt egyesiteni kellen a DOT-ban hasznalatos Typeokkal mert sokkal kifejezobb az és akkor mar egy helyen van
        
        public Guid? MosdositoSzemelyId { get; set; }
        public DateOnly? ModositasiIdo { get; set; }


    }
}
