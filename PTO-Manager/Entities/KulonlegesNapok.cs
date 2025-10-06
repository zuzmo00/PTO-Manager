using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.Entities
{
    public class KulonlegesNapok
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateOnly Datum { get; set; }
        public bool MukaszunetiNap { get; set; }
    }
}
