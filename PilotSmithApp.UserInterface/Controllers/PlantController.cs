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

namespace PilotSmithApp.UserInterface.Controllers
{
    public class PlantController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IPlantBusiness _plantBusiness;
        IUserBusiness _userBusiness;
        
        public PlantController(IPlantBusiness plantBusiness,IUserBusiness userBusiness)
        {
            _plantBusiness = plantBusiness;
            _userBusiness = userBusiness;
        }
        // GET: Plant
        public ActionResult Index()
        {
            return View();
        }
        #region Plant SelectList
        public ActionResult PlantSelectList(string required,bool? disabled)
        {            
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _userBusiness.GetSecurityCode(appUA.UserName, "Plant");
            if (permission.SubPermissionList != null)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            PlantViewModel plantVM = new PlantViewModel();
            plantVM.PlantSelectList = _plantBusiness.GetPlantForSelectList();
            return PartialView("_PlantSelectList", plantVM);
        }
        #endregion Plant SelectList
    }
}