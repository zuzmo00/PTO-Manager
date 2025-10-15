namespace PTO_Manager.DTOs;

public class PendingRequestBlockDto
{
    public string id { get; set; }
    public string name { get; set; }
    public string department { get; set; }
    public DateOnly begin { get; set; }
    public DateOnly end { get; set; }
}