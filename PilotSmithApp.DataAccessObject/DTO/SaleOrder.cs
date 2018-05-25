﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class SaleOrder
    {
        public Guid ID { get; set; }
        public string SaleOrderNo { get; set; }
        public string SaleOrderRefNo { get; set; }
        public DateTime SaleOrderDate { get; set; }
        public Guid? QuoteID { get; set; }
        public Guid? EnquiryID { get; set; }
        public Guid? CustomerID { get; set; }
        public string MailingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public int? DocumentStatusCode { get; set; }
        public DateTime ExpectedDelvDate { get; set; }
        public int? ReferredByCode { get; set; }
        public Guid? PreparedBy { get; set; }
        public string PurchaseOrdNo { get; set; }
        public DateTime PurchaseOrdDate { get; set; }
        public int? BankCode { get; set; }
        public int? CarrierCode { get; set; }
        public bool? EmailSentYN { get; set; }
        public Guid LatestApprovalID { get; set; }
        public int? LatestApprovalStatus { get; set; }
        public bool? IsFinalApproved { get; set; }
        public string EmailSentTo { get; set; }
        public string TermReferenceNo { get; set; }
        public string PrintRemark { get; set; }
        public string GeneralNotes { get; set; }
        public Guid? DocumentOwnerID { get; set; }
        public int? BranchCode { get; set; }
        public decimal? Discount { get; set; }
        public decimal? AdvanceAmount { get; set; }

        //Aditional Fields
        public string MailContant { get; set; }
        public string DocumentType { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public List<SaleOrderDetail> SaleOrderDetailList { get; set; }
        public string DetailXML { get; set; }
        public Guid hdnFileID { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string[] DocumentOwners { get; set; }
        public string DocumentOwner { get; set; }
        public string SaleOrderDateFormatted { get; set; }
        public string ExpectedDelvDateFormatted { get; set; }
        public string PurchaseOrdDateFormatted { get; set; }
        public Customer Customer { get; set; }
        public string LatestApprovalStatusDescription { get; set; }
        public Branch Branch { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public ReferencePerson ReferencePerson { get; set; }
    }

    public class SaleOrderAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }

    public class SaleOrderDetail
    {
        public Guid ID { get; set; }
        public Guid SaleOrderID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal? Qty { get; set; }
        public int? UnitCode { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Discount { get; set; }
        public int? TaxTypeCode { get; set; }
        public decimal? CGSTPerc { get; set; }
        public decimal? SGSTPerc { get; set; }
        public decimal? IGSTPerc { get; set; }
        public decimal? CessPerc { get; set; }
        public decimal? CessAmt { get; set; }
        public Guid SpecTag { get; set; }

        public PSASysCommon PSASysCommon { get; set; }
        public Product Product { get; set; }
        public ProductModel ProductModel { get; set; }
        public Unit Unit { get; set; }
    }
    public class SaleOrderOtherCharge
    {
        public Guid ID { get; set; }
        public Guid SaleOrderID { get; set; }
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
