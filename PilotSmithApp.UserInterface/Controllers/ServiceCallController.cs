﻿using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ServiceCallController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        #region Constructor Injection 
        private IServiceCallBusiness _serviceCallBusiness;
        ICustomerBusiness _customerBusiness;
        IBranchBusiness _branchBusiness;
        ICommonBusiness _commonBusiness;
        IAreaBusiness _areaBusiness;
        IDocumentStatusBusiness _documentStatusBusiness;
        IEmployeeBusiness _employeeBusiness;
        SecurityFilter.ToolBarAccess _tool;
        ISaleInvoiceBusiness _saleInvoiceBusiness;
        IServiceTypeBusiness _serviceTypeBusiness;
        ICurrencyBusiness _currencyBusiness;

        public ServiceCallController(IServiceCallBusiness serviceCallBusiness, ICustomerBusiness customerBusiness,
            IBranchBusiness branchBusiness, ICommonBusiness commonBusiness, IAreaBusiness areaBusiness,
            IDocumentStatusBusiness documentStatusBusiness, IEmployeeBusiness employeeBusiness,
            ISaleInvoiceBusiness saleInvoiceBusiness, SecurityFilter.ToolBarAccess tool,
            IServiceTypeBusiness serviceTypeBusiness,ICurrencyBusiness currencyBusiness)
        {
            _serviceCallBusiness = serviceCallBusiness;
            _customerBusiness = customerBusiness;
            _branchBusiness = branchBusiness;
            _commonBusiness = commonBusiness;
            _areaBusiness = areaBusiness;
            _documentStatusBusiness = documentStatusBusiness;
            _employeeBusiness = employeeBusiness;
            _tool = tool;
            _saleInvoiceBusiness = saleInvoiceBusiness;
            _serviceTypeBusiness = serviceTypeBusiness;
            _currencyBusiness = currencyBusiness;
        }
        #endregion Constructor Injection 

        // GET: ServiceCall
        [AuthSecurityFilter(ProjectObject = "ServiceCall", Mode = "R")]
        public ActionResult Index(string id)
        {
            ViewBag.ID = id;
            ServiceCallAdvanceSearchViewModel serviceCallAdvanceSearchVM = new ServiceCallAdvanceSearchViewModel();
            serviceCallAdvanceSearchVM.AdvArea = new AreaViewModel();
            serviceCallAdvanceSearchVM.AdvArea.AreaSelectList = _areaBusiness.GetAreaForSelectList();
            serviceCallAdvanceSearchVM.AdvCustomer = new CustomerViewModel();
            serviceCallAdvanceSearchVM.AdvCustomer.CustomerSelectList = _customerBusiness.GetCustomerSelectList();
            serviceCallAdvanceSearchVM.AdvBranch = new BranchViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            serviceCallAdvanceSearchVM.AdvBranch.BranchList = _branchBusiness.GetBranchForSelectList(appUA.UserName);
            serviceCallAdvanceSearchVM.AdvDocumentStatus = new DocumentStatusViewModel();
            serviceCallAdvanceSearchVM.AdvDocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("SRC");
            serviceCallAdvanceSearchVM.AdvEmployee = new EmployeeViewModel();
            serviceCallAdvanceSearchVM.AdvEmployee.EmployeeSelectList = _employeeBusiness.GetEmployeeSelectList();
            serviceCallAdvanceSearchVM.AdvServicedEmployee = new EmployeeViewModel();
            serviceCallAdvanceSearchVM.AdvServicedEmployee.EmployeeSelectList = _employeeBusiness.GetEmployeeSelectList();
            serviceCallAdvanceSearchVM.AdvServiceType = new ServiceTypeViewModel();
            serviceCallAdvanceSearchVM.AdvServiceType.ServiceTypeSelectList = _serviceTypeBusiness.GetServiceTypeSelectList();
            return View(serviceCallAdvanceSearchVM);
        }

        #region ServiceCall Form
        [AuthSecurityFilter(ProjectObject = "ServiceCall", Mode = "R")]
        public ActionResult ServiceCallForm(Guid id)
        {
            ServiceCallViewModel serviceCallVM = null;
            try
            {
                if (id != Guid.Empty)
                {
                    serviceCallVM = Mapper.Map<ServiceCall, ServiceCallViewModel>(_serviceCallBusiness.GetServiceCall(id));
                    serviceCallVM.IsUpdate = true;
                    AppUA appUA = Session["AppUA"] as AppUA;
                    serviceCallVM.IsDocLocked = serviceCallVM.DocumentOwners.Contains(appUA.UserName);
                    serviceCallVM.Customer = new CustomerViewModel()
                    {
                        CompanyName = serviceCallVM.Customer.CompanyName,
                        Titles = new TitlesViewModel()
                        {
                            TitlesSelectList = _customerBusiness.GetTitleSelectList(),
                        },
                    };
                    serviceCallVM.Currency = new CurrencyViewModel();

                }
                else //(id == Guid.Empty)
                {
                    serviceCallVM = new ServiceCallViewModel();
                    serviceCallVM.IsUpdate = false;
                    serviceCallVM.ID = Guid.Empty;
                    serviceCallVM.DocumentStatus = new DocumentStatusViewModel();
                    serviceCallVM.DocumentStatus.Description = "-";
                    serviceCallVM.Branch = new BranchViewModel();
                    serviceCallVM.Branch.Description = "-";
                    serviceCallVM.Customer = new CustomerViewModel
                    {
                        //CompanyName = serviceCallVM.Customer.CompanyName,
                        Titles = new TitlesViewModel()
                        {
                            TitlesSelectList = _customerBusiness.GetTitleSelectList(),
                        },
                    };
                    serviceCallVM.CurrencyCode = "INR";
                    serviceCallVM.CurrencyRate = 1;
                    serviceCallVM.Currency = new CurrencyViewModel()
                    {
                        CurrencyList = Mapper.Map<List<Currency>, List<CurrencyViewModel>>(_currencyBusiness.GetCurrencyForSelectList())
                    };
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_ServiceCallForm", serviceCallVM);
        }
        #endregion ServiceCall Form

        #region ServiceCall Detail Add
        [AuthSecurityFilter(ProjectObject = "ServiceCall", Mode = "R")]
        public ActionResult AddServiceCallDetail(bool update)
        {
            ServiceCallDetailViewModel serviceCallDetailVM = new ServiceCallDetailViewModel();
            serviceCallDetailVM.IsUpdate = update;
            return PartialView("_AddServiceCallDetail", serviceCallDetailVM);
        }
        #endregion ServiceCall Detail Add

        #region ServiceCall Charges Add
        [AuthSecurityFilter(ProjectObject = "ServiceCall", Mode = "R")]
        public ActionResult AddServiceCallCharge(bool update)
        {
            ServiceCallChargeViewModel serviceCallChargeVM = new ServiceCallChargeViewModel();
            serviceCallChargeVM.IsUpdate = update;
            return PartialView("_AddServiceCallCharge", serviceCallChargeVM);
        }
        #endregion ServiceCall Charges Add

        #region ServiceCall Spare Add
        public ActionResult AddServiceCallDetailSpare(bool update)
        {
            ServiceCallDetailViewModel serviceCallDetailVM = new ServiceCallDetailViewModel();
            serviceCallDetailVM.IsUpdate = update;
            return PartialView("_AddServiceCallDetailSpare", serviceCallDetailVM);
        }
        #endregion ServiceCall Spare Add

        #region GetAllServiceCall
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ServiceCall", Mode = "R")]
        public JsonResult GetAllServiceCall(DataTableAjaxPostModel model, ServiceCallAdvanceSearchViewModel serviceCallAdvanceSearchVM)
        {
            //setting options to our model
            serviceCallAdvanceSearchVM.DataTablePaging.Start = model.start;
            serviceCallAdvanceSearchVM.DataTablePaging.Length = (serviceCallAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : serviceCallAdvanceSearchVM.DataTablePaging.Length;

            List<ServiceCallViewModel> serviceCallVMList = Mapper.Map<List<ServiceCall>, List<ServiceCallViewModel>>(_serviceCallBusiness.GetAllServiceCall(Mapper.Map<ServiceCallAdvanceSearchViewModel, ServiceCallAdvanceSearch>(serviceCallAdvanceSearchVM)));

            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = serviceCallVMList.Count != 0 ? serviceCallVMList[0].TotalCount : 0,
                recordsFiltered = serviceCallVMList.Count != 0 ? serviceCallVMList[0].FilteredCount : 0,
                data = serviceCallVMList
            });
        }
        #endregion GetAllServiceCall

        #region InsertUpdate ServiceCall
        [AuthSecurityFilter(ProjectObject = "ServiceCall", Mode = "W")]
        public string InsertUpdateServiceCall(ServiceCallViewModel serviceCallVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                serviceCallVM.PSASysCommon = new PSASysCommonViewModel();
                serviceCallVM.PSASysCommon.CreatedBy = appUA.UserName;
                serviceCallVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                serviceCallVM.PSASysCommon.UpdatedBy = appUA.UserName;
                serviceCallVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                object ResultFromJS = JsonConvert.DeserializeObject(serviceCallVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                serviceCallVM.ServiceCallDetailList = JsonConvert.DeserializeObject<List<ServiceCallDetailViewModel>>(ReadableFormat);
                ResultFromJS = JsonConvert.DeserializeObject(serviceCallVM.CallChargeJSON);
                ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                serviceCallVM.ServiceCallChargeList = JsonConvert.DeserializeObject<List<ServiceCallChargeViewModel>>(ReadableFormat);
                object result = _serviceCallBusiness.InsertUpdateServiceCall(Mapper.Map<ServiceCallViewModel, ServiceCall>(serviceCallVM));

                if (serviceCallVM.ID == Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Insertion successfull" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Updation successfull" });
                }


            }
            catch (Exception ex)
            {

                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdate ServiceCall

        #region Get ServiceCall DetailList By ServiceCallID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ServiceCall", Mode = "R")]
        public string GetServiceCallDetailListByServiceCallID(Guid serviceCallID)
        {
            try
            {
                List<ServiceCallDetailViewModel> serviceCallItemViewModelList = new List<ServiceCallDetailViewModel>();
                if (serviceCallID == Guid.Empty)
                {
                    ServiceCallDetailViewModel serviceCallDetailVM = new ServiceCallDetailViewModel()
                    {
                        ID = Guid.Empty,
                        ServiceCallID = Guid.Empty,
                        ProductID = Guid.Empty,
                        ProductModelID = Guid.Empty,
                        ProductSpec = string.Empty,
                        GuaranteeYN = null,
                        ServiceStatusCode = null,
                        InstalledDate = null,
                        SpareID = Guid.Empty,
                        Product = new ProductViewModel()
                        {
                            ID = Guid.Empty,
                            Code = string.Empty,
                            Name = string.Empty,
                        },
                        ProductModel = new ProductModelViewModel()
                        {
                            ID = Guid.Empty,
                            Name = string.Empty
                        },
                        Spare = new SpareViewModel()
                        {
                            ID = Guid.Empty,
                            Code = string.Empty,
                            Name = string.Empty,
                        },
                    };
                    serviceCallItemViewModelList.Add(serviceCallDetailVM);
                }
                else
                {
                    serviceCallItemViewModelList = Mapper.Map<List<ServiceCallDetail>, List<ServiceCallDetailViewModel>>(_serviceCallBusiness.GetServiceCallDetailListByServiceCallID(serviceCallID));
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = serviceCallItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get serviceCall DetailList By ServiceCallID

        #region Get ServiceCall ChargeList By ServiceCallID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ServiceCall", Mode = "R")]
        public string GetServiceCallChargeListByServiceCallID(Guid serviceCallID)
        {
            try
            {
                List<ServiceCallChargeViewModel> serviceCallChargeViewModelList = new List<ServiceCallChargeViewModel>();
                if (serviceCallID == Guid.Empty)
                {
                    ServiceCallChargeViewModel serviceCallChargeVM = new ServiceCallChargeViewModel()
                    {
                        ID = Guid.Empty,
                        ServiceCallID = Guid.Empty,
                        ChargeAmount = 0,
                        OtherCharge = new OtherChargeViewModel()
                        {
                            Description = "",
                        },
                        TaxType = new TaxTypeViewModel()
                        {
                            ValueText = "",
                        }
                    };
                    serviceCallChargeViewModelList.Add(serviceCallChargeVM);
                }
                else
                {
                    serviceCallChargeViewModelList = Mapper.Map<List<ServiceCallCharge>, List<ServiceCallChargeViewModel>>(_serviceCallBusiness.GetServiceCallChargeDetailListByServiceCallID(serviceCallID));
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = serviceCallChargeViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get serviceCall OtherChargeList By serviceCallID

        #region GetSaleInvoiceSummaryByCustomer
        public ActionResult GetSaleInvoiceByCustomerID(Guid? customerID)
        {
            ServiceCallViewModel serviceCallVM = new ServiceCallViewModel();
            serviceCallVM.SaleInvoiceList = new List<SaleInvoiceViewModel>();
            if (customerID != null && customerID != Guid.Empty)
            {
                SaleInvoiceAdvanceSearchViewModel saleInvoiceAdvanceSearchVM = new SaleInvoiceAdvanceSearchViewModel()
                {
                    AdvCustomerID = (Guid)customerID,
                    DataTablePaging = new DataTablePagingViewModel()
                    {
                        Start = 0,
                        Length = 50
                    }
                };
                serviceCallVM.SaleInvoiceList = Mapper.Map<List<SaleInvoice>, List<SaleInvoiceViewModel>>(_saleInvoiceBusiness.GetAllSaleInvoice(Mapper.Map<SaleInvoiceAdvanceSearchViewModel, SaleInvoiceAdvanceSearch>(saleInvoiceAdvanceSearchVM)));
            }
            return PartialView("_GetSaleInvoiceByCustomerID", serviceCallVM);
        }
        #endregion GetSaleInvoiceByCustomer

        #region Delete ServiceCall
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ServiceCall", Mode = "D")]
        public string DeleteServiceCall(Guid id)
        {
            try
            {
                object result = _serviceCallBusiness.DeleteServiceCall(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion Delete ServiceCall

        #region Delete ServiceCall Detail
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ServiceCall", Mode = "D")]
        public string DeleteServiceCallDetail(Guid id)
        {
            try
            {
                object result = _serviceCallBusiness.DeleteServiceCallDetail(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion Delete ServiceCall Detail

        #region Delete ServiceCall Charge
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ServiceCall", Mode = "D")]
        public string DeleteServiceCallChargeDetail(Guid id)
        {
            try
            {
                object result = _serviceCallBusiness.DeleteServiceCallChargeDetail(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion Delete ServiceCall Charge

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ServiceCall", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType, Guid? id)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "ServiceCall");
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddServiceCall();";

                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export to excel";
                    toolboxVM.ExportBtn.Event = "ExportServiceCallData()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetServiceCallList();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddServiceCall();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveServiceCall();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetServiceCall();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteServiceCall();";

                    //toolboxVM.SendForApprovalBtn.Visible = true;
                    //toolboxVM.SendForApprovalBtn.Text = "Send";
                    //toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    //toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('QUO');";
                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','SRC');";
                    break;
                case "LockDocument":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddServiceCall();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Disable = true;
                    toolboxVM.savebtn.DisableReason = "Document Locked";
                    toolboxVM.savebtn.Event = "";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Disable = true;
                    toolboxVM.resetbtn.DisableReason = "Document Locked";
                    toolboxVM.resetbtn.Event = "";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Disable = true;
                    toolboxVM.deletebtn.DisableReason = "Document Locked";
                    toolboxVM.deletebtn.Event = "";

                    //toolboxVM.SendForApprovalBtn.Visible = true;
                    //toolboxVM.SendForApprovalBtn.Text = "Send";
                    //toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    //toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('QUO');";
                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','SRC');";
                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveServiceCall();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetServiceCall();";

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