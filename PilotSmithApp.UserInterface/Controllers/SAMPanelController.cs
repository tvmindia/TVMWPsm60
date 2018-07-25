using AutoMapper;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.SecurityFilter;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.DataAccessObject.DTO;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class SAMPanelController : Controller
    {
        //  public string ReadAccess;
        AppConst _appConstant = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        IHomeBusiness _homeBusiness;
        public SAMPanelController(IHomeBusiness home)
        { 
            _homeBusiness = home;
        }
    
        [AuthSecurityFilter(ProjectObject = "SAMPanel", Mode = "R")]
        public ActionResult Index() 
        {
            AppUA _appUA= Session["AppUA"] as AppUA;
            Permission _permission = _psaSysCommon.GetSecurityCode(_appUA.UserName, "SAMPanel");
            //Permission _permission = Session["UserRights"] as Permission;

            // string R = _permission.SubPermissionList.First(s => s.Name == "RHS").AccessCode;
            SAMPanelViewModel SAMPanelViewModel = new SAMPanelViewModel();
            List<SysMenuViewModel> SysMenuViewModelList = Mapper.Map<List<SysMenu>, List<SysMenuViewModel>>(_homeBusiness.GetAllSysLinks());
            if((_permission.SubPermissionList.Count>0? _permission.SubPermissionList.First(s => s.Name == "LHS").AccessCode:string.Empty).Contains("R"))
            {
                SAMPanelViewModel._LHSSysMenuViewModel = SysMenuViewModelList != null ? SysMenuViewModelList.Where(s => s.Type == "LHS").ToList() : new List<SysMenuViewModel>();
            }
            if ((_permission.SubPermissionList.Count>0 ? _permission.SubPermissionList.First(s => s.Name == "RHS").AccessCode : string.Empty).Contains("R"))
            {
                SAMPanelViewModel._RHSSysMenuViewModel = SysMenuViewModelList != null ? SysMenuViewModelList.Where(s => s.Type == "RHS").ToList() : new List<SysMenuViewModel>();
            }
            //Session.Remove("UserRights");
            return View(SAMPanelViewModel);
        }
    }
}