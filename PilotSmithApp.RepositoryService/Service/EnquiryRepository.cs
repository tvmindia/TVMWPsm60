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
    public class EnquiryRepository : IEnquiryRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public EnquiryRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region Get All Enquiry
        public List<Enquiry> GetAllEnquiry(EnquiryAdvanceSearch enquiryAdvanceSearch)
        {
            List<Enquiry> enquiryList = null;
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
                        cmd.CommandText = "[PSA].[GetAllEnquiry]";
                        if (string.IsNullOrEmpty(enquiryAdvanceSearch.SearchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchTerm", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = enquiryAdvanceSearch.SearchTerm;
                        }
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = enquiryAdvanceSearch.DataTablePaging.Start;
                        if (enquiryAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = enquiryAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = enquiryAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = enquiryAdvanceSearch.ToDate;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                enquiryList = new List<Enquiry>();
                                while (sdr.Read())
                                {
                                    Enquiry enquiry = new Enquiry();
                                    {
                                        enquiry.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : enquiry.ID);
                                        enquiry.EnquiryNo = (sdr["EnquiryNo"].ToString() != "" ? sdr["EnquiryNo"].ToString() : enquiry.EnquiryNo);
                                        enquiry.EnquiryDate = (sdr["EnquiryDate"].ToString() != "" ? DateTime.Parse(sdr["EnquiryDate"].ToString()) : enquiry.EnquiryDate);
                                        enquiry.EnquiryDateFormatted = (sdr["EnquiryDate"].ToString() != "" ? DateTime.Parse(sdr["EnquiryDate"].ToString()).ToString(_settings.DateFormat) : enquiry.EnquiryDateFormatted);
                                        enquiry.RequirementSpec = (sdr["RequirementSpec"].ToString() != "" ? sdr["RequirementSpec"].ToString() : enquiry.RequirementSpec);
                                        enquiry.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : enquiry.CustomerID);
                                        enquiry.Customer = new Customer();
                                        enquiry.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : enquiry.Customer.ID);
                                        enquiry.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : enquiry.Customer.CompanyName);
                                        enquiry.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : enquiry.Customer.ContactPerson);
                                        enquiry.Customer.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : enquiry.Customer.Mobile);
                                        enquiry.EnquiryGradeCode = (sdr["GradeCode"].ToString() != "" ? int.Parse(sdr["GradeCode"].ToString()) : enquiry.EnquiryGradeCode);
                                        enquiry.EnquiryGrade = new EnquiryGrade();
                                        enquiry.EnquiryGrade.Code = (sdr["GradeCode"].ToString() != "" ? int.Parse(sdr["GradeCode"].ToString()) : enquiry.EnquiryGrade.Code);
                                        enquiry.EnquiryGrade.Description = (sdr["EnquiryGradeDescription"].ToString() != "" ? sdr["EnquiryGradeDescription"].ToString() : enquiry.EnquiryGrade.Description);
                                        enquiry.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : enquiry.DocumentStatusCode);
                                        enquiry.DocumentStatus = new DocumentStatus();
                                        enquiry.DocumentStatus.Code = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : enquiry.DocumentStatus.Code);
                                        enquiry.DocumentStatus.Description = (sdr["DocumentStatusDescription"].ToString() != "" ? (sdr["DocumentStatusDescription"].ToString()) : enquiry.DocumentStatus.Description);
                                        enquiry.ReferredByCode = (sdr["ReferredByCode"].ToString() != "" ? int.Parse(sdr["ReferredByCode"].ToString()) : enquiry.ReferredByCode);
                                        enquiry.ReferencePerson = new ReferencePerson();
                                        enquiry.ReferencePerson.Code = (sdr["ReferredByCode"].ToString() != "" ? int.Parse(sdr["ReferredByCode"].ToString()) : enquiry.ReferencePerson.Code);
                                        enquiry.ReferencePerson.Name = (sdr["ReferencePersonName"].ToString() != "" ? (sdr["ReferencePersonName"].ToString()) : enquiry.ReferencePerson.Name);
                                        enquiry.ResponsiblePersonID = (sdr["ResponsiblePersonID"].ToString() != "" ? Guid.Parse(sdr["ResponsiblePersonID"].ToString()) : enquiry.ResponsiblePersonID);
                                        enquiry.AttendedByID = (sdr["AttendedByID"].ToString() != "" ? Guid.Parse(sdr["AttendedByID"].ToString()) : enquiry.AttendedByID);
                                        enquiry.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : enquiry.GeneralNotes);
                                        enquiry.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : enquiry.DocumentOwnerID);
                                        enquiry.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : enquiry.BranchCode);
                                        enquiry.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : enquiry.FilteredCount);
                                        enquiry.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : enquiry.FilteredCount);
                                    }
                                    enquiryList.Add(enquiry);
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

            return enquiryList;
        }
        #endregion Get All Enquiry
        #region Get Enquiry
        public Enquiry GetEnquiry(Guid id)
        {
            Enquiry enquiry = new Enquiry();
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
                        cmd.CommandText = "[PSA].[GetEnquiry]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    enquiry.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : enquiry.ID);
                                    enquiry.EnquiryNo = (sdr["EnquiryNo"].ToString() != "" ? sdr["EnquiryNo"].ToString() : enquiry.EnquiryNo);
                                    enquiry.EnquiryDate = (sdr["EnquiryDate"].ToString() != "" ? DateTime.Parse(sdr["EnquiryDate"].ToString()) : enquiry.EnquiryDate);
                                    enquiry.EnquiryDateFormatted = (sdr["EnquiryDate"].ToString() != "" ? DateTime.Parse(sdr["EnquiryDate"].ToString()).ToString("dd-MMM-yyyy") : enquiry.EnquiryDateFormatted);
                                    enquiry.RequirementSpec = (sdr["RequirementSpec"].ToString() != "" ? sdr["RequirementSpec"].ToString() : enquiry.RequirementSpec);
                                    enquiry.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : enquiry.CustomerID);
                                    enquiry.EnquiryGradeCode = (sdr["GradeCode"].ToString() != "" ? int.Parse(sdr["GradeCode"].ToString()) : enquiry.EnquiryGradeCode);
                                    enquiry.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : enquiry.DocumentStatusCode);
                                    enquiry.ReferredByCode = (sdr["ReferredByCode"].ToString() != "" ? int.Parse(sdr["ReferredByCode"].ToString()) : enquiry.ReferredByCode);
                                    enquiry.ResponsiblePersonID = (sdr["ResponsiblePersonID"].ToString() != "" ? Guid.Parse(sdr["ResponsiblePersonID"].ToString()) : enquiry.ResponsiblePersonID);
                                    enquiry.AttendedByID = (sdr["AttendedByID"].ToString() != "" ? Guid.Parse(sdr["AttendedByID"].ToString()) : enquiry.AttendedByID);
                                    enquiry.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : enquiry.GeneralNotes);
                                    enquiry.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : enquiry.DocumentOwnerID);
                                    enquiry.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : enquiry.BranchCode);
                                    enquiry.DocumentStatus = new DocumentStatus();
                                    enquiry.DocumentStatus.Code= (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : enquiry.DocumentStatus.Code);
                                    enquiry.DocumentStatus.Description= (sdr["DocumentStatusDescription"].ToString() != "" ? (sdr["DocumentStatusDescription"].ToString()) : enquiry.DocumentStatus.Description);
                                    enquiry.DocumentOwners= (sdr["DocumentOwners"].ToString() != "" ? (sdr["DocumentOwners"].ToString()).Split(',') : enquiry.DocumentOwners);
                                    enquiry.DocumentOwner = (sdr["DocumentOwner"].ToString() != "" ? (sdr["DocumentOwner"].ToString()) : enquiry.DocumentOwner);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return enquiry;
        }
        #endregion Get Enquiry
        #region GetAllEnquiryItems
        public List<EnquiryDetail> GetEnquiryDetailListByEnquiryID(Guid enquiryID)
        {
            List<EnquiryDetail> enquiryDetailList = new List<EnquiryDetail>();
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
                        cmd.CommandText = "[PSA].[GetEnquiryDetailListByEnquiryID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@EnquiryID", SqlDbType.UniqueIdentifier).Value = enquiryID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    EnquiryDetail enquiryDetail = new EnquiryDetail();
                                    {
                                        enquiryDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : enquiryDetail.ID);
                                        enquiryDetail.EnquiryID = (sdr["EnquiryID"].ToString() != "" ? Guid.Parse(sdr["EnquiryID"].ToString()) : enquiryDetail.EnquiryID);
                                        enquiryDetail.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString() : enquiryDetail.ProductSpec);
                                        enquiryDetail.SpecTag= (sdr["SpecTag"].ToString() != "" ? Guid.Parse(sdr["SpecTag"].ToString()) : enquiryDetail.SpecTag);
                                        enquiryDetail.Product = new Product()
                                        {
                                            ID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty),
                                            Code = (sdr["ProductCode"].ToString() != "" ? sdr["ProductCode"].ToString() : string.Empty),
                                            Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : string.Empty)
                                        };
                                        enquiryDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty);
                                        enquiryDetail.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        enquiryDetail.ProductModel = new ProductModel();
                                        enquiryDetail.ProductModel.ID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        enquiryDetail.ProductModel.Name = (sdr["ProductModelName"].ToString() != "" ? (sdr["ProductModelName"].ToString()) : enquiryDetail.ProductModel.Name);
                                        enquiryDetail.ProductModel.CostPrice = (sdr["CostPrice"].ToString() != "" ? decimal.Parse(sdr["CostPrice"].ToString()) : enquiryDetail.ProductModel.CostPrice);
                                        enquiryDetail.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : enquiryDetail.Qty);
                                        enquiryDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : enquiryDetail.Rate);
                                        enquiryDetail.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : enquiryDetail.UnitCode);
                                        enquiryDetail.Unit = new Unit();
                                        enquiryDetail.Unit.Code = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : enquiryDetail.Unit.Code);
                                        enquiryDetail.Unit.Description = (sdr["UnitDescription"].ToString() != "" ? (sdr["UnitDescription"].ToString()) : enquiryDetail.Unit.Description);
                                    }
                                    enquiryDetailList.Add(enquiryDetail);
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

            return enquiryDetailList;
        }


        #endregion GetQuotationDetails
        #region Insert Update Enquiry
        public object InsertUpdateEnquiry(Enquiry enquiry)
        {
            SqlParameter outputStatus, outputID, outputEnquiryNo = null;
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
                        cmd.CommandText = "[PSA].[InsertUpdateEnquiry]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = enquiry.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = enquiry.ID;
                        cmd.Parameters.Add("@EnquiryNo", SqlDbType.VarChar, 20).Value = enquiry.EnquiryNo;
                        cmd.Parameters.Add("@EnquiryDate", SqlDbType.DateTime).Value = enquiry.EnquiryDateFormatted;
                        cmd.Parameters.Add("@RequirementSpec", SqlDbType.NVarChar, -1).Value = enquiry.RequirementSpec;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = enquiry.CustomerID;
                        cmd.Parameters.Add("@GradeCode", SqlDbType.Int).Value = enquiry.EnquiryGradeCode;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = enquiry.DocumentStatusCode;
                        cmd.Parameters.Add("@ReferredByCode", SqlDbType.Int).Value = enquiry.ReferredByCode;
                        cmd.Parameters.Add("@ResponsiblePersonID", SqlDbType.UniqueIdentifier).Value = enquiry.ResponsiblePersonID;
                        cmd.Parameters.Add("@AttendedByID", SqlDbType.UniqueIdentifier).Value = enquiry.AttendedByID;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = enquiry.DetailXML;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = enquiry.hdnFileID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = enquiry.GeneralNotes;
                        cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = enquiry.DocumentOwnerID;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = enquiry.BranchCode;
                        //-----------------------//
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = enquiry.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = enquiry.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = enquiry.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = enquiry.PSASysCommon.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@StatusOut", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        outputEnquiryNo = cmd.Parameters.Add("@EnquiryNoOut", SqlDbType.VarChar, 20);
                        outputEnquiryNo.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        enquiry.ID = Guid.Parse(outputID.Value.ToString());
                        enquiry.EnquiryNo = outputEnquiryNo.Value.ToString();
                        return new
                        {
                            ID = enquiry.ID,
                            EnquiryNo = enquiry.EnquiryNo,
                            Status = outputStatus.Value.ToString(),
                            Message = enquiry.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                ID = enquiry.ID,
                EnquiryNo = enquiry.EnquiryNo,
                Status = outputStatus.Value.ToString(),
                Message = enquiry.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion Insert Update Enquiry
        #region Delete Enquiry
        public object DeleteEnquiry(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteEnquiry]";
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
        #endregion Delete Enquiry
        #region Delete Enquiry Detail
        public object DeleteEnquiryDetail(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteEnquiryDetail]";
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
        #endregion Delete Enquiry Detail

        #region GetEnquiryForSelectList
        public List<Enquiry> GetEnquiryForSelectList(Guid? id)
        {
            List<Enquiry> enquiryList = null;
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
                        cmd.CommandText = "[PSA].[GetSelectListForEnquiry]";
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
                                enquiryList = new List<Enquiry>();
                                while (sdr.Read())
                                {
                                    Enquiry enquiry = new Enquiry();
                                    {
                                        enquiry.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : enquiry.ID);
                                        enquiry.EnquiryNo = (sdr["EnquiryNo"].ToString() != "" ? sdr["EnquiryNo"].ToString() : enquiry.EnquiryNo);
                                        enquiry.Customer = new Customer();
                                        enquiry.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : enquiry.Customer.CompanyName);
                                    }
                                    enquiryList.Add(enquiry);
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
            return enquiryList;
        }
        #endregion GetEnquiryForSelectList

        #region GetEnquiryValueVsFollowupCountSummary
        public List<EnquiryValueFolloupSummary> GetEnquiryValueVsFollowupCountSummary() {
            List<EnquiryValueFolloupSummary> EnquiryValueList = new List<EnquiryValueFolloupSummary>();
            EnquiryValueFolloupSummary EnquiryValue = null;
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
                        cmd.CommandText = "[PSA].[GetEnquiryValueVsFollowupCountSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    EnquiryValue = new EnquiryValueFolloupSummary();
                                    EnquiryValue.Enquiry = (sdr["Enquiry"].ToString() != "" ? sdr["Enquiry"].ToString() : EnquiryValue.Enquiry);
                                    EnquiryValue.EnquiryValue = (sdr["EnqValue"].ToString() != "" ? decimal.Parse(sdr["EnqValue"].ToString()) : EnquiryValue.EnquiryValue);
                                    EnquiryValue.FollowupCount = (sdr["FollowupCount"].ToString() != "" ? int.Parse(sdr["FollowupCount"].ToString()) : EnquiryValue.FollowupCount);

                                    EnquiryValueList.Add(EnquiryValue);
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
            return EnquiryValueList;

        }
        #endregion GetEnquiryValueVsFollowupCountSummary

        #region GetEnquiryForSelectListOnDemand
        public List<Enquiry> GetEnquiryForSelectListOnDemand(string searchTerm)
        {
            List<Enquiry> enquiryList = null;
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
                        cmd.CommandText = "[PSA].[GetEnquiryForSelectListOnDemand]";
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
                                enquiryList = new List<Enquiry>();
                                while (sdr.Read())
                                {
                                    Enquiry enquiry = new Enquiry();
                                    {
                                        enquiry.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : enquiry.ID);
                                        enquiry.EnquiryNo = (sdr["EnquiryNo"].ToString() != "" ? sdr["EnquiryNo"].ToString() : enquiry.EnquiryNo);
                                    }
                                    enquiryList.Add(enquiry);
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
            return enquiryList;
        }
        #endregion GetEnquiryForSelectListOnDemand

    }
}
