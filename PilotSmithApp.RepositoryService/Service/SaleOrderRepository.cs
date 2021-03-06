﻿using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Service
{
    public class SaleOrderRepository : ISaleOrderRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public SaleOrderRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region Get All SaleOrder
        public List<SaleOrder> GetAllSaleOrder(SaleOrderAdvanceSearch saleOrderAdvanceSearch)
        {
            List<SaleOrder> saleOrderList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[GetAllSaleOrder]";
                        if (string.IsNullOrEmpty(saleOrderAdvanceSearch.SearchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchValue", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = saleOrderAdvanceSearch.SearchTerm;
                        }
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = saleOrderAdvanceSearch.DataTablePaging.Start;
                        if (saleOrderAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = saleOrderAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = saleOrderAdvanceSearch.AdvFromDate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = saleOrderAdvanceSearch.AdvToDate;
                        if (saleOrderAdvanceSearch.AdvCustomerID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = saleOrderAdvanceSearch.AdvCustomerID;
                        cmd.Parameters.Add("@AreaCode", SqlDbType.Int).Value = saleOrderAdvanceSearch.AdvAreaCode;
                        cmd.Parameters.Add("@ReferencePersonCode", SqlDbType.Int).Value = saleOrderAdvanceSearch.AdvReferencePersonCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = saleOrderAdvanceSearch.AdvBranchCode;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = saleOrderAdvanceSearch.AdvDocumentStatusCode;
                        if (saleOrderAdvanceSearch.AdvDocumentOwnerID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@DocumentOwnerID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = saleOrderAdvanceSearch.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@ApprovalStatusCode", SqlDbType.Int).Value = saleOrderAdvanceSearch.AdvApprovalStatusCode;
                        cmd.Parameters.Add("@EmailSentYN", SqlDbType.NVarChar, 5).Value = saleOrderAdvanceSearch.AdvEmailSentStatus;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                saleOrderList = new List<SaleOrder>();
                                while (sdr.Read())
                                {
                                    SaleOrder saleOrder = new SaleOrder();
                                    {
                                        saleOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : saleOrder.ID);
                                        saleOrder.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? sdr["SaleOrderNo"].ToString() : saleOrder.SaleOrderNo);
                                        saleOrder.SaleOrderDate = (sdr["SaleOrderDate"].ToString() != "" ? DateTime.Parse(sdr["SaleOrderDate"].ToString()) : saleOrder.SaleOrderDate);
                                        saleOrder.SaleOrderDateFormatted = (sdr["SaleOrderDate"].ToString() != "" ? DateTime.Parse(sdr["SaleOrderDate"].ToString()).ToString(_settings.DateFormat) : saleOrder.SaleOrderDateFormatted);
                                        //quotation.RequirementSpec = (sdr["RequirementSpec"].ToString() != "" ? sdr["RequirementSpec"].ToString() : quotation.RequirementSpec);
                                        saleOrder.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : saleOrder.CustomerID);
                                        saleOrder.Customer = new Customer();
                                        saleOrder.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : saleOrder.Customer.ID);
                                        saleOrder.Customer.CompanyName = (sdr["CustomerCompanyName"].ToString() != "" ? sdr["CustomerCompanyName"].ToString() : saleOrder.Customer.CompanyName);
                                        saleOrder.Customer.ContactPerson = (sdr["CustomerContactPerson"].ToString() != "" ? sdr["CustomerContactPerson"].ToString() : saleOrder.Customer.ContactPerson);
                                        saleOrder.Customer.Mobile = (sdr["CustomerMobile"].ToString() != "" ? sdr["CustomerMobile"].ToString() : saleOrder.Customer.Mobile);
                                        saleOrder.ExpectedDelvDate = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()) : saleOrder.ExpectedDelvDate);
                                        saleOrder.ExpectedDelvDateFormatted = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()).ToString(_settings.DateFormat) : saleOrder.ExpectedDelvDateFormatted);
                                        saleOrder.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : saleOrder.DocumentStatusCode);
                                        saleOrder.DocumentStatus = new DocumentStatus();
                                        saleOrder.DocumentStatus.Code = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : saleOrder.DocumentStatus.Code);
                                        saleOrder.DocumentStatus.Description = (sdr["DocumentStatusDescription"].ToString() != "" ? (sdr["DocumentStatusDescription"].ToString()) : saleOrder.DocumentStatus.Description);
                                        saleOrder.ReferredByCode = (sdr["ReferredByCode"].ToString() != "" ? int.Parse(sdr["ReferredByCode"].ToString()) : saleOrder.ReferredByCode);
                                        saleOrder.ReferencePerson = new ReferencePerson();
                                        saleOrder.ReferencePerson.Code = (sdr["ReferredByCode"].ToString() != "" ? int.Parse(sdr["ReferredByCode"].ToString()) : saleOrder.ReferencePerson.Code);
                                        saleOrder.ReferencePerson.Name = (sdr["ReferencePersonName"].ToString() != "" ? (sdr["ReferencePersonName"].ToString()) : saleOrder.ReferencePerson.Name);
                                        //quotation.ResponsiblePersonID = (sdr["ReferencePersonName"].ToString() != "" ? Guid.Parse(sdr["ResponsiblePersonID"].ToString()) : quotation.ResponsiblePersonID);
                                        saleOrder.PreparedBy = (sdr["PreparedBy"].ToString() != "" ? Guid.Parse(sdr["PreparedBy"].ToString()) : saleOrder.PreparedBy);
                                        //quotation.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : quotation.GeneralNotes);
                                        saleOrder.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : saleOrder.DocumentOwnerID);
                                        saleOrder.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : saleOrder.BranchCode);
                                        saleOrder.Branch = new Branch();
                                        saleOrder.Branch.Description = (sdr["BranchDescription"].ToString() != "" ? (sdr["BranchDescription"].ToString()) : saleOrder.Branch.Description);
                                        saleOrder.Area = new Area();
                                        saleOrder.Area.Description = (sdr["Area"].ToString() != "" ? (sdr["Area"].ToString()) : saleOrder.Area.Description);
                                        saleOrder.PSAUser = new PSAUser();
                                        saleOrder.PSAUser.LoginName = (sdr["DocumentOwner"].ToString() != "" ? (sdr["DocumentOwner"].ToString()) : saleOrder.PSAUser.LoginName);
                                        saleOrder.ApprovalStatus = new ApprovalStatus();
                                        saleOrder.ApprovalStatus.Code = (sdr["LatestApprovalStatus"].ToString() != "" ? int.Parse(sdr["LatestApprovalStatus"].ToString()) : saleOrder.ApprovalStatus.Code);
                                        saleOrder.ApprovalStatus.Description = (sdr["ApprovalStatus"].ToString() != "" ? (sdr["ApprovalStatus"].ToString()) : saleOrder.ApprovalStatus.Description);
                                        saleOrder.EnquiryID = (sdr["EnquiryID"].ToString() != "" ? Guid.Parse(sdr["EnquiryID"].ToString()) : saleOrder.EnquiryID);
                                        saleOrder.QuoteID = (sdr["QuoteID"].ToString() != "" ? Guid.Parse(sdr["QuoteID"].ToString()) : saleOrder.QuoteID);
                                        saleOrder.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : saleOrder.FilteredCount);
                                        saleOrder.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : saleOrder.TotalCount);
                                        saleOrder.EmailSentYN = (sdr["EmailSentYN"].ToString() != "" ? bool.Parse(sdr["EmailSentYN"].ToString()) : saleOrder.EmailSentYN);
                                        saleOrder.Quotation = new Quotation();
                                        saleOrder.Quotation.QuoteNo= (sdr["QuoteNo"].ToString() != "" ? (sdr["QuoteNo"].ToString()) : saleOrder.Quotation.QuoteNo);
                                        saleOrder.Enquiry = new Enquiry();
                                        saleOrder.Enquiry.EnquiryNo= (sdr["EnquiryNo"].ToString() != "" ? (sdr["EnquiryNo"].ToString()) : saleOrder.Enquiry.EnquiryNo);
                                        saleOrder.CopyFrom = (sdr["CopyFrom"].ToString() != "" ? Guid.Parse(sdr["CopyFrom"].ToString()) : saleOrder.CopyFrom);
                                    }
                                    saleOrderList.Add(saleOrder);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return saleOrderList;
        }
        #endregion Get All SaleOrder

        public DataSet GetCustomerHistory(Guid customerID)
        {
            using (SqlConnection con = _databaseFactory.GetDBConnection())
            {
                con.Open();
                DataSet ds = new DataSet();
                try
                {
                    SqlCommand cmd = new SqlCommand("[PSA].[GetCustomerHistory]", con);
                    cmd.CommandText = "[PSA].[GetCustomerHistory]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = customerID;
                    SqlDataReader sdr = cmd.ExecuteReader();
                    ds.Tables.Add("TableSaleOrder");
                    ds.Tables[0].Load(sdr);
                    if (ds.Tables.Count > 0)
                    {
                        return ds;
                    }
                    else
                    {
                        return ds = null;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #region GetSaleOrderForSelectListOnDemand
        public List<SaleOrder> GetSaleOrderForSelectListOnDemand(string searchTerm)
        {
            List<SaleOrder> productionOrderList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[GetSaleOrderForSelectListOnDemand]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (string.IsNullOrEmpty(searchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchTerm", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchTerm", SqlDbType.VarChar, 250).Value = searchTerm;
                        }
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productionOrderList = new List<SaleOrder>();
                                while (sdr.Read())
                                {
                                    SaleOrder productionOrder = new SaleOrder();
                                    {
                                        productionOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productionOrder.ID);
                                        productionOrder.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? sdr["SaleOrderNo"].ToString() : productionOrder.SaleOrderNo);
                                    }
                                    productionOrderList.Add(productionOrder);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return productionOrderList;
        }
        #endregion GetSaleOrderForSelectListOnDemand
        #region GetSaleOrderForSelectList
        public List<SaleOrder> GetSaleOrderForSelectList(Guid? id)
        {
            List<SaleOrder> productionOrderList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[GetSaleOrderForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (id == null)
                        {
                            cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        }
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productionOrderList = new List<SaleOrder>();
                                while (sdr.Read())
                                {
                                    SaleOrder productionOrder = new SaleOrder();
                                    {
                                        productionOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productionOrder.ID);
                                        productionOrder.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? sdr["SaleOrderNo"].ToString() : productionOrder.SaleOrderNo);
                                    }
                                    productionOrderList.Add(productionOrder);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return productionOrderList;
        }
        #endregion GetSaleOrderForSelectList
        #region GetSaleOrder
        public SaleOrder GetSaleOrder(Guid id)
        {
            SaleOrder saleOrder = new SaleOrder();
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[GetSaleOrder]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    saleOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : saleOrder.ID);
                                    saleOrder.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? sdr["SaleOrderNo"].ToString() : saleOrder.SaleOrderNo);
                                    saleOrder.SaleOrderRefNo = (sdr["SaleOrderRefNo"].ToString() != "" ? sdr["SaleOrderRefNo"].ToString() : saleOrder.SaleOrderRefNo);
                                    saleOrder.SaleOrderDate = (sdr["SaleOrderDate"].ToString() != "" ? DateTime.Parse(sdr["SaleOrderDate"].ToString()) : saleOrder.SaleOrderDate);
                                    saleOrder.SaleOrderDateFormatted = (sdr["SaleOrderDate"].ToString() != "" ? DateTime.Parse(sdr["SaleOrderDate"].ToString()).ToString(_settings.DateFormat) : saleOrder.SaleOrderDateFormatted);
                                    saleOrder.QuoteID = (sdr["QuoteID"].ToString() != "" ? Guid.Parse(sdr["QuoteID"].ToString()) : saleOrder.QuoteID);
                                    saleOrder.EnquiryID = (sdr["EnquiryID"].ToString() != "" ? Guid.Parse(sdr["EnquiryID"].ToString()) : saleOrder.EnquiryID);
                                    saleOrder.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : saleOrder.CustomerID);
                                    saleOrder.Customer = new Customer();
                                    saleOrder.Customer.CompanyName = (sdr["CustomerCompanyName"].ToString() != "" ? (sdr["CustomerCompanyName"].ToString()) : saleOrder.Customer.CompanyName);
                                    saleOrder.Customer.ID= (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : saleOrder.Customer.ID);
                                    saleOrder.Customer.AadharNo= (sdr["AadharNo"].ToString() != "" ? (sdr["AadharNo"].ToString()) : saleOrder.Customer.AadharNo);
                                    saleOrder.Customer.PANNO= (sdr["PANNO"].ToString() != "" ? (sdr["PANNO"].ToString()) : saleOrder.Customer.PANNO);
                                    saleOrder.Customer.TaxRegNo= (sdr["TaxRegNo"].ToString() != "" ? (sdr["TaxRegNo"].ToString()) : saleOrder.Customer.TaxRegNo);
                                    saleOrder.MailingAddress = (sdr["MailingAddress"].ToString() != "" ? sdr["MailingAddress"].ToString() : saleOrder.MailingAddress);
                                    saleOrder.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : saleOrder.ShippingAddress);
                                    saleOrder.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : saleOrder.DocumentStatusCode);
                                    saleOrder.ExpectedDelvDate = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()) : saleOrder.ExpectedDelvDate);
                                    saleOrder.ExpectedDelvDateFormatted = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()).ToString(_settings.DateFormat) : saleOrder.ExpectedDelvDateFormatted);
                                    saleOrder.ReferredByCode = (sdr["ReferredByCode"].ToString() != "" ? int.Parse(sdr["ReferredByCode"].ToString()) : saleOrder.ReferredByCode);
                                    saleOrder.PreparedBy = (sdr["PreparedBy"].ToString() != "" ? Guid.Parse(sdr["PreparedBy"].ToString()) : saleOrder.PreparedBy);
                                    saleOrder.PurchaseOrdNo = (sdr["PurchaseOrdNo"].ToString() != "" ? sdr["PurchaseOrdNo"].ToString() : saleOrder.PurchaseOrdNo);
                                    saleOrder.PurchaseOrdDate = (sdr["PurchaseOrdDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrdDate"].ToString()) : saleOrder.PurchaseOrdDate);
                                    saleOrder.PurchaseOrdDateFormatted = (sdr["PurchaseOrdDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrdDate"].ToString()).ToString(_settings.DateFormat) : saleOrder.PurchaseOrdDateFormatted);
                                    saleOrder.BankCode = (sdr["BankCode"].ToString() != "" ? int.Parse(sdr["BankCode"].ToString()) : saleOrder.BankCode);
                                    saleOrder.Bank = new Bank();
                                    saleOrder.Bank.Name= (sdr["Bank"].ToString() != "" ? sdr["Bank"].ToString() : saleOrder.Bank.Name);
                                    saleOrder.CarrierCode = (sdr["CarrierCode"].ToString() != "" ? int.Parse(sdr["CarrierCode"].ToString()) : saleOrder.CarrierCode);
                                    saleOrder.EmailSentYN = (sdr["EmailSentYN"].ToString() != "" ? bool.Parse(sdr["EmailSentYN"].ToString()) : saleOrder.EmailSentYN);
                                    saleOrder.LatestApprovalID = (sdr["LatestApprovalID"].ToString() != "" ? Guid.Parse(sdr["LatestApprovalID"].ToString()) : saleOrder.LatestApprovalID);
                                    saleOrder.LatestApprovalStatus = (sdr["LatestApprovalStatus"].ToString() != "" ? int.Parse(sdr["LatestApprovalStatus"].ToString()) : saleOrder.LatestApprovalStatus);
                                    saleOrder.LatestApprovalStatusDescription = (sdr["ApprovalDescription"].ToString() != "" ? (sdr["ApprovalDescription"].ToString()) : saleOrder.LatestApprovalStatusDescription);
                                    saleOrder.IsFinalApproved = (sdr["IsFinalApproved"].ToString() != "" ? bool.Parse(sdr["IsFinalApproved"].ToString()) : saleOrder.IsFinalApproved);
                                    saleOrder.EmailSentTo = (sdr["EmailSentTo"].ToString() != "" ? sdr["EmailSentTo"].ToString() : saleOrder.EmailSentTo);
                                    saleOrder.Cc = (sdr["Cc"].ToString() != "" ? (sdr["Cc"].ToString()) : saleOrder.Cc);
                                    saleOrder.Bcc = (sdr["Bcc"].ToString() != "" ? (sdr["Bcc"].ToString()) : saleOrder.Bcc);
                                    saleOrder.Subject = (sdr["Subject"].ToString() != "" ? (sdr["Subject"].ToString()) : saleOrder.Subject);
                                    saleOrder.TermReferenceNo = (sdr["TermReferenceNo"].ToString() != "" ? sdr["TermReferenceNo"].ToString() : saleOrder.TermReferenceNo);
                                    saleOrder.PrintRemark = (sdr["PrintRemark"].ToString() != "" ? sdr["PrintRemark"].ToString() : saleOrder.PrintRemark);
                                    saleOrder.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : saleOrder.GeneralNotes);
                                    saleOrder.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : saleOrder.BranchCode);
                                    saleOrder.Branch = new Branch();
                                    saleOrder.Branch.Description = (sdr["Branch"].ToString() != "" ? sdr["Branch"].ToString() : saleOrder.Branch.Description);
                                    saleOrder.Branch.Code= (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : saleOrder.Branch.Code);
                                    saleOrder.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : saleOrder.DocumentOwnerID);
                                    saleOrder.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : saleOrder.Discount);
                                    saleOrder.AdvanceAmount = (sdr["AdvanceAmount"].ToString() != "" ? decimal.Parse(sdr["AdvanceAmount"].ToString()) : saleOrder.AdvanceAmount);
                                    saleOrder.DocumentStatus = new DocumentStatus()
                                    {
                                        Description = (sdr["DocumentStatusDescription"].ToString() != "" ? (sdr["DocumentStatusDescription"].ToString()) : saleOrder.DocumentStatus.Description),
                                    };
                                    saleOrder.DocumentOwners = (sdr["DocumentOwners"].ToString() != "" ? (sdr["DocumentOwners"].ToString()).Split(',') : saleOrder.DocumentOwners);
                                    saleOrder.DocumentOwner = (sdr["DocumentOwner"].ToString() != "" ? (sdr["DocumentOwner"].ToString()) : saleOrder.DocumentOwner);
                                    //string mailfooter = (sdr["MailBodyFooter"].ToString() != "" ? (sdr["MailBodyFooter"].ToString()) : saleOrder.MailBodyFooter);
                                    //saleOrder.MailBodyFooter = mailfooter.Replace("\n", "<br />");
                                    //string mailheader = (sdr["MailBodyHeader"].ToString() != "" ? (sdr["MailBodyHeader"].ToString()) : saleOrder.MailBodyHeader);
                                    //saleOrder.MailBodyHeader = mailheader.Replace("\n", "<br />");
                                    saleOrder.MailBodyHeader = (sdr["MailBodyHeader"].ToString() != "" ? sdr["MailBodyHeader"].ToString() : saleOrder.MailBodyHeader);
                                    saleOrder.MailBodyFooter = (sdr["MailBodyFooter"].ToString() != "" ? sdr["MailBodyFooter"].ToString() : saleOrder.MailBodyFooter);
                                    string mailfrom = (sdr["MailFromAddress"].ToString() != "" ? (sdr["MailFromAddress"].ToString()) : saleOrder.MailFrom);
                                    saleOrder.MailFrom = mailfrom.Replace("\n", "<br />");
                                    saleOrder.ApproverLevel= (sdr["ApproverLevel"].ToString() != "" ? int.Parse(sdr["ApproverLevel"].ToString()) : saleOrder.ApproverLevel);
                                    saleOrder.Quotation = new Quotation();
                                    saleOrder.Quotation.QuoteNo = (sdr["QuoteNo"].ToString() != "" ? sdr["QuoteNo"].ToString() : saleOrder.Quotation.QuoteNo);
                                    saleOrder.Enquiry = new Enquiry();
                                    saleOrder.Enquiry.EnquiryNo = (sdr["EnquiryNo"].ToString() != "" ? sdr["EnquiryNo"].ToString() : saleOrder.Enquiry.EnquiryNo);
                                    saleOrder.CopyFrom = (sdr["CopyFrom"].ToString() != "" ? Guid.Parse(sdr["CopyFrom"].ToString()) : saleOrder.CopyFrom);
                                    saleOrder.CopySaleOrderNo = (sdr["CopySaleOrderNo"].ToString() != "" ? sdr["CopySaleOrderNo"].ToString() : saleOrder.CopySaleOrderNo);

                                    saleOrder.CurrencyCode = (sdr["CurrencyCode"].ToString() != "" ? sdr["CurrencyCode"].ToString() : saleOrder.CurrencyCode);
                                    saleOrder.CurrencyRate = (sdr["CurrencyRate"].ToString() != "" ? Decimal.Parse(sdr["CurrencyRate"].ToString()) : saleOrder.CurrencyRate);
                                    saleOrder.IsFileExist = (sdr["IsFileExist"].ToString() != "" ? int.Parse(sdr["IsFileExist"].ToString()) : saleOrder.IsFileExist);
                                    saleOrder.Currency = new Currency();
                                    saleOrder.Currency.Description = (sdr["CurrencyDescription"].ToString() != "" ? sdr["CurrencyDescription"].ToString() : saleOrder.Currency.Description);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return saleOrder;
        }
        #endregion GetSaleOrder
        #region GetSaleOrderDetailListBySaleOrderID
        public List<SaleOrderDetail> GetSaleOrderDetailListBySaleOrderID(Guid saleOrderID, bool isCopy)
        {
            List<SaleOrderDetail> saleOrderDetailList = new List<SaleOrderDetail>();
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[GetSaleOrderDetailListBySaleOrderID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SaleOrderID", SqlDbType.UniqueIdentifier).Value = saleOrderID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    SaleOrderDetail saleOrderDetail = new SaleOrderDetail();
                                    {
                                        if (isCopy)
                                        {
                                            saleOrderDetail.ID = Guid.Empty;
                                        }
                                        else
                                        {
                                            saleOrderDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : saleOrderDetail.ID);
                                        }
                                        saleOrderDetail.SaleOrderID = (sdr["SaleOrderID"].ToString() != "" ? Guid.Parse(sdr["SaleOrderID"].ToString()) : saleOrderDetail.SaleOrderID);
                                        saleOrderDetail.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : saleOrderDetail.Qty);
                                        saleOrderDetail.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString().Replace("$n$", "\n") : saleOrderDetail.ProductSpec);
                                        saleOrderDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : saleOrderDetail.ProductID);
                                        saleOrderDetail.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : saleOrderDetail.ProductModelID);
                                        saleOrderDetail.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : saleOrderDetail.UnitCode);
                                        saleOrderDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : saleOrderDetail.Rate);
                                        saleOrderDetail.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : saleOrderDetail.Discount);
                                        saleOrderDetail.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : saleOrderDetail.TaxTypeCode);
                                        saleOrderDetail.CGSTPerc = (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : saleOrderDetail.CGSTPerc);
                                        saleOrderDetail.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : saleOrderDetail.SGSTPerc);
                                        saleOrderDetail.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : saleOrderDetail.IGSTPerc);
                                        saleOrderDetail.CessPerc = (sdr["CessPerc"].ToString() != "" ? decimal.Parse(sdr["CessPerc"].ToString()) : saleOrderDetail.CessPerc);
                                        saleOrderDetail.CessAmt = (sdr["CessAmt"].ToString() != "" ? decimal.Parse(sdr["CessAmt"].ToString()) : saleOrderDetail.CessAmt);
                                        saleOrderDetail.Product = new Product();
                                        saleOrderDetail.Product.Code = (sdr["ProductCode"].ToString() != "" ? sdr["ProductCode"].ToString() : saleOrderDetail.Product.Code);
                                        saleOrderDetail.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : saleOrderDetail.Product.Name);
                                        saleOrderDetail.Product.HSNCode= (sdr["HSNCode"].ToString() != "" ? sdr["HSNCode"].ToString() : saleOrderDetail.Product.HSNCode);
                                        saleOrderDetail.ProductModel = new ProductModel();
                                        saleOrderDetail.ProductModel.Name = (sdr["ModelName"].ToString() != "" ? sdr["ModelName"].ToString() : saleOrderDetail.ProductModel.Name);
                                        saleOrderDetail.Unit = new Unit();
                                        saleOrderDetail.Unit.Description = (sdr["Unit"].ToString() != "" ? sdr["Unit"].ToString() : saleOrderDetail.Unit.Description);
                                        saleOrderDetail.SpecTag = (sdr["SpecTag"].ToString() != "" ? Guid.Parse(sdr["SpecTag"].ToString()) : saleOrderDetail.SpecTag);
                                        saleOrderDetail.TaxType = new TaxType();
                                        saleOrderDetail.TaxType.Code = (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : saleOrderDetail.TaxType.Code);
                                        saleOrderDetail.TaxType.ValueText = (sdr["TaxTypeText"].ToString() != "" ? (sdr["TaxTypeText"].ToString()) : saleOrderDetail.TaxType.ValueText);
                                       // saleOrderDetail.PrevProduceQty = (sdr["PrevProduceQty"].ToString() != "" ? decimal.Parse(sdr["PrevProduceQty"].ToString()) : saleOrderDetail.PrevProduceQty);
                                        saleOrderDetail.PrevProdOrderQty = (sdr["PrevProdOrderQty"].ToString() != "" ? decimal.Parse(sdr["PrevProdOrderQty"].ToString()) : saleOrderDetail.PrevProdOrderQty);
                                        saleOrderDetail.PrevDelQty= (sdr["PrevDelQty"].ToString() != "" ? decimal.Parse(sdr["PrevDelQty"].ToString()) : saleOrderDetail.PrevDelQty);
                                        //saleOrderDetail.DelvQty= (sdr["DelvQty"].ToString() != "" ? decimal.Parse(sdr["DelvQty"].ToString()) : saleOrderDetail.DelvQty);
                                    }
                                    saleOrderDetailList.Add(saleOrderDetail);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return saleOrderDetailList;
        }


        #endregion GetSaleOrderDetailListBySaleOrderID
        #region Insert Update SaleOrder
        public object InsertUpdateSaleOrder(SaleOrder saleOrder)
        {
            SqlParameter outputStatus, outputID, outputSaleOrderNo, outputCopyFromID = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[InsertUpdateSaleOrder]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = saleOrder.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = saleOrder.ID;
                        cmd.Parameters.Add("@CopyFromID", SqlDbType.UniqueIdentifier).Value = saleOrder.CopyFrom;
                        cmd.Parameters.Add("@CopySaleOrderNo", SqlDbType.VarChar, 20).Value = saleOrder.CopySaleOrderNo;
                        cmd.Parameters.Add("@SaleOrderRefNo", SqlDbType.VarChar, 20).Value = saleOrder.SaleOrderRefNo;
                        cmd.Parameters.Add("@SaleOrderNo", SqlDbType.VarChar, 20).Value = saleOrder.SaleOrderNo;
                        cmd.Parameters.Add("@SaleOrderDate", SqlDbType.DateTime).Value = saleOrder.SaleOrderDateFormatted;
                        cmd.Parameters.Add("@QuoteID", SqlDbType.UniqueIdentifier).Value = saleOrder.QuoteID;
                        cmd.Parameters.Add("@EnquiryID", SqlDbType.UniqueIdentifier).Value = saleOrder.EnquiryID;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = saleOrder.CustomerID;
                        cmd.Parameters.Add("@MailingAddress", SqlDbType.NVarChar, -1).Value = saleOrder.MailingAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar, -1).Value = saleOrder.ShippingAddress;
                      //  cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = saleOrder.DocumentStatusCode;
                        cmd.Parameters.Add("@ExpectedDelvDate", SqlDbType.DateTime).Value = saleOrder.ExpectedDelvDateFormatted;
                        cmd.Parameters.Add("@BankCode", SqlDbType.Int).Value = saleOrder.BankCode;
                        cmd.Parameters.Add("@ReferredByCode", SqlDbType.Int).Value = saleOrder.ReferredByCode;
                        cmd.Parameters.Add("@PreparedBy", SqlDbType.UniqueIdentifier).Value = saleOrder.PreparedBy;
                        cmd.Parameters.Add("@PurchaseOrdNo", SqlDbType.VarChar, 20).Value = saleOrder.PurchaseOrdNo;
                        cmd.Parameters.Add("@PurchaseOrdDate", SqlDbType.DateTime).Value = saleOrder.PurchaseOrdDateFormatted;
                        //cmd.Parameters.Add("@MailBodyHeader", SqlDbType.NVarChar, -1).Value = saleOrder.MailBodyHeader;
                        //cmd.Parameters.Add("@MailBodyFooter", SqlDbType.NVarChar, -1).Value = saleOrder.MailBodyFooter;
                        //cmd.Parameters.Add("@EmailSentYN", SqlDbType.Bit).Value = saleOrder.EmailSentYN;
                        cmd.Parameters.Add("@LatestApprovalID", SqlDbType.UniqueIdentifier).Value = saleOrder.LatestApprovalID;
                        cmd.Parameters.Add("@IsFinalApproved", SqlDbType.Bit).Value = saleOrder.IsFinalApproved;
                        //cmd.Parameters.Add("@EmailSentTo", SqlDbType.NVarChar, -1).Value = saleOrder.EmailSentTo;
                        cmd.Parameters.Add("@TermReferenceNo", SqlDbType.VarChar, 25).Value = saleOrder.TermReferenceNo;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = saleOrder.DetailXML;
                        cmd.Parameters.Add("@OtherChargeDetailXML", SqlDbType.Xml).Value = saleOrder.OtherChargeDetailXML;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = saleOrder.hdnFileID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = saleOrder.GeneralNotes;
                        cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = saleOrder.DocumentOwnerID;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = saleOrder.BranchCode;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = saleOrder.Discount;
                        cmd.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = saleOrder.AdvanceAmount;
                        //-----------------------//
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = saleOrder.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = saleOrder.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = saleOrder.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = saleOrder.PSASysCommon.UpdatedDate;
                        cmd.Parameters.Add("@CurrencyCode", SqlDbType.VarChar).Value = saleOrder.CurrencyCode;
                        cmd.Parameters.Add("@CurrencyRate", SqlDbType.Decimal).Value = saleOrder.CurrencyRate;
                        outputStatus = cmd.Parameters.Add("@StatusOut", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        outputSaleOrderNo = cmd.Parameters.Add("@SaleOrderNoOut", SqlDbType.VarChar, 20);
                        outputSaleOrderNo.Direction = ParameterDirection.Output;
                        outputCopyFromID = cmd.Parameters.Add("@CopyFromIDOut", SqlDbType.UniqueIdentifier);
                        outputCopyFromID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        saleOrder.ID = Guid.Parse(outputID.Value.ToString());
                        saleOrder.SaleOrderNo = outputSaleOrderNo.Value.ToString();
                        return new
                        {
                            ID = saleOrder.ID,
                            SaleOrderNo = saleOrder.SaleOrderNo,
                            QuoteID = saleOrder.QuoteID,
                            EnquiryID = saleOrder.EnquiryID,
                            Status = outputStatus.Value.ToString(),
                            CopyFrom = Guid.Parse(outputCopyFromID.Value.ToString()),
                            Message = saleOrder.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
                        };
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                ID = saleOrder.ID,
                SaleOrderNo = saleOrder.SaleOrderNo,
                QuoteID = saleOrder.QuoteID,
                EnquiryID = saleOrder.EnquiryID,
                Status = outputStatus.Value.ToString(),
                CopyFrom = Guid.Parse(outputCopyFromID.Value.ToString()),
                Message = saleOrder.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion Insert Update SaleOrder
        #region Update SaleOrder Email Info
        public object UpdateSaleOrderEmailInfo(SaleOrder saleOrder)
        {
            SqlParameter outputStatus = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[UpdateSaleOrderEmailInfo]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = saleOrder.ID;
                        //added null checking to header and footer fields, to allow header and footer to be empty
                        cmd.Parameters.Add("@MailBodyHeader", SqlDbType.NVarChar, -1).Value = saleOrder.MailBodyHeader!=null? saleOrder.MailBodyHeader:" ";
                        cmd.Parameters.Add("@MailBodyFooter", SqlDbType.NVarChar, -1).Value = saleOrder.MailBodyFooter!=null? saleOrder.MailBodyFooter:" ";
                        cmd.Parameters.Add("@EmailSentYN", SqlDbType.Bit).Value = saleOrder.EmailSentYN;
                        cmd.Parameters.Add("@EmailSentTo", SqlDbType.NVarChar, -1).Value = saleOrder.EmailSentTo;
                        cmd.Parameters.Add("@IsPrint", SqlDbType.Bit).Value = saleOrder.PrintFlag;
                        cmd.Parameters.Add("@Cc", SqlDbType.NVarChar, -1).Value = saleOrder.Cc;
                        cmd.Parameters.Add("@Bcc", SqlDbType.NVarChar, -1).Value = saleOrder.Bcc;
                        cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, -1).Value = saleOrder.Subject;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = saleOrder.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = saleOrder.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@StatusOut", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message = saleOrder.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
                        };
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = saleOrder.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion Update SaleOrder Email Info
        #region Delete SaleOrder
        public object DeleteSaleOrder(Guid id)
        {
            SqlParameter outputStatus = null;
            try
            {

                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[DeleteSaleOrder]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(_appConstant.DeleteFailure);

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConstant.DeleteSuccess
            };
        }
        #endregion Delete SaleOrder
        #region Delete SaleOrder Detail
        public object DeleteSaleOrderDetail(Guid id, string CreatedBy, DateTime CreatedDate)
        {
            SqlParameter outputStatus = null;
            try
            {

                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[DeleteSaleOrderDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(_appConstant.DeleteFailure);

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConstant.DeleteSuccess
            };
        }
        #endregion Delete SaleOrder Detail

        #region Delete SaleOrder OtherCharge
        public object DeleteSaleOrderOtherChargeDetail(Guid id, string CreatedBy, DateTime CreatedDate)
        {
            SqlParameter outputStatus = null;
            try
            {

                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[DeleteSaleOrderOtherCharge]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(_appConstant.DeleteFailure);

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConstant.DeleteSuccess
            };
        }
        #endregion Delete SaleOrder OtherCharge

        #region GetSaleOrderOtherChargeListBySaleOrderID
        public List<SaleOrderOtherCharge> GetSaleOrderOtherChargesDetailListBySaleOrderID(Guid SaleOrderID, bool isCopy)
        {
            List<SaleOrderOtherCharge> saleorderOtherChargeList = new List<SaleOrderOtherCharge>();
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[GetSaleOrderOtherChargeListBySaleOrderID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SaleOrderID", SqlDbType.UniqueIdentifier).Value = SaleOrderID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    SaleOrderOtherCharge saleOrderOtherCharge = new SaleOrderOtherCharge();
                                    {
                                        if (isCopy)
                                        {
                                            saleOrderOtherCharge.ID = Guid.Empty;
                                        }
                                        else
                                        {
                                            saleOrderOtherCharge.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : saleOrderOtherCharge.ID);
                                        }


                                        
                                        saleOrderOtherCharge.SaleOrderID = (sdr["SaleOrderID"].ToString() != "" ? Guid.Parse(sdr["SaleOrderID"].ToString()) : saleOrderOtherCharge.SaleOrderID);
                                        saleOrderOtherCharge.OtherChargeCode = (sdr["OtherChargeCode"].ToString() != "" ? int.Parse(sdr["OtherChargeCode"].ToString()) : saleOrderOtherCharge.OtherChargeCode);
                                        saleOrderOtherCharge.ChargeAmount = (sdr["ChargeAmount"].ToString() != "" ? decimal.Parse(sdr["ChargeAmount"].ToString()) : saleOrderOtherCharge.ChargeAmount);
                                        saleOrderOtherCharge.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : saleOrderOtherCharge.TaxTypeCode);
                                        saleOrderOtherCharge.TaxType = new TaxType();
                                        saleOrderOtherCharge.TaxType.Code = (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : saleOrderOtherCharge.TaxType.Code);
                                        saleOrderOtherCharge.TaxType.ValueText = (sdr["TaxTypeText"].ToString() != "" ? (sdr["TaxTypeText"].ToString()) : saleOrderOtherCharge.TaxType.ValueText);
                                        saleOrderOtherCharge.CGSTPerc = (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : saleOrderOtherCharge.CGSTPerc);
                                        saleOrderOtherCharge.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : saleOrderOtherCharge.SGSTPerc);
                                        saleOrderOtherCharge.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : saleOrderOtherCharge.IGSTPerc);
                                        saleOrderOtherCharge.AddlTaxPerc = (sdr["AddlTaxPerc"].ToString() != "" ? decimal.Parse(sdr["AddlTaxPerc"].ToString()) : saleOrderOtherCharge.AddlTaxPerc);
                                        saleOrderOtherCharge.AddlTaxAmt = (sdr["AddlTaxAmt"].ToString() != "" ? decimal.Parse(sdr["AddlTaxAmt"].ToString()) : saleOrderOtherCharge.AddlTaxAmt);
                                        saleOrderOtherCharge.OtherCharge = new OtherCharge();
                                        saleOrderOtherCharge.OtherCharge.Description = (sdr["OtherCharge"].ToString() != "" ? sdr["OtherCharge"].ToString() : saleOrderOtherCharge.OtherCharge.Description);
                                        saleOrderOtherCharge.OtherCharge.SACCode = (sdr["SACCode"].ToString() != "" ? sdr["SACCode"].ToString() : saleOrderOtherCharge.OtherCharge.SACCode);
                                    }
                                    saleorderOtherChargeList.Add(saleOrderOtherCharge);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return saleorderOtherChargeList;
        }
        #endregion GetSaleOrderOtherChargeListBySaleOrderID

        #region GetSaleOrderSummaryCount
        public SaleOrderSummary GetSaleOrderSummaryCount()
        {
            SaleOrderSummary saleOrderSummary = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[GetSaleOrderSummaryCount]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    saleOrderSummary = new SaleOrderSummary();
                                    saleOrderSummary.TotalSaleOrderCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : saleOrderSummary.TotalSaleOrderCount);
                                    saleOrderSummary.OpenSaleOrderCount = (sdr["OpenCount"].ToString() != "" ? int.Parse(sdr["OpenCount"].ToString()) : saleOrderSummary.OpenSaleOrderCount);
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return saleOrderSummary;
        }
        #endregion GetSaleOrderSummaryCount
    }
}