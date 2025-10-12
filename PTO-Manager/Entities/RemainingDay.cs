using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTO_Manager.Entities
{
    public class RemainingDay
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [ForeignKey("Szemely")]
        public Guid SzemelyId { get; set; }
        public User Szemely { get; set; }
        
        public DateOnly Ev { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public int OsszesSzab { get; set; }
        public int EddigKivett { get; set; } = 0; // Átírni megmaradtra, vagy valamire,hogy azt reprezentálja mennyi maradt még 
        public int Fuggoben { get; set; }=0; // Ez pedig igazából felesleges, akár ki is lehet törölni
    }
}
