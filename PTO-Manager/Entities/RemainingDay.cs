using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTO_Manager.Entities
{
    public class RemainingDay
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        public DateOnly Year { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public int AllHoliday { get; set; }
        public int RemainingDays { get; set; } = 0; // Átírni megmaradtra, vagy valamire,hogy azt reprezentálja mennyi maradt még 
    }
}
