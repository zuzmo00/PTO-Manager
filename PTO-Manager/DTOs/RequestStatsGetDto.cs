namespace PTO_Manager.DTOs;

public class RequestStatsGetDto
{
    public int AllHoliday { get; set; }
    public int RemainingDays { get; set; }
    public int TimeProportional { get; set; }
    public int requiredDayOff { get; set; }
    public string startDate { get; set; }
    public string endDate { get; set; }
    
}
