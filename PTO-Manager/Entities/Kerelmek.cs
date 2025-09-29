using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.Entities
{
    public enum SzabadsagTious
    {
        Fizetett,
        FizetesNelkuli,
        BetegsegMiatti,
        Egyeb
    }
    public class Kerelmek
    {
        [Key]
        public Guid Id { get; set; } = new Guid();
        public int Torzsszam { get; set; }
        public DateOnly AdottNap { get; set; }
        public int KerelemSazm { get; set; }
        public SzabadsagTious Tipus { get; set; }
        public int MosdositoSzemelyId { get; set; }
        public DateOnly ModositasiIdo { get; set; }


    }
}
