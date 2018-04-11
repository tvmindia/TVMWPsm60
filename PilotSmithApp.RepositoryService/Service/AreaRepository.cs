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
    public class AreaRepository:IAreaRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        //public ConnectionState Connectionstate { get; private set; }

        public AreaRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region InsertUpdateArea
        public object InsertUpdateArea(Area area)
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
                        cmd.CommandText = "[PSA].[InsertUpdateArea]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = area.IsUpdate;
                        if (area.Code == 0)

                            cmd.Parameters.AddWithValue("@Code", DBNull.Value);

                        else
                            cmd.Parameters.Add("@Code", SqlDbType.Int).Value = area.Code;

                        cmd.Parameters.Add("@StateCode", SqlDbType.Int).Value = area.StateCode;
                        cmd.Parameters.Add("@DistrictCode", SqlDbType.Int).Value = area.DistrictCode;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 200).Value = area.Description;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = area.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = area.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = area.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = area.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar, 5);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(area.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        area.Code = int.Parse(outputCode.Value.ToString());
                        return new
                        {
                            Code = outputCode.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = area.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = area.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion

        #region GetAllArea
        public List<Area> GetAllArea(AreaAdvanceSearch areaAdvanceSearch)
        {
            List<Area> areaList = null;
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
                        cmd.CommandText = "[PSA].[GetAllArea]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(areaAdvanceSearch.SearchTerm) ? "" : areaAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = areaAdvanceSearch.DataTablePaging.Start;
                        if (areaAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = areaAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                areaList = new List<Area>();
                                while (sdr.Read())
                                {
                                    Area area = new Area();
                                    {
                                        area.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : area.Code);
                                        area.State = new State();
                                        area.State.Description = (sdr["State"].ToString() != "" ? (sdr["State"].ToString()) : area.State.Description);
                                        area.District = new District();
                                        area.District.Description = (sdr["District"].ToString() != "" ? (sdr["District"].ToString()) : area.District.Description);
                                        area.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : area.Description);
                                        area.PSASysCommon = new PSASysCommon();
                                        area.PSASysCommon.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(settings.DateFormat) : area.PSASysCommon.CreatedDateString);
                                        area.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : area.TotalCount);
                                        area.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : area.FilteredCount);
                                    }
                                    areaList.Add(area);
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
            return areaList;
        }
        #endregion

        #region GetArea
        public Area GetArea(int code)
        {
            Area area = null;
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
                        cmd.CommandText = "[PSA].[GetArea]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    area = new Area();
                                    area.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : area.Code);
                                    area.StateCode = (sdr["StateCode"].ToString() != "" ? int.Parse(sdr["StateCode"].ToString()) : area.StateCode);
                                    area.DistrictCode = (sdr["DistrictCode"].ToString() != "" ? int.Parse(sdr["DistrictCode"].ToString()) : area.DistrictCode);
                                    area.State = new State();
                                    area.State.Code= (sdr["StateCode"].ToString() != "" ? int.Parse(sdr["StateCode"].ToString()) : area.State.Code);
                                    area.District = new District();
                                    area.District.Code = (sdr["DistrictCode"].ToString() != "" ? int.Parse(sdr["DistrictCode"].ToString()) : area.District.Code);
                                    area.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : area.Description);
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
            return area;
        }
        #endregion

        #region CheckAreaNameExist
        public bool CheckAreaNameExist(Area area)
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
                        cmd.CommandText = "[PSA].[CheckAreaNameExist]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = area.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = area.Description;
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

        #region DeleteArea
        public object DeleteArea(int code)
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
                        cmd.CommandText = "[PSA].[DeleteArea]";
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

        #region GetAreaForSelectList
        public List<Area> GetAreaForSelectList()
        {
            List<Area> areaList = null;
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
                        cmd.CommandText = "[PSA].[GetAreaForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                areaList = new List<Area>();
                                while (sdr.Read())
                                {
                                    Area area = new Area();
                                    {
                                        area.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : area.Code);
                                        area.StateCode = (sdr["StateCode"].ToString() != "" ? int.Parse(sdr["StateCode"].ToString()) : area.StateCode);
                                        area.DistrictCode = (sdr["DistrictCode"].ToString() != "" ? int.Parse(sdr["DistrictCode"].ToString()) : area.DistrictCode);
                                        area.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : area.Description);
                                    }
                                    areaList.Add(area);
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
            return areaList;
        }
        #endregion
    }
}
