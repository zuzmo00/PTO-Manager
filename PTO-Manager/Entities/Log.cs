using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.Entities
{
    public class Log
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Megjegyzes { get; set; }
        public DateOnly Datum { get; set; }

    }
}
