using PilotSmithApp.DataAccessObject.DTO;
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
    public class SaleInvoiceRepository: ISaleInvoiceRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public SaleInvoiceRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region Get All SaleInvoice
        public List<SaleInvoice> GetAllSaleInvoice(SaleInvoiceAdvanceSearch saleInvoiceAdvanceSearch)
        {
            List<SaleInvoice> saleInvoiceList = null;
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
                        cmd.CommandText = "[PSA].[GetAllSaleInvoice]";
                        if (string.IsNullOrEmpty(saleInvoiceAdvanceSearch.SearchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchTerm", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = saleInvoiceAdvanceSearch.SearchTerm;
                        }
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = saleInvoiceAdvanceSearch.DataTablePaging.Start;
                        if (saleInvoiceAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = saleInvoiceAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = saleInvoiceAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = saleInvoiceAdvanceSearch.ToDate;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                saleInvoiceList = new List<SaleInvoice>();
                                while (sdr.Read())
                                {
                                    SaleInvoice saleInvoice = new SaleInvoice();
                                    {
                                        saleInvoice.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : saleInvoice.ID);
                                        saleInvoice.SaleInvNo = (sdr["SaleInvNo"].ToString() != "" ? sdr["SaleInvNo"].ToString() : saleInvoice.SaleInvNo);
                                        saleInvoice.SaleInvRefNo = (sdr["SaleInvRefNo"].ToString() != "" ? sdr["SaleInvRefNo"].ToString() : saleInvoice.SaleInvRefNo);
                                        saleInvoice.SaleInvDate = (sdr["SaleInvDate"].ToString() != "" ? DateTime.Parse(sdr["SaleInvDate"].ToString()) : saleInvoice.SaleInvDate);
                                        saleInvoice.SaleInvDateFormatted = (sdr["SaleInvDate"].ToString() != "" ? DateTime.Parse(sdr["SaleInvDate"].ToString()).ToString(_settings.DateFormat) : saleInvoice.SaleInvDateFormatted);
                                        saleInvoice.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : saleInvoice.CustomerID);
                                        saleInvoice.Customer = new Customer();
                                        saleInvoice.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : saleInvoice.Customer.ID);
                                        saleInvoice.Customer.CompanyName = (sdr["CustomerCompanyName"].ToString() != "" ? sdr["CustomerCompanyName"].ToString() : saleInvoice.Customer.CompanyName);
                                        saleInvoice.Customer.ContactPerson = (sdr["CustomerContactPerson"].ToString() != "" ? sdr["CustomerContactPerson"].ToString() : saleInvoice.Customer.ContactPerson);
                                        saleInvoice.Customer.Mobile = (sdr["CustomerMobile"].ToString() != "" ? sdr["CustomerMobile"].ToString() : saleInvoice.Customer.Mobile);
                                        saleInvoice.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : saleInvoice.DocumentStatusCode);
                                        saleInvoice.DocumentStatus = new DocumentStatus();
                                        saleInvoice.DocumentStatus.Code = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : saleInvoice.DocumentStatus.Code);
                                        saleInvoice.DocumentStatus.Description = (sdr["DocumentStatusDescription"].ToString() != "" ? (sdr["DocumentStatusDescription"].ToString()) : saleInvoice.DocumentStatus.Description);
                                        saleInvoice.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : saleInvoice.GeneralNotes);
                                        saleInvoice.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : saleInvoice.DocumentOwnerID);
                                        saleInvoice.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : saleInvoice.BranchCode);
                                        saleInvoice.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : saleInvoice.FilteredCount);
                                        saleInvoice.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : saleInvoice.FilteredCount);
                                    }
                                    saleInvoiceList.Add(saleInvoice);
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

            return saleInvoiceList;
        }
        #endregion Get All SaleInvoice
        #region Get SaleInvoice
        public SaleInvoice GetSaleInvoice(Guid id)
        {
            SaleInvoice saleInvoice = new SaleInvoice();
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
                        cmd.CommandText = "[PSA].[GetSaleInvoice]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    saleInvoice.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : saleInvoice.ID);
                                    saleInvoice.QuoteID = (sdr["QuoteID"].ToString() != "" ? Guid.Parse(sdr["QuoteID"].ToString()) : saleInvoice.QuoteID);
                                    saleInvoice.SaleOrderID = (sdr["SaleOrderID"].ToString() != "" ? Guid.Parse(sdr["SaleOrderID"].ToString()) : saleInvoice.SaleOrderID);
                                    saleInvoice.SaleInvNo = (sdr["SaleInvNo"].ToString() != "" ? sdr["SaleInvNo"].ToString() : saleInvoice.SaleInvNo);
                                    saleInvoice.SaleInvRefNo = (sdr["SaleInvRefNo"].ToString() != "" ? sdr["SaleInvRefNo"].ToString() : saleInvoice.SaleInvRefNo);
                                    saleInvoice.SaleInvDate = (sdr["SaleInvDate"].ToString() != "" ? DateTime.Parse(sdr["SaleInvDate"].ToString()) : saleInvoice.SaleInvDate);
                                    saleInvoice.SaleInvDateFormatted = (sdr["SaleInvDate"].ToString() != "" ? DateTime.Parse(sdr["SaleInvDate"].ToString()).ToString("dd-MMM-yyyy") : saleInvoice.SaleInvDateFormatted);
                                    saleInvoice.ExpectedDelvDate = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()) : saleInvoice.ExpectedDelvDate);
                                    saleInvoice.ExpectedDelvDateFormatted = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()).ToString("dd-MMM-yyyy") : saleInvoice.ExpectedDelvDateFormatted);
                                    saleInvoice.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : saleInvoice.CustomerID);
                                    saleInvoice.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : saleInvoice.DocumentStatusCode);
                                    saleInvoice.DocumentStatus = new DocumentStatus();
                                    saleInvoice.DocumentStatus.Description = (sdr["DocumentStatusDescription"].ToString() != "" ?sdr["DocumentStatusDescription"].ToString() : saleInvoice.DocumentStatus.Description);
                                    saleInvoice.MailingAddress = (sdr["MailingAddress"].ToString() != "" ? sdr["MailingAddress"].ToString() : saleInvoice.MailingAddress);
                                    saleInvoice.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : saleInvoice.ShippingAddress);
                                    saleInvoice.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : saleInvoice.GeneralNotes);
                                    saleInvoice.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : saleInvoice.DocumentOwnerID);
                                    saleInvoice.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : saleInvoice.BranchCode);
                                    saleInvoice.PreparedBy = (sdr["PreparedBy"].ToString() != "" ? Guid.Parse(sdr["PreparedBy"].ToString()) : saleInvoice.PreparedBy);

                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return saleInvoice;
        }
        #endregion Get SaleInvoice
        #region GetAllSaleInvoiceItems
        public List<SaleInvoiceDetail> GetSaleInvoiceDetailListBySaleInvoiceID(Guid saleInvoiceID)
        {
            List<SaleInvoiceDetail> saleInvoiceDetailList = new List<SaleInvoiceDetail>();
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
                        cmd.CommandText = "[PSA].[GetSaleInvoiceDetailListBySaleInvoiceID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SaleInvoiceID", SqlDbType.UniqueIdentifier).Value = saleInvoiceID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    SaleInvoiceDetail saleInvoiceDetail = new SaleInvoiceDetail();
                                    {
                                        saleInvoiceDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : saleInvoiceDetail.ID);
                                        saleInvoiceDetail.SaleInvID = (sdr["SaleInvID"].ToString() != "" ? Guid.Parse(sdr["SaleInvID"].ToString()) : saleInvoiceDetail.SaleInvID);
                                        saleInvoiceDetail.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString() : saleInvoiceDetail.ProductSpec);
                                        saleInvoiceDetail.Product = new Product()
                                        {
                                            ID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty),
                                            Code = (sdr["ProductCode"].ToString() != "" ? sdr["ProductCode"].ToString() : string.Empty),
                                            Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : string.Empty)
                                        };
                                        saleInvoiceDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty);
                                        saleInvoiceDetail.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        saleInvoiceDetail.ProductModel = new ProductModel();
                                        saleInvoiceDetail.ProductModel.ID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        saleInvoiceDetail.ProductModel.Name = (sdr["ProductModelName"].ToString() != "" ? (sdr["ProductModelName"].ToString()) : saleInvoiceDetail.ProductModel.Name);
                                        saleInvoiceDetail.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : saleInvoiceDetail.Qty);
                                        saleInvoiceDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : saleInvoiceDetail.Rate);
                                        saleInvoiceDetail.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : saleInvoiceDetail.UnitCode);
                                        saleInvoiceDetail.Unit = new Unit();
                                        saleInvoiceDetail.Unit.Code = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : saleInvoiceDetail.Unit.Code);
                                        saleInvoiceDetail.Unit.Description = (sdr["UnitDescription"].ToString() != "" ? (sdr["UnitDescription"].ToString()) : saleInvoiceDetail.Unit.Description);
                                    }
                                    saleInvoiceDetailList.Add(saleInvoiceDetail);
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

            return saleInvoiceDetailList;
        }


        #endregion GetQuotationDetails
        #region Insert Update SaleInvoice
        public object InsertUpdateSaleInvoice(SaleInvoice saleInvoice)
        {
            SqlParameter outputStatus, outputID, outputProdOrderNo = null;
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
                        cmd.CommandText = "[PSA].[InsertUpdateSaleInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = saleInvoice.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = saleInvoice.ID;
                        cmd.Parameters.Add("@SaleInvNo", SqlDbType.VarChar, 20).Value = saleInvoice.SaleInvNo;
                        cmd.Parameters.Add("@SaleInvRefNo", SqlDbType.VarChar, 20).Value = saleInvoice.SaleInvRefNo;
                        cmd.Parameters.Add("@SaleInvDate", SqlDbType.DateTime).Value = saleInvoice.SaleInvDateFormatted;
                        if(saleInvoice.QuoteID!=Guid.Empty)
                            cmd.Parameters.Add("@QuoteID",SqlDbType.UniqueIdentifier).Value=saleInvoice.QuoteID;
                        if(saleInvoice.SaleOrderID!=Guid.Empty)
                        cmd.Parameters.Add("@SaleOrderID", SqlDbType.UniqueIdentifier).Value=saleInvoice.SaleOrderID;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = saleInvoice.CustomerID;
                        cmd.Parameters.Add("@MailingAddress", SqlDbType.NVarChar,-1).Value=saleInvoice.MailingAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar, -1).Value = saleInvoice.ShippingAddress;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = saleInvoice.DocumentStatusCode;
                        cmd.Parameters.Add("@ExpectedDelvDate", SqlDbType.DateTime).Value=saleInvoice.ExpectedDelvDateFormatted;
                        cmd.Parameters.Add("@CashInvoiceYN", SqlDbType.Bit).Value=saleInvoice.CashInvoiceYN;
                        if(saleInvoice.PreparedBy != Guid.Empty)
                            cmd.Parameters.Add("@PreparedBy", SqlDbType.UniqueIdentifier).Value=saleInvoice.PreparedBy;
                        cmd.Parameters.Add("@PurchaseOrdNo", SqlDbType.VarChar,20).Value=saleInvoice.PurchaseOrdNo;
                        cmd.Parameters.Add("@PurchaseOrdDate", SqlDbType.DateTime).Value=saleInvoice.PurchaseOrdDateFormatted;
                        cmd.Parameters.Add("@BillSeriesCode", SqlDbType.Int).Value=saleInvoice.BillSeriesCode;
                        cmd.Parameters.Add("@EmailSentYN", SqlDbType.Bit).Value=saleInvoice.EmailSentYN;
                        cmd.Parameters.Add("@LatestApprovalID", SqlDbType.UniqueIdentifier).Value=saleInvoice.LatestApprovalID;
                        cmd.Parameters.Add("@LatestApprovalStatus", SqlDbType.Int).Value=saleInvoice.LatestApprovalStatus;
                        cmd.Parameters.Add("@IsFinalApproved", SqlDbType.Bit).Value=saleInvoice.IsFinalApproved;
                        cmd.Parameters.Add("@EmailSentTo", SqlDbType.Bit).Value=saleInvoice.EmailSentYN;
                        cmd.Parameters.Add("@PrintRemark", SqlDbType.NVarChar,-1).Value=saleInvoice.PrintRemark;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value=saleInvoice.Discount;
                        cmd.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value=saleInvoice.AdvanceAmount;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = saleInvoice.DetailXML;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = saleInvoice.hdnFileID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = saleInvoice.GeneralNotes;
                        if (saleInvoice.DocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = saleInvoice.DocumentOwnerID;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = saleInvoice.BranchCode;
                        //-----------------------//
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = saleInvoice.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = saleInvoice.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = saleInvoice.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = saleInvoice.PSASysCommon.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@StatusOut", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        outputProdOrderNo = cmd.Parameters.Add("@SaleInvNoOut", SqlDbType.VarChar, 20);
                        outputProdOrderNo.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        saleInvoice.ID = Guid.Parse(outputID.Value.ToString());
                        saleInvoice.SaleInvNo = outputProdOrderNo.Value.ToString();
                        return new
                        {
                            ID = saleInvoice.ID,
                            SaleInvoiceNo = saleInvoice.SaleInvNo,
                            Status = outputStatus.Value.ToString(),
                            Message = saleInvoice.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                ID = saleInvoice.ID,
                SaleInvoiceNo = saleInvoice.SaleInvNo,
                Status = outputStatus.Value.ToString(),
                Message = saleInvoice.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion Insert Update SaleInvoice
        #region Delete SaleInvoice
        public object DeleteSaleInvoice(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteSaleInvoice]";
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
        #endregion Delete SaleInvoice
        #region Delete SaleInvoice Detail
        public object DeleteSaleInvoiceDetail(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteSaleInvoiceDetail]";
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
        #endregion Delete SaleInvoice Detail

        public List<SaleInvoiceOtherCharge> GetSaleInvoiceOtherChargesDetailListBySaleInvoiceID(Guid saleInvoiceID)
        {
            List<SaleInvoiceOtherCharge> saleInvoiceOtherChargeList = new List<SaleInvoiceOtherCharge>();
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
                        cmd.CommandText = "[PSA].[GetSaleInvoiceOtherChargeListBySaleInvoiceID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SaleInvoiceID", SqlDbType.UniqueIdentifier).Value = saleInvoiceID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    SaleInvoiceOtherCharge saleInvoiceOtherCharge = new SaleInvoiceOtherCharge();
                                    {
                                        saleInvoiceOtherCharge.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : saleInvoiceOtherCharge.ID);
                                        saleInvoiceOtherCharge.SaleInvID = (sdr["SaleInvoiceID"].ToString() != "" ? Guid.Parse(sdr["SaleOrderID"].ToString()) : saleInvoiceOtherCharge.SaleInvID);
                                        saleInvoiceOtherCharge.OtherChargeCode = (sdr["OtherChargeCode"].ToString() != "" ? int.Parse(sdr["OtherChargeCode"].ToString()) : saleInvoiceOtherCharge.OtherChargeCode);
                                        saleInvoiceOtherCharge.ChargeAmount = (sdr["ChargeAmount"].ToString() != "" ? decimal.Parse(sdr["ChargeAmount"].ToString()) : saleInvoiceOtherCharge.ChargeAmount);
                                        saleInvoiceOtherCharge.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : saleInvoiceOtherCharge.TaxTypeCode);
                                        saleInvoiceOtherCharge.TaxType = new TaxType();
                                        saleInvoiceOtherCharge.TaxType.Code = (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : saleInvoiceOtherCharge.TaxType.Code);
                                        saleInvoiceOtherCharge.TaxType.ValueText = (sdr["TaxTypeText"].ToString() != "" ? (sdr["TaxTypeText"].ToString()) : saleInvoiceOtherCharge.TaxType.ValueText);
                                        saleInvoiceOtherCharge.CGSTPerc = (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : saleInvoiceOtherCharge.CGSTPerc);
                                        saleInvoiceOtherCharge.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : saleInvoiceOtherCharge.SGSTPerc);
                                        saleInvoiceOtherCharge.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : saleInvoiceOtherCharge.IGSTPerc);
                                        saleInvoiceOtherCharge.AddlTaxPerc = (sdr["AddlTaxPerc"].ToString() != "" ? decimal.Parse(sdr["AddlTaxPerc"].ToString()) : saleInvoiceOtherCharge.AddlTaxPerc);
                                        saleInvoiceOtherCharge.AddlTaxAmt = (sdr["AddlTaxAmt"].ToString() != "" ? decimal.Parse(sdr["AddlTaxAmt"].ToString()) : saleInvoiceOtherCharge.AddlTaxAmt);
                                        saleInvoiceOtherCharge.OtherCharge = new OtherCharge();
                                        saleInvoiceOtherCharge.OtherCharge.Description = (sdr["OtherCharge"].ToString() != "" ? sdr["OtherCharge"].ToString() : saleInvoiceOtherCharge.OtherCharge.Description);
                                    }
                                    saleInvoiceOtherChargeList.Add(saleInvoiceOtherCharge);
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
            return saleInvoiceOtherChargeList;
        }
    }
}
