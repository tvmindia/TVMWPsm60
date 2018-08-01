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
    public class TakeOwnershipRepository: ITakeOwnershipRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public TakeOwnershipRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region InsertTakeOwnership
        public DocumentLog InsertTakeOwnership(DocumentLog documentLog)
        {
            SqlParameter outputStatus, outputCode, emailID, oldUserName , documentID, newDocumentOwner = null;
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
                        cmd.CommandText = "[PSA].[InsertTakeOwnership]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DocumentNo", SqlDbType.VarChar).Value = documentLog.DocumentNo;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = documentLog.Type;
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = documentLog.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = documentLog.Remarks;
                        cmd.Parameters.Add("@DocumentType", SqlDbType.VarChar).Value = documentLog.DocType;
                        cmd.Parameters.Add("@LoginName", SqlDbType.NVarChar, 250).Value = documentLog.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = documentLog.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = documentLog.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = documentLog.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = documentLog.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.Int);
                        outputCode.Direction = ParameterDirection.Output;
                        oldUserName = cmd.Parameters.Add("@oldUserName", SqlDbType.NVarChar,250);
                        oldUserName.Direction = ParameterDirection.Output;
                        emailID = cmd.Parameters.Add("@emailID", SqlDbType.NVarChar,250);
                        emailID.Direction = ParameterDirection.Output;
                        documentID = cmd.Parameters.Add("@DocumentID", SqlDbType.UniqueIdentifier);
                        documentID.Direction = ParameterDirection.Output;
                        newDocumentOwner = cmd.Parameters.Add("@NewDocumentOwner", SqlDbType.NVarChar,250);
                        newDocumentOwner.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception( _appConstant.InsertFailure);
                    case "1":
                        documentLog.Code = int.Parse(outputCode.Value.ToString());
                        documentLog.OldUserName = oldUserName.Value.ToString();
                        documentLog.OldUserEmail= emailID.Value.ToString();
                        documentLog.DateFormatted = DateTime.Parse(documentLog.PSASysCommon.CreatedDate.ToString()).ToString("dd-MMM-yyyy hh:mm tt");
                        documentLog.DocumentID= Guid.Parse(documentID.Value.ToString());
                        documentLog.NewDocumentOwner= newDocumentOwner.Value.ToString();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
             {
                throw ex;
            }
            return documentLog;
        }
        #endregion InsertTakeOwnership
        public List<DocumentLog> GetOwnershipHistory(Guid documentID, string documentTypeCode)
        {
            List<DocumentLog> ownershipHistoryList = null;
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
                        cmd.CommandText = "[PSA].[GetDocumentOwnershipHistory]";
                        cmd.Parameters.Add("@DocumentID", SqlDbType.UniqueIdentifier).Value = documentID;
                        cmd.Parameters.Add("@DocumentTypeCode", SqlDbType.NVarChar, 5).Value = documentTypeCode;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ownershipHistoryList = new List<DocumentLog>();
                                while (sdr.Read())
                                {
                                    DocumentLog ownershipHistory = new DocumentLog();
                                    {
                                        ownershipHistory.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()) : ownershipHistory.Date);
                                        ownershipHistory.DateFormatted = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(_settings.DateFormat) : ownershipHistory.DateFormatted);
                                        ownershipHistory.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : ownershipHistory.Type);
                                        ownershipHistory.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : ownershipHistory.Remarks);
                                    }
                                    ownershipHistoryList.Add(ownershipHistory);
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
            return ownershipHistoryList;


        }
    }
}
