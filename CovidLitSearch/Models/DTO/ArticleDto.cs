using System.ComponentModel.DataAnnotations.Schema;

namespace CovidLitSearch.Models.DTO;

public class ArticleDto : Article
{
    [Column("journal_name")]
    public string? JournalName { get; set; } = null!;

    [Column("volume")]
    public string? Volume { get; set; }

    [Column("pages")]
    public string? Pages { get; set; }
    
}
