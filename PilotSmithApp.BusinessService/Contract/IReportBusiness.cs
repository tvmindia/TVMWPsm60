using PilotSmithApp.DataAccessObject.DTO;
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
        List<PendingSaleOrderReport> GetPendingSaleOrderReport(PendingSaleOrderReport pendingSaleOrderReport);     
        List<SaleOrderReport> GetSaleOrderStandardReport(SaleOrderReport saleOrderReport);
        List<ProductionOrderReport> GetProductionOrderStandardReport(ProductionOrderReport productionOrderReport);
        List<ProductionOrderDetailForecastDateExceededReport> GetProductionOrderDetailForecastDateExceededReport(ProductionOrderDetailForecastDateExceededReport productionOrderdetailForecastDateReport);
        List<PendingProductionOrderReport> GetPendingProductionOrderReport(PendingProductionOrderReport pendingProductionOrderReport);
        List<ProductionQCStandardReport> GetProductionQCStandardReport(ProductionQCStandardReport productionQCStandardReport);
        List<PendingProductionQCReport> GetPendingProductionQCReport(PendingProductionQCReport pendingProductionQCReport);
        List<QuotationDetailReport> GetQuotationDetailReport(QuotationDetailReport quotationDetailReport);
        List<EnquiryDetailReport> GetEnquiryDetailReport(EnquiryDetailReport enquiryDetailReport);
        List<EstimateDetailReport> GetEstimateDetailReport(EstimateDetailReport estimateDetailReport);      
    }
}
