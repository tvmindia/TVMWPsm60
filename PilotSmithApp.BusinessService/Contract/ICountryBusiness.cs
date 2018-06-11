using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface ICountryBusiness
    {
        object InsertUpdateCountry(Country plant);
        List<Country> GetAllCountry(CountryAdvanceSearch countryAdvanceSearch);
        Country GetCountry(int code);
        bool CheckCountryExist(Country plant);
        object DeleteCountry(int code);
        List<SelectListItem> GetCountryForSelectList();
    }
}
