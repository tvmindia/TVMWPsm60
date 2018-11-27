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
    public class CurrencyBusiness:ICurrencyBusiness
    {
        ICurrencyRepository _currencyRepository;
        public CurrencyBusiness(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }
        public List<Currency> GetCurrencyForSelectList()
        {
            return _currencyRepository.GetCurrencyForSelectList();
             //selectListItem = currencyList != null ? (from currency in currencyList
                                                            //select new SelectListItem
                                                            //{
                                                            //    Text =currency.Code+"-"+currency.Description,
                                                            //    Value = currency.Code,
                                                            //    Selected = false,
                                                            //}).ToList() : new List<SelectListItem>();
        }
    }
}
