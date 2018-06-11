using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Service
{
    public class SaleInvoiceBusiness: ISaleInvoiceBusiness
    {
        ISaleInvoiceRepository _saleInvoiceRepository;
        ICommonBusiness _commonBusiness;
        IMailBusiness _mailBusiness;
        public SaleInvoiceBusiness(ISaleInvoiceRepository saleInvoiceRepository, ICommonBusiness commonBusiness, IMailBusiness mailBusiness)
        {
            _saleInvoiceRepository = saleInvoiceRepository;
            _commonBusiness = commonBusiness;
            _mailBusiness = mailBusiness;
        }
        public List<SaleInvoice> GetAllSaleInvoice(SaleInvoiceAdvanceSearch saleInvoiceAdvanceSearch)
        {
            return _saleInvoiceRepository.GetAllSaleInvoice(saleInvoiceAdvanceSearch);
        }
        public List<SaleInvoiceDetail> GetSaleInvoiceDetailListBySaleInvoiceID(Guid saleInvoiceID)
        {
            return _saleInvoiceRepository.GetSaleInvoiceDetailListBySaleInvoiceID(saleInvoiceID);
        }
        public object InsertUpdateSaleInvoice(SaleInvoice saleInvoice)
        {
            if (saleInvoice.SaleInvoiceDetailList.Count > 0)
            {
                saleInvoice.DetailXML = _commonBusiness.GetXMLfromSaleInvoiceObject(saleInvoice.SaleInvoiceDetailList, "ProductID");
            }
            if (saleInvoice.SaleInvoiceOtherChargeDetailList.Count > 0)
            {
                saleInvoice.OtherChargeDetailXML = _commonBusiness.GetXMLfromSaleInvoiceOtherChargeObject(saleInvoice.SaleInvoiceOtherChargeDetailList, "OtherChargeCode,ChargeAmount");
            }
            return _saleInvoiceRepository.InsertUpdateSaleInvoice(saleInvoice);
        }
        public SaleInvoice GetSaleInvoice(Guid id)
        {
            return _saleInvoiceRepository.GetSaleInvoice(id);
        }
        public object DeleteSaleInvoice(Guid id)
        {
            return _saleInvoiceRepository.DeleteSaleInvoice(id);
        }
        public object DeleteSaleInvoiceDetail(Guid id)
        {
            return _saleInvoiceRepository.DeleteSaleInvoiceDetail(id);
        }
        public List<SaleInvoiceOtherCharge> GetSaleInvoiceOtherChargesDetailListBySaleInvoiceID(Guid saleInvoiceID)
        {
            return _saleInvoiceRepository.GetSaleInvoiceOtherChargesDetailListBySaleInvoiceID(saleInvoiceID);
        }

        public async Task<bool> QuoteEmailPush(SaleInvoice saleInvoice)
        {
            bool sendsuccess = false;
            try
            {
                if (!string.IsNullOrEmpty(saleInvoice.EmailSentTo))
                {
                    string[] EmailList = saleInvoice.EmailSentTo.Split(',');
                    foreach (string email in EmailList)
                    {
                        Mail _mail = new Mail();
                        _mail.Body = saleInvoice.MailContant;
                        _mail.Subject = "Sale Invoice";
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

        public object UpdateSaleInvoiceEmailInfo(SaleInvoice saleInvoice)
        {
            return _saleInvoiceRepository.UpdateSaleInvoiceEmailInfo(saleInvoice);
        }

        public object DeleteSaleInvoiceOtherChargeDetail(Guid id)
        {
            return _saleInvoiceRepository.DeleteSaleInvoiceOtherChargeDetail(id);
        }
    }
}
