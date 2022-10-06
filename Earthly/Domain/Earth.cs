using System.ComponentModel.DataAnnotations;

namespace Earthly.Domain;

public class Earth
{
    [Key] 
    public int Id { get; set; }
    public int NumberOfCountries { get; set; }
    
}