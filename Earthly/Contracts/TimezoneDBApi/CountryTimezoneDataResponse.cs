using Newtonsoft.Json;

namespace Earthly.Contracts.TimezoneDBApiResponses;


public class Result
{
    public string status { get; set; }
    public string message { get; set; }
    public Zones zones { get; set; }
}

public class CountryTimezoneDataResponse
{
    [JsonProperty("?xml")]
    public Xml Xml { get; set; }
    public Result result { get; set; }
}

public class Xml
{
    [JsonProperty("@version")]
    public string Version { get; set; }

    [JsonProperty("@encoding")]
    public string Encoding { get; set; }
}

public class Zone
{
    public string countryCode { get; set; }
    public string countryName { get; set; }
    public string zoneName { get; set; }
    public string gmtOffset { get; set; }
    public string timestamp { get; set; }
}

public class Zones
{
    public List<Zone> zone { get; set; }
}
