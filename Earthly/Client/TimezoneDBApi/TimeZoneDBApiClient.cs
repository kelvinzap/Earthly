using System.Xml.Linq;
using Earthly.Data;
using Earthly.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace Earthly.Client.TimezoneDBApi;

public class TimeZoneDBApiClient : ICountryTimezoneApiService
{
    private readonly string _timezoneDbApiKey;


    public TimeZoneDBApiClient(string timezoneDbApiKey)
    {
        _timezoneDbApiKey = timezoneDbApiKey;
    }

    public async Task GetAllCountryTimeZones(DataContext _dataContext)
    {
        var httpClient = new HttpClient();

        //Made use of timezoneDB API
        var request = await httpClient.GetAsync($"http://api.timezonedb.com/v2.1/list-time-zone?key={_timezoneDbApiKey}");
        var requestContent = await request.Content.ReadAsStringAsync();
        var xdoc = XDocument.Parse(requestContent);
      
        var countriesTimezoneDataJsonFormat = JsonConvert.SerializeXNode(xdoc);
        
        //Used json converter online to get the c# class equivalent of the json string
        var countriesTimezoneData = JsonConvert
            .DeserializeObject<CountryTimezoneDataResponse>(countriesTimezoneDataJsonFormat);

        var countriesInDb = await _dataContext.Countries.ToListAsync();

        foreach (var countryZone in countriesTimezoneData.result.zones.zone)
        {
            countriesInDb.ForEach(x =>
            {
                if (string.IsNullOrEmpty(x.GMTOffset) && x.Code == countryZone.countryCode)
                {
                    x.GMTOffset = countryZone.gmtOffset;
                }
            });
        }
        
        await _dataContext.SaveChangesAsync();

    }
}