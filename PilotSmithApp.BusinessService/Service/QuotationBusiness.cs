﻿using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Service
{
    public class QuotationBusiness:IQuotationBusiness
    {
        IQuotationRepository _quotationRepository;
        ICommonBusiness _commonBusiness;
        ITaxTypeBusiness _taxTypeBusiness;
        IMailBusiness _mailBusiness;
        public QuotationBusiness(IQuotationRepository quotationRepository, ICommonBusiness commonBusiness, ITaxTypeBusiness taxTypeBusiness, IMailBusiness mailBusiness)
        {
            _quotationRepository = quotationRepository;
            _commonBusiness = commonBusiness;
            _taxTypeBusiness = taxTypeBusiness;
            _mailBusiness = mailBusiness;
        }
        public List<Quotation> GetAllQuotation(QuotationAdvanceSearch quotationAdvanceSearch)
        {
            return _quotationRepository.GetAllQuotation(quotationAdvanceSearch);
        }
        public List<QuotationDetail> GetQuotationDetailListByQuotationID(Guid quoteID)
        {
            return _quotationRepository.GetQuotationDetailListByQuotationID(quoteID);
        }
        public object InsertUpdateQuotation(Quotation quotation)
        {
            if (quotation.QuotationDetailList.Count > 0)
            {
                quotation.DetailXML = _commonBusiness.GetXMLfromQuotationObject(quotation.QuotationDetailList, "ProductID");
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
            quotationDetail.CGSTAmt = ((quotationDetail.Rate*quotationDetail.Qty-quotationDetail.Discount)*(taxType.CGSTPercentage))/100;
            quotationDetail.SGSTAmt = ((quotationDetail.Rate * quotationDetail.Qty - quotationDetail.Discount) * (taxType.SGSTPercentage)) / 100;
            quotationDetail.IGSTAmt = ((quotationDetail.Rate * quotationDetail.Qty - quotationDetail.Discount) * (taxType.IGSTPercentage)) / 100;
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
                    foreach (string email in EmailList)
                    {
                        Mail _mail = new Mail();
                        _mail.Body = quotation.MailContant;
                        _mail.Subject = "Quotation";
                        _mail.To = email;
                        sendsuccess = await _mailBusiness.MailSendAsync(_mail);
                    }
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
    }
}
