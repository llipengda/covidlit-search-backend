using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CovidLitSearch.Models.Enums;

namespace CovidLitSearch.Models;

[Table("user")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nickname")]
    [StringLength(100)]
    public string Nickname { get; set; } = null!;

    [Column("email")]
    [StringLength(200)]
    public string Email { get; set; } = null!;

    [Column("password")]
    [StringLength(100)]
    public string Password { get; set; } = null!;

    [Column("avatar")]
    [StringLength(500)]
    public string? Avatar { get; set; }

    [Column("motto")]
    [StringLength(1000)]
    public string? Motto { get; set; }

    [Column("collage")]
    [StringLength(500)]
    public string? Collage { get; set; }

    [Column("subscribe_email")]
    public bool? SubscribeEmail { get; set; }

    [Column("save_history")]
    public bool? SaveHistory { get; set; }

    [Column("role")]
    public UserRole Role { get; set; }

    [Column("salt")]
    [StringLength(32)]
    public string Salt { get; set; } = null!;
}
