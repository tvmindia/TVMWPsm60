using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface ISaleOrderRepository
    {
        List<SaleOrder> GetAllSaleOrder(SaleOrderAdvanceSearch saleOrderAdvanceSearch);
        DataSet GetCustomerHistory(Guid customerID);
        List<SaleOrder> GetSaleOrderForSelectListOnDemand(string searchTerm);
        List<SaleOrder> GetSaleOrderForSelectList(Guid? id);
        SaleOrder GetSaleOrder(Guid id);
        object InsertUpdateSaleOrder(SaleOrder saleOrder);
        object DeleteSaleOrder(Guid id);
        object DeleteSaleOrderDetail(Guid id, string CreatedBy, DateTime CreatedDate);
        object UpdateSaleOrderEmailInfo(SaleOrder saleOrder);
        object DeleteSaleOrderOtherChargeDetail(Guid id, string CreatedBy, DateTime CreatedDate);
        List<SaleOrderDetail> GetSaleOrderDetailListBySaleOrderID(Guid saleOrderID, bool isCopy);
        List<SaleOrderOtherCharge> GetSaleOrderOtherChargesDetailListBySaleOrderID(Guid SaleOrderID, bool isCopy);
        SaleOrderSummary GetSaleOrderSummaryCount();
    }
}
