using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Earthly.Domain;

public class Country
{
    [Key]
    public string Name { get; init; }

    public int EarthId { get; set; }
    public long Population { get; set; }
    public string? Code { get; set; }
    public string? ISO3 { get; set; }
    public string? GMTOffset { get; set; }
    [ForeignKey(nameof(EarthId))]
    public Earth Earth { get; set; }
    public Country()
    {
        EarthId = 1;
    }
   
}