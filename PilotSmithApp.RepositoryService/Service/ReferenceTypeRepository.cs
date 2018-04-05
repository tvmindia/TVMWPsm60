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
    public class ReferenceTypeRepository: IReferenceTypeRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ReferenceTypeRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region Get ReferenceType Dropdown
        public List<ReferenceType> GetReferenceTypeSelectList()
        {
            List<ReferenceType> referenceTypeList = null;
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
                        cmd.CommandText = "[PSA].[GetReferenceTypeForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                referenceTypeList = new List<ReferenceType>();
                                while (sdr.Read())
                                {
                                    ReferenceType referenceType = new ReferenceType();
                                    {
                                        referenceType.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : referenceType.Code);
                                        referenceType.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : referenceType.Description);
                                    }
                                    referenceTypeList.Add(referenceType);
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
            return referenceTypeList;
        }
        #endregion Get ReferenceType Dropdown
    }
}
