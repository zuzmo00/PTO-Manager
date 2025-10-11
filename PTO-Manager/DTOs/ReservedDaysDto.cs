using PTO_Manager.DTOs.Enums;

namespace PTO_Manager.DTOs;

public class ReservedDaysDto
{
    public DateOnly reservedDay { get; set; }
    public ReservationType reservationType { get; set; } //It will be sent back as an integer
}