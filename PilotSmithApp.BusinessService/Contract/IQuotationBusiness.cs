using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IQuotationBusiness
    {
        List<Quotation> GetAllQuotation(QuotationAdvanceSearch quotationAdvanceSearch);
        List<QuotationDetail> GetQuotationDetailListByQuotationID(Guid quotationID);
        List<QuotationOtherCharge> GetQuotationOtherChargesDetailListByQuotationID(Guid quotationID);
        Quotation GetQuotation(Guid id);
        QuotationDetail CalculateGST(QuotationDetail quotationDetail);
        object InsertUpdateQuotation(Quotation quotation);
        object DeleteQuotation(Guid id);
        object DeleteQuotationDetail(Guid id);
        object DeleteQuotationOtherChargeDetail(Guid id);
        object UpdateQuotationEmailInfo(Quotation quotation);
        Task<bool> QuoteEmailPush(Quotation quotation);
        List<SelectListItem> GetQuotationForSelectList(Guid? quoteID);
        List<Quotation> GetQuotationForSelectListOnDemand(string searchTerm);
    }
}
