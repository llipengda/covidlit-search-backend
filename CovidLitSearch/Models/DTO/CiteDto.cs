using System.ComponentModel.DataAnnotations.Schema;

namespace CovidLitSearch.Models.DTO;

public class CiteDto
{
    [Column("id")] 
    public string Id { get; set; } = null!;

    [Column("title")] 
    public string Title { get; set; } = null!;
}