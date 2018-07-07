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
    public class ProductionOrderBusiness:IProductionOrderBusiness
    {
        IProductionOrderRepository _productionOrderRepository;
        ICommonBusiness _commonBusiness;
        IMailBusiness _mailBusiness;
        IPDFGeneratorBusiness _pDFGeneratorBusiness;
        public ProductionOrderBusiness(IProductionOrderRepository productionOrderRepository, ICommonBusiness commonBusiness,IMailBusiness mailBusiness, IPDFGeneratorBusiness pDFGeneratorBusiness)
        {
            _productionOrderRepository = productionOrderRepository;
            _commonBusiness = commonBusiness;
            _mailBusiness = mailBusiness;
            _pDFGeneratorBusiness = pDFGeneratorBusiness;
        }
        public List<ProductionOrder> GetAllProductionOrder(ProductionOrderAdvanceSearch productionOrderAdvanceSearch)
        {
            return _productionOrderRepository.GetAllProductionOrder(productionOrderAdvanceSearch);
        }
        public List<ProductionOrderDetail> GetProductionOrderDetailListByProductionOrderID(Guid productionOrderID)
        {
            return _productionOrderRepository.GetProductionOrderDetailListByProductionOrderID(productionOrderID);
        }
        public object InsertUpdateProductionOrder(ProductionOrder productionOrder)
        {
            if (productionOrder.ProductionOrderDetailList.Count > 0)
            {
                productionOrder.DetailXML = _commonBusiness.GetXMLfromProductionOrderObject(productionOrder.ProductionOrderDetailList, "ProductID,ProductModelID,ProductSpec,UnitCode");
            }
            return _productionOrderRepository.InsertUpdateProductionOrder(productionOrder);
        }
        public ProductionOrder GetProductionOrder(Guid id)
        {
            return _productionOrderRepository.GetProductionOrder(id);
        }
        public object DeleteProductionOrder(Guid id)
        {
            return _productionOrderRepository.DeleteProductionOrder(id);
        }
        public object DeleteProductionOrderDetail(Guid id)
        {
            return _productionOrderRepository.DeleteProductionOrderDetail(id);
        }
        public List<ProductionOrder> GetProductionOrderForSelectListOnDemand(string searchTerm)
        {
            return _productionOrderRepository.GetProductionOrderForSelectListOnDemand(searchTerm);
        }
        public List<SelectListItem> GetProductionOrderForSelectList(Guid? id)
        {
            List<SelectListItem> selectListItem = null;
            List<ProductionOrder> productionOrderList = _productionOrderRepository.GetProductionOrderForSelectList(id);
            return selectListItem = productionOrderList!=null?(from productionOrder in productionOrderList
                                     select new SelectListItem
                                     {
                                         Text = productionOrder.ProdOrderNo,
                                         Value = productionOrder.ID.ToString(),
                                         Selected = false
                                     }).ToList():new List<SelectListItem>();
        }
        public object UpdateProductionOrderEmailInfo(ProductionOrder productionOrder)
        {
            return _productionOrderRepository.UpdateProductionOrderEmailInfo(productionOrder);
        }
        public async Task<bool> ProductionOrderEmailPush(ProductionOrder productionOrder)
        {

            bool sendsuccess = false;

            try
            {
                if (!string.IsNullOrEmpty(productionOrder.EmailSentTo))
                {
                    string[] EmailList = productionOrder.EmailSentTo.Split(',');
                    string mailBody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/MailTemplate/DocumentEmailBody.html"));
                    MailMessage _mail = new MailMessage();
                    PDFTools pDFTools = new PDFTools();
                    string link = WebConfigurationManager.AppSettings["AppURL"] + "/Content/images/Pilot1.png";
                    _mail.Body = mailBody.Replace("$Customer$", productionOrder.Customer.ContactPerson).Replace("$Document$", "Production Order").Replace("$DocumentNo$", productionOrder.ProdOrderNo).Replace("$DocumentDate$",productionOrder.ProdOrderDateFormatted).Replace("$Logo$", link);
                    pDFTools.Content = productionOrder.MailContant;
                    pDFTools.ContentFileName = "ProductionOrder";
                    _mail.Attachments.Add(new Attachment(new MemoryStream(_pDFGeneratorBusiness.GetPdfAttachment(pDFTools)), productionOrder.ProdOrderNo + ".pdf"));
                    _mail.Subject = "ProductionOrder";
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

        public ProductionOrderSummary GetProductionOrderSummaryCount()
        {
            return _productionOrderRepository.GetProductionOrderSummaryCount();
        }
    }
}
