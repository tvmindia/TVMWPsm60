﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
   public class PSASysReport
    {
        public Guid ID { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ReportGroup { get; set; }
        public int GroupOrder { get; set; }
        public string SPName { get; set; }
        public string SQL { get; set; }
        public int ReportOrder { get; set; }
        public List<PSASysReport> PSASysReportList { get; set; }
        public string SearchTerm { get; set; }
    }

    public class PendingSaleOrderProductionReport
    {
        public Guid? SaleOrderID { get; set; }
        public decimal? PendingQty { get; set; }
        public decimal? SaleOrderQty { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? ProducedQty { get; set; }
        public string SaleOrderNo { get; set; }        
        public DateTime SaleOrderDate { get; set; }
        public string SaleOrderDateFormatted { get; set; }
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public Customer Customer { get; set; }
        public Guid AdvCustomerID { get; set; }
        public Product Product { get; set; }
        public Guid? AdvProductID { get; set; }
        public string AdvSaleOrderNo { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<PendingSaleOrderProductionReport> PendingSaleOrderProductionReportList { get; set; }
    }

    public class EnquiryReport
    {
        public string EnquiryNo { get; set; }
        public string EnqNo { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string EnquiryDateFormatted { get; set; }
        public string RequirementSpec { get; set; }
        public string CustomerID { get; set; }
        public EnquiryGrade EnquiryGrade { get; set; }
        public int? AdvEnquiryGradeCode { get; set; }
        public int? DocumentStatusCode { get; set; }
        public int? ReferredByCode { get; set; }  
        public Guid? AdvAttendedByID { get; set; }    
        public Employee Employee { get; set; }
        public Guid? DocumentOwnerID { get; set; }
        public int? BranchCode { get; set; }
        public decimal? Amount { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string AdvCustomer { get; set; }
        public Customer Customer { get; set; }
        public int? AdvDistrictCode { get; set; }
        public int? AdvStateCode { get; set; }
        public int? AdvCountryCode { get; set; }
        public int? AdvAreaCode { get; set; }
        public Area Area { get; set; }
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePerson ReferencePerson { get; set; }
        public int? AdvBranchCode { get; set; }
        public Branch Branch { get; set; }
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUser PSAUser { get; set; }
        public decimal? AdvAmountFrom { get; set; }
        public decimal? AdvAmountTo { get; set; }
        public ReferenceType ReferenceType { get; set; }
        public int? AdvReferenceTypeCode { get; set; }
        public CustomerCategory CustomerCategory { get; set; }
        public int? AdvCustomerCategoryCode { get; set; }
        public string SearchTerm { get; set; }
        public List<EnquiryReport> EnquiryReportList { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
    
    public class EnquiryFollowupReport
    {        
        public DateTime FollowupDate { get; set; }
        public DateTime FollowupTime { get; set; }
        public string Priority { get; set; }
        public int PriorityCode { get; set; }  
        public string EnquiryNo { get; set; }
        public string EnqNo { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string EnquiryDateFormatted { get; set; }
        public string ContactName { get; set; }
        public string ContactNo { get; set; }      
        public string Status { get; set; }        
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }       
        public string FollowupDateFormatted { get; set; }
        public string FollowupTimeFormatted { get; set; }
        public string SearchTerm { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string AdvCustomer { get; set; }
        public Customer Customer { get; set; }
        public string AdvStatus { get; set; }
        public int? AdvFollowupPriority { get; set; }
        public string FollowupRemarks { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }
        public string DocumentOwnerName { get; set; }
        public List<EnquiryFollowupReport> EnquiryFollowupReportList { get; set;}
    }

    public class EstimateReport
    {
        public string EstimateNo { get; set; } 
        public string EstNo { get; set; }
        public DateTime EstimateDate { get; set; }      
        public string EstimateDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }       
        public Employee Employee { get; set; }       
        public decimal? Amount { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string AdvCustomer { get; set; }
        public Customer Customer { get; set; }
        public int? AdvDistrictCode { get; set; }
        public int? AdvStateCode { get; set; }
        public int? AdvCountryCode { get; set; }
        public int? AdvAreaCode { get; set; }
        public Area Area { get; set; }       
        public int? AdvBranchCode { get; set; }
        public Branch Branch { get; set; }
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUser PSAUser { get; set; }
        public decimal? AdvAmountFrom { get; set; }
        public decimal? AdvAmountTo { get; set; }       
        public CustomerCategory CustomerCategory { get; set; }
        public int? AdvCustomerCategoryCode { get; set; }
        public string SearchTerm { get; set; }
        public Guid AdvPreparedBy { get; set; }
        public string Notes { get; set; }
        public List<EstimateReport> EstimateReportList { get; set; }
        public string PreparedBy { get; set; }
        public decimal TotalAmount { get; set; }
    }


    public class QuotationReport
    {
        public string QuoteNo { get; set; } 
        public string QuotationNo { get; set; }
        public DateTime QuoteDate { get; set; }
        public string QuoteDateFormatted { get; set; }
        public Customer Customer { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string AdvCustomer { get; set; }        
        public int? AdvDistrictCode { get; set; }
        public int? AdvStateCode { get; set; }
        public int? AdvCountryCode { get; set; }
        public int? AdvAreaCode { get; set; }
        public Area Area { get; set; }
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePerson ReferencePerson { get; set; }
        public int? AdvBranchCode { get; set; }
        public Branch Branch { get; set; }
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUser PSAUser { get; set; }
        public Employee Employee { get; set; }
        public int? AdvCustomerCategoryCode { get; set; }
        public string SearchTerm { get; set; }
        public Guid AdvPreparedBy { get; set; }
        public string Notes { get; set; }
        public List<QuotationReport> QuotationReportList { get; set; }
        public string PreparedBy { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public int? AdvApprovalStatusCode { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public decimal? Amount { get; set; }        
        public decimal? AdvAmountFrom { get; set; }
        public decimal? AdvAmountTo { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class PendingSaleOrderReport
    {
        public string SaleOrderNo { get; set; }     
        public string SaleOrdNo { get; set; }
        public DateTime SaleOrderDate { get; set; }
        public string SaleOrderDateFormatted { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Qty { get; set; }
        public decimal? TaxableAmount { get; set; }
        public decimal? Amount { get; set; }
        public int? UnitCode { get; set; }        
        public ProductModel ProductModel { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Unit Unit { get; set; }       
        public Customer Customer { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public string AdvDelFromDate { get; set; }
        public string AdvDelToDate { get; set; }
        public string AdvCustomer { get; set; }
        public Guid AdvPreparedBy { get; set; }
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePerson ReferencePerson { get; set; }
        public decimal? AdvAmountFrom { get; set; }
        public decimal? AdvAmountTo { get; set; }
        public int? AdvDistrictCode { get; set; }
        public int? AdvStateCode { get; set; }
        public int? AdvCountryCode { get; set; }
        public int? AdvAreaCode { get; set; }
        public int? AdvCustomerCategoryCode { get; set; }
        public int? AdvBranchCode { get; set; }
        public Branch Branch { get; set; }
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUser PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid AdvProduct { get; set; }
        public Product Product { get; set; }
        public Guid AdvProductModel { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public int? AdvApprovalStatusCode { get; set; }
        public string SearchTerm { get; set; }
        public Employee Employee { get; set; }
        public int? AdvReportType { get; set; }
        public decimal? PendingQty { get; set; }
        public string DateFilter { get; set; }
        public List<PendingSaleOrderReport> PendingSaleOrderReportList { get; set; }
        public int? ReportType { get; set; }
        public int? hdnReportType { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTaxableAmount { get; set; }
    }


    public class SaleOrderReport
    {
        public string SaleOrderNo { get; set; }
        public string SaleOrdNo { get; set; }
        public DateTime SaleOrderDate { get; set; }
        public string SaleOrderDateFormatted { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Qty { get; set; }
        public decimal? TaxableAmount { get; set; }
        public decimal? Amount { get; set; }
        public int? UnitCode { get; set; }
        public ProductModel ProductModel { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Unit Unit { get; set; }
        public Customer Customer { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public string AdvDelFromDate { get; set; }
        public string AdvDelToDate { get; set; }
        public string AdvCustomer { get; set; }
        public Guid AdvPreparedBy { get; set; }
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePerson ReferencePerson { get; set; }
        public decimal? AdvAmountFrom { get; set; }
        public decimal? AdvAmountTo { get; set; }
        public int? AdvDistrictCode { get; set; }
        public int? AdvStateCode { get; set; }
        public int? AdvCountryCode { get; set; }
        public int? AdvAreaCode { get; set; }
        public int? AdvCustomerCategoryCode { get; set; }
        public int? AdvBranchCode { get; set; }
        public Branch Branch { get; set; }
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUser PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid AdvProduct { get; set; }
        public Product Product { get; set; }
        public Guid AdvProductModel { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public int? AdvApprovalStatusCode { get; set; }
        public string SearchTerm { get; set; }
        public Employee Employee { get; set; }
        public int? AdvReportType { get; set; }
        public List<SaleOrderReport> SaleOrderReportList { get; set; }
        public string DateFilter { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTaxableAmount { get; set; }
    }

    public class ProductionOrderReport
    {
        public string ProdOrderNo { get; set; }
        public string ProductionOrderNo { get; set; }
        public string SaleOrderNo { get; set; }
        public string SaleOrdNo { get; set; }
        public DateTime ProdOrderDate { get; set; }
        public string ProdOrderDateFormatted { get; set; }
        public Guid? SaleOrderID { get; set; }
        public Guid? CustomerID { get; set; }
        public DateTime? ExpectedDelvDate { get; set; }
        public string ExpectedDelvDateFormatted { get; set; }
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePerson ReferencePerson { get; set; }
        public Customer Customer { get; set; }
        public Guid AdvCustomerID { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public string AdvDelFromDate { get; set; }
        public string AdvDelToDate { get; set; }
        public Area Area { get; set; }
        public decimal? AdvAmountFrom { get; set; }
        public decimal? AdvAmountTo { get; set; }
        public int? AdvDistrictCode { get; set; }
        public int? AdvStateCode { get; set; }
        public int? AdvCountryCode { get; set; }
        public int? AdvAreaCode { get; set; }
        public int? AdvCustomerCategoryCode { get; set; }
        public int? AdvBranchCode { get; set; }
        public Branch Branch { get; set; }
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUser PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid AdvProduct { get; set; }
        public Product Product { get; set; }
        public ProductModel ProductModel { get; set; }
        public Guid AdvProductModel { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public int? AdvApprovalStatusCode { get; set; }
        public string SearchTerm { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }      
        public decimal? Amount { get; set; }      
        public string Remarks { get; set; }
        public int? AdvReportType { get; set; }
        public int? AdvProgress { get; set; }        
        public int? AdvPlantCode { get; set; }
        public string PreparedBy { get; set; }
        public string DateFilter { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public string AdvCustomer { get; set; }
        public Guid AdvPreparedBy { get; set; }
        public Employee Employee { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Plant Plant { get; set; }
        public decimal? Qty { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class ProductionOrderDetailForecastDateExceededReport
    {
        public string ProdOrderNo { get; set; }
        public string ProductionOrderNo { get; set; }
        public string SaleOrderNo { get; set; }
        public string SaleOrdNo { get; set; }
        public DateTime ProdOrderDate { get; set; }
        public string ProdOrderDateFormatted { get; set; }
        public Guid? SaleOrderID { get; set; }
        public Guid? CustomerID { get; set; }
        public DateTime? ExpectedDelvDate { get; set; }
        public string ExpectedDelvDateFormatted { get; set; }
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePerson ReferencePerson { get; set; }
        public Customer Customer { get; set; }

        
        public string AdvFromDate { get; set; }
        
        public string AdvToDate { get; set; }
        
        public string AdvDelFromDate { get; set; }
        
        public string AdvDelToDate { get; set; }
        
        public string AdvCustomer { get; set; }
        public Area Area { get; set; }
        
        public decimal? AdvAmountFrom { get; set; }
        
        public decimal? AdvAmountTo { get; set; }
        
        public int? AdvDistrictCode { get; set; }
        
        public int? AdvStateCode { get; set; }
        
        public int? AdvCountryCode { get; set; }
        
        public int? AdvAreaCode { get; set; }
        
        public int? AdvCustomerCategoryCode { get; set; }
       
        public int? AdvBranchCode { get; set; }
        public Branch Branch { get; set; }
        
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUser PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
       
        public Guid AdvProduct { get; set; }
        public Product Product { get; set; }
        
        public Guid AdvProductModel { get; set; }
        public ProductModel ProductModel { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        
        public int? AdvApprovalStatusCode { get; set; }
       
        public string SearchTerm { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
        public int? AdvReportType { get; set; }
        public int? AdvProgress { get; set; }
        public Plant Plant { get; set; }
        
        public int? AdvPlantCode { get; set; }
        public string PreparedBy { get; set; }
        public string DateFilter { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
       
        public Guid AdvPreparedBy { get; set; }
        public Employee Employee { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public decimal? Qty { get; set; }
        public decimal? ProducedQty { get; set; }
        public int? Progress { get; set; }
        public int? ExptProgress { get; set; }
        public int? ReportValue { get; set; }
        public string ExptCompletionDate { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class PendingProductionOrderReport
    {
        public string ProdOrderNo { get; set; }
        public string ProductionOrderNo { get; set; }
        public string SaleOrderNo { get; set; }
        public string SaleOrdNo { get; set; }
        public DateTime ProdOrderDate { get; set; }
        public string ProdOrderDateFormatted { get; set; }
        public DateTime ForecastDate { get; set; }
        public string ForecastDateFormatted { get; set; }
        public Guid? SaleOrderID { get; set; }
        public Guid? CustomerID { get; set; }
        public DateTime? ExpectedDelvDate { get; set; }
        public string ExpectedDelvDateFormatted { get; set; }
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePerson ReferencePerson { get; set; }
        public Customer Customer { get; set; }
        public string AdvCustomer { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public string AdvDelFromDate { get; set; }
        public string AdvDelToDate { get; set; }
        public Area Area { get; set; }
        public decimal? AdvAmountFrom { get; set; }
        public decimal? AdvAmountTo { get; set; }
        public int? AdvDistrictCode { get; set; }
        public int? AdvStateCode { get; set; }
        public int? AdvCountryCode { get; set; }
        public int? AdvAreaCode { get; set; }
        public int? AdvCustomerCategoryCode { get; set; }
        public int? AdvBranchCode { get; set; }
        public Branch Branch { get; set; }
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUser PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid AdvProduct { get; set; }
        public Product Product { get; set; }
        public ProductModel ProductModel { get; set; }
        public Guid AdvProductModel { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public int? AdvApprovalStatusCode { get; set; }
        public string SearchTerm { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
        public int? AdvReportType { get; set; }
        public int? AdvProgress { get; set; }
        public int? AdvPlantCode { get; set; }
        public string PreparedBy { get; set; }
        public string DateFilter { get; set; }
        public PSASysCommon PSASysCommon { get; set; }       
        public Guid AdvPreparedBy { get; set; }
        public Employee Employee { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Plant Plant { get; set; }
        public decimal? Qty { get; set; }
        public int? ReportType { get; set; }
        public decimal? PendingQty { get; set; }
        public decimal? SaleOrderQty { get; set; }
        public int? Progress { get; set; }
        public List<PendingProductionOrderReport> PendingProductionOrderReportList { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class ProductionQCStandardReport
    {
        public string ProdOrderNo { get; set; }
        public string ProductionOrderNo { get; set; }     
        public DateTime ProdOrderDate { get; set; }
        public string ProdOrderDateFormatted { get; set; }
        public string ProductionQCNo { get; set; }
        public string ProdQCNo { get; set; }
        public Guid? CustomerID { get; set; }
        public DateTime? ExpectedDelvDate { get; set; }
        public string ExpectedDelvDateFormatted { get; set; }
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePerson ReferencePerson { get; set; }
        public Customer Customer { get; set; }
        public string AdvCustomer { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }    
        public Area Area { get; set; }
        public decimal? AdvAmountFrom { get; set; }
        public decimal? AdvAmountTo { get; set; }
        public int? AdvDistrictCode { get; set; }
        public int? AdvStateCode { get; set; }
        public int? AdvCountryCode { get; set; }
        public int? AdvAreaCode { get; set; }
        public int? AdvCustomerCategoryCode { get; set; }
        public int? AdvBranchCode { get; set; }
        public Branch Branch { get; set; }
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUser PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid AdvProduct { get; set; }
        public Product Product { get; set; }
        public ProductModel ProductModel { get; set; }
        public Guid AdvProductModel { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public int? AdvApprovalStatusCode { get; set; }
        public string SearchTerm { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
        public int? AdvReportType { get; set; }
        public int? AdvProgress { get; set; }
        public int? AdvPlantCode { get; set; }
        public string PreparedBy { get; set; }
        public string DateFilter { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public Guid AdvPreparedBy { get; set; }
        public Employee Employee { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Plant Plant { get; set; }        
        public int? ReportType { get; set; }
        public decimal? ProdOrdQty { get; set; }
        public decimal? ProdQCQty { get; set; }      
        public List<ProductionQCStandardReport> ProductionQCStandardReportList { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class PendingProductionQCReport
    {
        public string ProdOrderNo { get; set; }
        public string ProductionOrderNo { get; set; }
        public DateTime ProdOrderDate { get; set; }
        public string ProdOrderDateFormatted { get; set; }
        public string ProductionQCNo { get; set; }
        public string ProdQCNo { get; set; }
        public Guid? CustomerID { get; set; }
        public DateTime? ExpectedDelvDate { get; set; }
        public string ExpectedDelvDateFormatted { get; set; }
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePerson ReferencePerson { get; set; }
        public Customer Customer { get; set; }
        public string AdvCustomer { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public Area Area { get; set; }
        public decimal? AdvAmountFrom { get; set; }
        public decimal? AdvAmountTo { get; set; }
        public int? AdvDistrictCode { get; set; }
        public int? AdvStateCode { get; set; }
        public int? AdvCountryCode { get; set; }
        public int? AdvAreaCode { get; set; }
        public int? AdvCustomerCategoryCode { get; set; }
        public int? AdvBranchCode { get; set; }
        public Branch Branch { get; set; }
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUser PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid AdvProduct { get; set; }
        public Product Product { get; set; }
        public ProductModel ProductModel { get; set; }
        public Guid AdvProductModel { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public int? AdvApprovalStatusCode { get; set; }
        public string SearchTerm { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
        public int? AdvReportType { get; set; }
        public int? AdvProgress { get; set; }
        public int? AdvPlantCode { get; set; }
        public string PreparedBy { get; set; }
        public string DateFilter { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public Guid AdvPreparedBy { get; set; }
        public Employee Employee { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Plant Plant { get; set; }
        public int? ReportType { get; set; }
        public decimal? ProdOrdQty { get; set; }
        public decimal? ProdQCQty { get; set; }
        public decimal? PendingQty { get; set; }
        public List<PendingProductionQCReport> PendingProductionQCReportList { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class QuotationDetailReport
    {
        public string QuotationNo { get; set; }
        public DateTime QuoteDate { get; set; }
        public string QuoteDateFormatted { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TaxableAmount { get; set; }
        public int? UnitCode { get; set; }
        public ProductModel ProductModel { get; set; }
        public Unit Unit { get; set; }
        public Customer Customer { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public string AdvCustomer { get; set; }
        public Guid AdvPreparedBy { get; set; }
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePerson ReferencePerson { get; set; }
        public decimal? AdvAmountFrom { get; set; }
        public decimal? AdvAmountTo { get; set; }
        public int? AdvDistrictCode { get; set; }
        public int? AdvStateCode { get; set; }
        public int? AdvCountryCode { get; set; }
        public int? AdvAreaCode { get; set; }
        public int? AdvCustomerCategoryCode { get; set; }
        public int? AdvBranchCode { get; set; }
        public Branch Branch { get; set; }
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUser PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Product Product { get; set; }
        public Guid AdvProduct { get; set; }
        public Guid AdvProductModel { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public int? AdvApprovalStatusCode { get; set; }
        public string SearchTerm { get; set; }
        public int? AdvReportType { get; set; }
        public Employee Employee { get; set; }
        public List<QuotationDetailReport> QuotationReportList { get; set; }
        public string QuoteNo { get; set; }
        public string DateFilter { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTaxableAmount { get; set; }
    }
    public class EnquiryDetailReport
    {
        public string EnquiryNo { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string EnquiryDateFormatted { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Amount { get; set; }
        public int? UnitCode { get; set; }
        public ProductModel ProductModel { get; set; }
        public Unit Unit { get; set; }
        public Customer Customer { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public string AdvCustomer { get; set; }
        public Guid AdvAttendedBy { get; set; }
        public Guid AdvResponsibleBy { get; set; }
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePerson ReferencePerson { get; set; }
        public decimal? AdvAmountFrom { get; set; }
        public decimal? AdvAmountTo { get; set; }
        public int? AdvDistrictCode { get; set; }
        public int? AdvStateCode { get; set; }
        public int? AdvCountryCode { get; set; }
        public int? AdvAreaCode { get; set; }
        public int? AdvCustomerCategoryCode { get; set; }
        public int? AdvBranchCode { get; set; }
        public int? AdvEnquiryGradeCode { get; set; }
        public EnquiryGrade EnquiryGrade { get; set; }
        public Branch Branch { get; set; }
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUser PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Product Product { get; set; }
        public Guid AdvProduct { get; set; }
        public Guid AdvProductModel { get; set; }
        public string SearchTerm { get; set; }
        public int? AdvReportType { get; set; }
        public Employee Employee { get; set; }
        public List<EnquiryDetailReport> EnquiryReportList { get; set; }
        public string EnqNo { get; set; }
        public string DateFilter { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class EstimateDetailReport
    {
        public string EstimateNo { get; set; }
        public DateTime EstimateDate { get; set; }
        public string EstimateDateFormatted { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Qty { get; set; }
        public decimal? TotalCostRate { get; set; }
        public decimal? TotalSellingRate { get; set; }
        public int? UnitCode { get; set; }
        public ProductModel ProductModel { get; set; }
        public Unit Unit { get; set; }
        public Customer Customer { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public string AdvValidFromDate { get; set; }
        public string AdvValidToDate { get; set; }
        public string AdvCustomer { get; set; }
        public Guid AdvPreparedBy { get; set; }
        public decimal? AdvTotalCostRateFrom { get; set; }
        public decimal? AdvTotalCostRateTo { get; set; }
        public decimal? AdvTotalSellingRateFrom { get; set; }
        public decimal? AdvTotalSellingRateTo { get; set; }
        public int? AdvDistrictCode { get; set; }
        public int? AdvStateCode { get; set; }
        public int? AdvCountryCode { get; set; }
        public int? AdvAreaCode { get; set; }
        public int? AdvCustomerCategoryCode { get; set; }
        public int? AdvBranchCode { get; set; }
        public Branch Branch { get; set; }
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUser PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Product Product { get; set; }
        public Guid AdvProduct { get; set; }
        public Guid AdvProductModel { get; set; }
        public string SearchTerm { get; set; }
        public int? AdvReportType { get; set; }
        public Employee Employee { get; set; }
        public List<EstimateDetailReport> EstimateReportList { get; set; }
        public string EstNo { get; set; }
        public string DateFilter { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public bool CostPriceHasAccess { get; set; }
        public decimal TotalCostAmount { get; set; }
        public decimal TotalSellingAmount { get; set; }
    }
}
