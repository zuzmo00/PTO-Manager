namespace PTO_Manager.DTOs;

public class PermissionGetDto
{
    public string departmentId { get; set; }
    public string departmentName { get; set; }
    public bool canRequest { get; set; }
    public bool canDecide { get; set; }
    public bool canRevoke { get; set; }
}