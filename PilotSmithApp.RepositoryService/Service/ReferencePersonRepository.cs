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

        #region InsertUpdateReferencePerson
        public object InsertUpdateReferencePerson(ReferencePerson referencePerson)
        {
            SqlParameter outputStatus, outputCode = null;
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
                        cmd.CommandText = "[PSA].[InsertUpdateReferencePerson]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = referencePerson.IsUpdate;
                        if (referencePerson.Code == 0)

                            cmd.Parameters.AddWithValue("@Code", DBNull.Value);

                        else
                            cmd.Parameters.Add("@Code", SqlDbType.Int).Value = referencePerson.Code;

                        cmd.Parameters.Add("@Name", SqlDbType.VarChar,50).Value = referencePerson.Name;
                        cmd.Parameters.Add("@ReferenceTypeCode", SqlDbType.Int).Value = referencePerson.ReferenceTypeCode;
                        cmd.Parameters.Add("@AreaCode", SqlDbType.Int).Value = referencePerson.AreaCode;
                        cmd.Parameters.Add("@Organization", SqlDbType.VarChar, 50).Value = referencePerson.Organization;
                        cmd.Parameters.Add("@Address", SqlDbType.VarChar, 500).Value = referencePerson.Address;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar, 150).Value = referencePerson.Email;
                        cmd.Parameters.Add("@PhoneNos", SqlDbType.VarChar, 250).Value = referencePerson.PhoneNos;
                        cmd.Parameters.Add("@FaxNos", SqlDbType.VarChar, 100).Value = referencePerson.FaxNos;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = referencePerson.GeneralNotes;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = referencePerson.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = referencePerson.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = referencePerson.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = referencePerson.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar, 5);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(referencePerson.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        referencePerson.Code = int.Parse(outputCode.Value.ToString());
                        return new
                        {
                            Code = outputCode.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = referencePerson.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Code = outputCode.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = referencePerson.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion

        #region GetAllReferencePerson
        public List<ReferencePerson> GetAllReferencePerson(ReferencePersonAdvanceSearch referencePersonAdvanceSearch)
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
                        cmd.CommandText = "[PSA].[GetAllReferencePerson]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(referencePersonAdvanceSearch.SearchTerm) ? "" : referencePersonAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = referencePersonAdvanceSearch.DataTablePaging.Start;
                        if (referencePersonAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = referencePersonAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@ReferenceTypeCode", SqlDbType.Int).Value = referencePersonAdvanceSearch.ReferenceTypeCode;
                        cmd.Parameters.Add("@AreaCode", SqlDbType.Int).Value = referencePersonAdvanceSearch.AreaCode;
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
                                        referencePerson.ReferenceType = new ReferenceType();
                                        referencePerson.ReferenceType.Description = (sdr["ReferenceType"].ToString() != "" ? (sdr["ReferenceType"].ToString()) : referencePerson.ReferenceType.Description);
                                        referencePerson.Area = new Area();
                                        referencePerson.Area.Description = (sdr["Area"].ToString() != "" ? (sdr["Area"].ToString()) : referencePerson.Area.Description);
                                        referencePerson.Organization = (sdr["Organization"].ToString() != "" ? sdr["Organization"].ToString() : referencePerson.Organization);
                                        referencePerson.Address = (sdr["Address"].ToString() != "" ? sdr["Address"].ToString() : referencePerson.Address);
                                        referencePerson.Email = (sdr["Email"].ToString() != "" ? sdr["Email"].ToString() : referencePerson.Email);
                                        referencePerson.PhoneNos = (sdr["PhoneNos"].ToString() != "" ? sdr["PhoneNos"].ToString() : referencePerson.PhoneNos);
                                        referencePerson.FaxNos = (sdr["FaxNos"].ToString() != "" ? sdr["FaxNos"].ToString() : referencePerson.FaxNos);
                                        referencePerson.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : referencePerson.GeneralNotes);
                                       // referencePerson.PSASysCommon = new PSASysCommon();
                                       // referencePerson.PSASysCommon.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(settings.DateFormat) : referencePerson.PSASysCommon.CreatedDateString);
                                        referencePerson.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : referencePerson.TotalCount);
                                        referencePerson.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : referencePerson.FilteredCount);
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
        #endregion

        #region GetReferencePerson
        public ReferencePerson GetReferencePerson(int code)
        {
            ReferencePerson referencePerson = null;
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
                        cmd.CommandText = "[PSA].[GetReferencePerson]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    referencePerson = new ReferencePerson();
                                    referencePerson.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : referencePerson.Code);
                                    referencePerson.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : referencePerson.Name);
                                    referencePerson.ReferenceTypeCode = (sdr["ReferenceTypeCode"].ToString() != "" ? int.Parse(sdr["ReferenceTypeCode"].ToString()) : referencePerson.ReferenceTypeCode);
                                    referencePerson.AreaCode = (sdr["AreaCode"].ToString() != "" ? int.Parse(sdr["AreaCode"].ToString()) : referencePerson.AreaCode);
                                    referencePerson.Organization = (sdr["Organization"].ToString() != "" ? (sdr["Organization"].ToString()) : referencePerson.Organization);
                                    referencePerson.Address = (sdr["Address"].ToString() != "" ? (sdr["Address"].ToString()) : referencePerson.Address);
                                    referencePerson.Email = (sdr["Email"].ToString() != "" ? (sdr["Email"].ToString()) : referencePerson.Email);
                                    referencePerson.PhoneNos = (sdr["PhoneNos"].ToString() != "" ? (sdr["PhoneNos"].ToString()) : referencePerson.PhoneNos);
                                    referencePerson.FaxNos = (sdr["FaxNos"].ToString() != "" ? (sdr["FaxNos"].ToString()) : referencePerson.FaxNos);
                                    referencePerson.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? (sdr["GeneralNotes"].ToString()) : referencePerson.GeneralNotes);
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
            return referencePerson;
        }
        #endregion

        #region CheckReferencePersonNameExist
        public bool CheckReferencePersonNameExist(ReferencePerson referencePerson)
        {
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
                        cmd.CommandText = "[PSA].[CheckReferencePersonNameExist]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = referencePerson.Code;
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = referencePerson.Name;
                        cmd.CommandType = CommandType.StoredProcedure;
                        Object res = cmd.ExecuteScalar();
                        return (res.ToString() == "Exists" ? true : false);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DeleteReferencePerson
        public object DeleteReferencePerson(int code)
        {
            SqlParameter outputStatus = null;
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
                        cmd.CommandText = "[PSA].[DeleteReferencePerson]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = code;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(_appConst.DeleteFailure);

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConst.DeleteSuccess
            };
        }
        #endregion

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
