﻿using PilotSmithApp.DataAccessObject.DTO;
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
    public class CustomerRepository:ICustomerRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public CustomerRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region Check Company Name Exist For Customer
        public bool CheckCompanyNameExistForCustomer(Customer customer)
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
                        cmd.CommandText = "[PSA].[CheckCompanyNameExistForCustomer]";
                        cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = customer.CompanyName;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = customer.ID;
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
        #endregion Check Company Name Exist For Customer
        #region Check Contact Email Exist
        public bool CheckCustomerEmailExist(Customer customer)
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
                        cmd.CommandText = "[PSA].[CheckCustomerEmailExist]";
                        cmd.Parameters.Add("@ContactEmail", SqlDbType.NVarChar).Value = customer.ContactEmail;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = customer.ID;
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
        #endregion Check Contact Email Exist
        #region Check Contact Email Exist
        public bool CheckMobileNumberExist(Customer customer)
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
                        cmd.CommandText = "[PSA].[CheckMobileNumberExist ]";
                        cmd.Parameters.Add("@Mobile", SqlDbType.NVarChar).Value = customer.Mobile;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = customer.ID;
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
        #endregion Check Contact Email Exist
        #region GetAllTitles
        public List<Titles> GetAllTitles()
        {
            List<Titles> titlesList = null;
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
                        cmd.CommandText = "[PSA].[GetAllTitle]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                titlesList = new List<Titles>();
                                while (sdr.Read())
                                {
                                    Titles _titlesObj = new Titles();
                                    {
                                        _titlesObj.Title = (sdr["Title"].ToString() != "" ? sdr["Title"].ToString() : _titlesObj.Title);

                                    }
                                    titlesList.Add(_titlesObj);
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

            return titlesList;
        }
        #endregion GetAllTitles
        #region InsertUpdateCustomer
        public object InsertUpdateCustomer(Customer customer)
        {
            SqlParameter outputStatus, outputID = null;
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
                        cmd.CommandText = "[PSA].[InsertUpdateCustomer]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = customer.IsUpdate;
                        if(customer.ID==Guid.Empty)
                        {
                            cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = customer.ID;
                        }
                        cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 150).Value = customer.CompanyName;
                        cmd.Parameters.Add("@TallyName", SqlDbType.VarChar, 150).Value = customer.TallyName;
                        cmd.Parameters.Add("@ContactPerson", SqlDbType.VarChar, 100).Value = customer.ContactPerson;
                        cmd.Parameters.Add("@ContactEmail", SqlDbType.VarChar, 150).Value = customer.ContactEmail;
                        cmd.Parameters.Add("@ContactTitle", SqlDbType.VarChar, 10).Value = customer.ContactTitle;
                        cmd.Parameters.Add("@Website", SqlDbType.NVarChar, 500).Value = customer.Website;
                        cmd.Parameters.Add("@LandLine", SqlDbType.VarChar, 50).Value = customer.LandLine;
                        cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 50).Value = customer.Mobile;
                        cmd.Parameters.Add("@Fax", SqlDbType.VarChar, 50).Value = customer.Fax;
                        cmd.Parameters.Add("@OtherPhoneNos", SqlDbType.VarChar, 250).Value = customer.OtherPhoneNos;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.NVarChar, -1).Value = customer.BillingAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar, -1).Value = customer.ShippingAddressCus;
                        cmd.Parameters.Add("@PaymentTermCode", SqlDbType.VarChar, 10).Value = customer.PaymentTermCode;
                        cmd.Parameters.Add("@TaxRegNo", SqlDbType.VarChar, 50).Value = customer.TaxRegNo;
                        cmd.Parameters.Add("@PANNo", SqlDbType.VarChar, 50).Value = customer.PANNO;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = customer.GeneralNotes;
                        cmd.Parameters.Add("@CountryCode", SqlDbType.Int).Value = customer.CountryCode;
                        cmd.Parameters.Add("@StateCode", SqlDbType.Int).Value = customer.StateCode;
                        cmd.Parameters.Add("@DistrictCode", SqlDbType.Int).Value = customer.DistrictCode;
                        cmd.Parameters.Add("@AreaCode", SqlDbType.Int).Value = customer.AreaCode;
                        if(customer.CustomerCategoryList.Count()>0)
                        {
                            cmd.Parameters.Add("@CustomerCategoryList", SqlDbType.VarChar).Value = string.Join(",", customer.CustomerCategoryList);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@CustomerCategoryList", DBNull.Value);
                        }
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = customer.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = customer.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = customer.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = customer.PSASysCommon.UpdatedDate;
                        cmd.Parameters.Add("@AadharNo", SqlDbType.VarChar, 50).Value = customer.AadharNo;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":                       
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        customer.ID = Guid.Parse(outputID.Value.ToString());
                        return new
                        {
                            ID = customer.ID,
                            Status = outputStatus.Value.ToString(),
                            Message = customer.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
                        };
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
                ID = customer.ID,
                Status = outputStatus.Value.ToString(),
                Message = customer.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion InsertUpdateCustomer
        #region Get All Customer
        public List<Customer> GetAllCustomer(CustomerAdvanceSearch customerAdvanceSearch)
        {
            List<Customer> customerList = null;
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
                        cmd.CommandText = "[PSA].[GetAllCustomer]";
                        if(string.IsNullOrEmpty(customerAdvanceSearch.SearchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchTerm", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = customerAdvanceSearch.SearchTerm;
                        }
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = customerAdvanceSearch.DataTablePaging.Start;
                        if (customerAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = customerAdvanceSearch.DataTablePaging.Length;
                        //cmd.Parameters.Add("@OrderDir", SqlDbType.NVarChar, 5).Value = model.order[0].dir;
                        //cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar, -1).Value = model.order[0].column;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = customerAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = customerAdvanceSearch.ToDate;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerList = new List<Customer>();
                                while (sdr.Read())
                                {
                                    Customer customer = new Customer();
                                    {
                                        customer.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customer.ID);
                                        customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : customer.CompanyName);
                                        customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : customer.ContactPerson);
                                        customer.ContactEmail = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : customer.ContactEmail);
                                        customer.ContactTitle = (sdr["ContactTitle"].ToString() != "" ? sdr["ContactTitle"].ToString() : customer.ContactTitle);
                                        customer.Website = (sdr["Website"].ToString() != "" ? sdr["Website"].ToString() : customer.Website);
                                        customer.LandLine = (sdr["LandLine"].ToString() != "" ? sdr["LandLine"].ToString() : customer.LandLine);
                                        customer.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : customer.Mobile);
                                        customer.Fax = (sdr["Fax"].ToString() != "" ? sdr["Fax"].ToString() : customer.Fax);
                                        customer.OtherPhoneNos = (sdr["OtherPhoneNos"].ToString() != "" ? sdr["OtherPhoneNos"].ToString() : customer.OtherPhoneNos);
                                        customer.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : customer.BillingAddress);
                                        customer.ShippingAddressCus = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : customer.ShippingAddressCus);
                                        customer.PaymentTermCode = (sdr["PaymentTermCode"].ToString() != "" ? sdr["PaymentTermCode"].ToString() : customer.PaymentTermCode);
                                        customer.TaxRegNo = (sdr["TaxRegNo"].ToString() != "" ? sdr["TaxRegNo"].ToString() : customer.TaxRegNo);
                                        customer.PANNO = (sdr["PANNO"].ToString() != "" ? sdr["PANNO"].ToString() : customer.PANNO);
                                        //customer.OutStanding = (sdr["OutStanding"].ToString() != "" ? decimal.Parse(sdr["OutStanding"].ToString()) : customer.OutStanding);
                                        customer.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : customer.GeneralNotes);
                                        customer.CountryCode = (sdr["CountryCode"].ToString() != "" ? int.Parse(sdr["CountryCode"].ToString()) : customer.CountryCode);
                                        customer.StateCode = (sdr["StateCode"].ToString() != "" ? int.Parse(sdr["StateCode"].ToString()) : customer.StateCode);
                                        customer.DistrictCode = (sdr["DistrictCode"].ToString() != "" ? int.Parse(sdr["DistrictCode"].ToString()) : customer.DistrictCode);
                                        customer.AreaCode = (sdr["AreadCode"].ToString() != "" ? int.Parse(sdr["AreadCode"].ToString()) : customer.AreaCode);
                                        customer.PSASysCommon = new PSASysCommon();
                                        customer.PSASysCommon.CreatedBy = (sdr["CreatedBy"].ToString() != "" ? sdr["CreatedBy"].ToString() : customer.PSASysCommon.CreatedBy);
                                        customer.PSASysCommon.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(_settings.DateFormat) : customer.PSASysCommon.CreatedDateString);
                                        customer.PSASysCommon.CreatedDate = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()) : customer.PSASysCommon.CreatedDate);
                                        customer.PSASysCommon.UpdatedBy = (sdr["UpdatedBy"].ToString() != "" ? sdr["UpdatedBy"].ToString() : customer.PSASysCommon.UpdatedBy);
                                        customer.PSASysCommon.UpdatedDate = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()) : customer.PSASysCommon.UpdatedDate);
                                        customer.PSASysCommon.UpdatedDateString = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()).ToString(_settings.DateFormat) : customer.PSASysCommon.UpdatedDateString);
                                        customer.FilteredCount= (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : customer.FilteredCount);
                                        customer.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : customer.FilteredCount);
                                        customer.AadharNo = (sdr["AadharNo"].ToString() != "" ? sdr["AadharNo"].ToString() : customer.AadharNo);
                                    }
                                    customerList.Add(customer);
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

            return customerList;
        }
        #endregion Get All Customer
        #region Get Customer
        public Customer GetCustomer(Guid id)
        {
            Customer customer = new Customer();
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
                        cmd.CommandText = "[PSA].[GetCustomer]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    customer.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customer.ID);
                                    customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : customer.CompanyName);
                                    customer.TallyName = (sdr["TallyName"].ToString() != "" ? sdr["TallyName"].ToString() : customer.TallyName);
                                    customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : customer.ContactPerson);
                                    customer.ContactEmail = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : customer.ContactEmail);
                                    customer.ContactTitle = (sdr["ContactTitle"].ToString() != "" ? sdr["ContactTitle"].ToString() : customer.ContactTitle);
                                    customer.Website = (sdr["Website"].ToString() != "" ? sdr["Website"].ToString() : customer.Website);
                                    customer.LandLine = (sdr["LandLine"].ToString() != "" ? sdr["LandLine"].ToString() : customer.LandLine);
                                    customer.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : customer.Mobile);
                                    customer.Fax = (sdr["Fax"].ToString() != "" ? sdr["Fax"].ToString() : customer.Fax);
                                    customer.OtherPhoneNos = (sdr["OtherPhoneNos"].ToString() != "" ? sdr["OtherPhoneNos"].ToString() : customer.OtherPhoneNos);
                                    customer.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : customer.BillingAddress);
                                    customer.ShippingAddressCus = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : customer.ShippingAddressCus);
                                    customer.PaymentTermCode = (sdr["PaymentTermCode"].ToString() != "" ? sdr["PaymentTermCode"].ToString() : customer.PaymentTermCode);
                                    customer.TaxRegNo = (sdr["TaxRegNo"].ToString() != "" ? sdr["TaxRegNo"].ToString() : customer.TaxRegNo);
                                    customer.PANNO = (sdr["PANNO"].ToString() != "" ? sdr["PANNO"].ToString() : customer.PANNO);
                                    customer.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : customer.GeneralNotes);
                                    customer.CustomerCategoryList = (sdr["CustomerCategoryList"].ToString() != "" ? (sdr["CustomerCategoryList"].ToString()).Split(',').Select(h => int.Parse(h)).ToArray() : customer.CustomerCategoryList);
                                    customer.CountryCode = (sdr["CountryCode"].ToString() != "" ? int.Parse(sdr["CountryCode"].ToString()) : customer.CountryCode);
                                    customer.StateCode= (sdr["StateCode"].ToString() != "" ? int.Parse(sdr["StateCode"].ToString()) : customer.StateCode);
                                    customer.DistrictCode= (sdr["DistrictCode"].ToString() != "" ? int.Parse(sdr["DistrictCode"].ToString()) : customer.DistrictCode);
                                    customer.AreaCode= (sdr["AreadCode"].ToString() != "" ? int.Parse(sdr["AreadCode"].ToString()) : customer.AreaCode);
                                    customer.PSASysCommon = new PSASysCommon();
                                    customer.PSASysCommon.CreatedBy = (sdr["CreatedBy"].ToString() != "" ? sdr["CreatedBy"].ToString() : customer.PSASysCommon.CreatedBy);
                                    customer.PSASysCommon.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(_settings.DateFormat) : customer.PSASysCommon.CreatedDateString);
                                    customer.PSASysCommon.CreatedDate = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()) : customer.PSASysCommon.CreatedDate);
                                    customer.PSASysCommon.UpdatedBy = (sdr["UpdatedBy"].ToString() != "" ? sdr["UpdatedBy"].ToString() : customer.PSASysCommon.UpdatedBy);
                                    customer.PSASysCommon.UpdatedDate = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()) : customer.PSASysCommon.UpdatedDate);
                                    customer.PSASysCommon.UpdatedDateString = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()).ToString(_settings.DateFormat) : customer.PSASysCommon.UpdatedDateString);
                                    customer.AadharNo= (sdr["AadharNo"].ToString() != "" ? sdr["AadharNo"].ToString() : customer.AadharNo);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return customer;
        }
        #endregion Get Customer
        #region DeleteCustomer
        public object DeleteCustomer(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteCustomer]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(_appConstant.DeleteFailure);

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
                Message = _appConstant.DeleteSuccess
            };
        }
        #endregion DeleteCustomer
        #region Get Customer SelectList
        public List<Customer> GetCustomerSelectList()
        {
            List<Customer> customerList = null;
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
                        cmd.CommandText = "[PSA].[GetCustomerForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerList = new List<Customer>();
                                while (sdr.Read())
                                {
                                    Customer customer = new Customer();
                                    {
                                        customer.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customer.ID);
                                        customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : customer.CompanyName);
                                    }
                                    customerList.Add(customer);
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
            return customerList;
        }
        #endregion Get Customer SelectList


        #region GetCustomerSelectListOnDemand
        public List<Customer> GetCustomerSelectListOnDemand()
        {
            List<Customer> customerList = null;
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
                        cmd.CommandText = "[PSA].[GetCustomerSelectListOnDemand]";
                        cmd.CommandType = CommandType.StoredProcedure;                       
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerList = new List<Customer>();
                                while (sdr.Read())
                                {
                                    Customer customer = new Customer();
                                    {
                                        customer.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customer.ID);
                                        customer.CustomerSelect = (sdr["Customer"].ToString() != "" ? sdr["Customer"].ToString() : customer.CustomerSelect);
                                    }
                                    customerList.Add(customer);
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
            return customerList;
        }
        #endregion GetCustomerSelectListOnDemand


    }
}
