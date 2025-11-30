using PTO_Manager.Entities.Enums;

namespace PTO_Manager.DTOs;

public class GetUsersInputDTO
{
    public List<string> DepartmentIds { get; set; }
    public string inputText { get; set; }
}