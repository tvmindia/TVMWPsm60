using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Service
{
    public class CountryBusiness:ICountryBusiness
    {
        ICountryRepository _countryRepository;
        public CountryBusiness(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        public object InsertUpdateCountry(Country country)
        {
            return _countryRepository.InsertUpdateCountry(country);
        }
        public List<Country> GetAllCountry(CountryAdvanceSearch countryAdvanceSearch)
        {
            return _countryRepository.GetAllCountry(countryAdvanceSearch);
        }
        public Country GetCountry(int code)
        {
            return _countryRepository.GetCountry(code);
        }
        public bool CheckCountryExist(Country country)
        {
            return _countryRepository.CheckCountryExist(country);
        }
        public object DeleteCountry(int code)
        {
            return _countryRepository.DeleteCountry(code);
        }
        public List<SelectListItem> GetCountryForSelectList()
        {
            List<SelectListItem> selectListItem = null;
            List<Country> countryList = _countryRepository.GetCountryForSelectList();
            return selectListItem = countryList != null ? (from country in countryList
                                                           select new SelectListItem
                                                         {
                                                             Text = country.Description,
                                                             Value = country.Code.ToString(),
                                                             Selected = false
                                                         }).ToList() : new List<SelectListItem>();
        }
    }
}
