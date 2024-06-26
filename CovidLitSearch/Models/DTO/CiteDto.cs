using System.ComponentModel.DataAnnotations.Schema;

namespace CovidLitSearch.Models.DTO;

public class CiteDto
{
    [Column("id")] 
    public string Id { get; set; } = null!;

    [Column("title")] 
    public string? Title { get; set; }

    [Column("flag")] public int Flag { get; set; } = 0;
    
    [Column("citing_title")] public string? CitingTitle { get; set; }
}