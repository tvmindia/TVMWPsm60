using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using SAMTool.BusinessServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.IO;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class ReportController : Controller
    {
        #region Constructor Injection  
        AppConst _appConst = new AppConst();
        private PSASysCommon _pSASysCommon = new PSASysCommon();
        IReportBusiness _reportBusiness;
        IProductBusiness _productBusiness;
        IDocumentStatusBusiness _documentStatusBusiness;
        IReferenceTypeBusiness _referenceTypeBusiness;
        ICustomerCategoryBusiness _customerCategoryBusiness;
        IEnquiryGradeBusiness _enquiryGradeBusiness;
        IEmployeeBusiness _employeeBusiness;
        ICustomerBusiness _customerBusiness;
        IProductModelBusiness _productModelBusiness;
        public ReportController(IReportBusiness reportBusiness, IProductBusiness productBusiness, IDocumentStatusBusiness documentStatusBusiness, IReferenceTypeBusiness referenceTypeBusiness,
        ICustomerCategoryBusiness customerCategoryBusiness, IEnquiryGradeBusiness enquiryGradeBusiness,
        IEmployeeBusiness employeeBusiness, ICustomerBusiness customerBusiness, IProductModelBusiness productModelBusiness)
        {
            _reportBusiness = reportBusiness;
            _productBusiness = productBusiness;
            _documentStatusBusiness = documentStatusBusiness;
            _referenceTypeBusiness = referenceTypeBusiness;
            _customerCategoryBusiness = customerCategoryBusiness;
            _enquiryGradeBusiness = enquiryGradeBusiness;
            _employeeBusiness = employeeBusiness;
            _customerBusiness = customerBusiness;
            _productModelBusiness = productModelBusiness;
        }
        #endregion Constructor Injection  
        // GET: Report
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Report", Mode = "R")]
        public ActionResult Index(string searchTerm)
        {
            PSASysReportViewModel PSASysReport = new PSASysReportViewModel();
            PSASysReport.PSASysReportList = Mapper.Map<List<PSASysReport>, List<PSASysReportViewModel>>(_reportBusiness.GetAllReport(searchTerm));
            PSASysReport.PSASysReportList = PSASysReport.PSASysReportList != null ? PSASysReport.PSASysReportList.OrderBy(s => s.GroupOrder).ToList() : null;
            return View(PSASysReport);
        }

        //[HttpGet]
        //[AuthSecurityFilter(ProjectObject = "PendingSaleOrderProductionReport", Mode = "R")]
        //public ActionResult PendingSaleOrderProductionReport()
        //{
        //    PendingSaleOrderProductionReportViewModel pendingSaleOrderProductionReportVM = new PendingSaleOrderProductionReportViewModel();
        //    pendingSaleOrderProductionReportVM.Product = new ProductViewModel();
        //    pendingSaleOrderProductionReportVM.Product.ProductSelectList = _productBusiness.GetProductForSelectList();
        //    return View(pendingSaleOrderProductionReportVM);
        //}

        //#region GetPendingSaleOrderProductionReport
        //[HttpPost]
        //[AuthSecurityFilter(ProjectObject = "PendingSaleOrderProductionReport", Mode = "R")]
        //public JsonResult GetPendingSaleOrderProductionReport(DataTableAjaxPostModel model, PendingSaleOrderProductionReportViewModel pendingSaleOrderProductionReportVM)
        //{
        //    pendingSaleOrderProductionReportVM.DataTablePaging.Start = model.start;
        //    pendingSaleOrderProductionReportVM.DataTablePaging.Length = (pendingSaleOrderProductionReportVM.DataTablePaging.Length == 0 ? model.length : pendingSaleOrderProductionReportVM.DataTablePaging.Length);

        //    List<PendingSaleOrderProductionReportViewModel> pendingSaleOrderProductionList = Mapper.Map<List<PendingSaleOrderProductionReport>, List<PendingSaleOrderProductionReportViewModel>>(_reportBusiness.GetPendingSaleOrderProductionReport(Mapper.Map<PendingSaleOrderProductionReportViewModel, PendingSaleOrderProductionReport>(pendingSaleOrderProductionReportVM)));

        //    if (pendingSaleOrderProductionReportVM.DataTablePaging.Length == -1)
        //    {
        //        int totalResult = pendingSaleOrderProductionList.Count != 0 ? pendingSaleOrderProductionList[0].TotalCount : 0;
        //        int filteredResult = pendingSaleOrderProductionList.Count != 0 ? pendingSaleOrderProductionList[0].FilteredCount : 0;
        //        pendingSaleOrderProductionList = pendingSaleOrderProductionList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
        //    }
        //    var settings = new JsonSerializerSettings
        //    {
        //        //ContractResolver = new CamelCasePropertyNamesContractResolver(),
        //        Formatting = Formatting.None
        //    };

        //    return Json(new
        //    {
        //        draw = model.draw,
        //        recordsTotal = pendingSaleOrderProductionList.Count != 0 ? pendingSaleOrderProductionList[0].TotalCount : 0,
        //        recordsFiltered = pendingSaleOrderProductionList.Count != 0 ? pendingSaleOrderProductionList[0].FilteredCount : 0,
        //        data = pendingSaleOrderProductionList
        //    });
        //}
        //#endregion GetPendingSaleOrderProductionReport


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "EnquiryReport", Mode = "R")]
        public ActionResult EnquiryReport()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            EnquiryReportViewModel EnquiryReportVM = new EnquiryReportViewModel();
            EnquiryReportVM.DocumentStatus = new DocumentStatusViewModel();
            EnquiryReportVM.DocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("ENQ");
            EnquiryReportVM.ReferenceType = new ReferenceTypeViewModel();
            EnquiryReportVM.ReferenceType.ReferenceTypeSelectList = _referenceTypeBusiness.GetReferenceTypeSelectList();
            EnquiryReportVM.CustomerCategory = new CustomerCategoryViewModel();
            EnquiryReportVM.CustomerCategory.CustomerCategorySelectList = _customerCategoryBusiness.GetCustomerCategoryForSelectList();
            EnquiryReportVM.EnquiryGrade = new EnquiryGradeViewModel();
            EnquiryReportVM.EnquiryGrade.EnquiryGradeSelectList = _enquiryGradeBusiness.GetEnquiryGradeSelectList();
            EnquiryReportVM.Employee = new EmployeeViewModel();
            EnquiryReportVM.Employee.EmployeeSelectList = _employeeBusiness.GetEmployeeSelectList();
            EnquiryReportVM.Customer = new CustomerViewModel();
            EnquiryReportVM.Customer.CustomerList = _customerBusiness.GetCustomerSelectListOnDemand();

            return View(EnquiryReportVM);
        }

        #region GetEnquiryReport
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "EnquiryReport", Mode = "R")]
        public JsonResult GetEnquiryReport(DataTableAjaxPostModel model, EnquiryReportViewModel enquiryReportVM)
        {
            enquiryReportVM.DataTablePaging.Start = model.start;
            enquiryReportVM.DataTablePaging.Length = (enquiryReportVM.DataTablePaging.Length == 0 ? model.length : enquiryReportVM.DataTablePaging.Length);

            List<EnquiryReportViewModel> enquiryReportList = Mapper.Map<List<EnquiryReport>, List<EnquiryReportViewModel>>(_reportBusiness.GetEnquiryReport(Mapper.Map<EnquiryReportViewModel, EnquiryReport>(enquiryReportVM)));
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = enquiryReportList.Count != 0 ? enquiryReportList[0].TotalCount : 0,
                recordsFiltered = enquiryReportList.Count != 0 ? enquiryReportList[0].FilteredCount : 0,
                data = enquiryReportList
            });
        }
        #endregion GetEnquiryReport
        [HttpGet]

        [AuthSecurityFilter(ProjectObject = "EnquiryFollowupReport", Mode = "R")]
        public ActionResult EnquiryFollowupReport()
        {
            EnquiryFollowupReportViewModel EnquiryFollowupReportVM = new EnquiryFollowupReportViewModel();
            EnquiryFollowupReportVM.Customer = new CustomerViewModel();
            EnquiryFollowupReportVM.Customer.CustomerList = _customerBusiness.GetCustomerSelectListOnDemand();
            return View(EnquiryFollowupReportVM);
        }

        #region GetEnquiryFollowupReport
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "EnquiryFollowupReport", Mode = "R")]
        public JsonResult GetEnquiryFollowupReport(DataTableAjaxPostModel model, EnquiryFollowupReportViewModel enquiryFollowupReportVM)
        {
            enquiryFollowupReportVM.DataTablePaging.Start = model.start;
            enquiryFollowupReportVM.DataTablePaging.Length = (enquiryFollowupReportVM.DataTablePaging.Length == 0 ? model.length : enquiryFollowupReportVM.DataTablePaging.Length);

            List<EnquiryFollowupReportViewModel> enquiryFollowupReportList = Mapper.Map<List<EnquiryFollowupReport>, List<EnquiryFollowupReportViewModel>>(_reportBusiness.GetEnquiryFollowupReport(Mapper.Map<EnquiryFollowupReportViewModel, EnquiryFollowupReport>(enquiryFollowupReportVM)));
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = enquiryFollowupReportList.Count != 0 ? enquiryFollowupReportList[0].TotalCount : 0,
                recordsFiltered = enquiryFollowupReportList.Count != 0 ? enquiryFollowupReportList[0].FilteredCount : 0,
                data = enquiryFollowupReportList
            });
        }
        #endregion GetEnquiryFollowupReport

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "EstimateReport", Mode = "R")]
        public ActionResult EstimateReport()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            EstimateReportViewModel EstimateReportVM = new EstimateReportViewModel();
            EstimateReportVM.DocumentStatus = new DocumentStatusViewModel();
            EstimateReportVM.DocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("EST");
            EstimateReportVM.CustomerCategory = new CustomerCategoryViewModel();
            EstimateReportVM.CustomerCategory.CustomerCategorySelectList = _customerCategoryBusiness.GetCustomerCategoryForSelectList();
            EstimateReportVM.Employee = new EmployeeViewModel();
            EstimateReportVM.Employee.EmployeeSelectList = _employeeBusiness.GetEmployeeSelectList();
            EstimateReportVM.Customer = new CustomerViewModel();
            EstimateReportVM.Customer.CustomerList = _customerBusiness.GetCustomerSelectListOnDemand();
            return View(EstimateReportVM);       
    }
        #region GetEstimateReport      
        public JsonResult GetEstimateReport(DataTableAjaxPostModel model, EstimateReportViewModel estimateReportVM)
        {
            estimateReportVM.DataTablePaging.Start = model.start;
            estimateReportVM.DataTablePaging.Length = (estimateReportVM.DataTablePaging.Length == 0 ? model.length : estimateReportVM.DataTablePaging.Length);

            List<EstimateReportViewModel> estimateReportList = Mapper.Map<List<EstimateReport>, List<EstimateReportViewModel>>(_reportBusiness.GetEstimateReport(Mapper.Map<EstimateReportViewModel, EstimateReport>(estimateReportVM)));
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = estimateReportList.Count != 0 ? estimateReportList[0].TotalCount : 0,
                recordsFiltered = estimateReportList.Count != 0 ? estimateReportList[0].FilteredCount : 0,
                data = estimateReportList
            });
        }


        #endregion GetEstimateReport


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "QuotationReport", Mode = "R")]
        public ActionResult QuotationReport()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            QuotationReportViewModel QuotationReportVM = new QuotationReportViewModel();
            QuotationReportVM.DocumentStatus = new DocumentStatusViewModel();
            QuotationReportVM.DocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("QUO");           
            QuotationReportVM.Employee = new EmployeeViewModel();
            QuotationReportVM.Employee.EmployeeSelectList = _employeeBusiness.GetEmployeeSelectList();
            QuotationReportVM.Customer = new CustomerViewModel();
            QuotationReportVM.Customer.CustomerList = _customerBusiness.GetCustomerSelectListOnDemand();
            return View(QuotationReportVM);
        }

        #region GetQuotationReport
        public JsonResult GetQuotationReport(DataTableAjaxPostModel model, QuotationReportViewModel quotationReportVM)
        {
            quotationReportVM.DataTablePaging.Start = model.start;
            quotationReportVM.DataTablePaging.Length = (quotationReportVM.DataTablePaging.Length == 0 ? model.length : quotationReportVM.DataTablePaging.Length);

            List<QuotationReportViewModel> quotationReportList = Mapper.Map<List<QuotationReport>, List<QuotationReportViewModel>>(_reportBusiness.GetQuotationReport(Mapper.Map<QuotationReportViewModel, QuotationReport>(quotationReportVM)));
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = quotationReportList.Count != 0 ? quotationReportList[0].TotalCount : 0,
                recordsFiltered = quotationReportList.Count != 0 ? quotationReportList[0].FilteredCount : 0,
                data = quotationReportList
            });
        }

        #endregion GetQuotationReport

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleOrderReport", Mode = "R")]
        public ActionResult SaleOrderReport()
        {        

            List<SelectListItem> selectListItem = new List<SelectListItem>();
            SaleOrderReportViewModel SaleOrderReportVM = new SaleOrderReportViewModel();
            SaleOrderReportVM.DocumentStatus = new DocumentStatusViewModel();
            SaleOrderReportVM.DocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("SOD");
            SaleOrderReportVM.Employee = new EmployeeViewModel();
            SaleOrderReportVM.Employee.EmployeeSelectList = _employeeBusiness.GetEmployeeSelectList();
            SaleOrderReportVM.Customer = new CustomerViewModel();
            SaleOrderReportVM.Customer.CustomerList = _customerBusiness.GetCustomerSelectListOnDemand();
            SaleOrderReportVM.Product = new ProductViewModel();
            SaleOrderReportVM.Product.ProductSelectList = _productBusiness.GetProductForSelectList();
            SaleOrderReportVM.ProductModel = new ProductModelViewModel();
            SaleOrderReportVM.ProductModel.ProductModelSelectList = _productModelBusiness.GetProductModelSelectList();
            return View(SaleOrderReportVM);
        }


        #region GetSaleOrderStandardReport
        public JsonResult GetSaleOrderStandardReport(DataTableAjaxPostModel model, SaleOrderReportViewModel saleOrderReportVM)
        {
            //PSASysCommon pSASysCommon = new PSASysCommon();
            //DateTime dt = pSASysCommon.GetCurrentDateTime();
            //if (saleOrderReportVM != null)
            //{
            //    if (saleOrderReportVM.DateFilter == "3")
            //    {                    
            //        saleOrderReportVM.AdvFromDate = dt.AddMonths(-3).ToString("dd-MMM-yyyy");
            //        saleOrderReportVM.AdvToDate = dt.ToString("dd-MMM-yyyy");
            //    }
            //    if (saleOrderReportVM.DateFilter == "6")
            //    {
            //        saleOrderReportVM.AdvFromDate = dt.AddMonths(-6).ToString("dd-MMM-yyyy");                  
            //        saleOrderReportVM.AdvToDate = dt.ToString("dd-MMM-yyyy");
            //    }
            //    if (saleOrderReportVM.DateFilter == "12")
            //    {
            //        saleOrderReportVM.AdvFromDate = dt.AddMonths(-12).ToString("dd-MMM-yyyy");                 
            //        saleOrderReportVM.AdvToDate = dt.ToString("dd-MMM-yyyy");
            //    }
            //}

            saleOrderReportVM.DataTablePaging.Start = model.start;
            saleOrderReportVM.DataTablePaging.Length = (saleOrderReportVM.DataTablePaging.Length == 0 ? model.length : saleOrderReportVM.DataTablePaging.Length);

            List<SaleOrderReportViewModel> saleOrderReportList = Mapper.Map<List<SaleOrderReport>, List<SaleOrderReportViewModel>>(_reportBusiness.GetSaleOrderStandardReport(Mapper.Map<SaleOrderReportViewModel, SaleOrderReport>(saleOrderReportVM)));
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = saleOrderReportList.Count != 0 ? saleOrderReportList[0].TotalCount : 0,
                recordsFiltered = saleOrderReportList.Count != 0 ? saleOrderReportList[0].FilteredCount : 0,
                data = saleOrderReportList
            });
        }

        #endregion GetSaleOrderStandardReport



        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PendingSaleOrderReport", Mode = "R")]
        public ActionResult PendingSaleOrderReport()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            PendingSaleOrderReportViewModel PendingSaleOrderReportVM = new PendingSaleOrderReportViewModel();
            PendingSaleOrderReportVM.DocumentStatus = new DocumentStatusViewModel();
            PendingSaleOrderReportVM.DocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("SOD");
            PendingSaleOrderReportVM.Employee = new EmployeeViewModel();
            PendingSaleOrderReportVM.Employee.EmployeeSelectList = _employeeBusiness.GetEmployeeSelectList();
            PendingSaleOrderReportVM.Customer = new CustomerViewModel();
            PendingSaleOrderReportVM.Customer.CustomerList = _customerBusiness.GetCustomerSelectListOnDemand();
            PendingSaleOrderReportVM.Product = new ProductViewModel();
            PendingSaleOrderReportVM.Product.ProductSelectList = _productBusiness.GetProductForSelectList();
            PendingSaleOrderReportVM.ProductModel = new ProductModelViewModel();
            PendingSaleOrderReportVM.ProductModel.ProductModelSelectList = _productModelBusiness.GetProductModelSelectList();
            return View(PendingSaleOrderReportVM);
        }

        #region GetPendingSaleOrderReport
        public JsonResult GetPendingSaleOrderReport(DataTableAjaxPostModel model, PendingSaleOrderReportViewModel pendingSaleOrderReportVM)
        {
            pendingSaleOrderReportVM.DataTablePaging.Start = model.start;
            pendingSaleOrderReportVM.DataTablePaging.Length = (pendingSaleOrderReportVM.DataTablePaging.Length == 0 ? model.length : pendingSaleOrderReportVM.DataTablePaging.Length);

            List<PendingSaleOrderReportViewModel> pendingSaleOrderReportList = Mapper.Map<List<PendingSaleOrderReport>, List<PendingSaleOrderReportViewModel>>(_reportBusiness.GetPendingSaleOrderReport(Mapper.Map<PendingSaleOrderReportViewModel, PendingSaleOrderReport>(pendingSaleOrderReportVM)));
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = pendingSaleOrderReportList.Count != 0 ? pendingSaleOrderReportList[0].TotalCount : 0,
                recordsFiltered = pendingSaleOrderReportList.Count != 0 ? pendingSaleOrderReportList[0].FilteredCount : 0,
                data = pendingSaleOrderReportList
            });
        }

        #endregion GetPendingSaleOrderReport


        public void DownloadExcel(ExcelExportViewModel excelExportVM)
        {
            try
            {
                string fileName = "";
                PSASysCommon pSASysCommon = new PSASysCommon();
                ExcelPackage excel = new ExcelPackage();
                object ResultFromJS = null;
                string ReadableFormat = null;
                switch (excelExportVM.DocumentType)
                {
                    case "ENQ":
                        fileName = "EnquiryReport" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        EnquiryReportViewModel enquiryReportVM = new EnquiryReportViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        enquiryReportVM = JsonConvert.DeserializeObject<EnquiryReportViewModel>(ReadableFormat);
                        List<EnquiryReportViewModel> enquiryReportList = Mapper.Map<List<EnquiryReport>, List<EnquiryReportViewModel>>(_reportBusiness.GetEnquiryReport(Mapper.Map<EnquiryReportViewModel, EnquiryReport>(enquiryReportVM)));
                        var enquiryreportworkSheet = excel.Workbook.Worksheets.Add("EnquiryReport");
                        EnquiryReportViewModel[] enquiryReportVMListArray = enquiryReportList.ToArray();
                        enquiryreportworkSheet.Cells[1, 1].LoadFromCollection(enquiryReportVMListArray.Select(x => new {
                        EnquiryNo = x.EnqNo,
                        EnquiryDate=x.EnquiryDateFormatted,
                        ContactPerson = x.Customer.ContactPerson,
                        CompanyName = x.Customer.CompanyName,
                        RequirementSpecification = x.RequirementSpec,
                        Area = x.Area.Description,
                        ReferredBy = x.ReferencePerson.Name,
                        DocumentOwner = x.PSAUser.LoginName,
                        DocumentStatus = x.DocumentStatus.Description,
                        Branch = x.Branch.Description,
                        AttendedBy =x.Employee.Name,
                        Grade =x.EnquiryGrade.Description,
                        Amount =x.Amount
                        }), true, TableStyles.Light1);
                        enquiryreportworkSheet.Column(1).AutoFit();
                        enquiryreportworkSheet.Column(2).AutoFit();
                        enquiryreportworkSheet.Column(3).AutoFit();
                        enquiryreportworkSheet.Column(4).Width = 40;
                        enquiryreportworkSheet.Column(5).Width = 40;
                        enquiryreportworkSheet.Column(6).AutoFit();
                        enquiryreportworkSheet.Column(7).AutoFit();
                        enquiryreportworkSheet.Column(8).AutoFit();
                        enquiryreportworkSheet.Column(9).AutoFit();
                        enquiryreportworkSheet.Column(10).AutoFit();
                        enquiryreportworkSheet.Column(11).AutoFit();
                        enquiryreportworkSheet.Column(12).AutoFit();
                        enquiryreportworkSheet.Column(13).AutoFit();                      
                        break;
                    case "EnquiryFollowUp":
                        fileName = "EnquiryFollowupReport" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        EnquiryFollowupReportViewModel enquiryFollowupReportVM = new EnquiryFollowupReportViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        enquiryFollowupReportVM = JsonConvert.DeserializeObject<EnquiryFollowupReportViewModel>(ReadableFormat);
                        List<EnquiryFollowupReportViewModel> enquiryFollowupReportList = Mapper.Map<List<EnquiryFollowupReport>, List<EnquiryFollowupReportViewModel>>(_reportBusiness.GetEnquiryFollowupReport(Mapper.Map<EnquiryFollowupReportViewModel, EnquiryFollowupReport>(enquiryFollowupReportVM)));
                        var enquiryfollowupreportworkSheet = excel.Workbook.Worksheets.Add("EnquiryFollowUp");
                        EnquiryFollowupReportViewModel[] enquiryFollowupReportVMListArray = enquiryFollowupReportList.ToArray();
                        enquiryfollowupreportworkSheet.Cells[1, 1].LoadFromCollection(enquiryFollowupReportVMListArray.Select(x => new {
                           Followupdate = x.FollowupDateFormatted,
                           FollowupTime = x.FollowupTimeFormatted,
                           Priority =x.Priority,
                           Status =x.Status,
                           EnquiryNo =x.EnqNo,
                           EnquiryDate = x.EnquiryDateFormatted,
                           ContactPerson = x.Customer.ContactPerson,
                           CompanyName = x.Customer.CompanyName,
                           ContactNo = x.ContactNo,
                           Remarks=x.FollowupRemarks                            
                            
                        }), true, TableStyles.Light1);
                        enquiryfollowupreportworkSheet.Column(1).AutoFit();
                        enquiryfollowupreportworkSheet.Column(2).AutoFit();
                        enquiryfollowupreportworkSheet.Column(3).AutoFit();
                        enquiryfollowupreportworkSheet.Column(4).AutoFit();
                        enquiryfollowupreportworkSheet.Column(5).AutoFit();
                        enquiryfollowupreportworkSheet.Column(6).AutoFit();
                        enquiryfollowupreportworkSheet.Column(7).AutoFit();
                        enquiryfollowupreportworkSheet.Column(8).Width = 40;
                        enquiryfollowupreportworkSheet.Column(9).AutoFit();   
                        enquiryfollowupreportworkSheet.Column(10).AutoFit();

                        break;
                    case "EstimateReport":
                        fileName = "EstimateReport" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        EstimateReportViewModel estimateReportVM = new EstimateReportViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        estimateReportVM = JsonConvert.DeserializeObject<EstimateReportViewModel>(ReadableFormat);
                        List<EstimateReportViewModel> estimateReportList = Mapper.Map<List<EstimateReport>, List<EstimateReportViewModel>>(_reportBusiness.GetEstimateReport(Mapper.Map<EstimateReportViewModel, EstimateReport>(estimateReportVM)));
                        var estimatereportworkSheet = excel.Workbook.Worksheets.Add("EstimateReport");
                        EstimateReportViewModel[] estimateReportVMListArray = estimateReportList.ToArray();
                        estimatereportworkSheet.Cells[1, 1].LoadFromCollection(estimateReportVMListArray.Select(x => new {
                           EstimateNo=x.EstNo,
                           EstimateDate=x.EstimateDateFormatted,
                           CompanyName =x.Customer.CompanyName ,
                           ContactPerson =x.Customer.ContactPerson,
                           Area = x.Area.Description,
                           PreparedBy =x.PreparedBy,
                           DocumentStatus=x.DocumentStatus.Description,
                           Branch = x.Branch.Description,
                           DocumentOwner=x.PSAUser.LoginName,
                           Amount = x.Amount,
                           Notes = x.Notes
                        }), true, TableStyles.Light1);
                        estimatereportworkSheet.Column(1).AutoFit();
                        estimatereportworkSheet.Column(2).AutoFit();
                        estimatereportworkSheet.Column(3).AutoFit();
                        estimatereportworkSheet.Column(4).Width = 40;
                        estimatereportworkSheet.Column(5).Width = 40;
                        estimatereportworkSheet.Column(6).AutoFit();
                        estimatereportworkSheet.Column(7).AutoFit();
                        estimatereportworkSheet.Column(8).AutoFit();
                        estimatereportworkSheet.Column(9).AutoFit();
                        estimatereportworkSheet.Column(10).AutoFit();
                        estimatereportworkSheet.Column(11).AutoFit();                       
                        break;
                    case "QuotationReport":
                        fileName = "QuotationReport" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        QuotationReportViewModel quotationReportVM = new QuotationReportViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        quotationReportVM = JsonConvert.DeserializeObject<QuotationReportViewModel>(ReadableFormat);
                        List<QuotationReportViewModel> quotationReportList = Mapper.Map<List<QuotationReport>, List<QuotationReportViewModel>>(_reportBusiness.GetQuotationReport(Mapper.Map<QuotationReportViewModel, QuotationReport>(quotationReportVM)));
                        var quotationreportworkSheet = excel.Workbook.Worksheets.Add("QuotationReport");
                        QuotationReportViewModel[] quotationReportVMListArray = quotationReportList.ToArray();
                        quotationreportworkSheet.Cells[1, 1].LoadFromCollection(quotationReportVMListArray.Select(x => new {
                            QuotationNo = x.QuotationNo,
                            QuotationDate = x.QuoteDateFormatted,
                            CompanyName = x.Customer.CompanyName,
                            ContactPerson = x.Customer.ContactPerson,
                            Area = x.Area.Description,
                            ReferredBy = x.ReferencePerson.Name,
                            PreparedBy = x.PreparedBy,
                            Branch = x.Branch.Description,                          
                            DocumentOwner = x.PSAUser.LoginName,
                            DocumentStatus = x.DocumentStatus.Description,
                            ApprovalStatus = x.ApprovalStatus.Description,
                            Amount = x.Amount,
                            Notes = x.Notes
                        }), true, TableStyles.Light1);
                        quotationreportworkSheet.Column(1).AutoFit();
                        quotationreportworkSheet.Column(2).AutoFit();
                        quotationreportworkSheet.Column(3).AutoFit();
                        quotationreportworkSheet.Column(4).Width = 40;
                        quotationreportworkSheet.Column(5).Width = 40;
                        quotationreportworkSheet.Column(6).AutoFit();
                        quotationreportworkSheet.Column(7).AutoFit();
                        quotationreportworkSheet.Column(8).AutoFit();
                        quotationreportworkSheet.Column(9).AutoFit();
                        quotationreportworkSheet.Column(10).AutoFit();
                        quotationreportworkSheet.Column(11).AutoFit();
                        quotationreportworkSheet.Column(12).AutoFit();
                        quotationreportworkSheet.Column(13).AutoFit();

                        break;
                    case "SaleOrderReport":
                        fileName = "SaleOrderReport" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        SaleOrderReportViewModel saleOrderReportVM = new SaleOrderReportViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        saleOrderReportVM = JsonConvert.DeserializeObject<SaleOrderReportViewModel>(ReadableFormat);
                        List<SaleOrderReportViewModel> saleOrderReportList = Mapper.Map<List<SaleOrderReport>, List<SaleOrderReportViewModel>>(_reportBusiness.GetSaleOrderStandardReport(Mapper.Map<SaleOrderReportViewModel,SaleOrderReport>(saleOrderReportVM)));                        
                        var saleorderreportworkSheet = excel.Workbook.Worksheets.Add("SaleOrderReport");
                        SaleOrderReportViewModel[] saleOrderReportVMListArray = saleOrderReportList.ToArray();
                        saleorderreportworkSheet.Cells[1, 1].LoadFromCollection(saleOrderReportVMListArray.Select(x => new {
                            SaleOrderNo=x.SaleOrdNo,
                            SaleOrderDate=x.SaleOrderDateFormatted,
                            CompanyName=x.Customer.CompanyName,
                            ContactPerson=x.Customer.ContactPerson,
                            ProductName=x.Product.Name,
                            ProductModel = x.ProductModel.Name,
                            ProductSpecification=x.ProductSpec,
                            Qty=x.Qty,
                            Unit=x.Unit.Description,
                            Amount=x.Amount,
                            Branch=x.Branch.Description,
                            DocumentOwner=x.PSAUser.LoginName

                        }), true, TableStyles.Light1);
                        saleorderreportworkSheet.Column(1).AutoFit();
                        saleorderreportworkSheet.Column(2).AutoFit();
                        saleorderreportworkSheet.Column(3).Width = 40;
                        saleorderreportworkSheet.Column(4).AutoFit();
                        saleorderreportworkSheet.Column(5).AutoFit();
                        saleorderreportworkSheet.Column(6).AutoFit();
                        saleorderreportworkSheet.Column(7).Width = 40;
                        saleorderreportworkSheet.Column(8).AutoFit();
                        saleorderreportworkSheet.Column(9).AutoFit();
                        saleorderreportworkSheet.Column(10).AutoFit();
                        saleorderreportworkSheet.Column(11).AutoFit();
                        saleorderreportworkSheet.Column(12).AutoFit();


                        saleorderreportworkSheet.Column(2).AutoFit();


                        break;


                    case "PendingSaleOrderReport":
                        fileName = "PendingSaleOrderReport" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        PendingSaleOrderReportViewModel pendingSaleOrderReportVM = new PendingSaleOrderReportViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        pendingSaleOrderReportVM = JsonConvert.DeserializeObject<PendingSaleOrderReportViewModel>(ReadableFormat);
                        List<PendingSaleOrderReportViewModel> pendingSaleOrderReportList = Mapper.Map<List<PendingSaleOrderReport>, List<PendingSaleOrderReportViewModel>>(_reportBusiness.GetPendingSaleOrderReport(Mapper.Map<PendingSaleOrderReportViewModel, PendingSaleOrderReport>(pendingSaleOrderReportVM)));
                        var pendingSaleorderreportworkSheet = excel.Workbook.Worksheets.Add("PendingSaleOrderReport");
                        PendingSaleOrderReportViewModel[] pendingSaleOrderReportVMListArray = pendingSaleOrderReportList.ToArray();
                        pendingSaleorderreportworkSheet.Cells[1, 1].LoadFromCollection(pendingSaleOrderReportVMListArray.Select(x => new {
                            SaleOrderNo = x.SaleOrdNo,
                            SaleOrderDate = x.SaleOrderDateFormatted,
                            CompanyName = x.Customer.CompanyName,
                            ContactPerson = x.Customer.ContactPerson,
                            ProductName = x.Product.Name,
                            ProductModel = x.ProductModel.Name,
                            ProductSpecification = x.ProductSpec,
                            Quantity = x.Qty,
                            PendingQty=x.PendingQty,
                            Unit = x.Unit.Description,
                            Amount = x.Amount,
                            Branch = x.Branch.Description,
                            DocumentOwner = x.PSAUser.LoginName

                        }), true, TableStyles.Light1);
                        pendingSaleorderreportworkSheet.Column(1).AutoFit();
                        pendingSaleorderreportworkSheet.Column(2).AutoFit();
                        pendingSaleorderreportworkSheet.Column(3).Width = 40;
                        pendingSaleorderreportworkSheet.Column(4).AutoFit();
                        pendingSaleorderreportworkSheet.Column(5).AutoFit();
                        pendingSaleorderreportworkSheet.Column(6).AutoFit();
                        pendingSaleorderreportworkSheet.Column(7).Width = 40;
                        pendingSaleorderreportworkSheet.Column(8).AutoFit();
                        pendingSaleorderreportworkSheet.Column(9).AutoFit();
                        pendingSaleorderreportworkSheet.Column(10).AutoFit();
                        pendingSaleorderreportworkSheet.Column(11).AutoFit();
                        pendingSaleorderreportworkSheet.Column(12).AutoFit();
                        pendingSaleorderreportworkSheet.Column(13).AutoFit();


                        break;
                    default:break;
                }
                using (var memoryStream = new MemoryStream())
                        {
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;  filename=" + fileName + ".xlsx");
                            excel.SaveAs(memoryStream);
                            memoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                            memoryStream.Close();
                            memoryStream.Dispose();
                        }

                }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Report", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType, Guid? id)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                 
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export to excel";
                    toolboxVM.ExportBtn.Event = "ExportReportData()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetReportList();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "GoToList();";

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion ButtonStyling

    }
}