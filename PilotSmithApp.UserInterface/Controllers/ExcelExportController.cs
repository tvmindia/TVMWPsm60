using AutoMapper;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class ExcelExportController : Controller
    {
        IEnquiryBusiness _enquiryBusiness;
        IEstimateBusiness _estimateBusiness;
        IQuotationBusiness _quotationBusiness;
        ISaleOrderBusiness _saleOrderBusiness;
        IProductionOrderBusiness _productionOrderBusiness;
        IProductionQCBusiness _productionQCBusiness;
        ISaleInvoiceBusiness _saleInvoiceBusiness;
        IServiceCallBusiness _serviceCallBusiness;
        IDeliveryChallanBusiness _deliveryChallanBusiness;
        public ExcelExportController(IEnquiryBusiness enquiryBusiness, IEstimateBusiness estimateBusiness, IQuotationBusiness quotationBusiness, ISaleOrderBusiness saleOrderBusiness,IProductionOrderBusiness productionOrderBusiness,IProductionQCBusiness productionQCBusiness,ISaleInvoiceBusiness saleInvoiceBusiness,IServiceCallBusiness serviceCallBusiness,IDeliveryChallanBusiness deliveryChallanBusiness)
        {
            _enquiryBusiness = enquiryBusiness;
            _estimateBusiness = estimateBusiness;
            _quotationBusiness = quotationBusiness;
            _saleOrderBusiness = saleOrderBusiness;
            _productionOrderBusiness = productionOrderBusiness;
            _productionQCBusiness = productionQCBusiness;
            _saleInvoiceBusiness = saleInvoiceBusiness;
            _serviceCallBusiness = serviceCallBusiness;
            _deliveryChallanBusiness = deliveryChallanBusiness;
        }
        // GET: ExcelExport
        public ActionResult Index()
        {
            return View();
        }


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
                        fileName = "Enquiry" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        EnquiryAdvanceSearchViewModel enquiryAdvanceSearchVM = new EnquiryAdvanceSearchViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        enquiryAdvanceSearchVM = JsonConvert.DeserializeObject<EnquiryAdvanceSearchViewModel>(ReadableFormat);
                        List<EnquiryViewModel> enquiryVMList = Mapper.Map<List<Enquiry>, List<EnquiryViewModel>>(_enquiryBusiness.GetAllEnquiry(Mapper.Map<EnquiryAdvanceSearchViewModel, EnquiryAdvanceSearch>(enquiryAdvanceSearchVM)));
                        var enquiryworkSheet = excel.Workbook.Worksheets.Add("Enquiry");
                        EnquiryViewModel[] enquiryVMListArray = enquiryVMList.ToArray();
                        enquiryworkSheet.Cells[1, 1].LoadFromCollection(enquiryVMListArray.Select(x => new { EnquiryNo = x.EnquiryNo, ContactPerson = x.Customer.ContactPerson, CompanyName = x.Customer.CompanyName, RequirementSpecification = x.RequirementSpec, Area = x.Area.Description, ReferencePerson = x.ReferencePerson.Name, DocumentOwner = x.PSAUser.LoginName, DocumentStatus = x.DocumentStatus.Description, Branch = x.Branch.Description }), true,TableStyles.Light1);
                        enquiryworkSheet.Column(1).AutoFit();
                        enquiryworkSheet.Column(2).AutoFit();
                        enquiryworkSheet.Column(3).AutoFit();
                        enquiryworkSheet.Column(4).Width = 40;
                        enquiryworkSheet.Column(5).AutoFit();
                        enquiryworkSheet.Column(6).AutoFit();
                        enquiryworkSheet.Column(7).AutoFit();
                        enquiryworkSheet.Column(8).AutoFit();
                        enquiryworkSheet.Column(9).AutoFit();
                        break;
                    case "EST":
                        fileName = "Estimate" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        EstimateAdvanceSearchViewModel estimateAdvanceSearchVM = new EstimateAdvanceSearchViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        estimateAdvanceSearchVM = JsonConvert.DeserializeObject<EstimateAdvanceSearchViewModel>(ReadableFormat);
                        List<EstimateViewModel> estimateVMList = Mapper.Map<List<Estimate>, List<EstimateViewModel>>(_estimateBusiness.GetAllEstimate(Mapper.Map<EstimateAdvanceSearchViewModel, EstimateAdvanceSearch>(estimateAdvanceSearchVM)));
                        var estimateworkSheet = excel.Workbook.Worksheets.Add("Estimate");
                        EstimateViewModel[] estimateVMListArray = estimateVMList.ToArray();
                        estimateworkSheet.Cells[1, 1].LoadFromCollection(estimateVMListArray.Select(x => new { EstimateNo = x.EstimateNo, EstimateDate = x.EstimateDateFormatted, EnquiryNo = x.Enquiry.EnquiryNo, ContactPerson = x.Customer.ContactPerson, CompanyName = x.Customer.CompanyName, Area = x.Area.Description, ReferredBy = x.ReferencePerson.Name, DocumentOwner = x.UserName, DocumentStatus = x.DocumentStatus.Description, Branch = x.Branch.Description }), true, TableStyles.Light1);
                        estimateworkSheet.Column(1).AutoFit();
                        estimateworkSheet.Column(2).AutoFit();
                        estimateworkSheet.Column(3).AutoFit();
                        estimateworkSheet.Column(4).Width = 40;
                        estimateworkSheet.Column(5).AutoFit();
                        estimateworkSheet.Column(6).AutoFit();
                        estimateworkSheet.Column(7).AutoFit();
                        estimateworkSheet.Column(8).AutoFit();
                        estimateworkSheet.Column(9).AutoFit();
                        break;
                    case "QUO":
                        fileName = "Quotation" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        QuotationAdvanceSearchViewModel quotationAdvanceSearchVM = new QuotationAdvanceSearchViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        quotationAdvanceSearchVM = JsonConvert.DeserializeObject<QuotationAdvanceSearchViewModel>(ReadableFormat);
                        List<QuotationViewModel> QuotationVMList = Mapper.Map<List<Quotation>, List<QuotationViewModel>>(_quotationBusiness.GetAllQuotation(Mapper.Map<QuotationAdvanceSearchViewModel, QuotationAdvanceSearch>(quotationAdvanceSearchVM)));
                        var quotationworkSheet = excel.Workbook.Worksheets.Add("Quotation");
                        QuotationViewModel[] quotationVMListArray = QuotationVMList.ToArray();
                        quotationworkSheet.Cells[1, 1].LoadFromCollection(quotationVMListArray.Select(x => new { QuotationNo = x.QuoteNo, QuotationDate = x.QuoteDateFormatted, ContactPerson = x.Customer.ContactPerson, CompanyName = x.Customer.CompanyName, Area = x.Area.Description, ReferredBy = x.ReferencePerson.Name, DocumentOwner = x.PSAUser.LoginName, DocumentStatus = x.DocumentStatus.Description, Branch = x.Branch.Description, ApprovalStatus = x.ApprovalStatus.Description, EmailSent = x.EmailSentYN == true ? "Yes" : "No" }), true, TableStyles.Light1);
                        quotationworkSheet.Column(1).AutoFit();
                        quotationworkSheet.Column(2).AutoFit();
                        quotationworkSheet.Column(3).AutoFit();
                        quotationworkSheet.Column(4).Width = 40;
                        quotationworkSheet.Column(5).AutoFit();
                        quotationworkSheet.Column(6).AutoFit();
                        quotationworkSheet.Column(7).AutoFit();
                        quotationworkSheet.Column(8).AutoFit();
                        quotationworkSheet.Column(9).AutoFit();
                        break;
                    case "SOD":
                        fileName = "Sale Order" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        SaleOrderAdvanceSearchViewModel saleOrderAdvanceSearchVM = new SaleOrderAdvanceSearchViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        saleOrderAdvanceSearchVM = JsonConvert.DeserializeObject<SaleOrderAdvanceSearchViewModel>(ReadableFormat);
                        List<SaleOrderViewModel> saleOrderVMList = Mapper.Map<List<SaleOrder>, List<SaleOrderViewModel>>(_saleOrderBusiness.GetAllSaleOrder(Mapper.Map<SaleOrderAdvanceSearchViewModel, SaleOrderAdvanceSearch>(saleOrderAdvanceSearchVM)));
                        var saleOrderworkSheet = excel.Workbook.Worksheets.Add("Sale Order");
                        SaleOrderViewModel[] saleOrderVMListArray = saleOrderVMList.ToArray();
                        saleOrderworkSheet.Cells[1, 1].LoadFromCollection(saleOrderVMListArray.Select(x => new { SaleOrderNo = x.SaleOrderNo, SaleOrderDate = x.SaleOrderDateFormatted, ContactPerson = x.Customer.ContactPerson, CompanyName = x.Customer.CompanyName,  ReferredBy = x.ReferencePerson.Name, QuotationNo=x.Quotation.QuoteNo, EnquiryNo=x.Enquiry.EnquiryNo, Area = x.Area.Description, DocumentOwner = x.PSAUser.LoginName, DocumentStatus = x.DocumentStatus.Description, Branch = x.Branch.Description, ApprovalStatus = x.ApprovalStatus.Description,EmailSent=x.EmailSentYN==true?"Yes":"No" }), true, TableStyles.Light1);
                        saleOrderworkSheet.Column(1).AutoFit();
                        saleOrderworkSheet.Column(2).AutoFit();
                        saleOrderworkSheet.Column(3).AutoFit();
                        saleOrderworkSheet.Column(4).Width = 40;
                        saleOrderworkSheet.Column(5).AutoFit();
                        saleOrderworkSheet.Column(6).AutoFit();
                        saleOrderworkSheet.Column(7).AutoFit();
                        saleOrderworkSheet.Column(8).AutoFit();
                        saleOrderworkSheet.Column(9).AutoFit();
                        break;
                    case "POD":
                        fileName = "ProductionOrder" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        ProductionOrderAdvanceSearchViewModel productionOrderAdvanceSearchVM = new ProductionOrderAdvanceSearchViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        productionOrderAdvanceSearchVM = JsonConvert.DeserializeObject<ProductionOrderAdvanceSearchViewModel>(ReadableFormat);
                        List<ProductionOrderViewModel> productionOrderVMList = Mapper.Map<List<ProductionOrder>, List<ProductionOrderViewModel>>(_productionOrderBusiness.GetAllProductionOrder(Mapper.Map<ProductionOrderAdvanceSearchViewModel, ProductionOrderAdvanceSearch>(productionOrderAdvanceSearchVM)));
                        var productionOrderworkSheet = excel.Workbook.Worksheets.Add("ProductionOrder");
                        ProductionOrderViewModel[] productionOrderVMListArray = productionOrderVMList.ToArray();
                        productionOrderworkSheet.Cells[1, 1].LoadFromCollection(productionOrderVMListArray.Select(x => new { ProductionOrderNo = x.ProdOrderNo,ProductionOrderDate = x.ProdOrderDateFormatted, ContactPerson = x.Customer.ContactPerson, CompanyName = x.Customer.CompanyName, Area = x.Area.Description, DocumentOwner = x.PSAUser.LoginName, Branch = x.Branch.Description, DocumentStatus = x.DocumentStatus.Description,  ApprovalStatus = x.ApprovalStatus.Description , EmailSent = x.EmailSentYN==true?"YES":"NO"}), true, TableStyles.Light1);
                        productionOrderworkSheet.Column(1).AutoFit();
                        productionOrderworkSheet.Column(2).AutoFit();
                        productionOrderworkSheet.Column(3).AutoFit();
                        productionOrderworkSheet.Column(4).Width = 40;
                        productionOrderworkSheet.Column(5).AutoFit();
                        productionOrderworkSheet.Column(6).AutoFit();
                        productionOrderworkSheet.Column(7).AutoFit();
                        productionOrderworkSheet.Column(8).AutoFit();
                        productionOrderworkSheet.Column(9).AutoFit();                       
                        break;
                    case "PQC":
                        fileName = "ProductionQC" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        ProductionQCAdvanceSearchViewModel productionQCAdvanceSearchVM = new ProductionQCAdvanceSearchViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        productionQCAdvanceSearchVM = JsonConvert.DeserializeObject<ProductionQCAdvanceSearchViewModel>(ReadableFormat);
                        List<ProductionQCViewModel> productionQCVMList = Mapper.Map<List<ProductionQC>, List<ProductionQCViewModel>>(_productionQCBusiness.GetAllProductionQC(Mapper.Map<ProductionQCAdvanceSearchViewModel, ProductionQCAdvanceSearch>(productionQCAdvanceSearchVM)));
                        var productionQCworkSheet = excel.Workbook.Worksheets.Add("ProductionQC");
                        ProductionQCViewModel[] productionQCVMListArray = productionQCVMList.ToArray();
                        productionQCworkSheet.Cells[1, 1].LoadFromCollection(productionQCVMListArray.Select(x => new { ProductionQCNo = x.ProdQCNo, ProductionQCDate = x.ProdQCDateFormatted,ProductionOrderNo = x.ProdOrderNo, ContactPerson = x.Customer.ContactPerson, CompanyName = x.Customer.CompanyName, Area = x.Area.Description,Plant = x.Plant.Description ,DocumentOwner = x.PSAUser.LoginName, Branch = x.Branch.Description, DocumentStatus = x.DocumentStatus.Description, ApprovalStatus = x.ApprovalStatus.Description ,EmailSent = x.EmailSentYN == true ? "YES" : "NO" }), true, TableStyles.Light1);
                        productionQCworkSheet.Column(1).AutoFit();
                        productionQCworkSheet.Column(2).AutoFit();
                        productionQCworkSheet.Column(3).AutoFit();
                        productionQCworkSheet.Column(4).AutoFit();
                        productionQCworkSheet.Column(5).Width = 40;
                        productionQCworkSheet.Column(6).AutoFit();
                        productionQCworkSheet.Column(7).AutoFit();
                        productionQCworkSheet.Column(8).AutoFit();
                        productionQCworkSheet.Column(9).AutoFit();
                        productionQCworkSheet.Column(10).AutoFit();

                        break;
                    case "SIV":
                        fileName = "SaleInvoice" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        SaleInvoiceAdvanceSearchViewModel saleInvoiceAdvanceSearchVM = new SaleInvoiceAdvanceSearchViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        saleInvoiceAdvanceSearchVM = JsonConvert.DeserializeObject<SaleInvoiceAdvanceSearchViewModel>(ReadableFormat);
                        List<SaleInvoiceViewModel> saleInvoiceVMList = Mapper.Map<List<SaleInvoice>, List<SaleInvoiceViewModel>>(_saleInvoiceBusiness.GetAllSaleInvoice(Mapper.Map<SaleInvoiceAdvanceSearchViewModel, SaleInvoiceAdvanceSearch>(saleInvoiceAdvanceSearchVM)));
                        var saleInvoiceworkSheet = excel.Workbook.Worksheets.Add("SaleInvoice");
                        SaleInvoiceViewModel[] saleInvoiceVMListArray = saleInvoiceVMList.ToArray();
                        saleInvoiceworkSheet.Cells[1, 1].LoadFromCollection(saleInvoiceVMListArray.Select(x => new { SaleInvoiceNo = x.SaleInvNo, SaleInvoiceDate = x.SaleInvDateFormatted, ContactPerson = x.Customer.ContactPerson, CompanyName = x.Customer.CompanyName,SaleOrderNo = x.SaleOrder.SaleOrderNo,QuotationNo = x.Quotation.QuoteNo, Area = x.Area.Description,  DocumentOwner = x.PSAUser.LoginName, Branch = x.Branch.Description, DocumentStatus = x.DocumentStatus.Description, ApprovalStatus = x.ApprovalStatus.Description,EmailSent = x.EmailSentYN == true ? "YES" : "NO" }), true, TableStyles.Light1);
                        saleInvoiceworkSheet.Column(1).AutoFit();
                        saleInvoiceworkSheet.Column(2).AutoFit();
                        saleInvoiceworkSheet.Column(3).AutoFit();
                        saleInvoiceworkSheet.Column(4).Width = 40;
                        saleInvoiceworkSheet.Column(5).AutoFit();
                        saleInvoiceworkSheet.Column(6).AutoFit();
                        saleInvoiceworkSheet.Column(7).AutoFit();
                        saleInvoiceworkSheet.Column(8).AutoFit();
                        saleInvoiceworkSheet.Column(9).AutoFit();
                        break;
                    case "SRC":
                        fileName = "ServiceCall" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        ServiceCallAdvanceSearchViewModel serviceCallAdvanceSearchVM = new ServiceCallAdvanceSearchViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        serviceCallAdvanceSearchVM = JsonConvert.DeserializeObject<ServiceCallAdvanceSearchViewModel>(ReadableFormat);
                        List<ServiceCallViewModel> serviceCallVMList = Mapper.Map<List<ServiceCall>, List<ServiceCallViewModel>>(_serviceCallBusiness.GetAllServiceCall(Mapper.Map<ServiceCallAdvanceSearchViewModel, ServiceCallAdvanceSearch>(serviceCallAdvanceSearchVM)));
                        var serviceCallworkSheet = excel.Workbook.Worksheets.Add("ServiceCall");
                        ServiceCallViewModel[] serviceCallVMListArray = serviceCallVMList.ToArray();
                        serviceCallworkSheet.Cells[1, 1].LoadFromCollection(serviceCallVMListArray.Select(x => new { ServiceCallNo = x.ServiceCallNo, ServiceCallDate = x.ServiceCallDateFormatted, ContactPerson = x.Customer.ContactPerson, CompanyName = x.Customer.CompanyName, Area = x.Area.Description,AttendedBy= x.Employee.Name,ServicedBy=x.ServicedByName,ServiceDate=x.ServiceDateFormatted, Branch = x.Branch.Description, DocumentStatus = x.DocumentStatus.Description }), true, TableStyles.Light1);
                        serviceCallworkSheet.Column(1).AutoFit();
                        serviceCallworkSheet.Column(2).AutoFit();
                        serviceCallworkSheet.Column(3).AutoFit();
                        serviceCallworkSheet.Column(4).Width = 40;
                        serviceCallworkSheet.Column(5).AutoFit();
                        serviceCallworkSheet.Column(6).AutoFit();
                        serviceCallworkSheet.Column(7).AutoFit();
                        serviceCallworkSheet.Column(8).AutoFit();
                        serviceCallworkSheet.Column(9).AutoFit();
                        break;
                    case "DLC":
                        fileName = "CancellationChallan" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        DeliveryChallanAdvanceSearchViewModel deliveryChallanAdvanceSearchVM = new DeliveryChallanAdvanceSearchViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        deliveryChallanAdvanceSearchVM = JsonConvert.DeserializeObject<DeliveryChallanAdvanceSearchViewModel>(ReadableFormat);
                        List<DeliveryChallanViewModel> deliveryChallanVMList = Mapper.Map<List<DeliveryChallan>, List<DeliveryChallanViewModel>>(_deliveryChallanBusiness.GetAllDeliveryChallan(Mapper.Map<DeliveryChallanAdvanceSearchViewModel, DeliveryChallanAdvanceSearch>(deliveryChallanAdvanceSearchVM)));
                        var deliveryChallanworkSheet = excel.Workbook.Worksheets.Add("CancellationChallan");
                        DeliveryChallanViewModel[] deliveryChallanVMListArray = deliveryChallanVMList.ToArray();
                        deliveryChallanworkSheet.Cells[1, 1].LoadFromCollection(deliveryChallanVMListArray.Select(x => new {CancellationChallanNo = x.DelvChallanNo, ChallanDate = x.DelvChallanDateFormatted, ContactPerson = x.Customer.ContactPerson, CompanyName = x.Customer.CompanyName,SaleOrderNo = x.SaleOrder.SaleOrderNo,ProductionOrderNo = x.ProductionOrder.ProdOrderNo, Area = x.Area.Description, Plant = x.Plant.Description, DocumentOwner = x.PSAUser.LoginName, Branch = x.Branch.Description, ApprovalStatus = x.ApprovalStatus.Description, EmailSent = x.EmailSentYN == true ? "YES" : "NO" //}
                        }), true, TableStyles.Light1);
                        deliveryChallanworkSheet.Column(1).AutoFit();
                        deliveryChallanworkSheet.Column(2).AutoFit();
                        deliveryChallanworkSheet.Column(3).AutoFit();
                        deliveryChallanworkSheet.Column(4).Width = 40;
                        deliveryChallanworkSheet.Column(5).AutoFit();
                        deliveryChallanworkSheet.Column(6).AutoFit();
                        deliveryChallanworkSheet.Column(7).AutoFit();
                        deliveryChallanworkSheet.Column(8).AutoFit();
                        deliveryChallanworkSheet.Column(9).AutoFit();
                        deliveryChallanworkSheet.Column(10).AutoFit();
                        deliveryChallanworkSheet.Column(11).AutoFit();
                        deliveryChallanworkSheet.Column(12).AutoFit();

                        break;
                }
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename="+fileName+".xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}