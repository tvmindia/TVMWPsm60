using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class SysSettingController : Controller
    {
        #region Constructor_Injection
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        ISysSettingBusiness _sysSettingBusiness;
        SecurityFilter.ToolBarAccess _tool;
        public SysSettingController(ISysSettingBusiness sysSettingBusiness, SecurityFilter.ToolBarAccess tool)
        {
            _sysSettingBusiness = sysSettingBusiness;
            _tool = tool;
        }
        #endregion Constructor_Injection

        // GET: SysSetting
        [AuthSecurityFilter(ProjectObject = "SysSetting", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }

        #region GetAllSysSetting
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "SysSetting", Mode = "R")]
        public JsonResult GetAllSysSetting(DataTableAjaxPostModel model, SysSettingAdvanceSearchViewModel sysSettingAdvanceSearchVM)
        {
            sysSettingAdvanceSearchVM.DataTablePaging.Start = model.start;
            sysSettingAdvanceSearchVM.DataTablePaging.Length = (sysSettingAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : sysSettingAdvanceSearchVM.DataTablePaging.Length;
            List<SysSettingViewModel> sysSettingVMList = Mapper.Map<List<SysSetting>, List<SysSettingViewModel>>(_sysSettingBusiness.GetAllSysSetting(Mapper.Map<SysSettingAdvanceSearchViewModel, SysSettingAdvanceSearch>(sysSettingAdvanceSearchVM)));
            if (sysSettingAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = sysSettingVMList.Count != 0 ? sysSettingVMList[0].TotalCount : 0;
                int filteredResult = sysSettingVMList.Count != 0 ? sysSettingVMList[0].FilteredCount : 0;
                sysSettingVMList = sysSettingVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = sysSettingVMList.Count != 0 ? sysSettingVMList[0].TotalCount : 0,
                recordsFiltered = sysSettingVMList.Count != 0 ? sysSettingVMList[0].FilteredCount : 0,
                data = sysSettingVMList
            });
        }
        #endregion GetAllSysSetting

        #region MasterPartial
        //[HttpGet]
        public ActionResult MasterPartial(Guid masterCode)
        {
            SysSettingViewModel sysSettingVM = masterCode == Guid.Empty ? new SysSettingViewModel() : Mapper.Map<SysSetting, SysSettingViewModel>(_sysSettingBusiness.GetSysSetting(masterCode));
            sysSettingVM.IsUpdate = masterCode == Guid.Empty ? false : true;
            return PartialView("_AddSysSetting", sysSettingVM);
        }
        #endregion MasterPartial

        #region InsertUpdateSysSetting
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "SysSetting", Mode = "W")]
        public string InsertUpdateSysSetting(SysSettingViewModel sysSettingVM)
        {
            object result = null;
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                sysSettingVM.PSASysCommon = new PSASysCommonViewModel();
                sysSettingVM.PSASysCommon.CreatedBy = appUA.UserName;
                sysSettingVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                sysSettingVM.PSASysCommon.UpdatedBy = appUA.UserName;
                sysSettingVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                result = _sysSettingBusiness.InsertUpdateSysSetting(Mapper.Map<SysSettingViewModel, SysSetting>(sysSettingVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateSysSetting

        #region DeleteSysSetting
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SysSetting", Mode = "D")]
        public string DeleteSysSetting(Guid id)
        {
            try
            {
                var result = _sysSettingBusiness.DeleteSysSetting(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion DeleteSysSetting

        #region ButtonStyling
        [HttpGet]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddSysSettingMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetSysSettingList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportSysSettingData();";
                    //---------------------------------------
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }
        #endregion ButtonStyling
    }
}