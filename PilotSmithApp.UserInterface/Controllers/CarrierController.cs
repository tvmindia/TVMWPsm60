using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class CarrierController : Controller
    {
        #region Constructor_Injection
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        ICarrierBusiness _carrierBusiness;
        public CarrierController(ICarrierBusiness carrierBusiness)
        {
            _carrierBusiness = carrierBusiness;
        }
        #endregion Constructor_Injection
        // GET: Carrier
        public ActionResult Index()
        {
            return View();
        }
        #region CarrierSelectList
        public ActionResult CarrierSelectList(string required)
        {
            ViewBag.IsRequired = required;
            CarrierViewModel carrierVM = new CarrierViewModel();
            carrierVM.CarrierSelectList = _carrierBusiness.GetCarrierForSelectList();
            return PartialView("_CarrierSelectList", carrierVM);
        }
        #endregion UnitSelectList
    }
}