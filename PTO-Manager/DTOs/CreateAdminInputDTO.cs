namespace PTO_Manager.DTOs;

public class CreateAdminInputDTO
{
    public Guid id { get; set; }
    public string departmentName { get; set; }
    
    public bool CanRequest { get; set; }
    public bool CanDecide { get; set; }
    public bool CanRevoke { get; set; }
}