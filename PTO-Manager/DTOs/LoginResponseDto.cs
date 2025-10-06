namespace PTO_Manager.DTOs;

public class LoginResponseDto
{
    public string Token { get; set; }
    public List<ReszlegGetDto>? Reszlegek { get; set; }
}