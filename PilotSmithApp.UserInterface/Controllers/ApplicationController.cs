﻿using AutoMapper;
using Newtonsoft.Json;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PilotSmithApp.UserInterface.Models;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class ApplicationController : Controller
    {
        private IApplicationBusiness _applicationBusiness;
        PSASysCommon _pSASysCommon = new PSASysCommon();
        SecurityFilter.ToolBarAccess _tool;

        public ApplicationController(IApplicationBusiness applicationBusiness,SecurityFilter.ToolBarAccess tool)
        {
            _applicationBusiness = applicationBusiness;
            _tool = tool;
        }


        // GET: Application
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Application", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }


        #region InsertUpdateApplication

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Application", Mode = "W")]
        public string InsertUpdateApplication(ApplicationViewModel appObj)
        {
            object result = null;
            if (ModelState.IsValid)
            {
                AppUA _appUA = Session["AppUA"] as AppUA;
                if (appObj.ID == Guid.Empty)
                {
                    try
                    {
                        appObj.commonDetails = new PSASysCommonViewModel();
                        appObj.commonDetails.CreatedBy = _appUA.UserName;
                        appObj.commonDetails.CreatedDate = _appUA.LoginDateTime;
                        result = _applicationBusiness.InsertApplication(Mapper.Map<ApplicationViewModel, Application>(appObj));
                    }
                    catch (Exception ex)
                    {
                        return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                    }
                }
                else
                {
                    try
                    {
                        appObj.commonDetails = new PSASysCommonViewModel();
                        appObj.commonDetails.UpdatedBy = _appUA.UserName;
                        appObj.commonDetails.UpdatedDate = _appUA.LoginDateTime;
                        result = _applicationBusiness.UpdateApplication(Mapper.Map<ApplicationViewModel, Application>(appObj));
                    }
                    catch (Exception ex)
                    {
                        return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                    }
                }
            }
            return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
        }

        #endregion InsertUpdateEvent

        #region GetAllApplication
        [HttpGet]
       [AuthSecurityFilter(ProjectObject = "Application", Mode = "R")]
        public string GetAllApplication()
        {
            try
            {

                List<ApplicationViewModel> appList = Mapper.Map<List<Application>, List<ApplicationViewModel>>(_applicationBusiness.GetAllApplication());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = appList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetAllApplication

        #region DeleteApplication

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Application", Mode = "D")]
        public string DeleteApplication(ApplicationViewModel appObj)
        {
            object result = null;
            if (appObj.ID != Guid.Empty)
            {
                try
                {
                    result = _applicationBusiness.DeleteApplication(Mapper.Map<ApplicationViewModel, Application>(appObj));
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }
            else
            {

            }
            return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
        }

        #endregion DeleteApplication

        #region ButtonStyling
        [HttpGet]
     [AuthSecurityFilter(ProjectObject = "Application", Mode = "R")]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            //Permission _permission = Session["UserRights"] as Permission;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission _permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "Application");
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    if ((_permission.SubPermissionList.Count>0 ? _permission.SubPermissionList.First(s => s.Name == "ButtonAdd").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.addbtn.Visible = true;
                    }
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "Add();";


                    if ((_permission.SubPermissionList.Count>0? _permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.backbtn.Visible = true;
                    }
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "goHome()";

                    break;
                case "Edit":
                    if ((_permission.SubPermissionList.Count>0 ? _permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.backbtn.Visible = true;
                    }
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "Back()";

                    if ((_permission.SubPermissionList.Count>0 ? _permission.SubPermissionList.First(s => s.Name == "ButtonSave").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.savebtn.Visible = true;
                    }
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    if ((_permission.SubPermissionList.Count>0 ? _permission.SubPermissionList.First(s => s.Name == "ButtonDelete").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.deletebtn.Visible = true;
                    }
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteClick();";

                    if ((_permission.SubPermissionList.Count>0 ? _permission.SubPermissionList.First(s => s.Name == "ButtonReset").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.resetbtn.Visible = true;
                    }
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "reset();";

                    break;
                case "Add":
                    if ((_permission.SubPermissionList.Count>0 ? _permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.backbtn.Visible = true;
                    }
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "Back()";

                    if ((_permission.SubPermissionList.Count>0 ? _permission.SubPermissionList.First(s => s.Name == "ButtonSave").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.savebtn.Visible = true;
                    }
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    if ((_permission.SubPermissionList.Count>0 ? _permission.SubPermissionList.First(s => s.Name == "ButtonDelete").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.deletebtn.Visible = true;
                    }
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Event = "DeleteClick()";

                    if ((_permission.SubPermissionList.Count>0 ? _permission.SubPermissionList.First(s => s.Name == "ButtonReset").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.resetbtn.Visible = true;
                    }
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "reset();";

                    break;
              
                default:
                    return Content("Nochange");
            }
            ToolboxViewModelObj = _tool.SetToolbarAccess(ToolboxViewModelObj, _permission);
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}