namespace Earthly.Contracts.CountriesNowApiResponses;

public class CountryPopulationDataResponse
{
    public bool error { get; set; }
    public string msg { get; set; }
    public List<Data> data { get; set; }
}
public class Data
{
    public string country { get; set; }
    public string code { get; set; }
    public string iso3 { get; set; }
    public List<PopulationCount> populationCounts { get; set; }
}

public class PopulationCount
{
    public int year { get; set; }
    public long value { get; set; }
}


