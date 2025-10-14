using PTO_Manager.DTOs.Enums;
using PTO_Manager.Entities.Enums;
using PTO_Manager.Entities;

namespace PTO_Manager.DTOs
{
    public class RequiestGetDto
    {
        public Guid UserId { get; set; }
        public HolidayStatus Status { get; set; } 
        public DateOnly Date { get; set; }
        public Guid RequestNumber { get; set; }
        public ReservationType Type { get; set; } // modositani kellene majd mert van még egy ugyan ilyen dto azonos tartalommal, csak bővebben
        public List< SpecialDays>? SpecialDays { get; set; }
    }
}
