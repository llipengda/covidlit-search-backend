using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Models;

[Table("article")]
[Index("Id", Name = "idx_article_id")]
[Index("Title", Name = "idx_article_title")]
public partial class Article
{
    [Key]
    [Column("id")]
    [StringLength(50)]
    public string Id { get; set; } = null!;

    [Column("title")]
    public string Title { get; set; } = null!;

    [Column("abstract")]
    public string? Abstract { get; set; }

    [Column("doi")]
    [StringLength(100)]
    public string? Doi { get; set; }

    [Column("license")]
    [StringLength(50)]
    public string? License { get; set; }

    [Column("publish_time", TypeName = "timestamp(6) without time zone")]
    public DateTime? PublishTime { get; set; }

    [Column("url")]
    [StringLength(800)]
    public string? Url { get; set; }

    [Column("study_type")]
    [StringLength(500)]
    public string? StudyType { get; set; }

    [Column("addressed_population")]
    [StringLength(1000)]
    public string? AddressedPopulation { get; set; }

    [Column("challenge")]
    [StringLength(2000)]
    public string? Challenge { get; set; }

    [Column("focus")]
    [StringLength(100)]
    public string? Focus { get; set; }
}
