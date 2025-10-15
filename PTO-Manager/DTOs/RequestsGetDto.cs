using PTO_Manager.DTOs.Enums;
using PTO_Manager.Entities.Enums;
using PTO_Manager.Entities;

namespace PTO_Manager.DTOs
{
    public class RequestsGetDto
    {
        public string Id { get; set; }
        public DateOnly Date { get; set; }
        public ReservationType Type { get; set; } 
    }
}
