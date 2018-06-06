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

namespace PilotSmithApp.UserInterface.Controllers
{
    public class CustomerCategoryController : Controller
    {
        AppConst _appConst = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        private ICustomerCategoryBusiness _customerCategoryBusiness;
        // GET: CustomerCategory
        #region Constructor Injection
        public CustomerCategoryController(ICustomerCategoryBusiness customerCategoryBusiness)
        {
            _customerCategoryBusiness = customerCategoryBusiness;
        }
        #endregion       
        [AuthSecurityFilter(ProjectObject = "CustomerCategory", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }

        #region InsertUpdateCustomerCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "CustomerCategory", Mode = "W")]
        public string InsertUpdateCustomerCategory(CustomerCategoryViewModel customerCategoryVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                customerCategoryVM.PSASysCommon = new PSASysCommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _psaSysCommon.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _psaSysCommon.GetCurrentDateTime(),
                };
                var result = _customerCategoryBusiness.InsertUpdateCustomerCategory(Mapper.Map<CustomerCategoryViewModel, CustomerCategory>(customerCategoryVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region CheckCustomerCategoryNameExist        
        [AcceptVerbs("Get", "Post")]
        [AuthSecurityFilter(ProjectObject = "CustomerCategory", Mode = "R")]
        public ActionResult CheckCustomerCategoryExist(CustomerCategoryViewModel customerCategoryVM)
        {
            bool exists = _customerCategoryBusiness.CheckCustomerCategoryNameExist(Mapper.Map<CustomerCategoryViewModel, CustomerCategory>(customerCategoryVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>CustomerCategory already is in use </span> <i class='fas fa-times' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MasterPartial      
        public ActionResult MasterPartial(int masterCode)
        {
            CustomerCategoryViewModel customerCategoryVM = masterCode == 0 ? new CustomerCategoryViewModel() : Mapper.Map<CustomerCategory, CustomerCategoryViewModel>(_customerCategoryBusiness.GetCustomerCategory(masterCode));
            customerCategoryVM.IsUpdate = masterCode == 0 ? false : true;
            return PartialView("_AddCustomerCategory", customerCategoryVM);
        }
        #endregion MasterPartial

        #region GetAllCustomerCategory
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "CustomerCategory", Mode = "R")]
        public JsonResult GetAllCustomerCategory(DataTableAjaxPostModel model, CustomerCategoryAdvanceSearchViewModel customerCategoryAdvanceSearchVM)
        {
            customerCategoryAdvanceSearchVM.DataTablePaging.Start = model.start;
            customerCategoryAdvanceSearchVM.DataTablePaging.Length = (customerCategoryAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : customerCategoryAdvanceSearchVM.DataTablePaging.Length;
            List<CustomerCategoryViewModel> customerCategoryVMList = Mapper.Map<List<CustomerCategory>, List<CustomerCategoryViewModel>>(_customerCategoryBusiness.GetAllCustomerCategory(Mapper.Map<CustomerCategoryAdvanceSearchViewModel, CustomerCategoryAdvanceSearch>(customerCategoryAdvanceSearchVM)));
            if (customerCategoryAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = customerCategoryVMList.Count != 0 ? customerCategoryVMList[0].TotalCount : 0;
                int filteredResult = customerCategoryVMList.Count != 0 ? customerCategoryVMList[0].FilteredCount : 0;
                customerCategoryVMList = customerCategoryVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = customerCategoryVMList.Count != 0 ? customerCategoryVMList[0].TotalCount : 0,
                recordsFiltered = customerCategoryVMList.Count != 0 ? customerCategoryVMList[0].FilteredCount : 0,
                data = customerCategoryVMList
            });
        }
        #endregion

        #region DeleteCustomerCategory
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "CustomerCategory", Mode = "D")]
        public string DeleteCustomerCategory(int code)
        {
            try
            {
                var result = _customerCategoryBusiness.DeleteCustomerCategory(code);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region CustomerCategorySelectList
        public ActionResult CustomerCategorySelectList(string required)
        {
            ViewBag.IsRequired = required;
            CustomerCategoryViewModel customerCategoryVM = new CustomerCategoryViewModel();
            customerCategoryVM.CustomerCategorySelectList = _customerCategoryBusiness.GetCustomerCategoryForSelectList();
            return PartialView("_CustomerCategorySelectList", customerCategoryVM);
        }
        #endregion CustomerCategorySelectList

        #region ButtonStyling
        [HttpGet]
         [AuthSecurityFilter(ProjectObject = "CustomerCategory", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddCustomerCategoryMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetCustomerCategoryList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportCustomerCategoryData();";
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