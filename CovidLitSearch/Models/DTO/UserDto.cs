using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidLitSearch.Models.DTO;

public class UserDto
{
    [Column("nickname")]
    public string? Nickname { get; set; } 

    [Column("avatar")]
    public string? Avatar { get; set; }

    [Column("motto")]
    public string? Motto { get; set; }

    [Column("collage")]
    public string? Collage { get; set; }

    [Column("subscribe_email")]
    public bool? SubscribeEmail { get; set; }

    [Column("save_history")]
    public bool? SaveHistory { get; set; }
}