using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Service
{
    public class QuotationBusiness:IQuotationBusiness
    {
        IQuotationRepository _quotationRepository;
        ICommonBusiness _commonBusiness;
        ITaxTypeBusiness _taxTypeBusiness;
        IMailBusiness _mailBusiness;
        IPDFGeneratorBusiness _pDFGeneratorBusiness;
        public QuotationBusiness(IQuotationRepository quotationRepository, ICommonBusiness commonBusiness, ITaxTypeBusiness taxTypeBusiness, IMailBusiness mailBusiness, IPDFGeneratorBusiness pDFGeneratorBusiness)
        {
            _quotationRepository = quotationRepository;
            _commonBusiness = commonBusiness;
            _taxTypeBusiness = taxTypeBusiness;
            _mailBusiness = mailBusiness;
            _pDFGeneratorBusiness = pDFGeneratorBusiness;
        }
        public List<Quotation> GetAllQuotation(QuotationAdvanceSearch quotationAdvanceSearch)
        {
            return _quotationRepository.GetAllQuotation(quotationAdvanceSearch);
        }
        public List<QuotationDetail> GetQuotationDetailListByQuotationID(Guid quoteID)
        {
            return _quotationRepository.GetQuotationDetailListByQuotationID(quoteID);
        }
        public List<QuotationOtherCharge> GetQuotationOtherChargesDetailListByQuotationID(Guid quotationID)
        {
            return _quotationRepository.GetQuotationOtherChargesDetailListByQuotationID(quotationID);
        }
        public object InsertUpdateQuotation(Quotation quotation)
        {
            if (quotation.QuotationDetailList.Count > 0)
            {
                quotation.DetailXML = _commonBusiness.GetXMLfromQuotationObject(quotation.QuotationDetailList, "ProductID, Qty, Rate, UnitCode");
            }
            if(quotation.QuotationOtherChargeList.Count>0)
            {
                quotation.OtherChargeDetailXML= _commonBusiness.GetXMLfromQuotationOtherChargeObject(quotation.QuotationOtherChargeList, "OtherChargeCode , ChargeAmount");
            }
            return _quotationRepository.InsertUpdateQuotation(quotation);
        }
        public Quotation GetQuotation(Guid id)
        {
            return _quotationRepository.GetQuotation(id);
        }
        public object DeleteQuotation(Guid id)
        {
            return _quotationRepository.DeleteQuotation(id);
        }
        public object DeleteQuotationDetail(Guid id)
        {
            return _quotationRepository.DeleteQuotationDetail(id);
        }
        public QuotationDetail CalculateGST(QuotationDetail quotationDetail)
        {
            TaxType taxType = _taxTypeBusiness.GetTaxType((int)quotationDetail.TaxTypeCode);
            quotationDetail.CGSTPerc = ((quotationDetail.Rate*quotationDetail.Qty-quotationDetail.Discount)*(taxType.CGSTPercentage))/100;
            quotationDetail.SGSTPerc = ((quotationDetail.Rate * quotationDetail.Qty - quotationDetail.Discount) * (taxType.SGSTPercentage)) / 100;
            quotationDetail.IGSTPerc = ((quotationDetail.Rate * quotationDetail.Qty - quotationDetail.Discount) * (taxType.IGSTPercentage)) / 100;
            return quotationDetail;
        }
        public object UpdateQuotationEmailInfo(Quotation quotation)
        {
            return _quotationRepository.UpdateQuotationEmailInfo(quotation);
        }
        public async Task<bool> QuoteEmailPush(Quotation quotation)
        {

            bool sendsuccess = false;

            try
            {
                if (!string.IsNullOrEmpty(quotation.EmailSentTo))
                {
                        string[] EmailList = quotation.EmailSentTo.Split(',');
                        string mailBody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/MailTemplate/DocumentEmailBody.html"));
                        MailMessage _mail = new MailMessage();
                        PDFTools pDFTools = new PDFTools();
                        _mail.Body = mailBody.Replace("$Customer$",quotation.Customer.ContactPerson).Replace("$Document$","Quotation").Replace("$DocumentNo$",quotation.QuoteNo);
                        pDFTools.Content = quotation.MailContant;
                        _mail.Attachments.Add(new Attachment(new MemoryStream(_pDFGeneratorBusiness.GetPdfAttachment(pDFTools)), quotation.QuoteNo+".pdf"));
                        
                        _mail.Subject = "Quotation";
                        _mail.IsBodyHtml = true;
                        foreach(string email in EmailList)
                        {
                            _mail.To.Add(email);
                        }                        
                        sendsuccess = await _mailBusiness.MailMessageSendAsync(_mail);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sendsuccess;
        }

        public List<SelectListItem> GetQuotationForSelectList(Guid? quoteID)
        {
            List<SelectListItem> selectListItem = null;
            List<Quotation> quotationList = _quotationRepository.GetQuotationForSelectList(quoteID);
            return selectListItem = quotationList!=null?(from quotation in quotationList
                                     select new SelectListItem
                                     {
                                         Text = quotation.QuoteNo,
                                         Value = quotation.ID.ToString(),
                                         Selected = false
                                     }).ToList():new List<SelectListItem>();
        }
        public List<Quotation> GetQuotationForSelectListOnDemand(string searchTerm)
        {
            return _quotationRepository.GetQuotationForSelectListOnDemand(searchTerm);
        }
        public object DeleteQuotationOtherChargeDetail(Guid id)
        {
            return _quotationRepository.DeleteQuotationOtherChargeDetail(id);
        }
        public QuotationSummary GetQuotationSummaryCount()
        {
            return _quotationRepository.GetQuotationSummaryCount();
        }
    }
}
