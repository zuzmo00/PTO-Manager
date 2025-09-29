using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.Entities
{
    public class Log
    {
        [Key]
        public Guid Id { get; set; } = new Guid();
        public string Megjegyzes { get; set; }
        public DateOnly Datum { get; set; }

    }
}
