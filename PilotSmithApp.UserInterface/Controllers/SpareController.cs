using AutoMapper;
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
    public class SpareController : Controller
    {

        #region Constructor_Injection
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        ISpareBusiness _spareBusiness;
        SecurityFilter.ToolBarAccess _tool;     
        public SpareController( ISpareBusiness spareBusiness, SecurityFilter.ToolBarAccess tool)
        {
            _spareBusiness = spareBusiness;
            _tool = tool;
        }
        #endregion Constructor_Injection

        // GET: Spare
        [AuthSecurityFilter(ProjectObject = "Spare", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }

        #region GetAllSpare
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Spare", Mode = "R")]
        public JsonResult GetAllSpare(DataTableAjaxPostModel model, SpareAdvanceSearchViewModel spareAdvanceSearchVM)
        {
            spareAdvanceSearchVM.DataTablePaging.Start = model.start;
            spareAdvanceSearchVM.DataTablePaging.Length = (spareAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : spareAdvanceSearchVM.DataTablePaging.Length;
            List<SpareViewModel> spareVMList = Mapper.Map<List<Spare>, List<SpareViewModel>>(_spareBusiness.GetAllSpare(Mapper.Map<SpareAdvanceSearchViewModel, SpareAdvanceSearch>(spareAdvanceSearchVM)));
            if (spareAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = spareVMList.Count != 0 ? spareVMList[0].TotalCount : 0;
                int filteredResult = spareVMList.Count != 0 ? spareVMList[0].FilteredCount : 0;
                spareVMList = spareVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = spareVMList.Count != 0 ? spareVMList[0].TotalCount : 0,
                recordsFiltered = spareVMList.Count != 0 ? spareVMList[0].FilteredCount : 0,
                data = spareVMList
            });
        }
        #endregion GetAllSpare

        #region MasterPartial
        //[HttpGet]
        public ActionResult MasterPartial(Guid masterCode)
        {
            SpareViewModel spareVM = masterCode == Guid.Empty ? new SpareViewModel() : Mapper.Map<Spare, SpareViewModel>(_spareBusiness.GetSpare(masterCode));
            spareVM.IsUpdate = masterCode == Guid.Empty ? false : true;
            if (spareVM.IsUpdate == false)
            {
                spareVM.Code = _spareBusiness.GetSpareCode();
            }
            return PartialView("_AddSpare", spareVM);
        }
        #endregion MasterPartial

        #region InsertUpdateSpare
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Spare", Mode = "W")]
        public string InsertUpdateSpare(SpareViewModel spareVM)
        {
            object result = null;
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                spareVM.PSASysCommon = new PSASysCommonViewModel();
                spareVM.PSASysCommon.CreatedBy = appUA.UserName;
                spareVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                spareVM.PSASysCommon.UpdatedBy = appUA.UserName;
                spareVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                result = _spareBusiness.InsertUpdateSpare(Mapper.Map<SpareViewModel, Spare>(spareVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateSpare

        #region CheckSpareCodeExist      
        [AcceptVerbs("Get", "Post")]
        [AuthSecurityFilter(ProjectObject = "Spare", Mode = "R")]
        public ActionResult CheckSpareCodeExist(SpareViewModel spareVM)
        {
            bool exists = _spareBusiness.CheckSpareCodeExist(Mapper.Map<SpareViewModel, Spare>(spareVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Spare code is in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion CheckSpareCodeExist

        #region DeleteSpare
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Spare", Mode = "D")]
        public string DeleteSpare(Guid id)
        {
            try
            {
                var result = _spareBusiness.DeleteSpare(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion DeleteSpare

        # region SpareSelectList
        public ActionResult SpareSelectList(string required, bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "Spare");
            if (permission.SubPermissionList.Count>0)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            SpareViewModel spareVM = new SpareViewModel();
            spareVM.SpareSelectList = _spareBusiness.GetSpareForSelectList();
            return PartialView("_SpareSelectList", spareVM);
        }
        #endregion SpareSelectList

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Spare", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "Spare");
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddSpareMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetSpareList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportSpareData();";
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