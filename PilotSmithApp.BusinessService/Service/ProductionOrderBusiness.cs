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
    }
}
