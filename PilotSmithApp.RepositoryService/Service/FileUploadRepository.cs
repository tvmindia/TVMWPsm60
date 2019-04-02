using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web;

namespace PilotSmithApp.RepositoryService.Service
{
    public class FileUploadRepository: IFileUploadRepository
    {

        private IDatabaseFactory _databaseFactory;
        public FileUploadRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public FileUpload InsertAttachment(FileUpload fileUpload)
        {
            try
            {
                SqlParameter outputStatus, outputParentID, outputID = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[InsertAttachment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ParentID", SqlDbType.UniqueIdentifier).Value = fileUpload.ParentID;
                        cmd.Parameters.Add("@ParentType", SqlDbType.VarChar, 20).Value = fileUpload.ParentType;
                        cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 255).Value = fileUpload.FileName;
                        cmd.Parameters.Add("@FileType", SqlDbType.VarChar, 5).Value = fileUpload.FileType;
                        cmd.Parameters.Add("@FileSize", SqlDbType.VarChar, 50).Value = fileUpload.FileSize;
                        cmd.Parameters.Add("@AttachmentURL", SqlDbType.NVarChar, -1).Value = fileUpload.AttachmentURL;
                        cmd.Parameters.Add("@IsDocumentApprover", SqlDbType.Bit).Value = fileUpload.IsDocumentApprover;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = fileUpload.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = fileUpload.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = fileUpload.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = fileUpload.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        outputParentID = cmd.Parameters.Add("@DemoID", SqlDbType.UniqueIdentifier);
                        outputParentID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        fileUpload.ID = Guid.Parse(outputID.Value.ToString());
                        fileUpload.ParentID = Guid.Parse(outputParentID.Value.ToString());
                        break;
                    case "2":
                        AppConst Cobj1 = new AppConst();
                        throw new Exception(Cobj1.Duplicate);
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return fileUpload;
        }
        public List<FileUpload> GetAttachments(Guid id)
        {
            List<FileUpload> fileUploadList = null;
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
                        cmd.CommandText = "[PSA].[GetAttachments]";
                        cmd.Parameters.Add("@ParentID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                fileUploadList = new List<FileUpload>();
                                while (sdr.Read())
                                {
                                    FileUpload fileUpload = new FileUpload();
                                    {
                                        fileUpload.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : fileUpload.ID);
                                        fileUpload.ParentID = (sdr["ParentID"].ToString() != "" ? Guid.Parse(sdr["ParentID"].ToString()) : fileUpload.ParentID);
                                        fileUpload.ParentType = (sdr["ParentType"].ToString());
                                        fileUpload.FileName = sdr["FileName"].ToString();
                                        fileUpload.FileType = sdr["FileType"].ToString();
                                        fileUpload.FileSize = sdr["FileSize"].ToString();
                                        fileUpload.AttachmentURL = sdr["AttachmentURL"].ToString();
                                        fileUpload.DocumentOwners = (sdr["DocumentOwners"].ToString() != "" ? (sdr["DocumentOwners"].ToString()).Split(',') : fileUpload.DocumentOwners);
                                    }
                                    fileUploadList.Add(fileUpload);
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

            return fileUploadList;
        }
        public object DeleteFile(Guid id,string CreatedBy,DateTime CreatedDate)
        {
            AppConst Cobj = new AppConst();
            try
            {
                SqlParameter outputStatus = null;
                SqlParameter OutparameterURL = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[DeleteFile]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        OutparameterURL = cmd.Parameters.Add("@AttacmentURL", SqlDbType.NVarChar, -1);
                        OutparameterURL.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "1":
                        if (OutparameterURL.Value.ToString() != "")
                        {
                            System.IO.File.Delete(HttpContext.Current.Server.MapPath(OutparameterURL.Value.ToString()));
                        }
                        break;
                    case "0":
                        throw new Exception(Cobj.InsertFailure);
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new { Message = Cobj.DeleteSuccess };
        }
    }
}