using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTO_Manager.Entities
{
    public class FennmaradoNapok
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [ForeignKey("Szemely")]
        public Guid SzemelyId { get; set; }
        public Szemelyek Szemely { get; set; }
        
        public DateOnly Ev { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public int OsszeesSzab { get; set; }
        public int EddigKivett { get; set; } = 0;
        public int Fuggoben { get; set; }=0;
    }
}
