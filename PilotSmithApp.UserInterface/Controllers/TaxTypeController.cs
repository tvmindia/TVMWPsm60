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
    public class TaxTypeController : Controller
    {
        AppConst _appConst = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        private ITaxTypeBusiness _taxTypeBusiness;
        IUserBusiness _userBusiness;
        #region Constructor Injection
        public TaxTypeController(ITaxTypeBusiness taxTypeBusiness,IUserBusiness userBusiness)
        {
            _taxTypeBusiness = taxTypeBusiness;
            _userBusiness = userBusiness;
        }
        #endregion
        // GET: TaxType
        [AuthSecurityFilter(ProjectObject = "TaxType", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }

        #region InsertUpdateTaxType
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "TaxType", Mode = "W")]
        public string InsertUpdateTaxType(TaxTypeViewModel taxTypeVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                taxTypeVM.PSASysCommon = new PSASysCommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _psaSysCommon.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _psaSysCommon.GetCurrentDateTime(),
                };
                var result = _taxTypeBusiness.InsertUpdateTaxType(Mapper.Map<TaxTypeViewModel, TaxType>(taxTypeVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region CheckTaxTypeNameExist        
        [AcceptVerbs("Get", "Post")]
        [AuthSecurityFilter(ProjectObject = "TaxType", Mode = "R")]
        public ActionResult CheckTaxTypeNameExist(TaxTypeViewModel taxTypeVM)
        {
            bool exists =  _taxTypeBusiness.CheckTaxTypeNameExist(Mapper.Map<TaxTypeViewModel, TaxType>(taxTypeVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>TaxType already is in use </span> <i class='fa fa-times' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MasterPartial
        //[HttpGet]
        //[AuthSecurityFilter(ProjectObject = "TaxType", Mode = "R")]
        public ActionResult MasterPartial(int masterCode)
        {
            TaxTypeViewModel taxTypeVM = masterCode == 0 ? new TaxTypeViewModel() : Mapper.Map<TaxType, TaxTypeViewModel>(_taxTypeBusiness.GetTaxType(masterCode));
            taxTypeVM.IsUpdate = masterCode == 0 ? false : true;
            return PartialView("_AddTaxType", taxTypeVM);
        }
        #endregion MasterPartial

        #region GetAllTaxType
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "TaxType", Mode = "R")]
        public JsonResult GetAllTaxType(DataTableAjaxPostModel model, TaxTypeAdvanceSearchViewModel taxTypeAdvanceSearchVM)
        {
            taxTypeAdvanceSearchVM.DataTablePaging.Start = model.start;
            taxTypeAdvanceSearchVM.DataTablePaging.Length = (taxTypeAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : taxTypeAdvanceSearchVM.DataTablePaging.Length;
            List<TaxTypeViewModel> taxTypeVMList = Mapper.Map<List<TaxType>, List<TaxTypeViewModel>>(_taxTypeBusiness.GetAllTaxType(Mapper.Map<TaxTypeAdvanceSearchViewModel, TaxTypeAdvanceSearch>(taxTypeAdvanceSearchVM)));
            if (taxTypeAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = taxTypeVMList.Count != 0 ? taxTypeVMList[0].TotalCount : 0;
                int filteredResult = taxTypeVMList.Count != 0 ? taxTypeVMList[0].FilteredCount : 0;
                taxTypeVMList = taxTypeVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = taxTypeVMList.Count != 0 ? taxTypeVMList[0].TotalCount : 0,
                recordsFiltered = taxTypeVMList.Count != 0 ? taxTypeVMList[0].FilteredCount : 0,
                data = taxTypeVMList
            });
        }
        #endregion

        #region DeleteTaxType
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "TaxType", Mode = "D")]
        public string DeleteTaxType(int code)
        {
            try
            {
                var result = _taxTypeBusiness.DeleteTaxType(code);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region TaxType SelectList
        public ActionResult TaxTypeSelectList(string required,bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _userBusiness.GetSecurityCode(appUA.UserName, "TaxType");
            if (permission.SubPermissionList != null)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            TaxTypeViewModel taxTypeVM = new TaxTypeViewModel();
            taxTypeVM.TaxTypeSelectList = _taxTypeBusiness.GetTaxTypeForSelectList();
            return PartialView("_TaxTypeSelectList", taxTypeVM);
        }
        #endregion TaxType SelectList

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "TaxType", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddTaxTypeMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetTaxTypeList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportTaxTypeData();";
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