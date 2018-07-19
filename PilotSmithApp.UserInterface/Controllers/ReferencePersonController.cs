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
    public class ReferencePersonController : Controller
    {
        AppConst _appConst = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        private IReferencePersonBusiness _referencePersonBusiness;
        private IReferenceTypeBusiness _referenceTypeBusiness;
        private IAreaBusiness _areaBusiness;
        //IUserBusiness _userBusiness;

        public ReferencePersonController(IReferencePersonBusiness referencePersonBusiness, IReferenceTypeBusiness referenceTypeBusiness, IAreaBusiness areaBusiness)//, IUserBusiness userBusiness)
        {
            _referencePersonBusiness = referencePersonBusiness;
            _referenceTypeBusiness = referenceTypeBusiness;
            _areaBusiness = areaBusiness;
           // _userBusiness = userBusiness;
        }
        [AuthSecurityFilter(ProjectObject = "ReferencePerson", Mode = "R")]
        public ActionResult Index()
        {
            ReferencePersonAdvanceSearchViewModel referencePersonAdvanceSearchVM = new ReferencePersonAdvanceSearchViewModel();
            referencePersonAdvanceSearchVM.Area = new AreaViewModel();
            referencePersonAdvanceSearchVM.Area.AreaSelectList = _areaBusiness.GetAreaForSelectList();
            referencePersonAdvanceSearchVM.ReferenceType = new ReferenceTypeViewModel();
            referencePersonAdvanceSearchVM.ReferenceType.ReferenceTypeSelectList = _referenceTypeBusiness.GetReferenceTypeSelectList();
            return View(referencePersonAdvanceSearchVM);
        }

        #region InsertUpdateReferencePerson
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ReferencePerson", Mode = "W")]
        public string InsertUpdateReferencePerson(ReferencePersonViewModel referencePersonVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                referencePersonVM.PSASysCommon = new PSASysCommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _psaSysCommon.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _psaSysCommon.GetCurrentDateTime(),
                };
                var result = _referencePersonBusiness.InsertUpdateReferencePerson(Mapper.Map<ReferencePersonViewModel, ReferencePerson>(referencePersonVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region CheckReferencePersonNameExist        
        [AcceptVerbs("Get", "Post")]
        [AuthSecurityFilter(ProjectObject = "ReferencePerson", Mode = "R")]
        public ActionResult CheckReferencePersonNameExist(ReferencePersonViewModel referencePersonVM)
        {
            bool exists =  _referencePersonBusiness.CheckReferencePersonNameExist(Mapper.Map<ReferencePersonViewModel, ReferencePerson>(referencePersonVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>ReferencePerson already is in use </span> <i class='fa fa-times' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MasterPartial
        [HttpGet]
        public ActionResult MasterPartial(int masterCode)
        {
            ReferencePersonViewModel referencePersonVM = masterCode == 0 ? new ReferencePersonViewModel() : Mapper.Map<ReferencePerson, ReferencePersonViewModel>(_referencePersonBusiness.GetReferencePerson(masterCode));
            referencePersonVM.ReferenceType = new ReferenceTypeViewModel();
            referencePersonVM.ReferenceType.ReferenceTypeSelectList = _referenceTypeBusiness.GetReferenceTypeSelectList();
            referencePersonVM.IsUpdate = masterCode == 0 ? false : true;
            return PartialView("_AddReferencePerson", referencePersonVM);
        }
        #endregion MasterPartial

        #region GetAllReferencePerson
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ReferencePerson", Mode = "R")]
        public JsonResult GetAllReferencePerson(DataTableAjaxPostModel model, ReferencePersonAdvanceSearchViewModel referencePersonAdvanceSearchVM)
        {
            referencePersonAdvanceSearchVM.DataTablePaging.Start = model.start;
            referencePersonAdvanceSearchVM.DataTablePaging.Length = (referencePersonAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : referencePersonAdvanceSearchVM.DataTablePaging.Length;
            List<ReferencePersonViewModel> referencePersonVMList = Mapper.Map<List<ReferencePerson>, List<ReferencePersonViewModel>>(_referencePersonBusiness.GetAllReferencePerson(Mapper.Map<ReferencePersonAdvanceSearchViewModel, ReferencePersonAdvanceSearch>(referencePersonAdvanceSearchVM)));
            if (referencePersonAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = referencePersonVMList.Count != 0 ? referencePersonVMList[0].TotalCount : 0;
                int filteredResult = referencePersonVMList.Count != 0 ? referencePersonVMList[0].FilteredCount : 0;
                referencePersonVMList = referencePersonVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = referencePersonVMList.Count != 0 ? referencePersonVMList[0].TotalCount : 0,
                recordsFiltered = referencePersonVMList.Count != 0 ? referencePersonVMList[0].FilteredCount : 0,
                data = referencePersonVMList
            });
        }
        #endregion

        #region DeleteReferencePerson
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ReferencePerson", Mode = "D")]
        public string DeleteReferencePerson(int code)
        {
            try
            {
                var result = _referencePersonBusiness.DeleteReferencePerson(code);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region ReferencePerson SelectList
        public ActionResult ReferencePersonSelectList(string required, bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _psaSysCommon.GetSecurityCode(appUA.UserName, "ReferencePerson");
            if (permission.SubPermissionList != null)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            ReferencePersonViewModel referencePersonVM = new ReferencePersonViewModel();
            referencePersonVM.ReferencePersonSelectList = _referencePersonBusiness.GetReferencePersonSelectList();
            return PartialView("_ReferencePersonSelectList", referencePersonVM);
        }
        #endregion ReferencePerson SelectList

        #region Get ReferencePerson SelectList On Demand
        [HttpPost]
        public ActionResult GetReferencePersonForSelectListOnDemand(string searchTerm)
        {
            List<SelectListItem> referencePersonSelectList = _referencePersonBusiness.GetReferencePersonSelectList();
            var list = referencePersonSelectList != null ? (from SelectListItem in referencePersonSelectList.Where(x => x.Text.ToLower().Contains(searchTerm.ToLower())).ToList()
                                                 select new Select2Model
                                                 {
                                                     text = SelectListItem.Text,
                                                     id = SelectListItem.Value,
                                                 }).ToList() : new List<Select2Model>();
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion Get ReferencePerson SelectList On Demand

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ReferencePerson", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddReferencePersonMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetReferencePersonList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportReferencePersonData();";
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