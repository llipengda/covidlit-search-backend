using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CovidLitSearch.Models.DTO;

public class ArticleDTO : Article
{
    [Column("journal_name")]
    public string? JournalName { get; set; } = null!;

    [JsonIgnore]
    [Column("author")]
    public string? Author { get; set; } = null!;

    [NotMapped]
    public IList<string> Authors { get; set; } = [];

    [Column("volume")]
    public string? Volume { get; set; }

    [Column("pages")]
    public string? Pages { get; set; }

    [NotMapped]
    [JsonIgnore]
    public override ICollection<History> Histories { get; set; } = [];

    [NotMapped]
    [JsonIgnore]
    public override ICollection<Publish> Publishes { get; set; } = [];

    [NotMapped]
    [JsonIgnore]
    public override ICollection<Author> AuthorNames { get; set; } = [];

    [NotMapped]
    [JsonIgnore]
    public override ICollection<Article> Citeds { get; set; } = [];

    [NotMapped]
    [JsonIgnore]
    public override ICollection<Article> Citings { get; set; } = [];

    [NotMapped]
    [JsonIgnore]
    public override ICollection<User> Users { get; set; } = [];
}
