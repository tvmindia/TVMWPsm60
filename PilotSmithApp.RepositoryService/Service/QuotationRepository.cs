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
    public class QuotationRepository: IQuotationRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public QuotationRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region Get All Quotation
        public List<Quotation> GetAllQuotation(QuotationAdvanceSearch quotationAdvanceSearch)
        {
            List<Quotation> quotationList = null;
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
                        cmd.CommandText = "[PSA].[GetAllQuotation]";
                        if (string.IsNullOrEmpty(quotationAdvanceSearch.SearchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchValue", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = quotationAdvanceSearch.SearchTerm;
                        }
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = quotationAdvanceSearch.DataTablePaging.Start;
                        if (quotationAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = quotationAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = quotationAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = quotationAdvanceSearch.ToDate;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                quotationList = new List<Quotation>();
                                while (sdr.Read())
                                {
                                    Quotation quotation = new Quotation();
                                    {
                                        quotation.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : quotation.ID);
                                        quotation.QuoteNo = (sdr["QuoteNo"].ToString() != "" ? sdr["QuoteNo"].ToString() : quotation.QuoteNo);
                                        quotation.QuoteDate = (sdr["QuoteDate"].ToString() != "" ? DateTime.Parse(sdr["QuoteDate"].ToString()) : quotation.QuoteDate);
                                        quotation.QuoteDateFormatted = (sdr["QuoteDate"].ToString() != "" ? DateTime.Parse(sdr["QuoteDate"].ToString()).ToString(_settings.DateFormat) : quotation.QuoteDateFormatted);
                                        //quotation.RequirementSpec = (sdr["RequirementSpec"].ToString() != "" ? sdr["RequirementSpec"].ToString() : quotation.RequirementSpec);
                                        quotation.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : quotation.CustomerID);
                                        quotation.Customer = new Customer();
                                        quotation.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : quotation.Customer.ID);
                                        quotation.Customer.CompanyName = (sdr["CustomerCompanyName"].ToString() != "" ? sdr["CustomerCompanyName"].ToString() : quotation.Customer.CompanyName);
                                        quotation.Customer.ContactPerson = (sdr["CustomerContactPerson"].ToString() != "" ? sdr["CustomerContactPerson"].ToString() : quotation.Customer.ContactPerson);
                                        quotation.Customer.Mobile = (sdr["CustomerMobile"].ToString() != "" ? sdr["CustomerMobile"].ToString() : quotation.Customer.Mobile);
                                        quotation.ValidUpToDate = (sdr["ValidUpToDate"].ToString() != "" ? DateTime.Parse(sdr["ValidUpToDate"].ToString()) : quotation.ValidUpToDate);
                                        quotation.ValidUpToDateFormatted = (sdr["ValidUpToDate"].ToString() != "" ? DateTime.Parse(sdr["ValidUpToDate"].ToString()).ToString(_settings.DateFormat) : quotation.ValidUpToDateFormatted);
                                        quotation.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : quotation.DocumentStatusCode);
                                        quotation.DocumentStatus = new DocumentStatus();
                                        quotation.DocumentStatus.Code = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : quotation.DocumentStatus.Code);
                                        quotation.DocumentStatus.Description = (sdr["DocumentStatusDescription"].ToString() != "" ? (sdr["DocumentStatusDescription"].ToString()) : quotation.DocumentStatus.Description);
                                        quotation.ReferredByCode = (sdr["ReferredByCode"].ToString() != "" ? int.Parse(sdr["ReferredByCode"].ToString()) : quotation.ReferredByCode);
                                        quotation.ReferencePerson = new ReferencePerson();
                                        quotation.ReferencePerson.Code = (sdr["ReferredByCode"].ToString() != "" ? int.Parse(sdr["ReferredByCode"].ToString()) : quotation.ReferencePerson.Code);
                                        quotation.ReferencePerson.Name = (sdr["ReferredByCode"].ToString() != "" ? (sdr["ReferencePersonName"].ToString()) : quotation.ReferencePerson.Name);
                                        //quotation.ResponsiblePersonID = (sdr["ReferencePersonName"].ToString() != "" ? Guid.Parse(sdr["ResponsiblePersonID"].ToString()) : quotation.ResponsiblePersonID);
                                        quotation.PreparedBy = (sdr["PreparedBy"].ToString() != "" ? Guid.Parse(sdr["PreparedBy"].ToString()) : quotation.PreparedBy);
                                        //quotation.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : quotation.GeneralNotes);
                                        quotation.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : quotation.DocumentOwnerID);
                                        quotation.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : quotation.BranchCode);
                                        quotation.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : quotation.FilteredCount);
                                        quotation.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : quotation.FilteredCount);
                                    }
                                    quotationList.Add(quotation);
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

            return quotationList;
        }
        #endregion Get All Quotation
        #region Get Quotation
        public Quotation GetQuotation(Guid id)
        {
            Quotation quotation = new Quotation();
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
                        cmd.CommandText = "[PSA].[GetQuotation]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    quotation.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : quotation.ID);
                                    quotation.QuoteNo = (sdr["QuoteNo"].ToString() != "" ? sdr["QuoteNo"].ToString() : quotation.QuoteNo);
                                    quotation.QuoteRefNo = (sdr["QuoteRefNo"].ToString() != "" ? sdr["QuoteRefNo"].ToString() : quotation.QuoteRefNo);
                                    quotation.QuoteDate = (sdr["QuoteDate"].ToString() != "" ? DateTime.Parse(sdr["QuoteDate"].ToString()) : quotation.QuoteDate);
                                    quotation.QuoteDateFormatted = (sdr["QuoteDate"].ToString() != "" ? DateTime.Parse(sdr["QuoteDate"].ToString()).ToString("dd-MMM-yyyy") : quotation.QuoteDateFormatted);
                                    quotation.EstimateID = (sdr["EstimateID"].ToString() != "" ? Guid.Parse(sdr["EstimateID"].ToString()) : quotation.EstimateID);
                                    quotation.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : quotation.CustomerID);
                                    quotation.MailingAddress = (sdr["MailingAddress"].ToString() != "" ? sdr["MailingAddress"].ToString() : quotation.MailingAddress);
                                    quotation.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : quotation.ShippingAddress);
                                    quotation.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : quotation.DocumentStatusCode);
                                    quotation.ValidUpToDate = (sdr["ValidUpToDate"].ToString() != "" ? DateTime.Parse(sdr["ValidUpToDate"].ToString()) : quotation.ValidUpToDate);
                                    quotation.ValidUpToDateFormatted = (sdr["ValidUpToDate"].ToString() != "" ? DateTime.Parse(sdr["ValidUpToDate"].ToString()).ToString("dd-MMM-yyyy") : quotation.ValidUpToDateFormatted);
                                    quotation.ReferredByCode = (sdr["ReferredByCode"].ToString() != "" ? int.Parse(sdr["ReferredByCode"].ToString()) : quotation.ReferredByCode);
                                    quotation.PreparedBy = (sdr["PreparedBy"].ToString() != "" ? Guid.Parse(sdr["PreparedBy"].ToString()) : quotation.PreparedBy);
                                    quotation.MailBodyHeader = (sdr["MailBodyHeader"].ToString() != "" ? sdr["MailBodyHeader"].ToString() : quotation.MailBodyHeader);
                                    quotation.MailBodyFooter = (sdr["MailBodyFooter"].ToString() != "" ? sdr["MailBodyFooter"].ToString() : quotation.MailBodyFooter);
                                    quotation.EmailSentYN = (sdr["EmailSentYN"].ToString() != "" ? bool.Parse(sdr["EmailSentYN"].ToString()) : quotation.EmailSentYN);
                                    quotation.LatestApprovalID = (sdr["LatestApprovalID"].ToString() != "" ? Guid.Parse(sdr["LatestApprovalID"].ToString()) : quotation.LatestApprovalID);
                                    quotation.LatestApprovalStatus = (sdr["LatestApprovalStatus"].ToString() != "" ? int.Parse(sdr["LatestApprovalStatus"].ToString()) : quotation.LatestApprovalStatus);
                                    quotation.IsFinalApproved = (sdr["IsFinalApproved"].ToString() != "" ? bool.Parse(sdr["IsFinalApproved"].ToString()) : quotation.IsFinalApproved);
                                    quotation.EmailSentTo = (sdr["EmailSentTo"].ToString() != "" ? (sdr["EmailSentTo"].ToString()) : quotation.EmailSentTo);
                                    quotation.TermReferenceNo = (sdr["TermReferenceNo"].ToString() != "" ? (sdr["TermReferenceNo"].ToString()) : quotation.TermReferenceNo);
                                    quotation.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : quotation.Discount);
                                    quotation.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : quotation.GeneralNotes);
                                    quotation.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : quotation.DocumentOwnerID);
                                    quotation.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : quotation.BranchCode);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return quotation;
        }
        #endregion Get Quotation
        #region GetQuotationDetailListByQuotationID
        public List<QuotationDetail> GetQuotationDetailListByQuotationID(Guid quotationID)
        {
            List<QuotationDetail> quotationDetailList = new List<QuotationDetail>();
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
                        cmd.CommandText = "[PSA].[GetQuotationDetailListByQuotationID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@QuoteID", SqlDbType.UniqueIdentifier).Value = quotationID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    QuotationDetail quotationDetail = new QuotationDetail();
                                    {
                                        quotationDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : quotationDetail.ID);
                                        quotationDetail.QuoteID = (sdr["QuoteID"].ToString() != "" ? Guid.Parse(sdr["QuoteID"].ToString()) : quotationDetail.QuoteID);
                                        quotationDetail.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString() : quotationDetail.ProductSpec);
                                        quotationDetail.Product = new Product()
                                        {
                                            ID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty),
                                            Code = (sdr["ProductCode"].ToString() != "" ? sdr["ProductCode"].ToString() : string.Empty),
                                            Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : string.Empty)
                                        };
                                        quotationDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty);
                                        quotationDetail.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        quotationDetail.ProductModel = new ProductModel();
                                        quotationDetail.ProductModel.ID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        quotationDetail.ProductModel.Name = (sdr["ProductModelName"].ToString() != "" ? (sdr["ProductModelName"].ToString()) : quotationDetail.ProductModel.Name);
                                        quotationDetail.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : quotationDetail.Qty);
                                        quotationDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : quotationDetail.Rate);
                                        quotationDetail.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : quotationDetail.UnitCode);
                                        quotationDetail.Unit = new Unit();
                                        quotationDetail.Unit.Code = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : quotationDetail.Unit.Code);
                                        quotationDetail.Unit.Description = (sdr["UnitDescription"].ToString() != "" ? (sdr["UnitDescription"].ToString()) : quotationDetail.Unit.Description);
                                        quotationDetail.Discount= (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : quotationDetail.Discount);
                                        quotationDetail.TaxTypeCode= (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : quotationDetail.TaxTypeCode);
                                        quotationDetail.CGSTAmt = (sdr["CGSTAmt"].ToString() != "" ? decimal.Parse(sdr["CGSTAmt"].ToString()) : quotationDetail.CGSTAmt);
                                        quotationDetail.SGSTAmt = (sdr["SGSTAmt"].ToString() != "" ? decimal.Parse(sdr["SGSTAmt"].ToString()) : quotationDetail.SGSTAmt);
                                        quotationDetail.IGSTAmt = (sdr["IGSTAmt"].ToString() != "" ? decimal.Parse(sdr["IGSTAmt"].ToString()) : quotationDetail.IGSTAmt);
                                    }
                                    quotationDetailList.Add(quotationDetail);
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

            return quotationDetailList;
        }


        #endregion GetQuotationDetailsByQuotationID
        #region Insert Update Quotation
        public object InsertUpdateQuotation(Quotation quotation)
        {
            SqlParameter outputStatus, outputID, outputQuotationNo = null;
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
                        cmd.CommandText = "[PSA].[InsertUpdateQuotation]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = quotation.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = quotation.ID;
                        cmd.Parameters.Add("@QuoteRefNo", SqlDbType.VarChar, 20).Value = quotation.QuoteRefNo;
                        cmd.Parameters.Add("@QuoteNo", SqlDbType.VarChar, 20).Value = quotation.QuoteNo;
                        cmd.Parameters.Add("@QuoteDate", SqlDbType.DateTime).Value = quotation.QuoteDateFormatted;
                        cmd.Parameters.Add("@EstimateID", SqlDbType.UniqueIdentifier).Value = quotation.EstimateID;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = quotation.CustomerID;
                        cmd.Parameters.Add("@MailingAddress", SqlDbType.NVarChar, -1).Value = quotation.MailingAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar, -1).Value = quotation.ShippingAddress;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = quotation.DocumentStatusCode;
                        cmd.Parameters.Add("@ValidUpToDate", SqlDbType.DateTime).Value = quotation.ValidUpToDateFormatted;
                        cmd.Parameters.Add("@ReferredByCode", SqlDbType.Int).Value = quotation.ReferredByCode;
                        cmd.Parameters.Add("@PreparedBy", SqlDbType.UniqueIdentifier).Value = quotation.PreparedBy;
                        cmd.Parameters.Add("@MailBodyHeader", SqlDbType.NVarChar, -1).Value = quotation.MailBodyHeader;
                        cmd.Parameters.Add("@MailBodyFooter", SqlDbType.NVarChar, -1).Value = quotation.MailBodyFooter;
                        cmd.Parameters.Add("@EmailSentYN", SqlDbType.Bit, -1).Value = quotation.EmailSentYN;
                        cmd.Parameters.Add("@LatestApprovalID", SqlDbType.UniqueIdentifier).Value = quotation.LatestApprovalID;
                        cmd.Parameters.Add("@IsFinalApproved", SqlDbType.Bit).Value = quotation.IsFinalApproved;
                        cmd.Parameters.Add("@EmailSentTo", SqlDbType.NVarChar,-1).Value = quotation.EmailSentTo;
                        cmd.Parameters.Add("@TermReferenceNo", SqlDbType.VarChar,25).Value = quotation.TermReferenceNo;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = quotation.DetailXML;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = quotation.hdnFileID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = quotation.GeneralNotes;
                        cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = quotation.DocumentOwnerID;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = quotation.BranchCode;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = quotation.Discount;
                        //-----------------------//
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = quotation.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = quotation.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = quotation.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = quotation.PSASysCommon.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@StatusOut", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        outputQuotationNo = cmd.Parameters.Add("@QuoteNoOut", SqlDbType.VarChar, 20);
                        outputQuotationNo.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        quotation.ID = Guid.Parse(outputID.Value.ToString());
                        quotation.QuoteNo = outputQuotationNo.Value.ToString();
                        return new
                        {
                            ID = quotation.ID,
                            QuotationNo = quotation.QuoteNo,
                            Status = outputStatus.Value.ToString(),
                            Message = quotation.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                ID = quotation.ID,
                QuotationNo = quotation.QuoteNo,
                Status = outputStatus.Value.ToString(),
                Message = quotation.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion Insert Update Quotation
        #region Delete Quotation
        public object DeleteQuotation(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteQuotation]";
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
        #endregion Delete Quotation
        #region Delete Quotation Detail
        public object DeleteQuotationDetail(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteQuotationDetail]";
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
        #endregion Delete Quotation Detail
    }
}