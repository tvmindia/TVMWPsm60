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
    public class DistrictController : Controller
    {
        AppConst _appConst = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        private IDistrictBusiness _districtBusiness;
        private IStateBusiness _stateBusiness;
        IUserBusiness _userBusiness;
        // GET: District
        #region Constructor Injection
        public DistrictController(IDistrictBusiness districtBusiness, IStateBusiness stateBusiness,IUserBusiness userBusiness)
        {
            _districtBusiness = districtBusiness;
            _stateBusiness = stateBusiness;
            _userBusiness = userBusiness;
        }
        #endregion
        [AuthSecurityFilter(ProjectObject = "District", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            DistrictAdvanceSearchViewModel districtAdvanceSearchVM = new DistrictAdvanceSearchViewModel();
            return View();
        }

        #region InsertUpdateDistrict
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "District", Mode = "W")]
        public string InsertUpdateDistrict(DistrictViewModel stateVM)
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
                var result = _districtBusiness.InsertUpdateDistrict(Mapper.Map<DistrictViewModel, District>(stateVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record ="", Message = cm.Message });
            }
        }
        #endregion

        #region CheckDistrictNameExist        
        [AcceptVerbs("Get", "Post")]
        [AuthSecurityFilter(ProjectObject = "District", Mode = "R")]
        public ActionResult CheckDistrictNameExist(DistrictViewModel districtVM)
        {
            bool exists = districtVM.IsUpdate ? false : _districtBusiness.CheckDistrictNameExist(Mapper.Map<DistrictViewModel,District>(districtVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>District alredy is in use </span> <i class='fas fa-times' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion CheckDistrictNameExist

        #region MasterPartial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "District", Mode = "R")]
        public ActionResult MasterPartial(int masterCode)
        {
            DistrictViewModel districtVM = masterCode==0 ? new DistrictViewModel() : Mapper.Map<District, DistrictViewModel>(_districtBusiness.GetDistrict(masterCode));
            districtVM.IsUpdate = masterCode==0 ? false : true;
            return PartialView("_AddDistrict", districtVM);
        }
        #endregion

        #region GetAllDistrict
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "District", Mode = "R")]
        public JsonResult GetAllDistrict(DataTableAjaxPostModel model, DistrictAdvanceSearchViewModel districtAdvanceSearchVM)
        {
            districtAdvanceSearchVM.DataTablePaging.Start = model.start;
            districtAdvanceSearchVM.DataTablePaging.Length = (districtAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : districtAdvanceSearchVM.DataTablePaging.Length;
            List<DistrictViewModel> districtVMList = Mapper.Map<List<District>, List<DistrictViewModel>>(_districtBusiness.GetAllDistrict(Mapper.Map<DistrictAdvanceSearchViewModel, DistrictAdvanceSearch>(districtAdvanceSearchVM)));
            if (districtAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = districtVMList.Count != 0 ? districtVMList[0].TotalCount : 0;
                int filteredResult = districtVMList.Count != 0 ? districtVMList[0].FilteredCount : 0;
                districtVMList = districtVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = districtVMList.Count != 0 ? districtVMList[0].TotalCount : 0,
                recordsFiltered = districtVMList.Count != 0 ? districtVMList[0].FilteredCount : 0,
                data = districtVMList
            });
        }
        #endregion

        #region DeleteDistrict
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "District", Mode = "D")]
        public string DeleteDistrict(int code)
        {
            try
            {
                var result = _districtBusiness.DeleteDistrict(code);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region District SelectList
        public ActionResult DistrictSelectList(string required,bool? disabled, int? stateCode)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            //Permission _permission = Session["UserRights"] as Permission;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _userBusiness.GetSecurityCode(appUA.UserName, "District");
            if (permission.SubPermissionList != null)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListDistrictAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            DistrictViewModel districtVM = new DistrictViewModel();
            districtVM.DistrictSelectList = _districtBusiness.GetDistrictForSelectList(stateCode);
            return PartialView("_DistrictSelectList", districtVM);
        }
        #endregion District SelectList

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "District", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddDistrictMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetDistrictList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportDistrictData();";
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