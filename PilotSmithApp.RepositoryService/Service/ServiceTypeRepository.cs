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
    public class ServiceTypeRepository: IServiceTypeRepository
    {

        private IDatabaseFactory _databaseFactory;
        public ServiceTypeRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region Get ServiceType SelectList
        public List<ServiceType> GetServiceTypeSelectList()
        {
            List<ServiceType> ServiceTypeList = null;
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
                        cmd.CommandText = "[PSA].[GetServiceTypeForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ServiceTypeList = new List<ServiceType>();
                                while (sdr.Read())
                                {
                                    ServiceType ServiceType = new ServiceType();
                                    {
                                        ServiceType.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : ServiceType.Code);
                                        ServiceType.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : ServiceType.Name);
                                    }
                                    ServiceTypeList.Add(ServiceType);
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
            return ServiceTypeList;
        }
        #endregion Get ServiceType SelectList
    }
}
