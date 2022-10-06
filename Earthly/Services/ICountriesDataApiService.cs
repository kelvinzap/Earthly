using Earthly.Data;
using Earthly.Domain;

namespace Earthly.Services;

public interface ICountriesDataApiService
{
    Task GetAllCountriesAndData(DataContext _dataContext);
}