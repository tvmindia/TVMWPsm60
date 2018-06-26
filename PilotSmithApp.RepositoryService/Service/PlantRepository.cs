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
    public class PlantRepository: IPlantRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public PlantRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region InsertUpdatePlant
        public object InsertUpdatePlant(Plant plant)
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
                        cmd.CommandText = "[PSA].[InsertUpdatePlant]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = plant.IsUpdate;
                        if (plant.Code == 0)

                            cmd.Parameters.AddWithValue("@Code", DBNull.Value);

                        else
                            cmd.Parameters.Add("@Code", SqlDbType.Int).Value = plant.Code;

                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 250).Value = plant.Description;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 250).Value = plant.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = plant.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 250).Value = plant.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = plant.PSASysCommon.UpdatedDate;
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
                        throw new Exception(plant.IsUpdate ? _appConstant.UpdateFailure : _appConstant.InsertFailure);
                    case "1":
                        plant.Code = int.Parse(outputCode.Value.ToString());
                        return new
                        {
                            Code = outputCode.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = plant.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                Message = plant.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion

        #region GetAllPlant
        public List<Plant> GetAllPlant(PlantAdvanceSearch plantAdvanceSearch)
        {
            List<Plant> plantList = null;
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
                        cmd.CommandText = "[PSA].[GetAllPlant]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(plantAdvanceSearch.SearchTerm) ? "" : plantAdvanceSearch.SearchTerm.Trim();
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = plantAdvanceSearch.DataTablePaging.Start;
                        if (plantAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = plantAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                plantList = new List<Plant>();
                                while (sdr.Read())
                                {
                                    Plant plant = new Plant();
                                    {
                                        plant.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : plant.Code);
                                        plant.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : plant.Description);
                                        plant.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : plant.TotalCount);
                                        plant.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : plant.FilteredCount);
                                    }
                                    plantList.Add(plant);
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
            return plantList;
        }
        #endregion

        #region GetPlant
        public Plant GetPlant(int code)
        {
            Plant plant = null;
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
                        cmd.CommandText = "[PSA].[GetPlant]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    plant = new Plant();
                                    plant.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : plant.Code);
                                    plant.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : plant.Description);
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
            return plant;
        }
        #endregion

        #region CheckPlantNameExist
        public bool CheckPlantNameExist(Plant plant)
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
                        cmd.CommandText = "[PSA].[CheckPlantExist]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = plant.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 250).Value = plant.Description;
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

        #region DeletePlant
        public object DeletePlant(int code)
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
                        cmd.CommandText = "[PSA].[DeletePlant]";
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
        #endregion

        #region Get Plant SelectList
        public List<Plant> GetPlantForSelectList()
        {
            List<Plant> plantList = null;
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
                        cmd.CommandText = "[PSA].[GetPlantForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                plantList = new List<Plant>();
                                while (sdr.Read())
                                {
                                    Plant plant = new Plant();
                                    {
                                        plant.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : plant.Code);
                                        plant.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : plant.Description);
                                    }
                                    plantList.Add(plant);
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
            return plantList;
        }
        #endregion Get Plant SelectList
    }
}
