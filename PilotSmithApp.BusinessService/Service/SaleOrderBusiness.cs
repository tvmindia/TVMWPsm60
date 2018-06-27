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
using System.Web.Configuration;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Service
{
    public class SaleOrderBusiness:ISaleOrderBusiness
    {
        ISaleOrderRepository _saleOrderRepository;
        ICommonBusiness _commonBusiness;
        IMailBusiness _mailBusiness;
        IPDFGeneratorBusiness _pDFGeneratorBusiness;
        public SaleOrderBusiness(ISaleOrderRepository saleOrderRepository, ICommonBusiness commonBusiness, IMailBusiness mailBusiness, IPDFGeneratorBusiness pDFGeneratorBusiness)
        {
            _saleOrderRepository = saleOrderRepository;
            _commonBusiness = commonBusiness;
            _mailBusiness = mailBusiness;
            _pDFGeneratorBusiness = pDFGeneratorBusiness;
        }
        public List<SaleOrder> GetAllSaleOrder(SaleOrderAdvanceSearch saleOrderAdvanceSearch)
        {
            return _saleOrderRepository.GetAllSaleOrder(saleOrderAdvanceSearch);
        }
        public List<SaleOrder> GetSaleOrderForSelectListOnDemand(string searchTerm)
        {
            return _saleOrderRepository.GetSaleOrderForSelectListOnDemand(searchTerm);
        }
        public object InsertUpdateSaleOrder(SaleOrder saleOrder)
        {
            if (saleOrder.SaleOrderDetailList.Count > 0)
            {
                saleOrder.DetailXML = _commonBusiness.GetXMLfromSaleOrderObject(saleOrder.SaleOrderDetailList, "ProductID, ProductModelID, UnitCode, Qty, Rate");
            }
            if (saleOrder.SaleOrderOtherChargeList.Count > 0)
            {
                saleOrder.OtherChargeDetailXML = _commonBusiness.GetXMLfromSaleOrderOtherChargeObject(saleOrder.SaleOrderOtherChargeList, "OtherChargeCode , ChargeAmount");
            }
            return _saleOrderRepository.InsertUpdateSaleOrder(saleOrder);
        }
        public SaleOrder GetSaleOrder(Guid id)
        {
            return _saleOrderRepository.GetSaleOrder(id);
        }
        public object DeleteSaleOrder(Guid id)
        {
            return _saleOrderRepository.DeleteSaleOrder(id);
        }
        public object DeleteSaleOrderDetail(Guid id)
        {
            return _saleOrderRepository.DeleteSaleOrderDetail(id);
        }
        public object DeleteSaleOrderOtherChargeDetail(Guid id)
        {
            return _saleOrderRepository.DeleteSaleOrderOtherChargeDetail(id);
        }
        public object UpdateSaleOrderEmailInfo(SaleOrder saleOrder)
        {
            return _saleOrderRepository.UpdateSaleOrderEmailInfo(saleOrder);
        }
        public async Task<bool> QuoteEmailPush(SaleOrder saleOrder)
        {

            bool sendsuccess = false;

            try
            {
                if (!string.IsNullOrEmpty(saleOrder.EmailSentTo))
                {
                    string[] EmailList = saleOrder.EmailSentTo.Split(',');
                    string mailBody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/MailTemplate/DocumentEmailBody.html"));
                    MailMessage _mail = new MailMessage();
                    PDFTools pDFTools = new PDFTools();
                    string link = WebConfigurationManager.AppSettings["AppURL"] + "/Content/images/Pilot1.png";
                    _mail.Body = mailBody.Replace("$Customer$", saleOrder.Customer.ContactPerson).Replace("$Document$", "Sale Order").Replace("$DocumentNo$", saleOrder.SaleOrderNo).Replace("$DocumentDate$",saleOrder.SaleOrderDateFormatted).Replace("$Logo$", link);
                    pDFTools.Content = saleOrder.MailContant;
                    _mail.Attachments.Add(new Attachment(new MemoryStream(_pDFGeneratorBusiness.GetPdfAttachment(pDFTools)), saleOrder.SaleOrderNo + ".pdf"));
                    _mail.Subject = "SaleOrder";
                    _mail.IsBodyHtml = true;
                    foreach (string email in EmailList)
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
        public List<SelectListItem> GetSaleOrderForSelectList(Guid? id)
        {
            List<SelectListItem> selectListItem = null;
            List<SaleOrder> saleOrderList = _saleOrderRepository.GetSaleOrderForSelectList(id);
            return selectListItem = saleOrderList != null ? (from saleOrder in saleOrderList
                                                             select new SelectListItem
                                                             {
                                                                 Text = saleOrder.SaleOrderNo,
                                                                 Value = saleOrder.ID.ToString(),
                                                                 Selected = false
                                                             }).ToList() : new List<SelectListItem>();
        }
        public List<SaleOrderDetail> GetSaleOrderDetailListBySaleOrderID(Guid saleOrderID)
        {
            return _saleOrderRepository.GetSaleOrderDetailListBySaleOrderID(saleOrderID);
        }

        public List<SaleOrderOtherCharge> GetSaleOrderOtherChargesDetailListBySaleOrderID(Guid SaleOrderID)
        {
            return _saleOrderRepository.GetSaleOrderOtherChargesDetailListBySaleOrderID(SaleOrderID);
        }

        public SaleOrderSummary GetSaleOrderSummaryCount()
        {
            return _saleOrderRepository.GetSaleOrderSummaryCount();
        }
    }
}
