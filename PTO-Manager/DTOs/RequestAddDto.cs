using PTO_Manager.Entities.Enums;
using PTO_Manager.Entities;

namespace PTO_Manager.DTOs
{
    public class RequestAddDto
    {
        public DateOnly Datum { get; set; }
        public SzabadsagTipus Tipus { get; set; }
    }
}
