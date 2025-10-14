using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.Entities
{
    public class Log
    {
        [Required,Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateOnly Date { get; set; }
        public required string Descirption { get; set; }
        

    }
}
