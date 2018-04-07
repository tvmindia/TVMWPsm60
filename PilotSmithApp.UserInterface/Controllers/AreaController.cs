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
    public class AreaController : Controller
    {
        AppConst _appConst = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        private IAreaBusiness _areaBusiness;
        private IStateBusiness _stateBusiness;
        private IDistrictBusiness _districtBusiness;
        // GET: Area
        #region Constructor Injection
        public AreaController(IAreaBusiness areaBusiness, IStateBusiness stateBusiness, IDistrictBusiness districtBusiness)
        {
            _areaBusiness = areaBusiness;
            _stateBusiness = stateBusiness;
            _districtBusiness = districtBusiness;
        }
        #endregion
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            AreaAdvanceSearchViewModel areaAdvanceSearchVM = new AreaAdvanceSearchViewModel();
            return View();
        }

        #region InsertUpdateArea
        public string InsertUpdateArea(AreaViewModel areaVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                areaVM.PSASysCommon = new PSASysCommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _psaSysCommon.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _psaSysCommon.GetCurrentDateTime(),
                };
                var result = _areaBusiness.InsertUpdateArea(Mapper.Map<AreaViewModel, Area>(areaVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result,Message="Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region CheckAreaCodeExist
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckAreaCodeExist(Area areaVM)
        {
            bool exists = areaVM.IsUpdate ? false : _areaBusiness.CheckAreaCodeExist(areaVM.Code);
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Area Code is in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MasterPartial
        [HttpGet]
        public ActionResult MasterPartial(int masterCode)
        {
            AreaViewModel areaVM = masterCode==0? new AreaViewModel() : Mapper.Map<Area, AreaViewModel>(_areaBusiness.GetArea(masterCode));
            areaVM.IsUpdate = masterCode==0 ? false : true;
            return PartialView("_AddArea", areaVM);
        }
        #endregion

        #region GetAllArea
        public JsonResult GetAllArea(DataTableAjaxPostModel model, AreaAdvanceSearchViewModel areaAdvanceSearchVM)
        {
            areaAdvanceSearchVM.DataTablePaging.Start = model.start;
            areaAdvanceSearchVM.DataTablePaging.Length = (areaAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : areaAdvanceSearchVM.DataTablePaging.Length;
            List<AreaViewModel> areaVMList = Mapper.Map<List<Area>, List<AreaViewModel>>(_areaBusiness.GetAllArea(Mapper.Map<AreaAdvanceSearchViewModel, AreaAdvanceSearch>(areaAdvanceSearchVM)));
            if (areaAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = areaVMList.Count != 0 ? areaVMList[0].TotalCount : 0;
                int filteredResult = areaVMList.Count != 0 ? areaVMList[0].FilteredCount : 0;
                areaVMList = areaVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = areaVMList.Count != 0 ? areaVMList[0].TotalCount : 0,
                recordsFiltered = areaVMList.Count != 0 ? areaVMList[0].FilteredCount : 0,
                data = areaVMList
            });
        }
        #endregion

        #region DeleteArea
        public string DeleteArea(int code)
        {
            try
            {
                var result = _areaBusiness.DeleteArea(code);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region Area SelectList
        public ActionResult AreaSelectList(string required)
        {
            ViewBag.IsRequired = required;
            AreaViewModel areaVM = new AreaViewModel();
            areaVM.AreaSelectList = _areaBusiness.GetAreaForSelectList();
            return PartialView("_AreaSelectList", areaVM);
        }
        #endregion Area SelectList

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
                    toolboxVM.addbtn.Event = "AddAreaMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetAreaList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportAreaData();";
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