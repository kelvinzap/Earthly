using Earthly.Contracts.V1.Response;
using Earthly.Data;
using Earthly.Domain;
using Microsoft.EntityFrameworkCore;

namespace Earthly.Services;

public class CountryService : ICountryService
{
    private readonly DataContext _dataContext;
    
    public CountryService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<Country>> GetAllAsync()
    {
        return await _dataContext.Countries.ToListAsync();
    }

    public async Task<Country> GetCountryAsync(string query)
    {
        var country = await _dataContext.Countries.SingleOrDefaultAsync(x => 
            x.ISO3.ToLower() == query.ToLower());

        return country;
    }
}