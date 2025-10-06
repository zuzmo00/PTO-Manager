using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PTO_Manager.Entities.Enums;

namespace PTO_Manager.Entities
{
 
    public class Kerelmek
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [ForeignKey("Szemely")]
        public Guid SzemelyId { get; set; }
        public DateOnly Datum { get; set; }
        public Guid KerelemSzam { get; set; }
        public SzabadsagTipus Tipus { get; set; }
        public int MosdositoSzemelyId { get; set; }
        public DateOnly ModositasiIdo { get; set; }
        public SzabStatusz Statusz { get; set; }
        public string Megjegyzes { get; set; }

        public Szemelyek Szemely { get; set; }
    }
}
