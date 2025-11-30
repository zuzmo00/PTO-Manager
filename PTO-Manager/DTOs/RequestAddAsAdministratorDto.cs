using PTO_Manager.DTOs.Enums;

namespace PTO_Manager.DTOs;

public class RequestAddAsAdministratorDto
{
    public string UserId { get; set; }
    public DateOnly Begin_Date { get; set; }
    public DateOnly End_Date { get; set; }
    public ReservationType RequestType { get; set; }
}