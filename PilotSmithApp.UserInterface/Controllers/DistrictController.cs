using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
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
        // GET: District
        #region Constructor Injection
        public DistrictController(IDistrictBusiness districtBusiness, IStateBusiness stateBusiness)
        {
            _districtBusiness = districtBusiness;
            _stateBusiness = stateBusiness;
        }
        #endregion
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            DistrictAdvanceSearchViewModel districtAdvanceSearchVM = new DistrictAdvanceSearchViewModel();
            return View();
        }

        #region InsertUpdateDistrict
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
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion

        #region CheckDistrictCodeExist
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckStateCodeExist(DistrictViewModel districtVM)
        {
            bool exists = districtVM.IsUpdate ? false : _districtBusiness.CheckDistrictCodeExist(districtVM.Code);
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Distriction Code is in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MasterPartial
        [HttpGet]
        public ActionResult MasterPartial(string masterCode)
        {
            DistrictViewModel districtVM = masterCode=="0" ? new DistrictViewModel() : Mapper.Map<District, DistrictViewModel>(_districtBusiness.GetDistrict(int.Parse(masterCode)));
            districtVM.IsUpdate = masterCode=="0" ? false : true;
            districtVM.State = new StateViewModel();
            //districtVM.State.SelectList = StateDropDown();
            return PartialView("_AddDistrict", districtVM);
        }
        #endregion

        #region GetAllDistrict
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

        #region DistrictDropDown
        public ActionResult DistrictDropDown(DistrictViewModel districtVM)
        {
            districtVM.DistrictCode = districtVM.Code;
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            districtVM.SelectList = new List<SelectListItem>();
            List<DistrictViewModel> districtList = Mapper.Map<List<District>, List<DistrictViewModel>>(_districtBusiness.GetDistrictForSelectList());
            if (districtList != null)
                foreach (DistrictViewModel district in districtList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = district.Description,
                        Value = district.Code.ToString(),
                        Selected = false
                    });
                }
            districtVM.SelectList = selectListItem;
            return PartialView("_DistrictDropDown", districtVM);
        }
        #endregion

        #region ButtonStyling
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
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