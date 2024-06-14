using System.ComponentModel.DataAnnotations.Schema;

namespace CovidLitSearch.Models.DTO;

public class HistoryDto : History
{
    [Column("title")]
    public string Title { get; set; } = null!;
    
    [Column("authors")]
    public string? Authors { get; set; }
    
    [Column("abstract")]
    public string? Abstract { get; set; }
    
    [Column("journal_name")]
    public string JournalName { get; set; } = null!;
}