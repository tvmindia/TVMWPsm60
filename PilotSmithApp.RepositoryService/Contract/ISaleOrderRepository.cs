using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface ISaleOrderRepository
    {
        List<SaleOrder> GetAllSaleOrder(SaleOrderAdvanceSearch saleOrderAdvanceSearch);
        List<SaleOrder> GetSaleOrderForSelectListOnDemand(string searchTerm);
        List<SaleOrder> GetSaleOrderForSelectList(Guid? id);
        SaleOrder GetSaleOrder(Guid id);
        object InsertUpdateSaleOrder(SaleOrder saleOrder);
        object DeleteSaleOrder(Guid id);
        object DeleteSaleOrderDetail(Guid id);
        object UpdateSaleOrderEmailInfo(SaleOrder saleOrder);
        List<SaleOrderDetail> GetSaleOrderDetailListBySaleOrderID(Guid saleOrderID);
    }
}
