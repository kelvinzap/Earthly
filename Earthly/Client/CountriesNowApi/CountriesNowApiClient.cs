using Earthly.Data;
using Earthly.Domain;
using Earthly.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Earthly.Client.CountriesNowApi;

public class CountriesDataApiService : ICountriesDataApiService
{

    public async Task GetAllCountriesAndData(DataContext _dataContext)
    {
        var httpClient = new HttpClient();

        //Made use of countriesNow API
        var request = await httpClient.GetAsync("https://countriesnow.space/api/v0.1/countries/population");
        var requestContent = await request.Content.ReadAsStringAsync();


            var countriesAndPopulationData =
                JsonConvert
                    .DeserializeObject<CountryPopulationDataResponse>(requestContent);

            var countriesPopulationData = countriesAndPopulationData.data.Select(x => new Country
            {
                Name = x.country,
                Code = x.code.Remove(x.code.Length - 1),
                ISO3 = x.iso3,
                Population = x.populationCounts.LastOrDefault().value
            });

            var countriesInDb = await _dataContext.Countries.ToListAsync();

            foreach (var country in countriesPopulationData)
            {
                if (!countriesInDb.Exists(x => x.Name == country.Name))
                {
                    await _dataContext.Countries.AddAsync(country);
                }

                countriesInDb.ForEach(x =>
                {
                    if (x.Code == country.Code && x.Population != country.Population)
                    {
                        x.Population = country.Population;
                    }
                });

            }

            await _dataContext.SaveChangesAsync();
        
    }

 
}