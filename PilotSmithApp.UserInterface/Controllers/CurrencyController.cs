using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.DataAccessObject.DTO;
using SAMTool.DataAccessObject.DTO;
using PilotSmithApp.BusinessService.Contract;
using AutoMapper;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class CurrencyController : Controller
    {

        ICurrencyBusiness _currencyBusiness;    
        public CurrencyController(ICurrencyBusiness currencyBusiness)
        {
            _currencyBusiness = currencyBusiness;
        }
        // GET: Currency
        public ActionResult Index()
        {
            return View();
        }

        #region AddCurrency
        public ActionResult AddCurrency()
        {
            CurrencyViewModel currencyVM = new CurrencyViewModel();
            currencyVM.IsUpdate = false;
            //currencyVM.CurrencyRate = 1;
            return PartialView("_CurrencyPopup", currencyVM);
        }
        #endregion AddCurrency

        #region CurrencySelectList
        public ActionResult CurrencySelectList(string required,bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            CurrencyViewModel currencyVM = new CurrencyViewModel();
            currencyVM.CurrencyList = Mapper.Map<List<Currency>, List<CurrencyViewModel>>(_currencyBusiness.GetCurrencyForSelectList());
            return PartialView("_CurrencySelectList", currencyVM);
        }
        #endregion CurrencySelectList
    }
}