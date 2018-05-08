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
        public ProductionOrderBusiness(IProductionOrderRepository productionOrderRepository, ICommonBusiness commonBusiness)
        {
            _productionOrderRepository = productionOrderRepository;
            _commonBusiness = commonBusiness;
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
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<ProductionOrder> productionOrderList = _productionOrderRepository.GetProductionOrderForSelectList(id);
            if (productionOrderList != null)
                foreach (ProductionOrder productionOrder in productionOrderList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = productionOrder.ProdOrderNo,
                        Value = productionOrder.ID.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}
