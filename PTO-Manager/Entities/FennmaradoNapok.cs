using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.Entities
{
    public class FennmaradoNapok
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateOnly Ev { get; set; } 
        public int OsszeesSzab { get; set; }
        public int EddigKivett { get; set; }
        public int Fuggoben { get; set; }

    }
}
