using PilotSmithApp.DataAccessObject.DTO;
using System.Collections.Generic;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface ICommonBusiness
    {
        string ConvertCurrency(decimal value, int DecimalPoints = 0, bool Symbol = true);
        string GetXMLfromEnquiryObject(List<EnquiryDetail> enquiryDetailList, string mandatoryProperties);
        string GetXMLfromQuotationObject(List<QuotationDetail> quotationDetailList, string mandatoryProperties);
        string GetXMLfromEstimateObject(List<EstimateDetail> estimateDetailList, string mandatoryProperties);
        string GetXMLfromProductionOrderObject(List<ProductionOrderDetail> productionOrderDetailList, string mandatoryProperties);
        string GetXMLfromProductionQCObject(List<ProductionQCDetail> productionQCDetailList, string mandatoryProperties);
        string SendMessage(string message, string mobileNo,string provider,string type);
    }
}
