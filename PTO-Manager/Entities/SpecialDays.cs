using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.Entities
{
    public class SpecialDays
    {
        [Required,Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateOnly Date { get; set; }
        public bool IsWorkingDay { get; set; }
    }
}
