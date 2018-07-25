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
    public class CustomerController : Controller
    {
        #region Constructor_Injection 

        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        ICustomerBusiness _customerBusiness;
        IPaymentTermBusiness _paymentTermBusiness;
        ICustomerCategoryBusiness _customerCategoryBusiness;
        //IUserBusiness _userBusiness;
        SecurityFilter.ToolBarAccess _tool;
        public CustomerController(ICustomerBusiness customerBusiness, IPaymentTermBusiness paymentTermBusiness, 
            ICustomerCategoryBusiness customerCategoryBusiness,SecurityFilter.ToolBarAccess tool)//, IUserBusiness userBusiness)
        {
            _customerBusiness = customerBusiness;
            _paymentTermBusiness = paymentTermBusiness;
            _customerCategoryBusiness = customerCategoryBusiness;
            _tool = tool;
            //_userBusiness = userBusiness;
        }
        #endregion Constructor_Injection
        // GET: Customer
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "R")]
        public ActionResult Index()
        {
            CustomerAdvanceSearchViewModel customerAdvanceSearchVM = new CustomerAdvanceSearchViewModel();
            return View(customerAdvanceSearchVM);
        }
        #region Check Company Name Exist ForCustomer
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckCompanyNameExistForCustomer(CustomerViewModel customerVM)
        {
            bool exists = _customerBusiness.CheckCompanyNameExistForCustomer(Mapper.Map<CustomerViewModel, Customer>(customerVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Company name is in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion  Check Company Name Exist For Customer
        #region Check CustomerEmail Exists
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckCustomerEmailExist(CustomerViewModel customerVM)
        {
            bool exists = _customerBusiness.CheckCustomerEmailExist(Mapper.Map<CustomerViewModel, Customer>(customerVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Email is in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion Check CustomerEmail Exists
        #region Check Mobile number Exists
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckMobileNumberExist(CustomerViewModel customerVM)
        {
            bool exists = _customerBusiness.CheckMobileNumberExist(Mapper.Map<CustomerViewModel, Customer>(customerVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Mobile number is in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion Check Mobile number Exists
        #region Customer Form
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "R")]
        public ActionResult CustomerForm(Guid id)
        {
            CustomerViewModel customerVM = null;
            try
            {

                if (id != Guid.Empty)
                {
                    customerVM = Mapper.Map<Customer, CustomerViewModel>(_customerBusiness.GetCustomer(id));
                    customerVM.IsUpdate = true;
                }
                else
                {
                    customerVM = new CustomerViewModel();
                    customerVM.IsUpdate = false;
                    customerVM.ID = Guid.Empty;
                }
                customerVM.Titles = new TitlesViewModel()
                {
                    TitlesSelectList = _customerBusiness.GetTitleSelectList()
                };
                //customerVM.PaymentTerm = new PaymentTermViewModel()
                //{
                //    PaymentTermSelectList = _paymentTermBusiness.GetPaymentTermForSelectList()
                //};
                customerVM.CustomerCategory = new CustomerCategoryViewModel()
                {
                    CustomerCategorySelectList = _customerCategoryBusiness.GetCustomerCategoryForSelectList()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_CustomerForm", customerVM);
        }
        #endregion Customer Form
        #region Customer Basic Information
        public ActionResult CustomerBasicInfo(CustomerViewModel customerVM)
        {
            if(customerVM.ID!=Guid.Empty)
            {
                customerVM = Mapper.Map<Customer, CustomerViewModel>(_customerBusiness.GetCustomer(customerVM.ID));
            }
            return PartialView("_CustomerBasicInfo", customerVM);
        }
        #endregion Customer Basic Information
        #region Customer SelectList
        public ActionResult CustomerSelectList(string required,bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            //Permission _permission = Session["UserRights"] as Permission;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "Customer");
            if (permission.SubPermissionList.Count>0)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            CustomerViewModel customerVM = new CustomerViewModel();
            customerVM.CustomerSelectList = _customerBusiness.GetCustomerSelectList();
            return PartialView("_CustomerSelectList", customerVM);
        }
        #endregion Customer SelectList
        #region AddCustomerPartial
        [HttpGet]
        public ActionResult AddCustomerPartial()
        {
            CustomerViewModel customerVM = new CustomerViewModel();
            customerVM.Titles=new TitlesViewModel() {
                TitlesSelectList = _customerBusiness.GetTitleSelectList()
            };
            customerVM.IsUpdate = false;
            return PartialView("_AddCustomerMaster", customerVM);
        }
        #endregion AddCustomerPartial
        #region GetAllCustomer
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "R")]
        public JsonResult GetAllCustomer(DataTableAjaxPostModel model, CustomerAdvanceSearchViewModel customerAdvanceSearchVM)
        {
            //setting options to our model
            customerAdvanceSearchVM.DataTablePaging.Start = model.start;
            customerAdvanceSearchVM.DataTablePaging.Length = (customerAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : customerAdvanceSearchVM.DataTablePaging.Length;

            //CustomerAdvanceSearchVM.OrderColumn = model.order[0].column;
            //CustomerAdvanceSearchVM.OrderDir = model.order[0].dir;

            // action inside a standard controller
            List<CustomerViewModel> CustomerVMList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetAllCustomer(Mapper.Map<CustomerAdvanceSearchViewModel, CustomerAdvanceSearch>(customerAdvanceSearchVM)));
            if (customerAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = CustomerVMList.Count != 0 ? CustomerVMList[0].TotalCount : 0;
                int filteredResult = CustomerVMList.Count != 0 ? CustomerVMList[0].FilteredCount : 0;
                CustomerVMList = CustomerVMList.Skip(0).Take(filteredResult > 1000 ? 1000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = CustomerVMList.Count != 0 ? CustomerVMList[0].TotalCount : 0,
                recordsFiltered = CustomerVMList.Count != 0 ? CustomerVMList[0].FilteredCount : 0,
                data = CustomerVMList
            });
        }
        #endregion GetAllCustomer
        #region InsertUpdateCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "W")]
        public string InsertUpdateCustomer(CustomerViewModel customerVM)
        {
            object result = null;
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                customerVM.PSASysCommon = new PSASysCommonViewModel();
                customerVM.PSASysCommon.CreatedBy = appUA.UserName;
                customerVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                customerVM.PSASysCommon.UpdatedBy = appUA.UserName;
                customerVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                result = _customerBusiness.InsertUpdateCustomer(Mapper.Map<CustomerViewModel, Customer>(customerVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {

                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateCustomer
        #region InsertUpdateCustomer Master
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "W")]
        public string InsertUpdateCustomerMaster(CustomerViewModel customerVM)
        {
            object result = null;
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                customerVM.PSASysCommon = new PSASysCommonViewModel();
                customerVM.PSASysCommon.CreatedBy = appUA.UserName;
                customerVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                customerVM.PSASysCommon.UpdatedBy = appUA.UserName;
                customerVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                result = _customerBusiness.InsertUpdateCustomer(Mapper.Map<CustomerViewModel, Customer>(customerVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {

                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateCustomer Master
        #region DeleteCustomer
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "D")]
        public string DeleteCustomer(Guid id)
        {

            try
            {
                object result = _customerBusiness.DeleteCustomer(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion DeleteCustomer

        #region Get Customer SelectList On Demand
        [HttpPost]
        public ActionResult GetCustomerForSelectListOnDemand(string searchTerm)
        {
            List<SelectListItem> customerSelectList = _customerBusiness.GetCustomerSelectList();
            var list = customerSelectList != null ? (from SelectListItem in customerSelectList.Where(x=>x.Text.ToLower().Contains(searchTerm.ToLower())).ToList()
                                                     select new Select2Model
                                                     {
                                                         text = SelectListItem.Text,
                                                         id = SelectListItem.Value,
                                                     }).ToList()  : new List<Select2Model>();
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion Get Customer SelectList On Demand

   
        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "Customer");
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddCustomer();";

                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export to excel";
                    toolboxVM.ExportBtn.Event = "ExportCustomerData()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetCustomerList();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddCustomer();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveCustomer();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetCustomer();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteCustomer();";

                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveCustomer();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetCustomer();";

                    break;
                case "AddSub":

                    break;
                case "tab1":

                    break;
                case "tab2":

                    break;
                default:
                    return Content("Nochange");
            }
            toolboxVM = _tool.SetToolbarAccess(toolboxVM, permission);
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}