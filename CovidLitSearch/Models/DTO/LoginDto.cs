namespace CovidLitSearch.Models.DTO;

public class LoginDto : UserDto
{
    public string Email { get; set; } = null!;

    public string? Token { get; set; }
}
