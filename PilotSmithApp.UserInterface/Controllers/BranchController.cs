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
    public class BranchController : Controller
    {
        IBranchBusiness _branchBusiness;
        IUserBusiness _userBusiness;
        public BranchController(IBranchBusiness branchBusiness, IUserBusiness userBusiness)
        {
            _branchBusiness = branchBusiness;
            _userBusiness=userBusiness;
    }
        // GET: Branch
        public ActionResult Index()
        {
            return View();
        }
        #region Branch SelectList
        public ActionResult BranchSelectList(string required, bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _userBusiness.GetSecurityCode(appUA.UserName, "Branch");
            if (permission.SubPermissionList != null)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            BranchViewModel branchVM = new BranchViewModel();
            branchVM.BranchList = _branchBusiness.GetBranchForSelectList(appUA.UserName);
            return PartialView("_BranchSelectList", branchVM);
        }
        #endregion Branch SelectList
    }
}