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

namespace PilotSmithApp.UserInterface.Controllers
{
    public class OtherChargeController : Controller
    {
        AppConst _appConstant = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        private IOtherChargeBusiness _otherChargeBusiness;
        IUserBusiness _userBusiness;

        // GET: OtherCharge
        #region Constructor Injection
        public OtherChargeController(IOtherChargeBusiness otherChargeBusiness, IUserBusiness userBusiness)
        {
            _otherChargeBusiness = otherChargeBusiness;
            _userBusiness = userBusiness;
        }
        #endregion
        [AuthSecurityFilter(ProjectObject = "OtherCharge", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }
        #region InsertUpdateOtherCharge
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "OtherCharge", Mode = "W")]
        public string InsertUpdateOtherCharge(OtherChargeViewModel otherChargeVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                otherChargeVM.PSASysCommon = new PSASysCommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _psaSysCommon.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _psaSysCommon.GetCurrentDateTime(),
                };
                var result = _otherChargeBusiness.InsertUpdateOtherCharge(Mapper.Map<OtherChargeViewModel, OtherCharge>(otherChargeVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region CheckOtherChargeNameExist        
        [AcceptVerbs("Get", "Post")]
        [AuthSecurityFilter(ProjectObject = "OtherCharge", Mode = "R")]
        public ActionResult CheckOtherChargeCodeExist(OtherChargeViewModel otherChargeVM)
        {
            bool exists = _otherChargeBusiness.CheckOtherChargeCodeExist(Mapper.Map<OtherChargeViewModel, OtherCharge>(otherChargeVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>OtherCharge already is in use </span> <i class='fas fa-times' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MasterPartial
        public ActionResult MasterPartial(int masterCode)
        {
            OtherChargeViewModel otherChargeVM = masterCode == 0 ? new OtherChargeViewModel() : Mapper.Map<OtherCharge, OtherChargeViewModel>(_otherChargeBusiness.GetOtherCharge(masterCode));
            otherChargeVM.IsUpdate = masterCode == 0 ? false : true;
            return PartialView("_AddOtherCharge", otherChargeVM);
        }
        #endregion MasterPartial

        #region GetAllOtherCharge
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "OtherCharge", Mode = "R")]
        public JsonResult GetAllOtherCharge(DataTableAjaxPostModel model, OtherChargeAdvanceSearchViewModel otherChargeAdvanceSearchVM)
        {
            otherChargeAdvanceSearchVM.DataTablePaging.Start = model.start;
            otherChargeAdvanceSearchVM.DataTablePaging.Length = (otherChargeAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : otherChargeAdvanceSearchVM.DataTablePaging.Length;
            List<OtherChargeViewModel> otherChargeVMList = Mapper.Map<List<OtherCharge>, List<OtherChargeViewModel>>(_otherChargeBusiness.GetAllOtherCharge(Mapper.Map<OtherChargeAdvanceSearchViewModel, OtherChargeAdvanceSearch>(otherChargeAdvanceSearchVM)));
            if (otherChargeAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = otherChargeVMList.Count != 0 ? otherChargeVMList[0].TotalCount : 0;
                int filteredResult = otherChargeVMList.Count != 0 ? otherChargeVMList[0].FilteredCount : 0;
                otherChargeVMList = otherChargeVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = otherChargeVMList.Count != 0 ? otherChargeVMList[0].TotalCount : 0,
                recordsFiltered = otherChargeVMList.Count != 0 ? otherChargeVMList[0].FilteredCount : 0,
                data = otherChargeVMList
            });
        }
        #endregion

        #region DeleteOtherCharge
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherCharge", Mode = "D")]
        public string DeleteOtherCharge(int code)
        {
            try
            {
                var result = _otherChargeBusiness.DeleteOtherCharge(code);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        //#region OtherChargeSelectList
        //[AuthSecurityFilter(ProjectObject = "OtherCharge", Mode = "R")]
        //public ActionResult OtherChargeSelectList(string required)
        //{
        //    ViewBag.IsRequired = required;
        //    OtherChargeViewModel otherChargeVM = new OtherChargeViewModel();
        //    otherChargeVM.OtherChargeSelectList = _otherChargeBusiness.GetOtherChargeForSelectList();
        //    return PartialView("_OtherChargeSelectList", otherChargeVM);
        //}
        //#endregion 

        #region OtherChargeSelectList
        public ActionResult OtherChargeSelectList(string required, bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _userBusiness.GetSecurityCode(appUA.UserName, "OtherCharge");
            if (permission.SubPermissionList != null)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            OtherChargeViewModel otherChargeVM = new OtherChargeViewModel();
            otherChargeVM.OtherChargeSelectList = _otherChargeBusiness.GetOtherChargeForSelectList();
            return PartialView("_OtherChargeSelectList", otherChargeVM);
        }
        #endregion 

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherCharge", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddOtherChargeMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetOtherChargeList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportOtherChargeData();";
                    //---------------------------------------
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }
        #endregion

    }
}