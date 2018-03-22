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
    public class CustomerController : Controller
    {
        #region Constructor_Injection 

        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        ICustomerBusiness _customerBusiness;
        IPaymentTermBusiness _paymentTermBusiness;
        public CustomerController(ICustomerBusiness customerBusiness, IPaymentTermBusiness paymentTermBusiness)
        {
            _customerBusiness = customerBusiness;
            _paymentTermBusiness = paymentTermBusiness;
        }
        #endregion Constructor_Injection
        // GET: Customer
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "R")]
        public ActionResult Index()
        {
            CustomerAdvanceSearchViewModel customerAdvanceSearchVM = new CustomerAdvanceSearchViewModel();
            return View(customerAdvanceSearchVM);
        }
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
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                List<TitlesViewModel> titlesList = Mapper.Map<List<Titles>, List<TitlesViewModel>>(_customerBusiness.GetAllTitles());
                titlesList = titlesList == null ? null : titlesList.OrderBy(attset => attset.Title).ToList();
                foreach (TitlesViewModel tvm in titlesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = tvm.Title,
                        Value = tvm.Title,
                        Selected = false
                    });
                }
                customerVM.TitlesList = selectListItem;
                customerVM.DefaultPaymentTermList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<PaymentTermViewModel> PayTermList = Mapper.Map<List<PaymentTerm>, List<PaymentTermViewModel>>(_paymentTermBusiness.GetAllPayTerm());
                foreach (PaymentTermViewModel PayT in PayTermList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = PayT.Description,
                        Value = PayT.Code,
                        Selected = false
                    });
                }
                customerVM.DefaultPaymentTermList = selectListItem;
                         
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_CustomerForm", customerVM);
        }
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
        [AuthSecurityFilter(ProjectObject = "Customers", Mode = "W")]
        public string InsertUpdateCustomer(CustomerViewModel customerVM)
        {
            object result = null;
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                customerVM.common = new PSASysCommonViewModel();
                customerVM.common.CreatedBy = appUA.UserName;
                customerVM.common.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                customerVM.common.UpdatedBy = appUA.UserName;
                customerVM.common.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                result = _customerBusiness.InsertUpdateCustomer(Mapper.Map<CustomerViewModel, Customer>(customerVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {

                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record="", Message = cm.Message });
            }
        }
        #endregion InsertUpdateCustomer
        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
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
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}