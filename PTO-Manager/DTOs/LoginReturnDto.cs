using PTO_Manager.Entities;

namespace PTO_Manager.DTOs;

public class LoginReturnDto
{
    public string token { get; set; }
    public List<AdminPrivilegesDto>? adminPrivileges { get; set; }
}