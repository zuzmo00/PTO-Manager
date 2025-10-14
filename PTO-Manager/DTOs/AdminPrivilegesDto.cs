namespace PTO_Manager.DTOs;

public class AdminPrivilegesDto
{
    public int DepartmentId { get; set; }
    
    public bool CanRequest { get; set; }
    
    public bool CanDecide { get; set; }
    
    public bool CanRevoke { get; set; }
}