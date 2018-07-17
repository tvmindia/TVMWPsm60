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
    public class DistrictRepository:IDistrictRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        //public ConnectionState Connectionstate { get; private set; }

        public DistrictRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region InsertUpdateDistrict
        public object InsertUpdateDistrict(District district)
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
                        cmd.CommandText = "[PSA].[InsertUpdateDistrict]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = district.IsUpdate;
                        if (district.Code == 0)

                            cmd.Parameters.AddWithValue("@Code", DBNull.Value);

                        else
                            cmd.Parameters.Add("@Code", SqlDbType.Int).Value = district.Code;
                        cmd.Parameters.Add("@CountryCode", SqlDbType.Int).Value = district.CountryCode;
                        cmd.Parameters.Add("@StateCode", SqlDbType.Int).Value = district.StateCode;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 200).Value = district.Description;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = district.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = district.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = district.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = district.PSASysCommon.UpdatedDate;
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
                        throw new Exception(district.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        district.Code = int.Parse(outputCode.Value.ToString());
                        return new
                        {
                            Code = outputCode.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = district.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = district.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion

        #region GetAllDistrict
        public List<District>GetAllDistrict(DistrictAdvanceSearch districtAdvanceSearch)
        {
            List<District> districtList = null;
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
                        cmd.CommandText = "[PSA].[GetAllDistrict]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(districtAdvanceSearch.SearchTerm) ? "" : districtAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = districtAdvanceSearch.DataTablePaging.Start;
                        if (districtAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = districtAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                districtList = new List<District>();
                                while (sdr.Read())
                                {
                                    District district = new District();
                                    {
                                        district.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : district.Code);
                                        district.Country = new Country();
                                        district.Country.Description= (sdr["Country"].ToString() != "" ? (sdr["Country"].ToString()) : district.Country.Description);
                                        district.State = new State();                                 
                                        district.State.Description = (sdr["State"].ToString() != "" ? (sdr["State"].ToString()) : district.State.Description);
                                        district.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : district.Description);
                                        district.PSASysCommon = new PSASysCommon();
                                        district.PSASysCommon.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(settings.DateFormat) : district.PSASysCommon.CreatedDateString);
                                        district.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : district.TotalCount);
                                        district.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : district.FilteredCount);
                                    }
                                    districtList.Add(district);
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
            return districtList;
        }
        #endregion

        #region GetDistrict
        public District GetDistrict(int code)
        {
            District district = null;
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
                        cmd.CommandText = "[PSA].[GetDistrict]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    district = new District();
                                    district.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : district.Code);
                                    district.StateCode = (sdr["StateCode"].ToString() != "" ? int.Parse(sdr["StateCode"].ToString()) : district.StateCode);
                                    district.CountryCode= (sdr["CountryCode"].ToString() != "" ? int.Parse(sdr["CountryCode"].ToString()) : district.CountryCode);
                                    district.State = new State();
                                    district.State.Code = (sdr["StateCode"].ToString() != "" ? int.Parse(sdr["StateCode"].ToString()) : district.State.Code);
                                   // district.State.Description = (sdr["State"].ToString() != "" ? (sdr["State"].ToString()) : district.State.Description);
                                    district.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : district.Description);
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
            return district;
        }
        #endregion

        #region CheckDistrictNameExist
        public bool CheckDistrictNameExist(District district)
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
                        cmd.CommandText = "[PSA].[CheckDistrictNameExist]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = district.Code;
                        cmd.Parameters.Add("@Description",SqlDbType.NVarChar).Value=district.Description;
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

        #region DeleteDistrict
        public object DeleteDistrict(int code)
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
                        cmd.CommandText = "[PSA].[DeleteDistrict]";
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

        #region GetDistrictForSelectList
        public List<District> GetDistrictForSelectList(int? stateCode, int? countryCode)
        {
            List<District> districtList = null;
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
                        cmd.CommandText = "[PSA].[GetDistrictForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CountryCode", SqlDbType.Int).Value = countryCode;
                        cmd.Parameters.Add("@StateCode", SqlDbType.Int).Value = stateCode;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                districtList = new List<District>();
                                while (sdr.Read())
                                {
                                    District district = new District();
                                    {
                                        district.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : district.Code);
                                        district.CountryCode= (sdr["CountryCode"].ToString() != "" ? int.Parse(sdr["CountryCode"].ToString()) : district.CountryCode);
                                        district.StateCode = (sdr["StateCode"].ToString() != "" ? int.Parse(sdr["StateCode"].ToString()) : district.StateCode);
                                        district.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : district.Description);
                                    }
                                    districtList.Add(district);
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
            return districtList;
        }
        #endregion
    }
}
