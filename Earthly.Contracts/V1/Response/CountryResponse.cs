namespace Earthly.Contracts.V1.Response;

public class CountryResponse
{
    public string Name { get; set; }
    public long Population { get; set; }
    public string GmtOffset { get; set; }
    public string CountryCode { get; set; }
    public string ISO3 { get; set; }
    public string TimeStamp { get; init; }

  
}