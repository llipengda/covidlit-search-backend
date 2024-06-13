using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Models;

[Table("journal")]
public partial class Journal
{
    [Key]
    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(1000)]
    public string? Description { get; set; }

    [InverseProperty("JournalNameNavigation")]
    public virtual ICollection<Publish> Publishes { get; set; } = new List<Publish>();

    [ForeignKey("JournalName")]
    [InverseProperty("JournalNames")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
