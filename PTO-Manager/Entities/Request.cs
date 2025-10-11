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
        public Guid KerelemSzam { get; set; }=Guid.NewGuid();
        public SzabadsagTipus Tipus { get; set; }
        
        public Guid? MosdositoSzemelyId { get; set; }
        public DateOnly? ModositasiIdo { get; set; }
        
        public string? Megjegyzes { get; set; }=string.Empty;


    }
}
