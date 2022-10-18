using Earthly.Contracts.V1;
using Earthly.Contracts.V1.Response;
using Earthly.Filter;
using Earthly.Services;
using Microsoft.AspNetCore.Mvc;

namespace Earthly.Controllers;

[ApiKeyAuth]
public class CountryController : Controller
{

    private readonly ICountryService _countryService;

    public CountryController(ICountryService countryService)
    {
        _countryService = countryService;
    }

    [HttpGet(ApiRoutes.Countries.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] string iso3)
    {

        if (!string.IsNullOrEmpty(iso3))
        {
            var country = await _countryService.GetCountryAsync(iso3);
            
            if (country == null)
                return NotFound();

            var countryResponse =  new CountryResponse
            {
                Name = country.Name,
                CountryCode = country.Code,
                ISO3 = country.ISO3,
                Population = country.Population,
                GmtOffset = country.GMTOffset,
                TimeStamp = DateTimeOffset.UtcNow
                    .AddSeconds(Convert.ToDouble(country.GMTOffset)).ToUnixTimeSeconds().ToString()
            };
            
            return Ok(countryResponse);
        }


        var countries = await _countryService.GetAllAsync();
        
        return Ok(countries.Select(x =>  new CountryResponse
        {
            Name = x.Name,
            CountryCode = x.Code,
            ISO3 = x.ISO3,
            GmtOffset = x.GMTOffset,
            Population = x.Population,
            TimeStamp = DateTimeOffset.UtcNow
                .AddSeconds(Convert.ToDouble(x.GMTOffset)).ToUnixTimeSeconds().ToString()
        })
            .OrderBy(x => x.Name));
    }

   

   
}