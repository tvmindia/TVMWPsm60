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
    public class OtherChargeRepository : IOtherChargeRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();

        public OtherChargeRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region InsertUpdateOtherCharge
        public object InsertUpdateOtherCharge(OtherCharge otherCharge)
        {
            SqlParameter outputStatus, outputCode = null;
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
                        cmd.CommandText = "[PSA].[InsertUpdateOtherCharge]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = otherCharge.IsUpdate;
                        if (otherCharge.Code == 0)

                            cmd.Parameters.AddWithValue("@Code", DBNull.Value);

                        else
                            cmd.Parameters.Add("@Code", SqlDbType.Int).Value = otherCharge.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 250).Value = otherCharge.Description;
                        cmd.Parameters.Add("@SACCode", SqlDbType.VarChar,30).Value = otherCharge.SACCode;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 250).Value = otherCharge.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = otherCharge.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 250).Value = otherCharge.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = otherCharge.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.Int);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(otherCharge.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        otherCharge.Code = int.Parse(outputCode.Value.ToString());
                        return new
                        {
                            Code = outputCode.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = otherCharge.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Code = outputCode.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = otherCharge.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion

        #region GetAllOtherCharge
        public List<OtherCharge> GetAllOtherCharge(OtherChargeAdvanceSearch otherChargeAdvanceSearch)
        {
            List<OtherCharge> otherChargeList = null;
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
                        cmd.CommandText = "[PSA].[GetAllOtherCharge]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(otherChargeAdvanceSearch.SearchTerm) ? "" : otherChargeAdvanceSearch.SearchTerm.Trim();
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = otherChargeAdvanceSearch.DataTablePaging.Start;
                        if (otherChargeAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = otherChargeAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                otherChargeList = new List<OtherCharge>();
                                while (sdr.Read())
                                {
                                    OtherCharge otherCharge = new OtherCharge();
                                    {
                                        otherCharge.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : otherCharge.Code);
                                        otherCharge.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : otherCharge.Description);
                                        otherCharge.SACCode = (sdr["SACCode"].ToString() != "" ? sdr["SACCode"].ToString() : otherCharge.SACCode);
                                        otherCharge.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : otherCharge.TotalCount);
                                        otherCharge.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : otherCharge.FilteredCount);

                                    }
                                    otherChargeList.Add(otherCharge);
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

            return otherChargeList;
        }
        #endregion

        #region GetOtherCharge
        public OtherCharge GetOtherCharge(int code)
        {
            OtherCharge otherCharge = null;
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
                        cmd.CommandText = "[PSA].[GetOtherCharge]";
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    otherCharge = new OtherCharge();
                                    otherCharge.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : otherCharge.Code);
                                    otherCharge.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : otherCharge.Description);
                                    otherCharge.SACCode = (sdr["SACCode"].ToString() != "" ? sdr["SACCode"].ToString() : otherCharge.SACCode);
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
            return otherCharge;
        }
        #endregion

        #region CheckOtherChargeCodeExist
        public bool CheckOtherChargeCodeExist(OtherCharge otherCharge)
        {
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
                        cmd.CommandText = "[PSA].[CheckOtherChargeCodeExist]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = otherCharge.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar,250).Value = otherCharge.Description;
                        cmd.CommandType = CommandType.StoredProcedure;
                        Object res = cmd.ExecuteScalar();
                        return (res.ToString() == "Exists" ? true : false);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DeleteOtherCharge
        public object DeleteOtherCharge(int code)
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
                        cmd.CommandText = "[PSA].[DeleteOtherCharge]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = code;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(_appConst.DeleteFailure);

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
                Message = _appConst.DeleteSuccess
            };
        }
        #endregion

        #region GetOtherChargeForSelectList
        public List<OtherCharge> GetOtherChargeForSelectList()
        {
            List<OtherCharge> otherChargeList = null;
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
                        cmd.CommandText = "[PSA].[GetOtherChargeForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                otherChargeList = new List<OtherCharge>();
                                while (sdr.Read())
                                {
                                    OtherCharge otherCharge = new OtherCharge();
                                    {
                                        otherCharge.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : otherCharge.Code);
                                        otherCharge.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : otherCharge.Description);
                                    }
                                    otherChargeList.Add(otherCharge);
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

            return otherChargeList;
        }
        #endregion

    }
}
