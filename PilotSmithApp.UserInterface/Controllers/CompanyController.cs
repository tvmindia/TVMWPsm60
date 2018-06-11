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
    public class CompanyController : Controller
    {
        AppConst _appConstant = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        private ICompanyBusiness _companyBusiness;
        IUserBusiness _userBusiness;
        #region Contructor Injection
        public CompanyController(ICompanyBusiness companyBusiness,IUserBusiness userBusiness)
        {
            _companyBusiness = companyBusiness;
            _userBusiness = userBusiness;
        }
        #endregion Contructor Injection
        // GET: Company
        [AuthSecurityFilter(ProjectObject = "Company", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModeuleCode = code;
            CompanyAdvanceSearchViewModel companyAdvanceSearchVM = new CompanyAdvanceSearchViewModel();
            return View(companyAdvanceSearchVM);
        }

        #region InsertUpdateCompany
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Company", Mode = "W")]
        public string InsertUpdateCompany(CompanyViewModel companyVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                companyVM.PSASysCommon = new PSASysCommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _psaSysCommon.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _psaSysCommon.GetCurrentDateTime(),
                };
                var result = _companyBusiness.InsertUpdateCompany(Mapper.Map<CompanyViewModel, Company>(companyVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateCompany

        #region CheckCompanyNameExist
        [AuthSecurityFilter(ProjectObject = "Company", Mode = "R")]
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckCompanyNameExist(CompanyViewModel companyVM)
        {
            bool exists = companyVM.IsUpdate ? false : _companyBusiness.CheckCompanyNameExist(Mapper.Map<CompanyViewModel, Company>(companyVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Company already is in use </span> <i class='fas fa-times' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MasterPartial
        //[HttpGet]
        //[AuthSecurityFilter(ProjectObject = "Company", Mode = "R")]
        public ActionResult MasterPartial(Guid masterCode)
        {
            CompanyViewModel companyVM = masterCode == Guid.Empty? new CompanyViewModel() : Mapper.Map<Company, CompanyViewModel>(_companyBusiness.GetCompany(masterCode));
            companyVM.IsUpdate = masterCode == Guid.Empty ? false : true;
           
            return PartialView("_AddCompany", companyVM);
        }
        #endregion MasterPartial

        #region CompanySelectList
        public ActionResult CompanySelectList(string required,bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _userBusiness.GetSecurityCode(appUA.UserName, "Company");
            if (permission.SubPermissionList != null)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListCompanyAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            CompanyViewModel companyVM = new CompanyViewModel();
            companyVM.CompanySelectList = _companyBusiness.GetCompanyForSelectList();
            return PartialView("_CompanySelectList", companyVM);
        }
        #endregion CompanySelectList

        #region GetAllCompany
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Company", Mode = "R")]
        public JsonResult GetAllCompany(DataTableAjaxPostModel model, CompanyAdvanceSearchViewModel companyAdvanceSearchVM)
        {
            companyAdvanceSearchVM.DataTablePaging.Start = model.start;
            companyAdvanceSearchVM.DataTablePaging.Length = (companyAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : companyAdvanceSearchVM.DataTablePaging.Length;
            List<CompanyViewModel> companyVMList = Mapper.Map<List<Company>, List<CompanyViewModel>>(_companyBusiness.GetAllCompany(Mapper.Map<CompanyAdvanceSearchViewModel, CompanyAdvanceSearch>(companyAdvanceSearchVM)));
            if (companyAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = companyVMList.Count != 0 ? companyVMList[0].TotalCount : 0;
                int filteredResult = companyVMList.Count != 0 ? companyVMList[0].FilteredCount : 0;
                companyVMList = companyVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = companyVMList.Count != 0 ? companyVMList[0].TotalCount : 0,
                recordsFiltered = companyVMList.Count != 0 ? companyVMList[0].FilteredCount : 0,
                data = companyVMList
            });
        }
        #endregion GetAllCompany

        #region DeleteCompany
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Company", Mode = "D")]
        public string DeleteCompany(Guid id)
        {
            try
            {
                var result = _companyBusiness.DeleteCompany(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion DeleteCompany

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Company", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddCompanyMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetCompanyList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportCompanyData();";
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