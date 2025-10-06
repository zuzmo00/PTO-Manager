namespace PTO_Manager.DTOs;

public class ReszlegGetDto
{
    public int ReszlegId { get; set; }
        
    public bool Kerhet { get; set; }
    public bool Biralhat { get; set; }
    public bool Visszavonhat { get; set; }
}