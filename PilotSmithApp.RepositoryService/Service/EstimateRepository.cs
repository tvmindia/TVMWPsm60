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
    public class EstimateRepository:IEstimateRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public EstimateRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetAllEstimate
        public List<Estimate> GetAllEstimate(EstimateAdvanceSearch estimateAdvanceSearch)
        {
            List<Estimate> estimateList = null;
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
                        cmd.CommandText = "[PSA].[GetAllEstimate]";
                        if (string.IsNullOrEmpty(estimateAdvanceSearch.SearchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchTerm", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = estimateAdvanceSearch.SearchTerm;

                        }
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = estimateAdvanceSearch.DataTablePaging.Start;
                        if (estimateAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = estimateAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = estimateAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = estimateAdvanceSearch.ToDate;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                estimateList = new List<Estimate>();
                                while (sdr.Read())
                                {
                                    Estimate estimate = new Estimate();
                                    {
                                        estimate.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : estimate.ID);
                                        estimate.EnquiryID = (sdr["EnquiryID"].ToString() != "" ? Guid.Parse(sdr["EnquiryID"].ToString()) : estimate.EnquiryID);
                                        estimate.EstimateNo = (sdr["EstimateNo"].ToString() != "" ? sdr["EstimateNo"].ToString() : estimate.EstimateNo);
                                        estimate.EstimateRefNo = (sdr["EstimateRefNo"].ToString() != "" ? sdr["EstimateRefNo"].ToString() : estimate.EstimateRefNo);
                                        estimate.EstimateDate = (sdr["EstimateDate"].ToString() != "" ? DateTime.Parse(sdr["EstimateDate"].ToString()) : estimate.EstimateDate);
                                        estimate.EstimateDateFormatted = (sdr["EstimateDate"].ToString() != "" ? DateTime.Parse(sdr["EstimateDate"].ToString()).ToString(_settings.DateFormat) : estimate.EstimateDateFormatted);
                                        estimate.Enquiry = new Enquiry();
                                        estimate.Enquiry.EnquiryNo = (sdr["EnquiryNo"].ToString() != "" ? sdr["EnquiryNo"].ToString() : estimate.Enquiry.EnquiryNo);
                                        estimate.Customer = new Customer();
                                        estimate.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : estimate.Customer.CompanyName);
                                        // estimate.DocumentStatus = new DocumentStatus();
                                        //.DocumentStatus.Description = (sdr["DocumentStatusDescription"].ToString() != "" ? sdr["DocumentStatusDescription"].ToString() : estimate.DocumentStatus.Description);
                                        estimate.Branch = new Branch();
                                        estimate.Branch.Description = (sdr["BranchCode"].ToString() != "" ? sdr["BranchCode"].ToString() : estimate.Branch.Description);
                                        estimate.UserName = (sdr["DocumentOwner"].ToString() != "" ? sdr["DocumentOwner"].ToString() : estimate.UserName);
                                        estimate.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : estimate.TotalCount);
                                        estimate.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : estimate.FilteredCount);
                                    }
                                    estimateList.Add(estimate);
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
            return estimateList;
        }
        #endregion GetAllEstimate

        #region GetEstimate
        public Estimate GetEstimate(Guid id)
        {
            Estimate estimate = new Estimate();
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
                        cmd.CommandText = "[PSA].[GetEstimate]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    estimate.EnquiryID = (sdr["EnquiryID"].ToString() != "" ? Guid.Parse(sdr["EnquiryID"].ToString()) : estimate.EnquiryID);
                                    estimate.EstimateNo = (sdr["EstimateNo"].ToString() != "" ? sdr["EstimateNo"].ToString() : estimate.EstimateNo);
                                    estimate.EstimateRefNo = (sdr["EstimateRefNo"].ToString() != "" ? sdr["EstimateRefNo"].ToString() : estimate.EstimateRefNo);
                                    estimate.EstimateDate = (sdr["EstimateDate"].ToString() != "" ? DateTime.Parse(sdr["EstimateDate"].ToString()) : estimate.EstimateDate);
                                    estimate.EstimateDateFormatted = (sdr["EstimateDate"].ToString() != "" ? DateTime.Parse(sdr["EstimateDate"].ToString()).ToString(_settings.DateFormat) : estimate.EstimateDateFormatted);
                                    estimate.ValidUpToDate= (sdr["ValidUpToDate"].ToString() != "" ? DateTime.Parse(sdr["ValidUpToDate"].ToString()) : estimate.ValidUpToDate);
                                    estimate.ValidUpToDateFormatted = (sdr["ValidUpToDate"].ToString() != "" ? DateTime.Parse(sdr["ValidUpToDate"].ToString()).ToString(_settings.DateFormat) : estimate.ValidUpToDateFormatted);
                                    estimate.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : estimate.GeneralNotes);
                                    estimate.Enquiry = new Enquiry();
                                    estimate.Enquiry.EnquiryNo= (sdr["EnquiryNo"].ToString() != "" ? sdr["EnquiryNo"].ToString() : estimate.Enquiry.EnquiryNo);
                                    estimate.Customer = new Customer();
                                    estimate.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : estimate.Customer.CompanyName);
                                    estimate.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : estimate.CustomerID);
                                    estimate.DocumentStatus = new DocumentStatus();
                                    estimate.DocumentStatus.Description = (sdr["DocumentStatus"].ToString() != "" ? sdr["DocumentStatus"].ToString() : estimate.DocumentStatus.Description);
                                    estimate.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : estimate.DocumentStatusCode);
                                    estimate.Employee = new Employee();
                                    estimate.Employee.Name = (sdr["PreparedByName"].ToString() != "" ? sdr["PreparedByName"].ToString() : estimate.Employee.Name);
                                    estimate.PreparedBy = (sdr["PreparedBy"].ToString() != "" ? Guid.Parse(sdr["PreparedBy"].ToString()) : estimate.PreparedBy);
                                    estimate.Branch = new Branch();
                                    estimate.Branch.Description = (sdr["BranchCode"].ToString() != "" ? sdr["BranchCode"].ToString() : estimate.Branch.Description);
                                    estimate.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : estimate.BranchCode);
                                    estimate.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : estimate.DocumentOwnerID);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return estimate;
        }
        #endregion GetEstimate

        #region GetAllEstimateItems
        public List<EstimateDetail> GetEstimateDetailListByEstimateID(Guid estimateID)
        {
            List<EstimateDetail> estimateDetailList = new List<EstimateDetail>();
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
                        cmd.CommandText = "[PSA].[GetEstimateDetailListByEstimateID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = estimateID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    EstimateDetail estimateDetail = new EstimateDetail();
                                    {
                                        estimateDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : estimateDetail.ID);
                                        estimateDetail.EstimateID = (sdr["EstimateID"].ToString() != "" ? Guid.Parse(sdr["EstimateID"].ToString()) : estimateDetail.EstimateID);
                                        estimateDetail.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : estimateDetail.Qty);
                                        estimateDetail.CostRate = (sdr["CostRate"].ToString() != "" ? decimal.Parse(sdr["CostRate"].ToString()) : estimateDetail.CostRate);
                                        estimateDetail.SellingRate = (sdr["SellingRate"].ToString() != "" ? decimal.Parse(sdr["SellingRate"].ToString()) : estimateDetail.SellingRate);
                                        estimateDetail.DrawingNo = (sdr["DrawingNo"].ToString() != "" ? sdr["DrawingNo"].ToString() : estimateDetail.DrawingNo);
                                        estimateDetail.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString() : estimateDetail.ProductSpec);
                                        estimateDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : estimateDetail.ProductID);
                                        estimateDetail.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : estimateDetail.ProductModelID);
                                        estimateDetail.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : estimateDetail.UnitCode);
                                        estimateDetail.Product = new Product();
                                        estimateDetail.Product.Code = (sdr["ProductCode"].ToString() != "" ? sdr["ProductCode"].ToString() : estimateDetail.Product.Code);
                                        estimateDetail.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : estimateDetail.Product.Name);
                                        estimateDetail.ProductModel = new ProductModel();
                                        estimateDetail.ProductModel.Name = (sdr["ModelName"].ToString() != "" ? sdr["ModelName"].ToString() : estimateDetail.ProductModel.Name);
                                        estimateDetail.Unit = new Unit();
                                        estimateDetail.Unit.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : estimateDetail.Unit.Description);
                                    }
                                    estimateDetailList.Add(estimateDetail);
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

            return estimateDetailList;
        }


        #endregion GetAllEstimateItems

        #region InsertUpdateEstimate
        public object InsertUpdateEstimate(Estimate estimate)
        {
            SqlParameter outputStatus, outputID, outputEstimateNo = null;
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
                        cmd.CommandText = "[PSA].[InsertUpdateEstimate]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = estimate.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = estimate.ID;
                        cmd.Parameters.Add("@EstimateNo", SqlDbType.VarChar, 20).Value = estimate.EstimateNo;
                        cmd.Parameters.Add("@EstimateRefNo", SqlDbType.VarChar, 20).Value = estimate.EstimateRefNo;
                        cmd.Parameters.Add("@EstimateDate", SqlDbType.DateTime).Value = estimate.EstimateDateFormatted;
                        cmd.Parameters.Add("@EnquiryID", SqlDbType.UniqueIdentifier).Value = estimate.EnquiryID;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = estimate.CustomerID;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = estimate.DocumentStatusCode;
                        cmd.Parameters.Add("@ValidUpToDate", SqlDbType.DateTime).Value = estimate.ValidUpToDateFormatted;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = estimate.DetailXML;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = estimate.hdnFileID;
                        cmd.Parameters.Add("@PreparedBy", SqlDbType.UniqueIdentifier).Value = estimate.PreparedBy;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar,-1).Value = estimate.GeneralNotes;
                        cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = estimate.DocumentOwnerID;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = estimate.BranchCode;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = estimate.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = estimate.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = estimate.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = estimate.PSASysCommon.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        outputEstimateNo = cmd.Parameters.Add("@EstimateNoOut", SqlDbType.VarChar, 20);
                        outputEstimateNo.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        estimate.ID = Guid.Parse(outputID.Value.ToString());
                        estimate.EstimateNo = outputEstimateNo.Value.ToString();
                        return new
                        {
                            ID = estimate.ID,
                            EstimateNo = estimate.EstimateNo,
                            Status = outputStatus.Value.ToString(),
                            Message = estimate.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
                        };
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return new
            {
                ID = estimate.ID,
                EstimatNo = estimate.EstimateNo,
                Status = outputStatus.Value.ToString(),
                Message = estimate.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion InsertUpdateEstimate

        #region GetEstimateForSelectList
        public List<Estimate> GetEstimateForSelectList()
        {
            List<Estimate> estimateList = null;
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
                        cmd.CommandText = "[PSA].[GetSelectListForEstimate]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                estimateList = new List<Estimate>();
                                while (sdr.Read())
                                {
                                    Estimate estimate = new Estimate();
                                    {
                                       estimate.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : estimate.ID);
                                        estimate.EstimateNo = (sdr["EstimateNo"].ToString() != "" ? sdr["EstimateNo"].ToString() : estimate.EstimateNo);
                                    }
                                    estimateList.Add(estimate);
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
            return estimateList;
        }
        #endregion GetEstimateForSelectList

        #region DeleteEstimate
        public object DeleteEstimate(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteEstimate]";
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
        #endregion DeleteEstimate

        #region DeleteEstimateDetail
        public object DeleteEstimateDetail(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteEstimateDetail]";
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
        #endregion DeleteEstimateDetail
    }
}
