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
    public class DocumentStatusRepository: IDocumentStatusRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public DocumentStatusRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region Get DocumentStatus SelectList
        public List<DocumentStatus> GetDocumentStatusSelectList(string code)
        {
            List<DocumentStatus> documentStatusList = null;
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
                        cmd.CommandText = "[PSA].[GetDocumentStatusForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DocumentTypeCode", SqlDbType.VarChar, 5).Value = code;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                documentStatusList = new List<DocumentStatus>();
                                while (sdr.Read())
                                {
                                    DocumentStatus documentStatus = new DocumentStatus();
                                    {
                                        documentStatus.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : documentStatus.Code);
                                        documentStatus.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : documentStatus.Description);
                                    }
                                    documentStatusList.Add(documentStatus);
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
            return documentStatusList;
        }
        #endregion Get DocumentStatus SelectList
    }
}
