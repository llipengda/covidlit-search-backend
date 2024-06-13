using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Models;

[Table("author")]
[Index("Name", Name = "idx_author_name")]
public partial class Author
{
    [Key]
    [Column("name")]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Column("email")]
    [StringLength(1000)]
    public string? Email { get; set; }

    [Column("lab")]
    [StringLength(1000)]
    public string? Lab { get; set; }

    [Column("institution")]
    [StringLength(1000)]
    public string? Institution { get; set; }

    [Column("country")]
    [StringLength(100)]
    public string? Country { get; set; }

    [Column("post_code")]
    [StringLength(100)]
    public string? PostCode { get; set; }

    [Column("settlement")]
    [StringLength(100)]
    public string? Settlement { get; set; }
}
