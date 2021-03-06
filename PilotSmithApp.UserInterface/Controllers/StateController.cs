﻿using AutoMapper;
using Newtonsoft.Json;
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
    public class StateController : Controller
    {
        AppConst _appConst = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        private IStateBusiness _stateBusiness;
        SecurityFilter.ToolBarAccess _tool;
        #region Constructor Injection
        public StateController(IStateBusiness stateBusiness, SecurityFilter.ToolBarAccess tool)
        {
            _stateBusiness = stateBusiness;
            _tool = tool;
        }
        #endregion
        // GET: State
        [AuthSecurityFilter(ProjectObject = "State", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            StateAdvanceSearchViewModel stateAdvanceSearchVM = new StateAdvanceSearchViewModel();
            return View();
        }

        #region InsertUpdateState
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "State", Mode = "W")]
        public string InsertUpdateState(StateViewModel stateVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                stateVM.PSASysCommon = new PSASysCommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _psaSysCommon.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _psaSysCommon.GetCurrentDateTime(),
                };
                var result = _stateBusiness.InsertUpdateState(Mapper.Map<StateViewModel, State>(stateVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region CheckStateNameExist
        [AcceptVerbs("Get", "Post")]
        [AuthSecurityFilter(ProjectObject = "State", Mode = "R")]
        public ActionResult CheckStateNameExist(StateViewModel stateVM)
        {
            bool exists = stateVM.IsUpdate ? false : _stateBusiness.CheckStateNameExist(Mapper.Map<StateViewModel,State>(stateVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>State already is in use </span> <i class='fa fa-times' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MasterPartial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "State", Mode = "R")]
        public ActionResult MasterPartial(int masterCode)
        {
            StateViewModel stateVM = masterCode==0 ? new StateViewModel() : Mapper.Map<State, StateViewModel>(_stateBusiness.GetState(masterCode));
            stateVM.IsUpdate = masterCode==0 ? false : true;
            return PartialView("_AddState", stateVM);
        }
        #endregion

        #region GetAllState
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "State", Mode = "R")]
        public JsonResult GetAllState(DataTableAjaxPostModel model, StateAdvanceSearchViewModel stateAdvanceSearchVM)
        {
            stateAdvanceSearchVM.DataTablePaging.Start = model.start;
            stateAdvanceSearchVM.DataTablePaging.Length = (stateAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : stateAdvanceSearchVM.DataTablePaging.Length;
            List<StateViewModel> stateVMList = Mapper.Map<List<State>, List<StateViewModel>>(_stateBusiness.GetAllState(Mapper.Map<StateAdvanceSearchViewModel, StateAdvanceSearch>(stateAdvanceSearchVM)));
            if (stateAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = stateVMList.Count != 0 ? stateVMList[0].TotalCount : 0;
                int filteredResult = stateVMList.Count != 0 ? stateVMList[0].FilteredCount : 0;
                stateVMList = stateVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = stateVMList.Count != 0 ? stateVMList[0].TotalCount : 0,
                recordsFiltered = stateVMList.Count != 0 ? stateVMList[0].FilteredCount : 0,
                data = stateVMList
            });
        }
        #endregion

        #region DeleteState
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "State", Mode = "D")]
        public string DeleteState(int code)
        {
            try
            {
                var result = _stateBusiness.DeleteState(code);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region State SelectList
        public ActionResult StateSelectList(string required,bool? disabled, int? countryCode)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            //Permission _permission = Session["UserRights"] as Permission;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _psaSysCommon.GetSecurityCode(appUA.UserName, "State");
            if (permission.SubPermissionList.Count>0)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            StateViewModel stateVM = new StateViewModel();
            stateVM.StateSelectList = _stateBusiness.GetStateForSelectList(countryCode);
            return PartialView("_StateSelectList", stateVM);
        }


        //public ActionResult StateSelectList(string required)
        //{
        //    ViewBag.IsRequired = required;
        //    StateViewModel stateVM = new StateViewModel();
        //    stateVM.StateSelectList = _stateBusiness.GetStateForSelectList();
        //    return PartialView("_StateSelectList", stateVM);
        //}
        #endregion State SelectList

        #region Get State SelectList On Demand
        [HttpPost]
        public ActionResult GetStateForSelectListOnDemand(string searchTerm, int? countryCode)
        {
            List<SelectListItem> stateSelectList = _stateBusiness.GetStateForSelectList(countryCode);
            var list = stateSelectList != null ? (from SelectListItem in stateSelectList.Where(x => x.Text.ToLower().Contains(searchTerm.ToLower())).ToList()
                                                    select new Select2Model
                                                    {
                                                        text = SelectListItem.Text,
                                                        id = SelectListItem.Value,
                                                    }).ToList() : new List<Select2Model>();
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion Get State SelectList On Demand

        #region GetState
        public string GetState(int code)
        {
            try
            {
                StateViewModel stateVM = Mapper.Map<State, StateViewModel>(_stateBusiness.GetState(code));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = stateVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion GetState

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "State", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _psaSysCommon.GetSecurityCode(appUA.UserName, "State");
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddStateMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetStateList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportStateData();";
                    //---------------------------------------
                    break;
                default:
                    return Content("Nochange");
            }
            toolboxVM = _tool.SetToolbarAccess(toolboxVM, permission);
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}