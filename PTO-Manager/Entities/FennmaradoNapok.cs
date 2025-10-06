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
        
        public DateOnly Ev { get; set; } 
        public int OsszeesSzab { get; set; }
        public int EddigKivett { get; set; }
        public int Fuggoben { get; set; }
    }
}
