using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.Entities
{
    public class KulonlegesNapok
    {
        [Key]
        public Guid Id { get; set; } = new Guid();
        public DateOnly Datum { get; set; }
        public DateOnly Szunet { get; set; }
    }
}
