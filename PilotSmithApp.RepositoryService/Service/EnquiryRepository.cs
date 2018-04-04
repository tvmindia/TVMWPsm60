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
    public class EnquiryRepository:IEnquiryRepository
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
        //#region GetAllTitles
        //public List<Titles> GetAllTitles()
        //{
        //    List<Titles> titlesList = null;
        //    try
        //    {
        //        using (SqlConnection con = _databaseFactory.GetDBConnection())
        //        {
        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                if (con.State == ConnectionState.Closed)
        //                {
        //                    con.Open();
        //                }
        //                cmd.Connection = con;
        //                cmd.CommandText = "[PSA].[GetAllTitle]";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                using (SqlDataReader sdr = cmd.ExecuteReader())
        //                {
        //                    if ((sdr != null) && (sdr.HasRows))
        //                    {
        //                        titlesList = new List<Titles>();
        //                        while (sdr.Read())
        //                        {
        //                            Titles _titlesObj = new Titles();
        //                            {
        //                                _titlesObj.Title = (sdr["Title"].ToString() != "" ? sdr["Title"].ToString() : _titlesObj.Title);

        //                            }
        //                            titlesList.Add(_titlesObj);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return titlesList;
        //}
        //#endregion GetAllTitles
        //#region InsertUpdateEnquiry
        //public object InsertUpdateEnquiry(Enquiry enquiry)
        //{
        //    SqlParameter outputStatus, outputID = null;
        //    try
        //    {
        //        using (SqlConnection con = _databaseFactory.GetDBConnection())
        //        {
        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                if (con.State == ConnectionState.Closed)
        //                {
        //                    con.Open();
        //                }
        //                cmd.Connection = con;
        //                cmd.CommandText = "[PSA].[InsertUpdateEnquiry]";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = enquiry.IsUpdate;
        //                if (enquiry.ID == Guid.Empty)
        //                {
        //                    cmd.Parameters.AddWithValue("@ID", DBNull.Value);
        //                }
        //                else
        //                {
        //                    cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = enquiry.ID;
        //                }
        //                cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 150).Value = enquiry.CompanyName;
        //                cmd.Parameters.Add("@ContactPerson", SqlDbType.VarChar, 100).Value = enquiry.ContactPerson;
        //                cmd.Parameters.Add("@ContactEmail", SqlDbType.VarChar, 150).Value = enquiry.ContactEmail;
        //                cmd.Parameters.Add("@ContactTitle", SqlDbType.VarChar, 10).Value = enquiry.ContactTitle;
        //                cmd.Parameters.Add("@Website", SqlDbType.NVarChar, 500).Value = enquiry.Website;
        //                cmd.Parameters.Add("@LandLine", SqlDbType.VarChar, 50).Value = enquiry.LandLine;
        //                cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 50).Value = enquiry.Mobile;
        //                cmd.Parameters.Add("@Fax", SqlDbType.VarChar, 50).Value = enquiry.Fax;
        //                cmd.Parameters.Add("@OtherPhoneNos", SqlDbType.VarChar, 250).Value = enquiry.OtherPhoneNos;
        //                cmd.Parameters.Add("@BillingAddress", SqlDbType.NVarChar, -1).Value = enquiry.BillingAddress;
        //                cmd.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar, -1).Value = enquiry.ShippingAddress;
        //                cmd.Parameters.Add("@PaymentTermCode", SqlDbType.VarChar, 10).Value = enquiry.PaymentTermCode;
        //                cmd.Parameters.Add("@TaxRegNo", SqlDbType.VarChar, 50).Value = enquiry.TaxRegNo;
        //                cmd.Parameters.Add("@PANNo", SqlDbType.VarChar, 50).Value = enquiry.PANNO;
        //                cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = enquiry.GeneralNotes;
        //                cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = enquiry.common.CreatedBy;
        //                cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = enquiry.common.CreatedDate;
        //                cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = enquiry.common.UpdatedBy;
        //                cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = enquiry.common.UpdatedDate;
        //                outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
        //                outputStatus.Direction = ParameterDirection.Output;
        //                outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
        //                outputID.Direction = ParameterDirection.Output;
        //                cmd.ExecuteNonQuery();
        //            }
        //        }

        //        switch (outputStatus.Value.ToString())
        //        {
        //            case "0":
        //                throw new Exception(_appConstant.InsertFailure);
        //            case "1":
        //                enquiry.ID = Guid.Parse(outputID.Value.ToString());
        //                return new
        //                {
        //                    ID = enquiry.ID,
        //                    Status = outputStatus.Value.ToString(),
        //                    Message = enquiry.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
        //                };
        //            default:
        //                break;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return new
        //    {
        //        ID = enquiry.ID,
        //        Status = outputStatus.Value.ToString(),
        //        Message = enquiry.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
        //    };
        //}
        //#endregion InsertUpdateEnquiry
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
                                        enquiry.EnquiryDateFormatted= (sdr["EnquiryDate"].ToString() != "" ? DateTime.Parse(sdr["EnquiryDate"].ToString()).ToString(_settings.DateFormat) : enquiry.EnquiryDateFormatted);
                                        enquiry.RequirementSpec = (sdr["RequirementSpec"].ToString() != "" ? sdr["RequirementSpec"].ToString() : enquiry.RequirementSpec);
                                        enquiry.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : enquiry.CustomerID);
                                        enquiry.Customer = new Customer
                                        {
                                            ID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : enquiry.Customer.ID),
                                            CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : enquiry.Customer.CompanyName),
                                        };
                                        enquiry.GradeCode = (sdr["GradeCode"].ToString() != "" ? int.Parse(sdr["GradeCode"].ToString()) : enquiry.GradeCode);
                                        enquiry.StatusCode = (sdr["StatusCode"].ToString() != "" ? int.Parse(sdr["StatusCode"].ToString()) : enquiry.StatusCode);
                                        enquiry.ReferredByCode = (sdr["ReferredByCode"].ToString() != "" ? int.Parse(sdr["Mobile"].ToString()) : enquiry.ReferredByCode);
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
        //#region Get Enquiry
        //public Enquiry GetEnquiry(Guid id)
        //{
        //    Enquiry enquiry = new Enquiry();
        //    try
        //    {
        //        using (SqlConnection con = _databaseFactory.GetDBConnection())
        //        {
        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                if (con.State == ConnectionState.Closed)
        //                {
        //                    con.Open();
        //                }
        //                cmd.Connection = con;
        //                cmd.CommandText = "[PSA].[GetEnquiry]";
        //                cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                using (SqlDataReader sdr = cmd.ExecuteReader())
        //                {
        //                    if ((sdr != null) && (sdr.HasRows))
        //                        if (sdr.Read())
        //                        {
        //                            enquiry.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : enquiry.ID);
        //                            enquiry.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : enquiry.CompanyName);
        //                            enquiry.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : enquiry.ContactPerson);
        //                            enquiry.ContactEmail = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : enquiry.ContactEmail);
        //                            enquiry.ContactTitle = (sdr["ContactTitle"].ToString() != "" ? sdr["ContactTitle"].ToString() : enquiry.ContactTitle);
        //                            enquiry.Website = (sdr["Website"].ToString() != "" ? sdr["Website"].ToString() : enquiry.Website);
        //                            enquiry.LandLine = (sdr["LandLine"].ToString() != "" ? sdr["LandLine"].ToString() : enquiry.LandLine);
        //                            enquiry.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : enquiry.Mobile);
        //                            enquiry.Fax = (sdr["Fax"].ToString() != "" ? sdr["Fax"].ToString() : enquiry.Fax);
        //                            enquiry.OtherPhoneNos = (sdr["OtherPhoneNos"].ToString() != "" ? sdr["OtherPhoneNos"].ToString() : enquiry.OtherPhoneNos);
        //                            enquiry.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : enquiry.BillingAddress);
        //                            enquiry.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : enquiry.ShippingAddress);
        //                            enquiry.PaymentTermCode = (sdr["PaymentTermCode"].ToString() != "" ? sdr["PaymentTermCode"].ToString() : enquiry.PaymentTermCode);
        //                            enquiry.TaxRegNo = (sdr["TaxRegNo"].ToString() != "" ? sdr["TaxRegNo"].ToString() : enquiry.TaxRegNo);
        //                            enquiry.PANNO = (sdr["PANNO"].ToString() != "" ? sdr["PANNO"].ToString() : enquiry.PANNO);
        //                            enquiry.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : enquiry.GeneralNotes);
        //                            enquiry.common = new PSASysCommon();
        //                            enquiry.common.CreatedBy = (sdr["CreatedBy"].ToString() != "" ? sdr["CreatedBy"].ToString() : enquiry.common.CreatedBy);
        //                            enquiry.common.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(_settings.DateFormat) : enquiry.common.CreatedDateString);
        //                            enquiry.common.CreatedDate = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()) : enquiry.common.CreatedDate);
        //                            enquiry.common.UpdatedBy = (sdr["UpdatedBy"].ToString() != "" ? sdr["UpdatedBy"].ToString() : enquiry.common.UpdatedBy);
        //                            enquiry.common.UpdatedDate = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()) : enquiry.common.UpdatedDate);
        //                            enquiry.common.UpdatedDateString = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()).ToString(_settings.DateFormat) : enquiry.common.UpdatedDateString);

        //                        }
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return enquiry;
        //}
        //#endregion Get Enquiry
        //#region DeleteEnquiry
        //public object DeleteEnquiry(Guid id)
        //{
        //    SqlParameter outputStatus = null;
        //    try
        //    {

        //        using (SqlConnection con = _databaseFactory.GetDBConnection())
        //        {
        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                if (con.State == ConnectionState.Closed)
        //                {
        //                    con.Open();
        //                }
        //                cmd.Connection = con;
        //                cmd.CommandText = "[PSA].[DeleteCustomer]";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
        //                outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
        //                outputStatus.Direction = ParameterDirection.Output;
        //                cmd.ExecuteNonQuery();


        //            }
        //        }

        //        switch (outputStatus.Value.ToString())
        //        {
        //            case "0":

        //                throw new Exception(_appConstant.DeleteFailure);

        //            default:
        //                break;
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return new
        //    {
        //        Status = outputStatus.Value.ToString(),
        //        Message = _appConstant.DeleteSuccess
        //    };
        //}
        //#endregion DeleteEnquiry
    }
}
