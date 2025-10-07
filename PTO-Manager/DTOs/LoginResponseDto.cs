namespace PTO_Manager.DTOs;

public class LoginResponseDto
{
    public string token { get; set; }
    public List<ReszlegGetDto>? ugyintezoiJogosultsagok { get; set; }
}