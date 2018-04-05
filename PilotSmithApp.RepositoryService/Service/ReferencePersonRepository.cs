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
    public class ReferencePersonRepository: IReferencePersonRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ReferencePersonRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region Get ReferencePerson Dropdown
        public List<ReferencePerson> GetReferencePersonSelectList()
        {
            List<ReferencePerson> referencePersonList = null;
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
                        cmd.CommandText = "[PSA].[GetReferencePersonForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                referencePersonList = new List<ReferencePerson>();
                                while (sdr.Read())
                                {
                                    ReferencePerson referencePerson = new ReferencePerson();
                                    {
                                        referencePerson.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : referencePerson.Code);
                                        referencePerson.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : referencePerson.Name);
                                    }
                                    referencePersonList.Add(referencePerson);
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
            return referencePersonList;
        }
        #endregion Get ReferencePerson Dropdown
    }
}
