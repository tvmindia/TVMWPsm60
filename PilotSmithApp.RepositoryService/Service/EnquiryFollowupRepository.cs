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
    public class EnquiryFollowupRepository : IEnquiryFollowupRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public EnquiryFollowupRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region Insert Update EnquiryFollowup
        public object InsertUpdateEnquiryFollowup(EnquiryFollowup enquiryFollowup)
        {
            SqlParameter outputStatus, outputID= null;
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
                        cmd.CommandText = "[PSA].[InsertUpdateEnquiryFollowup]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = enquiryFollowup.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value= enquiryFollowup.ID;
                        cmd.Parameters.Add("@EnquiryID", SqlDbType.UniqueIdentifier).Value = enquiryFollowup.EnquiryID;
                        cmd.Parameters.Add("@FollowupDate", SqlDbType.DateTime).Value = enquiryFollowup.FollowupDateFormatted;
                        cmd.Parameters.Add("@FollowupTime", SqlDbType.Time).Value = enquiryFollowup.FollowupTimeFormatted;
                        cmd.Parameters.Add("@PriorityCode", SqlDbType.Int).Value = enquiryFollowup.PriorityCode;
                        cmd.Parameters.Add("@Subject", SqlDbType.VarChar).Value = enquiryFollowup.Subject;
                        cmd.Parameters.Add("@ContactName", SqlDbType.VarChar).Value = enquiryFollowup.ContactName;
                        cmd.Parameters.Add("@ContactNo", SqlDbType.VarChar).Value = enquiryFollowup.ContactNo;
                        cmd.Parameters.Add("@RemindPriorTo", SqlDbType.Int).Value = enquiryFollowup.RemindPriorTo;
                        cmd.Parameters.Add("@ReminderType", SqlDbType.VarChar).Value = enquiryFollowup.ReminderType;
                        cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = enquiryFollowup.Status;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar).Value = enquiryFollowup.GeneralNotes;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = enquiryFollowup.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = enquiryFollowup.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = enquiryFollowup.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = enquiryFollowup.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@StatusOut", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        enquiryFollowup.ID = Guid.Parse(outputID.Value.ToString());
                        return new
                        {
                            ID = enquiryFollowup.ID,
                            Status = outputStatus.Value.ToString(),
                            Message = enquiryFollowup.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                ID = enquiryFollowup.ID,
                Status = outputStatus.Value.ToString(),
                Message = enquiryFollowup.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion Insert Update EnquiryFollowup
        #region GetAll Enquiry Followup
        public List<EnquiryFollowup> GetAllEnquiryFollowup(EnquiryFollowup enquiryFollowup)
        {
            List<EnquiryFollowup> enquiryFollowupList = null;
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
                        cmd.CommandText = "[PSA].[GetAllEnquiryFollowup]";
                        cmd.Parameters.Add("@EnquiryID", SqlDbType.UniqueIdentifier).Value = enquiryFollowup.EnquiryID;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = enquiryFollowup.DataTablePaging.Start;
                        cmd.Parameters.Add("@Length", SqlDbType.Int).Value = enquiryFollowup.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                enquiryFollowupList = new List<EnquiryFollowup>();
                                while (sdr.Read())
                                {
                                    enquiryFollowup = new EnquiryFollowup();
                                    {
                                        enquiryFollowup.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : enquiryFollowup.ID);
                                        enquiryFollowup.EnquiryID = (sdr["EnquiryID"].ToString() != "" ? Guid.Parse(sdr["EnquiryID"].ToString()) : enquiryFollowup.EnquiryID);
                                        enquiryFollowup.FollowupDate = (sdr["FollowupDate"].ToString() != "" ? DateTime.Parse(sdr["FollowupDate"].ToString()) : enquiryFollowup.FollowupDate);
                                        enquiryFollowup.FollowupDateFormatted = (sdr["FollowupDate"].ToString() != "" ? DateTime.Parse(sdr["FollowupDate"].ToString()).ToString(_settings.DateFormat) : enquiryFollowup.FollowupDateFormatted);
                                        enquiryFollowup.FollowupTimeFormatted = (sdr["FollowupTime"].ToString() != "" ? DateTime.Parse(sdr["FollowUpTime"].ToString()).ToString("hh:mm tt") : enquiryFollowup.FollowupTimeFormatted);
                                        enquiryFollowup.PriorityCode = (sdr["PriorityCode"].ToString() != "" ? int.Parse(sdr["PriorityCode"].ToString()) : enquiryFollowup.PriorityCode);
                                       
                                        enquiryFollowup.Subject = (sdr["Subject"].ToString() != "" ? sdr["Subject"].ToString() : enquiryFollowup.Subject);
                                        enquiryFollowup.ContactName = (sdr["ContactName"].ToString() != "" ? sdr["ContactName"].ToString() : enquiryFollowup.ContactName);
                                        enquiryFollowup.ContactNo = (sdr["ContactNo"].ToString() != "" ? (sdr["ContactNo"].ToString()) : enquiryFollowup.ContactNo);

                                        enquiryFollowup.RemindPriorTo = (sdr["RemindPriorTo"].ToString() != "" ? int.Parse(sdr["RemindPriorTo"].ToString()) : enquiryFollowup.RemindPriorTo);
                                        enquiryFollowup.ReminderType = (sdr["ReminderType"].ToString() != "" ? sdr["ReminderType"].ToString() : enquiryFollowup.ReminderType);
                                        enquiryFollowup.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : enquiryFollowup.GeneralNotes);
                                        enquiryFollowup.Status = (sdr["Status"].ToString() != "" ? sdr["Status"].ToString() : enquiryFollowup.Status);
                                        enquiryFollowup.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : enquiryFollowup.FilteredCount);
                                        enquiryFollowup.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : enquiryFollowup.FilteredCount);
                                    }
                                    enquiryFollowupList.Add(enquiryFollowup);
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

            return enquiryFollowupList;
        }
        #endregion GetAll Enquiry Followup
        #region Get Enquiry Followup
        public EnquiryFollowup GetEnquiryFollowup(Guid id)
        {
            EnquiryFollowup enquiryFollowup = new EnquiryFollowup();
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
                        cmd.CommandText = "[PSA].[GetEnquiryFollowup]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    enquiryFollowup.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : enquiryFollowup.ID);
                                    enquiryFollowup.EnquiryID = (sdr["EnquiryID"].ToString() != "" ? Guid.Parse(sdr["EnquiryID"].ToString()) : enquiryFollowup.EnquiryID);
                                    enquiryFollowup.FollowupDate = (sdr["FollowupDate"].ToString() != "" ? DateTime.Parse(sdr["FollowupDate"].ToString()) : enquiryFollowup.FollowupDate);
                                    enquiryFollowup.FollowupDateFormatted = (sdr["FollowupDate"].ToString() != "" ? DateTime.Parse(sdr["FollowupDate"].ToString()).ToString(_settings.DateFormat) : enquiryFollowup.FollowupDateFormatted);
                                    enquiryFollowup.FollowupTimeFormatted = (sdr["FollowupTime"].ToString() != "" ? DateTime.Parse(sdr["FollowUpTime"].ToString()).ToString("hh:mm tt") : enquiryFollowup.FollowupTimeFormatted);
                                    enquiryFollowup.PriorityCode = (sdr["PriorityCode"].ToString() != "" ? int.Parse(sdr["PriorityCode"].ToString()) : enquiryFollowup.PriorityCode);
                                    enquiryFollowup.Subject = (sdr["Subject"].ToString() != "" ? sdr["Subject"].ToString() : enquiryFollowup.Subject);
                                    enquiryFollowup.ContactName = (sdr["ContactName"].ToString() != "" ? sdr["ContactName"].ToString() : enquiryFollowup.ContactName);
                                    enquiryFollowup.ContactNo = (sdr["ContactNo"].ToString() != "" ? (sdr["ContactNo"].ToString()) : enquiryFollowup.ContactNo);
                                    enquiryFollowup.RemindPriorTo = (sdr["RemindPriorTo"].ToString() != "" ? int.Parse(sdr["RemindPriorTo"].ToString()) : enquiryFollowup.RemindPriorTo);
                                    enquiryFollowup.ReminderType = (sdr["ReminderType"].ToString() != "" ? sdr["ReminderType"].ToString() : enquiryFollowup.ReminderType);
                                    enquiryFollowup.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : enquiryFollowup.GeneralNotes);
                                    enquiryFollowup.Status = (sdr["Status"].ToString() != "" ? sdr["Status"].ToString() : enquiryFollowup.Status);

                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return enquiryFollowup;
        }
        #endregion Get Enquiry Followup
        #region Delete EnquiryFollowup
        public object DeleteEnquiryFollowup(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteEnquiryFollowup]";
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
        #endregion Delete EnquiryFollowup
    }
}
