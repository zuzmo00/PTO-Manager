using PTO_Manager.Entities.Enums;
using PTO_Manager.Entities;

namespace PTO_Manager.DTOs
{
    public class RequiestGetDto
    {
        public Guid SzemelyId { get; set; }
        public SzabStatusz Statusz { get; set; } 
        public DateOnly Datum { get; set; }
        public Guid KerelemSzam { get; set; }
        public SzabadsagTipus Tipus { get; set; } // modositani kellene majd mert van még egy ugyan ilyen dto azonos tartalommal, csak bővebben
        public List< SpecialDays>? SpecialDays { get; set; }
    }
}
