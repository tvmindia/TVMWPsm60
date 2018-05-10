﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class SaleInvoice
    {
        public Guid ID { get; set; }
        public string SaleInvNo { get; set; }
        public string SaleInvRefNo { get; set; }
        public DateTime SaleInvDate { get; set; }
        public Guid QuoteID { get; set; }
        public Guid SaleOrderID { get; set; }
        public Guid CustomerID { get; set; }
        public string MailingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public int DocumentStatusCode { get; set; }
        public DateTime ExpectedDelvDate { get; set; }
        public bool CashInvoiceYN { get; set; }
        public Guid PreparedBy { get; set; }
        public string PurchaseOrdNo { get; set; }
        public DateTime PurchaseOrdDate { get; set; }
        public int BillSeriesCode { get; set; }
        public bool EmailSentYN { get; set; }
        public Guid LatestApprovalID { get; set; }
        public int LatestApprovalStatus { get; set; }
        public bool IsFinalApproved { get; set; }
        public string EmailSentTo { get; set; }
        public string PrintRemark { get; set; }
        public string GeneralNotes { get; set; }
        public Guid DocumentOwnerID { get; set; }
        public int BranchCode { get; set; }
        public decimal Discount { get; set; }
        public decimal AdvanceAmount { get; set; }

        //additional Fields
        public string DetailXML { get; set; }
        public Guid hdnFileID { get; set; }
        public string SaleInvDateFormatted { get; set; }
        public bool IsUpdate { get; set; }
        public int FilteredCount { get; set; }
        public int TotalCount { get; set; }
        public string ExpectedDelvDateFormatted { get; set; }
        public string PurchaseOrdDateFormatted { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public Customer Customer { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public List<SaleInvoiceDetail> SaleInvoiceDetailList { get; set; }
    }
    public class SaleInvoiceAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
    public class SaleInvoiceDetail
    {
        public Guid ID { get; set; }
        public Guid SaleInvID { get; set; }
        public Guid ProductID { get; set; }
        public Guid ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal Qty { get; set; }
        public int UnitCode { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public int TaxTypeCode { get; set; }
        public decimal CGSTPerc { get; set; }
        public decimal SGSTPerc { get; set; }
        public decimal IGSTPerc { get; set; }
        public decimal CessPerc { get; set; }
        public decimal CessAmt { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public Product Product { get; set; }
        public ProductModel ProductModel { get; set;}
        public Unit Unit { get; set; }
    }
    public class SaleInvoiceOtherCharge
    {
        public Guid ID { get; set; }
        public Guid SaleInvID { get; set; }
        public int OtherChargeCode { get; set; }
        public decimal ChargeAmount { get; set; }
        public int TaxTypeCode { get; set; }
        public decimal CGSTPerc { get; set; }
        public decimal SGSTPerc { get; set; }
        public decimal IGSTPerc { get; set; }
        public decimal AddlTaxPec { get; set; }
        public decimal AddlTaxAmt { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
    }
}