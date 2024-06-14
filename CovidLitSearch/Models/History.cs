using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Models;

[PrimaryKey("UserId", "ArticleId", "Time")]
[Table("history")]
public class History
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Key]
    [Column("article_id")]
    [StringLength(50)]
    public string ArticleId { get; set; } = null!;

    [Key]
    [Column("time", TypeName = "timestamp(6) without time zone")]
    public DateTime Time { get; set; }
}
