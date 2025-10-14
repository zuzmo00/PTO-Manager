namespace PTO_Manager.DTOs;

public class RemainingDayGetDto
{
    public int AllHoliday { get; set; }
    public int RemainingDays { get; set; } = 0;
    public int TimeProportional { get; set; } // It has to be calculated seperatly, Cannot be mapped
    
}