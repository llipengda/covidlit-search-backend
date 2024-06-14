using System.ComponentModel.DataAnnotations.Schema;

namespace CovidLitSearch.Models;

public class Collect
{
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("article_id")]
    public string ArticleId { get; set; } = null!;
}