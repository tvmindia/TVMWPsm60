using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface ICountryRepository
    {
        object InsertUpdateCountry(Country plant);
        List<Country> GetAllCountry(CountryAdvanceSearch countryAdvanceSearch);
        Country GetCountry(int code);
        bool CheckCountryExist(Country plant);
        object DeleteCountry(int code);
        List<Country> GetCountryForSelectList();
    }
}
