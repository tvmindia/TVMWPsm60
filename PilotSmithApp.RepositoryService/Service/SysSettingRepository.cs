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
    public class SysSettingRepository: ISysSettingRepository
    {

        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public SysSettingRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetAllSysSetting
        public List<SysSetting> GetAllSysSetting(SysSettingAdvanceSearch sysSettingAdvanceSearch)
        {
            List<SysSetting> sysSettingList = null;
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
                        cmd.CommandText = "[PSA].[GetAllSysSetting]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(sysSettingAdvanceSearch.SearchTerm) ? "" : sysSettingAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = sysSettingAdvanceSearch.DataTablePaging.Start;
                        if (sysSettingAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = sysSettingAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                sysSettingList = new List<SysSetting>();
                                while (sdr.Read())
                                {
                                    SysSetting sysSetting = new SysSetting();
                                    {
                                        sysSetting.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : sysSetting.ID);
                                        sysSetting.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : sysSetting.Name);
                                        sysSetting.Value = (sdr["Value"].ToString() != "" ? sdr["Value"].ToString() : sysSetting.Value);
                                        sysSetting.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : sysSetting.TotalCount);
                                        sysSetting.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : sysSetting.FilteredCount);
                                    }
                                    sysSettingList.Add(sysSetting);
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
            return sysSettingList;
        }
        #endregion GetAllSysSetting
        
        #region InsertUpdateSysSetting
        public object InsertUpdateSysSetting(SysSetting sysSetting)
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
                        cmd.CommandText = "[PSA].[InsertUpdateSysSetting]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = sysSetting.IsUpdate;
                        if (sysSetting.ID == Guid.Empty)
                        {
                            cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = sysSetting.ID;
                        }
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = sysSetting.Name.Replace("-", "_ ");
                        cmd.Parameters.Add("@Value", SqlDbType.VarChar).Value = sysSetting.Value;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = sysSetting.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = sysSetting.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = sysSetting.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = sysSetting.PSASysCommon.UpdatedDate;
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
                        throw new Exception(sysSetting.IsUpdate ? _appConstant.UpdateFailure : _appConstant.InsertFailure);
                    case "1":
                        sysSetting.ID = Guid.Parse(outputID.Value.ToString());
                        return new
                        {
                            Code = outputID.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = sysSetting.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                ID = sysSetting.ID,
                Status = outputStatus.Value.ToString(),
                Message = sysSetting.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion InsertUpdateSysSetting
        
        #region GetSysSetting
        public SysSetting GetSysSetting(Guid id)
        {
            SysSetting sysSetting = null;
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
                        cmd.CommandText = "[PSA].[GetSysSetting]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    sysSetting = new SysSetting();
                                    sysSetting.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : sysSetting.ID);
                                    sysSetting.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : sysSetting.Name);
                                    sysSetting.Value = (sdr["Value"].ToString() != "" ? sdr["Value"].ToString() : sysSetting.Value);
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
            return sysSetting;
        }
        #endregion GetSysSetting

        #region DeleteSysSetting
        public object DeleteSysSetting(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteSysSetting]";
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
        #endregion DeleteSysSetting

    }
}
