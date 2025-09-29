using System.ComponentModel.DataAnnotations;
using PTO_Manager.Entities.Enums;

namespace PTO_Manager.Entities
{
 
    public class Kerelmek
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Torzsszam { get; set; }
        public DateOnly AdottNap { get; set; }
        public int KerelemSazm { get; set; }
        public SzabadsagTipus Tipus { get; set; }
        public int MosdositoSzemelyId { get; set; }
        public DateOnly ModositasiIdo { get; set; }


    }
}
