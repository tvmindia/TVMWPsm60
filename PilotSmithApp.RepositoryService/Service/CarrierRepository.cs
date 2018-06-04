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
    public class CarrierRepository : ICarrierRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public CarrierRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region GetCarrierForSelectList
        public List<Carrier> GetCarrierForSelectList()
        {
            List<Carrier> carrierList = null;
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
                        cmd.CommandText = "[PSA].[GetCarrierForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                carrierList = new List<Carrier>();
                                while (sdr.Read())
                                {
                                    Carrier carrier = new Carrier();
                                    {

                                        carrier.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : carrier.Code);
                                        carrier.Name = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : carrier.Name);
                                    }
                                    carrierList.Add(carrier);
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
            return carrierList;
        }
        #endregion GetCarrierForSelectList
    }
}
