using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PTO_Manager.Entities.Enums;

namespace PTO_Manager.Entities
{
 
    public class Request
    {
        [Required,Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [ForeignKey("Szemely")]
        public Guid SzemelyId { get; set; }
        public User Szemely { get; set; }
        
        public SzabStatusz Statusz { get; set; }
        public DateOnly Datum { get; set; }
        public Guid KerelemSzam { get; set; }
        public SzabadsagTipus Tipus { get; set; }
        
        public Guid MosdositoSzemelyId { get; set; }
        public DateOnly ModositasiIdo { get; set; }
        
        public string Megjegyzes { get; set; }

        
    }
}
