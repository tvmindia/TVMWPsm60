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
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
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
        IProformaInvoiceBusiness _proformaInvoiceBusiness;
        IProductModelBusiness _productModelBusiness;
        IProductBusiness _productBusiness;
        ICustomerBusiness _customerBusiness;
        public ExcelExportController(IEnquiryBusiness enquiryBusiness, 
            IEstimateBusiness estimateBusiness, IQuotationBusiness quotationBusiness, 
            ISaleOrderBusiness saleOrderBusiness,IProductionOrderBusiness productionOrderBusiness,
            IProductionQCBusiness productionQCBusiness,ISaleInvoiceBusiness saleInvoiceBusiness,
            IServiceCallBusiness serviceCallBusiness,IDeliveryChallanBusiness deliveryChallanBusiness,
            IProformaInvoiceBusiness proformaInvoiceBusiness, IProductModelBusiness productModelBusiness,
            IProductBusiness productBusiness, ICustomerBusiness customerBusiness)
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
            _proformaInvoiceBusiness = proformaInvoiceBusiness;
            _productModelBusiness = productModelBusiness;
            _productBusiness = productBusiness;
            _customerBusiness = customerBusiness;
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

                        int finalRowsENQ = enquiryworkSheet.Dimension.End.Row;
                        //Convert into a string for the range.
                        string columnStringENQ = "A1:A,B1:B,C1:C,D1:D,E1:E,F1:F,G1:G,H1:H,I1.I" + finalRowsENQ.ToString();
                        //Convert the range to align top
                        enquiryworkSheet.Cells[columnStringENQ].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        enquiryworkSheet.Column(1).AutoFit();
                        enquiryworkSheet.Column(2).AutoFit();
                        enquiryworkSheet.Column(3).AutoFit();    
                        enquiryworkSheet.Column(4).Width = 60;
                        enquiryworkSheet.Column(4).Style.WrapText = true;
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

                        int finalRowsEST = estimateworkSheet.Dimension.End.Row;
                        string columnStringEST = "A1:A,B1:B,C1:C,D1:D,E1:E,F1:F,G1:G,H1:H,I1.I,J1.J" + finalRowsEST.ToString(); 
                        estimateworkSheet.Cells[columnStringEST].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        estimateworkSheet.Column(1).AutoFit();
                        estimateworkSheet.Column(2).AutoFit();
                        estimateworkSheet.Column(3).AutoFit();
                        estimateworkSheet.Column(4).Width = 40;
                        estimateworkSheet.Column(5).AutoFit();
                        estimateworkSheet.Column(6).AutoFit();
                        estimateworkSheet.Column(7).AutoFit();
                        estimateworkSheet.Column(8).AutoFit();
                        estimateworkSheet.Column(9).AutoFit();
                        estimateworkSheet.Column(10).AutoFit();
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

                        int finalRowsQUO = quotationworkSheet.Dimension.End.Row;
                        string columnStringQUO = "A1:A,B1:B,C1:C,D1:D,E1:E,F1:F,G1:G,H1:H,I1.I,J1.J,K1.K" + finalRowsQUO.ToString();
                        quotationworkSheet.Cells[columnStringQUO].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        quotationworkSheet.Column(1).AutoFit();
                        quotationworkSheet.Column(2).AutoFit();
                        quotationworkSheet.Column(3).AutoFit();
                        quotationworkSheet.Column(4).Width = 40;
                        quotationworkSheet.Column(5).AutoFit();
                        quotationworkSheet.Column(6).AutoFit();
                        quotationworkSheet.Column(7).AutoFit();
                        quotationworkSheet.Column(8).AutoFit();
                        quotationworkSheet.Column(9).AutoFit();
                        quotationworkSheet.Column(10).AutoFit();
                        quotationworkSheet.Column(11).AutoFit();
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

                        int finalRowsSOD = saleOrderworkSheet.Dimension.End.Row;
                        string columnStringSOD = "A1:A,B1:B,C1:C,D1:D,E1:E,F1:F,G1:G,H1:H,I1.I,J1.J,K1.K,L1.L,M1.M" + finalRowsSOD.ToString();
                        saleOrderworkSheet.Cells[columnStringSOD].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        saleOrderworkSheet.Column(1).AutoFit();
                        saleOrderworkSheet.Column(2).AutoFit();
                        saleOrderworkSheet.Column(3).AutoFit();
                        saleOrderworkSheet.Column(4).Width = 40;
                        saleOrderworkSheet.Column(5).AutoFit();
                        saleOrderworkSheet.Column(6).AutoFit();
                        saleOrderworkSheet.Column(7).AutoFit();
                        saleOrderworkSheet.Column(8).AutoFit();
                        saleOrderworkSheet.Column(9).AutoFit();
                        saleOrderworkSheet.Column(10).AutoFit();
                        saleOrderworkSheet.Column(11).AutoFit();
                        saleOrderworkSheet.Column(12).AutoFit();
                        saleOrderworkSheet.Column(13).AutoFit();
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

                        int finalRowsPOD = productionOrderworkSheet.Dimension.End.Row;
                        string columnStringPOD = "A1:A,B1:B,C1:C,D1:D,E1:E,F1:F,G1:G,H1:H,I1.I,J1.J" + finalRowsPOD.ToString();
                        productionOrderworkSheet.Cells[columnStringPOD].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        productionOrderworkSheet.Column(1).AutoFit();
                        productionOrderworkSheet.Column(2).AutoFit();
                        productionOrderworkSheet.Column(3).AutoFit();
                        productionOrderworkSheet.Column(4).Width = 40;
                        productionOrderworkSheet.Column(5).AutoFit();
                        productionOrderworkSheet.Column(6).AutoFit();
                        productionOrderworkSheet.Column(7).AutoFit();
                        productionOrderworkSheet.Column(8).AutoFit();
                        productionOrderworkSheet.Column(9).AutoFit();
                        productionOrderworkSheet.Column(10).AutoFit();
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

                        int finalRowsPQC = productionQCworkSheet.Dimension.End.Row;
                        string columnStringPQC = "A1:A,B1:B,C1:C,D1:D,E1:E,F1:F,G1:G,H1:H,I1.I,J1.J,K1.K,L1.L" + finalRowsPQC.ToString();
                        productionQCworkSheet.Cells[columnStringPQC].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

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
                        productionQCworkSheet.Column(11).AutoFit();
                        productionQCworkSheet.Column(12).AutoFit();

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
                        saleInvoiceworkSheet.Cells[1, 1].LoadFromCollection(saleInvoiceVMListArray.Select(x => new { SaleInvoiceNo = x.SaleInvNo, SaleInvoiceDate = x.SaleInvDateFormatted, ContactPerson = x.Customer.ContactPerson, CompanyName = x.Customer.CompanyName,SaleOrderNo = x.SaleOrder.SaleOrderNo,QuotationNo = x.Quotation.QuoteNo,ProfInvNo=x.ProformaInvoice.ProfInvNo ,Area = x.Area.Description,  DocumentOwner = x.PSAUser.LoginName, Branch = x.Branch.Description, DocumentStatus = x.DocumentStatus.Description, ApprovalStatus = x.ApprovalStatus.Description,EmailSent = x.EmailSentYN == true ? "YES" : "NO" }), true, TableStyles.Light1);

                        int finalRowsSIV = saleInvoiceworkSheet.Dimension.End.Row;
                        string columnStringSIV = "A1:A,B1:B,C1:C,D1:D,E1:E,F1:F,G1:G,H1:H,I1.I,J1.J,K1.K,L1.L,M1.M" + finalRowsSIV.ToString();
                        saleInvoiceworkSheet.Cells[columnStringSIV].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        saleInvoiceworkSheet.Column(1).AutoFit();
                        saleInvoiceworkSheet.Column(2).AutoFit();
                        saleInvoiceworkSheet.Column(3).AutoFit();
                        saleInvoiceworkSheet.Column(4).Width = 40;
                        saleInvoiceworkSheet.Column(5).AutoFit();
                        saleInvoiceworkSheet.Column(6).AutoFit();
                        saleInvoiceworkSheet.Column(7).AutoFit();
                        saleInvoiceworkSheet.Column(8).AutoFit();
                        saleInvoiceworkSheet.Column(9).AutoFit();
                        saleInvoiceworkSheet.Column(10).AutoFit();
                        saleInvoiceworkSheet.Column(11).AutoFit();
                        saleInvoiceworkSheet.Column(12).AutoFit();
                        saleInvoiceworkSheet.Column(13).AutoFit();
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
                        serviceCallworkSheet.Cells[1, 1].LoadFromCollection(serviceCallVMListArray.Select(x => new { ServiceCallNo = x.ServiceCallNo, ServiceCallDate = x.ServiceCallDateFormatted, ContactPerson = x.Customer.ContactPerson, CompanyName = x.Customer.CompanyName, Area = x.Area.Description,AttendedBy= x.Employee.Name,ServicedBy=x.ServicedByName,ServiceDate=x.ServiceDateFormatted, Branch = x.Branch.Description, DocumentStatus = x.DocumentStatus.Description,ServiceType=x.ServiceType.Name }), true, TableStyles.Light1);

                        int finalRowsSRC = serviceCallworkSheet.Dimension.End.Row;
                        string columnStringSRC = "A1:A,B1:B,C1:C,D1:D,E1:E,F1:F,G1:G,H1:H,I1.I,J1.J,k1.k" + finalRowsSRC.ToString();
                        serviceCallworkSheet.Cells[columnStringSRC].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        serviceCallworkSheet.Column(1).AutoFit();
                        serviceCallworkSheet.Column(2).AutoFit();
                        serviceCallworkSheet.Column(3).AutoFit();
                        serviceCallworkSheet.Column(4).Width = 40;
                        serviceCallworkSheet.Column(5).AutoFit();
                        serviceCallworkSheet.Column(6).AutoFit();
                        serviceCallworkSheet.Column(7).AutoFit();
                        serviceCallworkSheet.Column(8).AutoFit();
                        serviceCallworkSheet.Column(9).AutoFit();
                        serviceCallworkSheet.Column(10).AutoFit();
                        serviceCallworkSheet.Column(11).AutoFit();
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

                        int finalRowsDLC = deliveryChallanworkSheet.Dimension.End.Row;
                        string columnStringDLC = "A1:A,B1:B,C1:C,D1:D,E1:E,F1:F,G1:G,H1:H,I1.I,J1.J,K1.K,L1.L" + finalRowsDLC.ToString();
                        deliveryChallanworkSheet.Cells[columnStringDLC].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


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
                    case "PIV":
                        fileName = "ProformaInvoice" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        ProformaInvoiceAdvanceSearchViewModel proformaInvoiceAdvanceSearchVM = new ProformaInvoiceAdvanceSearchViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        proformaInvoiceAdvanceSearchVM = JsonConvert.DeserializeObject<ProformaInvoiceAdvanceSearchViewModel>(ReadableFormat);
                        List<ProformaInvoiceViewModel> proformaInvoiceVMList = Mapper.Map<List<ProformaInvoice>, List<ProformaInvoiceViewModel>>(_proformaInvoiceBusiness.GetAllProformaInvoice(Mapper.Map<ProformaInvoiceAdvanceSearchViewModel, ProformaInvoiceAdvanceSearch>(proformaInvoiceAdvanceSearchVM)));
                        var proformaInvoiceworkSheet = excel.Workbook.Worksheets.Add("ProformaInvoice");
                        ProformaInvoiceViewModel[] proformaInvoiceVMListArray = proformaInvoiceVMList.ToArray();
                        proformaInvoiceworkSheet.Cells[1, 1].LoadFromCollection(proformaInvoiceVMListArray.Select(x => new { ProfInvoiceNo = x.ProfInvNo, ProfInvoiceDate = x.ProfInvDateFormatted, ContactPerson = x.Customer.ContactPerson, CompanyName = x.Customer.CompanyName, SaleOrderNo = x.SaleOrder.SaleOrderNo, QuotationNo = x.Quotation.QuoteNo, Area = x.Area.Description, DocumentOwner = x.PSAUser.LoginName, Branch = x.Branch.Description, DocumentStatus = x.DocumentStatus.Description, ApprovalStatus = x.ApprovalStatus.Description, EmailSent = x.EmailSentYN == true ? "YES" : "NO" }), true, TableStyles.Light1);

                        int finalRowsPIV = proformaInvoiceworkSheet.Dimension.End.Row;
                        string columnStringPIV = "A1:A,B1:B,C1:C,D1:D,E1:E,F1:F,G1:G,H1:H,I1.I,J1.J,K1.K,L1.L" + finalRowsPIV.ToString();
                        proformaInvoiceworkSheet.Cells[columnStringPIV].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        proformaInvoiceworkSheet.Column(1).AutoFit();
                        proformaInvoiceworkSheet.Column(2).AutoFit();
                        proformaInvoiceworkSheet.Column(3).AutoFit();
                        proformaInvoiceworkSheet.Column(4).Width = 40;
                        proformaInvoiceworkSheet.Column(5).AutoFit();
                        proformaInvoiceworkSheet.Column(6).AutoFit();
                        proformaInvoiceworkSheet.Column(7).AutoFit();
                        proformaInvoiceworkSheet.Column(8).AutoFit();
                        proformaInvoiceworkSheet.Column(9).AutoFit();
                        proformaInvoiceworkSheet.Column(10).AutoFit();
                        proformaInvoiceworkSheet.Column(11).AutoFit();
                        proformaInvoiceworkSheet.Column(12).AutoFit();
                        break;
                    case "PRDM":
                        fileName = "Product Model" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        ProductModelAdvanceSearchViewModel productModelAdvanceSearchVM = new ProductModelAdvanceSearchViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        productModelAdvanceSearchVM = JsonConvert.DeserializeObject<ProductModelAdvanceSearchViewModel>(ReadableFormat);
                        List<ProductModelViewModel> productModelVMList = Mapper.Map<List<ProductModel>, List<ProductModelViewModel>>(_productModelBusiness.GetAllProductModel(Mapper.Map<ProductModelAdvanceSearchViewModel, ProductModelAdvanceSearch>(productModelAdvanceSearchVM)));
                        var productModelworkSheet = excel.Workbook.Worksheets.Add("Product Model");
                        ProductModelViewModel[] productModelVMListArray = productModelVMList.ToArray();
                        productModelworkSheet.Cells[1, 1].LoadFromCollection(productModelVMListArray.Select(x => new { Product = x.Product.Name, ProductModel = x.Name, Unit = x.Unit.Description, ProductSpec = x.Specification, CostPrice = x.CostPrice, SellingPrice = x.SellingPrice, IntroducedDate = x.IntroducedDateFormatted, StockQty = x.StockQty }), true, TableStyles.Light1);

                        int finalRowsPRDM = productModelworkSheet.Dimension.End.Row;
                        string columnStringPRDM = "A1:A,B1:B,C1:C,D1:D,E1:E,F1:F,G1:G,H1:H" + finalRowsPRDM.ToString();
                        productModelworkSheet.Cells[columnStringPRDM].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        productModelworkSheet.Column(1).AutoFit();
                        productModelworkSheet.Column(2).AutoFit();
                        productModelworkSheet.Column(3).AutoFit();
                        productModelworkSheet.Column(4).Width = 60;
                        productModelworkSheet.Column(4).Style.WrapText = true;
                        productModelworkSheet.Column(5).AutoFit();               
                        productModelworkSheet.Column(6).AutoFit();               
                        productModelworkSheet.Column(7).AutoFit();                      
                        productModelworkSheet.Column(8).AutoFit();
                     
                        break;
                    case "PRD":
                        fileName = "Product" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        ProductAdvanceSearchViewModel productAdvanceSearchVM = new ProductAdvanceSearchViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        productAdvanceSearchVM = JsonConvert.DeserializeObject<ProductAdvanceSearchViewModel>(ReadableFormat);
                        List<ProductViewModel> productVMList = Mapper.Map<List<Product>, List<ProductViewModel>>(_productBusiness.GetAllProduct(Mapper.Map<ProductAdvanceSearchViewModel, ProductAdvanceSearch>(productAdvanceSearchVM)));
                        var productworkSheet = excel.Workbook.Worksheets.Add("Product");
                        ProductViewModel[] productVMListArray = productVMList.ToArray();
                        productworkSheet.Cells[1, 1].LoadFromCollection(productVMListArray.Select(x => new { Code = x.Code, Name = x.Name, Category = x.ProductCategory.Description, IntroducedDate = x.IntroducedDateFormatted, Company = x.Company.Name, HSNCode = x.HSNCode }), true, TableStyles.Light1);

                        int finalRowsPRD = productworkSheet.Dimension.End.Row;

                        //Convert into a string for the range.
                        string columnStringPRD = "A1:A,B1:B,C1:C,D1:D,E1:E,F1:F" + finalRowsPRD.ToString();
                        //Convert the range to align top
                        productworkSheet.Cells[columnStringPRD].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        productworkSheet.Column(1).AutoFit();
                        productworkSheet.Column(2).AutoFit();
                        productworkSheet.Column(3).AutoFit();
                        productworkSheet.Column(4).AutoFit();
                        productworkSheet.Column(5).AutoFit();
                        productworkSheet.Column(6).AutoFit();                   
                        break;
                    case "CUST":
                        fileName = "Customer" + pSASysCommon.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        CustomerAdvanceSearchViewModel customerAdvanceSearchVM = new CustomerAdvanceSearchViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        customerAdvanceSearchVM = JsonConvert.DeserializeObject<CustomerAdvanceSearchViewModel>(ReadableFormat);
                        List<CustomerViewModel> CustomerVMList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetAllCustomer(Mapper.Map<CustomerAdvanceSearchViewModel, CustomerAdvanceSearch>(customerAdvanceSearchVM)));
                        var customerworkSheet = excel.Workbook.Worksheets.Add("Customer");
                        CustomerViewModel[] customerVMListArray = CustomerVMList.ToArray();
                        customerworkSheet.Cells[1, 1].LoadFromCollection(customerVMListArray.Select(x => new { Company = x.CompanyName, ContactPerson = x.ContactPerson, Mobile = x.Mobile, TaxRegNo = x.TaxRegNo, PANNO = x.PANNO, AadharNo = x.AadharNo, OutStandingAmount = x.OutStanding }), true, TableStyles.Light1);

                        int finalRowsCUST = customerworkSheet.Dimension.End.Row;
                        string columnStringCUST = "A1:A,B1:B,C1:C,D1:D,E1:E,F1:F,G1:G" + finalRowsCUST.ToString();
                        customerworkSheet.Cells[columnStringCUST].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        customerworkSheet.Column(1).AutoFit();
                        customerworkSheet.Column(2).AutoFit();
                        customerworkSheet.Column(3).AutoFit();
                        customerworkSheet.Column(4).AutoFit();
                        customerworkSheet.Column(5).AutoFit();
                        customerworkSheet.Column(6).AutoFit();
                        customerworkSheet.Column(7).AutoFit();
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
                    memoryStream.Close();
                    memoryStream.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}