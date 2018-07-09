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
    public class SpareRepository: ISpareRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public SpareRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetAllSpare
        public List<Spare> GetAllSpare(SpareAdvanceSearch spareAdvanceSearch)
        {
            List<Spare> spareList = null;
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
                        cmd.CommandText = "[PSA].[GetAllSpare]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(spareAdvanceSearch.SearchTerm) ? "" : spareAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = spareAdvanceSearch.DataTablePaging.Start;
                        if (spareAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = spareAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                spareList = new List<Spare>();
                                while (sdr.Read())
                                {
                                    Spare spare = new Spare();
                                    {
                                        spare.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : spare.ID);
                                        spare.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : spare.Code);
                                        spare.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : spare.Name);
                                        spare.HSNCode = (sdr["HSNCode"].ToString() != "" ? sdr["HSNCode"].ToString() : spare.HSNCode);
                                        spare.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : spare.TotalCount);
                                        spare.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : spare.FilteredCount);
                                    }
                                    spareList.Add(spare);
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
            return spareList;
        }
        #endregion GetAllSpare

        #region GetSpareCode
        public string GetSpareCode()
        {
            SqlParameter outputCode = null;
            string code;
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
                        cmd.CommandText = "[PSA].[GetSpareCode]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar, 50);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        code = outputCode.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return code;
        }
        #endregion GetSpareCode

        #region InsertUpdateSpare
        public object InsertUpdateSpare(Spare spare)
        {
            SqlParameter outputStatus, outputID = null;
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
                        cmd.CommandText = "[PSA].[InsertUpdateSpare]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = spare.IsUpdate;
                        if (spare.ID == Guid.Empty)
                        {
                            cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = spare.ID;
                        }
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = spare.Code.Replace("-", "_ ");
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = spare.Name.Replace("-", "_ ");
                        cmd.Parameters.Add("@HSNCode", SqlDbType.VarChar).Value = spare.HSNCode;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = spare.hdnPopupFileID;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = spare.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = spare.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = spare.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = spare.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(spare.IsUpdate ? _appConstant.UpdateFailure : _appConstant.InsertFailure);
                    case "1":
                        spare.ID = Guid.Parse(outputID.Value.ToString());
                        return new
                        {
                            Code = outputID.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = spare.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                ID = spare.ID,
                Status = outputStatus.Value.ToString(),
                Message = spare.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion InsertUpdateSpare

        #region CheckSpareCodeExist
        public bool CheckSpareCodeExist(Spare spare)
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
                        cmd.CommandText = "[PSA].[CheckSpareCodeExist]";
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar).Value = spare.Code;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = spare.ID;
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
        #endregion CheckSpareCodeExist

        #region GetSpare
        public Spare GetSpare(Guid id)
        {
            Spare spare = null;
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
                        cmd.CommandText = "[PSA].[GetSpare]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    spare = new Spare();
                                    spare.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : spare.ID);
                                    spare.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : spare.Code);
                                    spare.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : spare.Name);
                                    spare.HSNCode = (sdr["HSNCode"].ToString() != "" ? sdr["HSNCode"].ToString() : spare.HSNCode);
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
            return spare;
        }
        #endregion GetSpare

        #region DeleteSpare
        public object DeleteSpare(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteSpare]";
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
        #endregion DeleteSpare

        #region GetSpareForSelectList
        public List<Spare> GetSpareForSelectList()
        {
            List<Spare> spareList = null;
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
                        cmd.CommandText = "[PSA].[GetSpareForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                spareList = new List<Spare>();
                                while (sdr.Read())
                                {
                                    Spare spare = new Spare();
                                    {
                                        spare.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : spare.ID);
                                        spare.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : spare.Code);
                                        spare.Name = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : spare.Name);
                                    }
                                    spareList.Add(spare);
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
            return spareList;
        }
        #endregion

    }
}
