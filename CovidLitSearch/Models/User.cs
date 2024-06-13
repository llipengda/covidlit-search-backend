using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Models;

[Table("user")]
public partial class User
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

    [InverseProperty("User")]
    public virtual ICollection<History> Histories { get; set; } = new List<History>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Journal> JournalNames { get; set; } = new List<Journal>();
}
