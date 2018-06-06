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
    public class BillLocationRepository : IBillLocationRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public BillLocationRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<BillLocation> GetBillLocationForSelectList()
        {
            List<BillLocation> billLocationList = null;
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
                        cmd.CommandText = "[PSA].[GetBillLocationForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                      //  cmd.Parameters.Add("@LoginName", SqlDbType.VarChar, 250).Value = loginName;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                billLocationList = new List<BillLocation>();
                                while (sdr.Read())
                                {
                                    BillLocation billLocation = new BillLocation();
                                    {
                                        billLocation.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : billLocation.Code);
                                        billLocation.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : billLocation.Name);
                                        billLocation.Address = (sdr["Address"].ToString() != "" ? sdr["Address"].ToString() : billLocation.Address);
                                    }
                                    billLocationList.Add(billLocation);
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
            return billLocationList;
        }
    }
}
