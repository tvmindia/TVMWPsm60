using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
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
        [AuthSecurityFilter(ProjectObject = "Carrier", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }
        #region CarrierSelectList
        public ActionResult CarrierSelectList(string required,bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "Carrier");
            if (permission.SubPermissionList.Count>0)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListCarrierAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            CarrierViewModel carrierVM = new CarrierViewModel();
            carrierVM.CarrierSelectList = _carrierBusiness.GetCarrierForSelectList();
            return PartialView("_CarrierSelectList", carrierVM);
        }
        #endregion UnitSelectList
    }
}