using System.ComponentModel.DataAnnotations.Schema;

namespace CovidLitSearch.Models.Common;

public class CountType
{
    [Column("count")]
    public int Count { get; set; }
}