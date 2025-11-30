namespace PTO_Manager.DTOs;

public class PermissionUpdateDto
{
    public  Guid UserId { get; set; }
    public int DepartmentId { get; set; }
        
    public bool CanRequest { get; set; }
        
    public bool CanDecide { get; set; }
        
    public bool CanRevoke { get; set; }
}