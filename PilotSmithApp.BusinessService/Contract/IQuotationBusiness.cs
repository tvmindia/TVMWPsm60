using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IQuotationBusiness
    {
        List<Quotation> GetAllQuotation(QuotationAdvanceSearch quotationAdvanceSearch);
        List<QuotationDetail> GetQuotationDetailListByQuotationID(Guid quotationID);
        Quotation GetQuotation(Guid id);
        QuotationDetail CalculateGST(QuotationDetail quotationDetail);
        object InsertUpdateQuotation(Quotation quotation);
        object DeleteQuotation(Guid id);
        object DeleteQuotationDetail(Guid id);
    }
}
