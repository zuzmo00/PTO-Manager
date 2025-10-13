using PTO_Manager.Entities.Enums;

namespace PTO_Manager.DTOs;

public class RequestAddAsUserDto
{
    public DateOnly Begin_Date { get; set; }
    public DateOnly End_Date { get; set; }
}