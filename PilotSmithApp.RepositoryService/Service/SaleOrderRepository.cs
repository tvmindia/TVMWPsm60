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
                        if (id==null)
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
                                    saleOrder.EnquiryID= (sdr["EnquiryID"].ToString() != "" ? Guid.Parse(sdr["EnquiryID"].ToString()) : saleOrder.EnquiryID);                                  
                                    saleOrder.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : saleOrder.CustomerID);
                                    saleOrder.MailingAddress = (sdr["MailingAddress"].ToString() != "" ? sdr["MailingAddress"].ToString() : saleOrder.MailingAddress);
                                    saleOrder.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : saleOrder.ShippingAddress);
                                    saleOrder.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : saleOrder.DocumentStatusCode);
                                    saleOrder.ExpectedDelvDate = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()) : saleOrder.ExpectedDelvDate);
                                    saleOrder.ExpectedDelvDateFormatted = (sdr["ExpectedDelvDate"].ToString()!="" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()).ToString(_settings.DateFormat) : saleOrder.ExpectedDelvDateFormatted);
                                    saleOrder.ReferredByCode = (sdr["ReferredByCode"].ToString() != "" ? int.Parse(sdr["ReferredByCode"].ToString()) : saleOrder.ReferredByCode);
                                    saleOrder.PreparedBy = (sdr["PreparedBy"].ToString() != "" ? Guid.Parse(sdr["PreparedBy"].ToString()) : saleOrder.PreparedBy);
                                    saleOrder.PurchaseOrdNo = (sdr["PurchaseOrdNo"].ToString() != "" ? sdr["PurchaseOrdNo"].ToString() : saleOrder.PurchaseOrdNo);
                                    saleOrder.PurchaseOrdDate = (sdr["PurchaseOrdDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrdDate"].ToString() ): saleOrder.PurchaseOrdDate);
                                    saleOrder.PurchaseOrdDateFormatted = (sdr["PurchaseOrdDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrdDate"].ToString()).ToString(_settings.DateFormat) : saleOrder.PurchaseOrdDateFormatted);
                                    saleOrder.BankCode = (sdr["BankCode"].ToString() != "" ? int.Parse(sdr["BankCode"].ToString()) : saleOrder.BankCode);
                                    saleOrder.CarrierCode= (sdr["CarrierCode"].ToString() != "" ? int.Parse(sdr["CarrierCode"].ToString()) : saleOrder.CarrierCode);
                                    saleOrder.EmailSentYN = (sdr["EmailSentYN"].ToString() != "" ? bool.Parse(sdr["EmailSentYN"].ToString()) : saleOrder.EmailSentYN);
                                    saleOrder.LatestApprovalID= (sdr["LatestApprovalID"].ToString() != "" ? Guid.Parse(sdr["LatestApprovalID"].ToString()) : saleOrder.LatestApprovalID);
                                    saleOrder.LatestApprovalStatus= (sdr["LatestApprovalStatus"].ToString() != "" ? int.Parse(sdr["LatestApprovalStatus"].ToString()) : saleOrder.LatestApprovalStatus);
                                    saleOrder.IsFinalApproved= (sdr["IsFinalApproved"].ToString() != "" ? bool.Parse(sdr["IsFinalApproved"].ToString()) : saleOrder.IsFinalApproved);
                                    saleOrder.EmailSentTo= (sdr["EmailSentTo"].ToString() != "" ? sdr["EmailSentTo"].ToString() : saleOrder.EmailSentTo);
                                    saleOrder.TermReferenceNo= (sdr["TermReferenceNo"].ToString() != "" ? sdr["TermReferenceNo"].ToString() : saleOrder.TermReferenceNo);
                                    saleOrder.PrintRemark= (sdr["PrintRemark"].ToString() != "" ? sdr["PrintRemark"].ToString() : saleOrder.PrintRemark);
                                    saleOrder.GeneralNotes= (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : saleOrder.GeneralNotes);
                                    saleOrder.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : saleOrder.BranchCode);
                                    saleOrder.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : saleOrder.DocumentOwnerID);
                                    saleOrder.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : saleOrder.Discount);
                                    saleOrder.AdvanceAmount = (sdr["AdvanceAmount"].ToString() != "" ? decimal.Parse(sdr["AdvanceAmount"].ToString()) : saleOrder.AdvanceAmount);
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
        public List<SaleOrderDetail> GetSaleOrderDetailListBySaleOrderID(Guid saleOrderID)
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
                                        saleOrderDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : saleOrderDetail.ID);
                                        saleOrderDetail.SaleOrderID = (sdr["SaleOrderID"].ToString() != "" ? Guid.Parse(sdr["SaleOrderID"].ToString()) : saleOrderDetail.SaleOrderID);
                                        saleOrderDetail.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : saleOrderDetail.Qty);
                                        saleOrderDetail.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString() : saleOrderDetail.ProductSpec);
                                        saleOrderDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : saleOrderDetail.ProductID);
                                        saleOrderDetail.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : saleOrderDetail.ProductModelID);
                                        saleOrderDetail.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : saleOrderDetail.UnitCode);
                                        saleOrderDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : saleOrderDetail.Rate);
                                        saleOrderDetail.Discount= (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : saleOrderDetail.Discount);
                                        saleOrderDetail.TaxTypeCode= (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : saleOrderDetail.TaxTypeCode);
                                        saleOrderDetail.CGSTPerc= (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : saleOrderDetail.CGSTPerc);
                                        saleOrderDetail.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : saleOrderDetail.SGSTPerc);
                                        saleOrderDetail.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : saleOrderDetail.IGSTPerc);
                                        saleOrderDetail.CessPerc = (sdr["CessPerc"].ToString() != "" ? decimal.Parse(sdr["CessPerc"].ToString()) : saleOrderDetail.CessPerc);
                                        saleOrderDetail.CessAmt = (sdr["CessAmt"].ToString() != "" ? decimal.Parse(sdr["CessAmt"].ToString()) : saleOrderDetail.CessAmt);
                                        saleOrderDetail.Product = new Product();
                                        saleOrderDetail.Product.Code = (sdr["ProductCode"].ToString() != "" ? sdr["ProductCode"].ToString() : saleOrderDetail.Product.Code);
                                        saleOrderDetail.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : saleOrderDetail.Product.Name);
                                        saleOrderDetail.ProductModel = new ProductModel();
                                        saleOrderDetail.ProductModel.Name = (sdr["ModelName"].ToString() != "" ? sdr["ModelName"].ToString() : saleOrderDetail.ProductModel.Name);
                                        saleOrderDetail.Unit = new Unit();
                                        saleOrderDetail.Unit.Description = (sdr["Unit"].ToString() != "" ? sdr["Unit"].ToString() : saleOrderDetail.Unit.Description);
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
    }
}