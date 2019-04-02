using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IProductionOrderRepository
    {
        List<ProductionOrder> GetAllProductionOrder(ProductionOrderAdvanceSearch productionOrderAdvanceSearch);
        ProductionOrder GetProductionOrder(Guid id);
        List<ProductionOrderDetail> GetProductionOrderDetailListByProductionOrderID(Guid productionOrderID);
        object InsertUpdateProductionOrder(ProductionOrder productionOrder);
        object DeleteProductionOrder(Guid id);
        object DeleteProductionOrderDetail(Guid id, string CreatedBy, DateTime CreatedDate);
        List<ProductionOrder> GetProductionOrderForSelectListOnDemand(string searchTerm);
        List<ProductionOrder> GetProductionOrderForSelectList(Guid? id);
        object UpdateProductionOrderEmailInfo(ProductionOrder productionOrder, int emailSendSuccess);
        ProductionOrderSummary GetProductionOrderSummaryCount();
        ProductionOrderDetail ValidateProductionOrderDetailOrderQty(Guid SaleOrderDetailID, Guid ProductionOrderDetailID);
    }
}
