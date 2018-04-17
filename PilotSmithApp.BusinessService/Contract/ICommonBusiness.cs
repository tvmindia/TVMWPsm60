using PilotSmithApp.DataAccessObject.DTO;
using System.Collections.Generic;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface ICommonBusiness
    {
        string ConvertCurrency(decimal value, int DecimalPoints = 0, bool Symbol = true);
        string GetXMLfromEnquiryObject(List<EnquiryDetail> enquiryDetailList, string mandatoryProperties);
        string GetXMLfromQuotationObject(List<QuotationDetail> quotationDetailList, string mandatoryProperties);
        string SendMessage(string message, string mobileNo,string provider,string type);
    }
}
