using PilotSmithApp.DataAccessObject.DTO;
using System;
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
        string GetXMLfromSaleInvoiceObject(List<SaleInvoiceDetail> saleInvoiceDetailList, string mandatoryProperties);
        string GetXMLfromDeliveryChallanObject(List<DeliveryChallanDetail> deliveryChallanDetailList, string mandatoryProperties);
        string GetXMLfromSaleOrderObject(List<SaleOrderDetail> saleOrderDetailList, string mandatoryProperties);
        string GetXMLfromQuotationOtherChargeObject(List<QuotationOtherCharge> quotationOtherChargeList, string mandatoryProperties);
        string GetXMLfromSaleOrderOtherChargeObject(List<SaleOrderOtherCharge> saleOrderOtherChargeDetailList, string mandatoryProperties);
        string GetXMLfromSaleInvoiceOtherChargeObject(List<SaleInvoiceOtherCharge> saleInvoiceOtherChargeDetailList, string mandatoryProperties);
        string SendMessage(string message, string mobileNo,string provider,string type);
        bool CheckDocumentIsDeletable(string docType, Guid? id);
    }
}
