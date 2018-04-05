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
    public class DocumentTypeRepository: IDocumentTypeRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public DocumentTypeRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region Get DocumentType Dropdown
        public List<DocumentType> GetDocumentTypeSelectList()
        {
            List<DocumentType> documentTypeList = null;
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
                        cmd.CommandText = "[PSA].[GetDocumentTypeForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                documentTypeList = new List<DocumentType>();
                                while (sdr.Read())
                                {
                                    DocumentType documentType = new DocumentType();
                                    {
                                        documentType.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : documentType.Code);
                                        documentType.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : documentType.Description);
                                    }
                                    documentTypeList.Add(documentType);
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
            return documentTypeList;
        }
        #endregion Get DocumentType Dropdown
    }
}
