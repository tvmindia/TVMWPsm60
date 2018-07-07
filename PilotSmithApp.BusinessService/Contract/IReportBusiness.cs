﻿using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PilotSmithApp.BusinessService.Contract
{
   public interface IReportBusiness
    {
        List<PSASysReport> GetAllReport(string searchTerm);
        //List<PendingSaleOrderProductionReport> GetPendingSaleOrderProductionReport(PendingSaleOrderProductionReport pendingSaleOrderProductionReport);
        List<EnquiryReport> GetEnquiryReport(EnquiryReport enquiryReport);
        List<EnquiryFollowupReport> GetEnquiryFollowupReport(EnquiryFollowupReport enquiryFollowupReport);
        List<EstimateReport> GetEstimateReport(EstimateReport estimateReport);
        List<QuotationReport> GetQuotationReport(QuotationReport quotationReport);
        
    }
}