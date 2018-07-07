﻿using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PilotSmithApp.BusinessService.Service
{
    public class ReportBusiness : IReportBusiness
    {
        private IReportRepository _reportRepository;

        public ReportBusiness(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;

        }

        #region GetAllReports
        public List<PSASysReport> GetAllReport(string searchTerm)
        {
            return _reportRepository.GetAllReport(searchTerm);
        }
        #endregion GetAllReports


        //#region GetPendingSaleOrderProductionReport
        //public List<PendingSaleOrderProductionReport> GetPendingSaleOrderProductionReport(PendingSaleOrderProductionReport pendingSaleOrderProductionReport)
        //{
        //    return _reportRepository.GetPendingSaleOrderProductionReport(pendingSaleOrderProductionReport);
        //}
        //#endregion GetPendingSaleOrderProductionReport


        #region GetEnquiryReport
        public List<EnquiryReport> GetEnquiryReport(EnquiryReport enquiryReport)
        {
            return _reportRepository.GetEnquiryReport(enquiryReport);
        }
        #endregion GetEnquiryReport


        #region GetEnquiryFollowupReport
        public List<EnquiryFollowupReport> GetEnquiryFollowupReport(EnquiryFollowupReport enquiryFollowupReport)
        {
            return _reportRepository.GetEnquiryFollowupReport(enquiryFollowupReport);
        }
        #endregion GetEnquiryFollowupReport

        #region GetEstimateReport
        public List<EstimateReport> GetEstimateReport(EstimateReport estimateReport)
        {
            return _reportRepository.GetEstimateReport(estimateReport);
        }
        #endregion GetEstimateReport


        #region GetQuotationReport
        public List<QuotationReport> GetQuotationReport(QuotationReport quotationReport)
        {
            return _reportRepository.GetQuotationReport(quotationReport);
        }
        #endregion GetQuotationReport
        
    }
}