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

        public ExcelExportController(IEnquiryBusiness enquiryBusiness, IEstimateBusiness estimateBusiness, IQuotationBusiness quotationBusiness, ISaleOrderBusiness saleOrderBusiness)
        {
            _enquiryBusiness = enquiryBusiness;
            _estimateBusiness = estimateBusiness;
            _quotationBusiness = quotationBusiness;
            _saleOrderBusiness = saleOrderBusiness;
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
                        break;
                    case "PQC":
                        break;
                    case "SIV":
                        break;
                    case "SRC":
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