﻿using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IQuotationRepository
    {
        List<Quotation> GetAllQuotation(QuotationAdvanceSearch quotationAdvanceSearch);
        List<QuotationDetail> GetQuotationDetailListByQuotationID(Guid quotationID, bool isCopy);
        List<QuotationOtherCharge> GetQuotationOtherChargesDetailListByQuotationID(Guid quotationID, bool isCopy);
        Quotation GetQuotation(Guid id);
        object InsertUpdateQuotation(Quotation quotation);
        object DeleteQuotation(Guid id);
        object DeleteQuotationDetail(Guid id, string CreatedBy, DateTime CreatedDate);
        object DeleteQuotationOtherChargeDetail(Guid id, string CreatedBy, DateTime CreatedDate);
        object UpdateQuotationEmailInfo(Quotation quotation);
        List<Quotation> GetQuotationForSelectList(Guid? quoteID);
        List<Quotation> GetQuotationForSelectListOnDemand(string searchTerm);
        QuotationSummary GetQuotationSummaryCount();
    }
}
