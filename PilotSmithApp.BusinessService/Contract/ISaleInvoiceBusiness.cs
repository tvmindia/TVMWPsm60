using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface ISaleInvoiceBusiness
    {
        List<SaleInvoice> GetAllSaleInvoice(SaleInvoiceAdvanceSearch saleInvoiceAdvanceSearch);
        SaleInvoice GetSaleInvoice(Guid id);
        List<SaleInvoiceDetail> GetSaleInvoiceDetailListBySaleInvoiceID(Guid saleInvoiceID);
        object InsertUpdateSaleInvoice(SaleInvoice saleInvoice);
        object DeleteSaleInvoice(Guid id);
        object DeleteSaleInvoiceDetail(Guid id);
        object DeleteSaleInvoiceOtherChargeDetail(Guid id);
        Task<bool> QuoteEmailPush(SaleInvoice saleInvoice);
        List<SaleInvoiceOtherCharge> GetSaleInvoiceOtherChargesDetailListBySaleInvoiceID(Guid saleInvoiceID);
        object UpdateSaleInvoiceEmailInfo(SaleInvoice saleInvoice);

    }
}
