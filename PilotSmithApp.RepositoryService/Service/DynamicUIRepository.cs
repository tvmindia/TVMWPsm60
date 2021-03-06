﻿using PilotSmithApp.RepositoryService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;

namespace PilotSmithApp.RepositoryService.Service
{
    public class DynamicUIRepository: IDynamicUIRepository
    {
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public DynamicUIRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }



        public List<PSASysMenu> GetAllMenues()
        {
            List<PSASysMenu> menuList = null;
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
                        cmd.CommandText = "[PSA].[GetAllMenuItem]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                menuList = new List<PSASysMenu>();
                                while (sdr.Read())
                                {
                                    PSASysMenu menuObj = new PSASysMenu();
                                    {
                                        menuObj.ID = (sdr["ID"].ToString() != "" ? Int16.Parse(sdr["ID"].ToString()) : menuObj.ID);
                                        menuObj.ParentID = (sdr["ParentID"].ToString() != "" ? Int16.Parse(sdr["ParentID"].ToString()) : menuObj.ParentID);
                                        menuObj.MenuText = sdr["MenuText"].ToString();
                                        menuObj.Controller = sdr["Controller"].ToString();
                                        menuObj.Action = sdr["Action"].ToString();
                                        menuObj.Parameters = sdr["Parameters"].ToString();
                                        menuObj.IconClass = sdr["IconClass"].ToString();
                                        menuObj.IconURL = sdr["IconURL"].ToString();                                        
                                            
                                    }
                                    menuList.Add(menuObj);
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

            return menuList;
        }

        public Boolean IsFileExisting(Guid id, string doctype)
        {
            bool result;
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
                        cmd.CommandText = "[PSA].[IsFileExisting]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.Parameters.Add("@DocType", SqlDbType.NVarChar, -1).Value = doctype;
                        cmd.CommandType = CommandType.StoredProcedure;
                        result = bool.Parse(cmd.ExecuteScalar().ToString());
                     
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}