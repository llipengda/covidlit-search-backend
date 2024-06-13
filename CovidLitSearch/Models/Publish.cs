﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Models;

[PrimaryKey("JournalName", "ArticleId")]
[Table("publish")]
[Index("ArticleId", Name = "idx_publish_arid")]
[Index("JournalName", Name = "idx_publish_jrname")]
public partial class Publish
{
    [Key]
    [Column("journal_name")]
    [StringLength(100)]
    public string JournalName { get; set; } = null!;

    [Key]
    [Column("article_id")]
    [StringLength(50)]
    public string ArticleId { get; set; } = null!;

    [Column("volume")]
    [StringLength(200)]
    public string? Volume { get; set; }

    [Column("pages")]
    [StringLength(200)]
    public string? Pages { get; set; }

    [ForeignKey("ArticleId")]
    [InverseProperty("Publishes")]
    public virtual Article Article { get; set; } = null!;

    [ForeignKey("JournalName")]
    [InverseProperty("Publishes")]
    public virtual Journal JournalNameNavigation { get; set; } = null!;
}