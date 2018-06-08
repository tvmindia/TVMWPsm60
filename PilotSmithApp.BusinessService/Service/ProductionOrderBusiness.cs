using PilotSmithApp.BusinessService.Contract;
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
    public class ProductionOrderBusiness:IProductionOrderBusiness
    {
        IProductionOrderRepository _productionOrderRepository;
        ICommonBusiness _commonBusiness;
        IMailBusiness _mailBusiness;
        public ProductionOrderBusiness(IProductionOrderRepository productionOrderRepository, ICommonBusiness commonBusiness,IMailBusiness mailBusiness)
        {
            _productionOrderRepository = productionOrderRepository;
            _commonBusiness = commonBusiness;
            _mailBusiness = mailBusiness;
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
                productionOrder.DetailXML = _commonBusiness.GetXMLfromProductionOrderObject(productionOrder.ProductionOrderDetailList, "ProductID");
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
                    foreach (string email in EmailList)
                    {
                        Mail _mail = new Mail();
                        _mail.Body = productionOrder.MailContant;
                        _mail.Subject = "ProductionOrder";
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

        public ProductionOrderSummary GetProductionOrderSummaryCount()
        {
            return _productionOrderRepository.GetProductionOrderSummaryCount();
        }
    }
}
