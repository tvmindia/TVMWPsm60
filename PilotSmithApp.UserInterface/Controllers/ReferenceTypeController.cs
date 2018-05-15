using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class ReferenceTypeController : Controller
    {
        AppConst _appConstant = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IReferenceTypeBusiness _referenceTypeBusiness;
        IUserBusiness _userBusiness;
        public ReferenceTypeController(IReferenceTypeBusiness referenceTypeBusiness, IUserBusiness userBusiness)
        {
            _referenceTypeBusiness = referenceTypeBusiness;
            _userBusiness = userBusiness;
        }
        // GET: ReferenceType
        public ActionResult Index()
        {
            return View();
        }

        #region ReferenceType SelectList
        public ActionResult ReferenceTypeSelectList(string required, bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _userBusiness.GetSecurityCode(appUA.UserName, "ReferenceType");
            if (permission.SubPermissionList != null)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            ReferenceTypeViewModel referenceTypeVM = new ReferenceTypeViewModel();
            referenceTypeVM.ReferenceTypeSelectList = _referenceTypeBusiness.GetReferenceTypeSelectList();
            return PartialView("_ReferenceTypeSelectList", referenceTypeVM);
        }
        #endregion ReferenceType SelectList

    }
}