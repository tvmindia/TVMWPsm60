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
    public class PlantController : Controller
    {
        AppConst _appConstant = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IPlantBusiness _plantBusiness;
        SecurityFilter.ToolBarAccess _tool;
        
        public PlantController(IPlantBusiness plantBusiness,SecurityFilter.ToolBarAccess tool)
        {
            _plantBusiness = plantBusiness;
            _tool = tool;
        }
        // GET: Plant
        [AuthSecurityFilter(ProjectObject = "Plant", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }


        #region Get Plant SelectList On Demand
        [HttpPost]
        public ActionResult GetPlantSelectListOnDemand(string searchTerm)
        {
            List<SelectListItem> plantSelectList = _plantBusiness.GetPlantForSelectList();
            var list = plantSelectList != null ? (from SelectListItem in plantSelectList.Where(x => x.Text.ToLower().Contains(searchTerm.ToLower())).ToList()
                                                           select new Select2Model
                                                           {
                                                               text = SelectListItem.Text,
                                                               id = SelectListItem.Value,
                                                           }).ToList() : new List<Select2Model>();
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion Get Plant SelectList On Demand

        #region InsertUpdatePlant
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Plant", Mode = "W")]
        public string InsertUpdatePlant(PlantViewModel plantVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                plantVM.PSASysCommon = new PSASysCommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _psaSysCommon.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _psaSysCommon.GetCurrentDateTime(),
                };
                var result = _plantBusiness.InsertUpdatePlant(Mapper.Map<PlantViewModel, Plant>(plantVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region CheckPlantNameExist        
        [AcceptVerbs("Get", "Post")]
        [AuthSecurityFilter(ProjectObject = "Plant", Mode = "R")]
        public ActionResult CheckPlantExist(PlantViewModel plantVM)
        {
            bool exists =_plantBusiness.CheckPlantNameExist(Mapper.Map<PlantViewModel, Plant>(plantVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Plant already is in use </span> <i class='fa fa-times' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MasterPartial
        //[HttpGet]
        //[AuthSecurityFilter(ProjectObject = "Plant", Mode = "R")]
        public ActionResult MasterPartial(int masterCode)
        {
            PlantViewModel plantVM = masterCode == 0 ? new PlantViewModel() : Mapper.Map<Plant, PlantViewModel>(_plantBusiness.GetPlant(masterCode));
            plantVM.IsUpdate = masterCode == 0 ? false : true;
            return PartialView("_AddPlant", plantVM);
        }
        #endregion MasterPartial

        #region GetAllPlant
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Plant", Mode = "R")]
        public JsonResult GetAllPlant(DataTableAjaxPostModel model, PlantAdvanceSearchViewModel plantAdvanceSearchVM)
        {
            plantAdvanceSearchVM.DataTablePaging.Start = model.start;
            plantAdvanceSearchVM.DataTablePaging.Length = (plantAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : plantAdvanceSearchVM.DataTablePaging.Length;
            List<PlantViewModel> plantVMList = Mapper.Map<List<Plant>, List<PlantViewModel>>(_plantBusiness.GetAllPlant(Mapper.Map<PlantAdvanceSearchViewModel, PlantAdvanceSearch>(plantAdvanceSearchVM)));
            if (plantAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = plantVMList.Count != 0 ? plantVMList[0].TotalCount : 0;
                int filteredResult = plantVMList.Count != 0 ? plantVMList[0].FilteredCount : 0;
                plantVMList = plantVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = plantVMList.Count != 0 ? plantVMList[0].TotalCount : 0,
                recordsFiltered = plantVMList.Count != 0 ? plantVMList[0].FilteredCount : 0,
                data = plantVMList
            });
        }
        #endregion

        #region DeletePlant
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Plant", Mode = "D")]
        public string DeletePlant(int code)
        {
            try
            {
                var result = _plantBusiness.DeletePlant(code);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region Plant SelectList
        public ActionResult PlantSelectList(string required,bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _psaSysCommon.GetSecurityCode(appUA.UserName, "Plant");
            if (permission.SubPermissionList.Count>0)
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

        #region ButtonStyling
        [HttpGet]
         [AuthSecurityFilter(ProjectObject = "Plant", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _psaSysCommon.GetSecurityCode(appUA.UserName, "Plant");
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddPlantMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetPlantList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportPlantData();";
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