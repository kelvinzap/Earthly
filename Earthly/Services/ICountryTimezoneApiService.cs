using Earthly.Data;

namespace Earthly.Services;

public interface ICountryTimezoneApiService
{
    Task GetAllCountryTimeZones(DataContext dataContext);
}