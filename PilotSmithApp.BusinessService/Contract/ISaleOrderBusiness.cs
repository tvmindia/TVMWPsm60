using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface ISaleOrderBusiness
    {
        List<SaleOrder> GetAllSaleOrder(SaleOrderAdvanceSearch saleOrderAdvanceSearch);
        List<SaleOrder> GetSaleOrderForSelectListOnDemand(string searchTerm);
        List<SelectListItem> GetSaleOrderForSelectList(Guid? id);
        SaleOrder GetSaleOrder(Guid id);
        List<SaleOrderDetail> GetSaleOrderDetailListBySaleOrderID(Guid saleOrderID, bool isCopy);
        object InsertUpdateSaleOrder(SaleOrder saleOrder);
        object DeleteSaleOrder(Guid id);
        object DeleteSaleOrderDetail(Guid id);
        object DeleteSaleOrderOtherChargeDetail(Guid id);
        object UpdateSaleOrderEmailInfo(SaleOrder saleOrder);
        Task<bool> QuoteEmailPush(SaleOrder saleOrder);
        List<SaleOrderOtherCharge> GetSaleOrderOtherChargesDetailListBySaleOrderID(Guid SaleOrderID, bool isCopy);
        SaleOrderSummary GetSaleOrderSummaryCount();
    }
}
