using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IProductionOrderBusiness
    {
        List<ProductionOrder> GetAllProductionOrder(ProductionOrderAdvanceSearch productionOrderAdvanceSearch);
        ProductionOrder GetProductionOrder(Guid id);
        List<ProductionOrderDetail> GetProductionOrderDetailListByProductionOrderID(Guid productionOrderID);
        object InsertUpdateProductionOrder(ProductionOrder productionOrder);
        object DeleteProductionOrder(Guid id);
        object DeleteProductionOrderDetail(Guid id);
        List<ProductionOrder> GetProductionOrderForSelectListOnDemand(string searchTerm);
        List<SelectListItem> GetProductionOrderForSelectList(Guid? id);
        object UpdateProductionOrderEmailInfo(ProductionOrder productionOrder);
        Task<bool> ProductionOrderEmailPush(ProductionOrder productionOrder);
        ProductionOrderSummary GetProductionOrderSummaryCount();
        ProductionOrderDetail ValidateProductionOrderDetailOrderQty(Guid SaleOrderDetailID);
    }
}
