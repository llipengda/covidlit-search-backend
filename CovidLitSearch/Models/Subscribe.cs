using System.ComponentModel.DataAnnotations.Schema;

namespace CovidLitSearch.Models;

public class Subscribe
{
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("journal_name")]
    public string JournalName { get; set; } = null!;
}