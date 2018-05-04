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
