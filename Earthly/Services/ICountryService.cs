using Earthly.Contracts.V1.Response;
using Earthly.Domain;

namespace Earthly.Services;

public interface ICountryService
{
    public Task<IEnumerable<Country>> GetAllAsync();
    public Task<Country> GetCountryAsync(string query);
}