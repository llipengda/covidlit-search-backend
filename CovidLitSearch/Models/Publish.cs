using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Models;

[PrimaryKey("JournalName", "ArticleId")]
[Table("publish")]
[Index("ArticleId", Name = "publish_article_id_idx")]
[Index("JournalName", Name = "publish_journal_name_idx")]
public class Publish
{
    [Key]
    [Column("journal_name")]
    [StringLength(100)]
    public string JournalName { get; set; } = null!;

    [Key]
    [Column("article_id")]
    [StringLength(50)]
    public string ArticleId { get; set; } = null!;

    [Column("volume")]
    [StringLength(200)]
    public string? Volume { get; set; }

    [Column("pages")]
    [StringLength(200)]
    public string? Pages { get; set; }
}
