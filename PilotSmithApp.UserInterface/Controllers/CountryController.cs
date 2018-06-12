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
    public class CountryController : Controller
    {
        AppConst _appConstant = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        ICountryBusiness _contryBusiness;
        IUserBusiness _userBusiness;
        public CountryController(ICountryBusiness countryBusiness,IUserBusiness userBusiness)
        {
            _contryBusiness = countryBusiness;
            _userBusiness = userBusiness;
        }
        // GET: Country
        [AuthSecurityFilter(ProjectObject = "Country", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }

        #region InsertUpdateCountry
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Country", Mode = "W")]
        public string InsertUpdateCountry(CountryViewModel CountryVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                CountryVM.PSASysCommon = new PSASysCommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _psaSysCommon.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _psaSysCommon.GetCurrentDateTime(),
                };
                var result = _contryBusiness.InsertUpdateCountry(Mapper.Map<CountryViewModel, Country>(CountryVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region CheckCountryExist        
        [AcceptVerbs("Get", "Post")]
        [AuthSecurityFilter(ProjectObject = "Country", Mode = "R")]
        public ActionResult CheckCountryExist(CountryViewModel CountryVM)
        {
            bool exists = _contryBusiness.CheckCountryExist(Mapper.Map<CountryViewModel, Country>(CountryVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Country already is in use </span> <i class='fas fa-times' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MasterPartial
        //[HttpGet]
        //[AuthSecurityFilter(ProjectObject = "Country", Mode = "R")]
        public ActionResult MasterPartial(int masterCode)
        {
            CountryViewModel CountryVM = masterCode == 0 ? new CountryViewModel() : Mapper.Map<Country, CountryViewModel>(_contryBusiness.GetCountry(masterCode));
            CountryVM.IsUpdate = masterCode == 0 ? false : true;
            return PartialView("_AddCountry", CountryVM);
        }
        #endregion MasterPartial

        #region GetAllCountry
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Country", Mode = "R")]
        public JsonResult GetAllCountry(DataTableAjaxPostModel model, CountryAdvanceSearchViewModel CountryAdvanceSearchVM)
        {
            CountryAdvanceSearchVM.DataTablePaging.Start = model.start;
            CountryAdvanceSearchVM.DataTablePaging.Length = (CountryAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : CountryAdvanceSearchVM.DataTablePaging.Length;
            List<CountryViewModel> CountryVMList = Mapper.Map<List<Country>, List<CountryViewModel>>(_contryBusiness.GetAllCountry(Mapper.Map<CountryAdvanceSearchViewModel, CountryAdvanceSearch>(CountryAdvanceSearchVM)));
            if (CountryAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = CountryVMList.Count != 0 ? CountryVMList[0].TotalCount : 0;
                int filteredResult = CountryVMList.Count != 0 ? CountryVMList[0].FilteredCount : 0;
                CountryVMList = CountryVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = CountryVMList.Count != 0 ? CountryVMList[0].TotalCount : 0,
                recordsFiltered = CountryVMList.Count != 0 ? CountryVMList[0].FilteredCount : 0,
                data = CountryVMList
            });
        }
        #endregion

        #region DeleteCountry
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Country", Mode = "D")]
        public string DeleteCountry(int code)
        {
            try
            {
                var result = _contryBusiness.DeleteCountry(code);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region Country SelectList
        public ActionResult CountrySelectList(string required, bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _userBusiness.GetSecurityCode(appUA.UserName, "Country");
            if (permission.SubPermissionList != null)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListCountryAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            CountryViewModel CountryVM = new CountryViewModel();
            CountryVM.CountrySelectList = _contryBusiness.GetCountryForSelectList();
            return PartialView("_CountrySelectList", CountryVM);
        }
        #endregion Country SelectList

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Country", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddCountryMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetCountryList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportCountryData();";
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