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
    public class ServiceCallRepository : IServiceCallRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ServiceCallRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region Get All ServiceCall
        public List<ServiceCall> GetAllServiceCall(ServiceCallAdvanceSearch serviceCallAdvanceSearch)
        {
            List<ServiceCall> serviceCallList = null;
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
                        cmd.CommandText = "[PSA].[GetAllServiceCall]";
                        if (string.IsNullOrEmpty(serviceCallAdvanceSearch.SearchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchValue", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = serviceCallAdvanceSearch.SearchTerm;
                        }
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = serviceCallAdvanceSearch.DataTablePaging.Start;
                        if (serviceCallAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = serviceCallAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = serviceCallAdvanceSearch.AdvFromDate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = serviceCallAdvanceSearch.AdvToDate;
                        if (serviceCallAdvanceSearch.AdvCustomerID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = serviceCallAdvanceSearch.AdvCustomerID;
                        if (serviceCallAdvanceSearch.AdvBranchCode == null)
                            cmd.Parameters.AddWithValue("@BranchCode", DBNull.Value);
                        else
                            cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = serviceCallAdvanceSearch.AdvBranchCode;
                        if (serviceCallAdvanceSearch.AdvAreaCode == null)
                            cmd.Parameters.AddWithValue("@AreaCode", DBNull.Value);
                        else
                            cmd.Parameters.Add("@AreaCode", SqlDbType.Int).Value = serviceCallAdvanceSearch.AdvAreaCode;
                        if (serviceCallAdvanceSearch.AdvAttendedBy == Guid.Empty)
                            cmd.Parameters.AddWithValue("@AttendedBy", DBNull.Value);
                        else
                            cmd.Parameters.Add("@AttendedBy", SqlDbType.UniqueIdentifier).Value = serviceCallAdvanceSearch.AdvAttendedBy;
                        if (serviceCallAdvanceSearch.AdvServiceTypeCode == null)
                            cmd.Parameters.AddWithValue("@ServiceTypeCode", DBNull.Value);
                        else
                            cmd.Parameters.Add("@ServiceTypeCode", SqlDbType.Int).Value = serviceCallAdvanceSearch.AdvServiceTypeCode;
                        if (serviceCallAdvanceSearch.AdvServicedBy == Guid.Empty)
                            cmd.Parameters.AddWithValue("@ServicedBy", DBNull.Value);
                        else
                            cmd.Parameters.Add("@ServicedBy", SqlDbType.UniqueIdentifier).Value = serviceCallAdvanceSearch.AdvServicedBy;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = serviceCallAdvanceSearch.AdvDocumentStatusCode;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                serviceCallList = new List<ServiceCall>();
                                while (sdr.Read())
                                {
                                    ServiceCall serviceCall = new ServiceCall();
                                    {
                                        serviceCall.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : serviceCall.ID);
                                        serviceCall.ServiceCallNo = (sdr["ServiceCallNo"].ToString() != "" ? sdr["ServiceCallNo"].ToString() : serviceCall.ServiceCallNo);
                                        serviceCall.ServiceCallDate = (sdr["ServiceCallDate"].ToString() != "" ? DateTime.Parse(sdr["ServiceCallDate"].ToString()) : serviceCall.ServiceCallDate);
                                        serviceCall.ServiceCallDateFormatted = (sdr["ServiceCallDate"].ToString() != "" ? DateTime.Parse(sdr["ServiceCallDate"].ToString()).ToString(_settings.DateFormat) : serviceCall.ServiceCallDateFormatted);
                                        serviceCall.ServiceCallTime = (sdr["ServiceCallTime"].ToString() != "" ? DateTime.Parse(sdr["ServiceCallTime"].ToString()) : serviceCall.ServiceCallTime);
                                        serviceCall.ServiceCallTimeFormatted = (sdr["ServiceCallTime"].ToString() != "" ? DateTime.Parse(sdr["ServiceCallTime"].ToString()).ToString("h:mm tt") : serviceCall.ServiceCallTimeFormatted);
                                        serviceCall.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : serviceCall.CustomerID);
                                        serviceCall.Customer = new Customer();
                                        serviceCall.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : serviceCall.Customer.ID);
                                        serviceCall.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : serviceCall.Customer.CompanyName);
                                        serviceCall.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : serviceCall.Customer.ContactPerson);
                                        serviceCall.Customer.Mobile = (sdr["CustomerMobile"].ToString() != "" ? sdr["CustomerMobile"].ToString() : serviceCall.Customer.Mobile);
                                        serviceCall.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : serviceCall.DocumentStatusCode);
                                        serviceCall.DocumentStatus = new DocumentStatus();
                                        serviceCall.DocumentStatus.Code = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : serviceCall.DocumentStatus.Code);
                                        serviceCall.DocumentStatus.Description = (sdr["DocumentStatusDescription"].ToString() != "" ? (sdr["DocumentStatusDescription"].ToString()) : serviceCall.DocumentStatus.Description);
                                        serviceCall.AttendedBy = (sdr["AttendedBy"].ToString() != "" ? Guid.Parse(sdr["AttendedBy"].ToString()) : serviceCall.AttendedBy);
                                        serviceCall.Employee = new Employee();
                                        {
                                            serviceCall.Employee.ID = (sdr["AttendedBy"].ToString() != "" ? Guid.Parse(sdr["AttendedBy"].ToString()) : serviceCall.Employee.ID);
                                            serviceCall.Employee.Name = (sdr["AttendedByName"].ToString() != "" ? sdr["AttendedByName"].ToString() : serviceCall.Employee.Name);
                                        }
                                        serviceCall.ServicedBy = (sdr["ServicedBy"].ToString() != "" ? Guid.Parse(sdr["ServicedBy"].ToString()) : serviceCall.ServicedBy);
                                        serviceCall.ServicedByName = (sdr["ServicedByName"].ToString() != "" ? sdr["ServicedByName"].ToString() : serviceCall.ServicedByName);
                                        serviceCall.ServiceDate = (sdr["ServiceDate"].ToString() != "" ? DateTime.Parse(sdr["ServiceDate"].ToString()) : serviceCall.ServiceDate);
                                        serviceCall.ServiceDateFormatted = (sdr["ServiceDate"].ToString() != "" ? DateTime.Parse(sdr["ServiceDate"].ToString()).ToString(_settings.DateFormat) : serviceCall.ServiceDateFormatted);
                                        serviceCall.ServiceComments = (sdr["ServiceComments"].ToString() != "" ? sdr["ServiceComments"].ToString() : serviceCall.ServiceComments);
                                        serviceCall.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : serviceCall.BranchCode);
                                        serviceCall.Branch = new Branch();
                                        {
                                            serviceCall.Branch.Code = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : serviceCall.Branch.Code);
                                            serviceCall.Branch.Description = (sdr["Branch"].ToString() != "" ? sdr["Branch"].ToString() : serviceCall.Branch.Description);
                                        };
                                        serviceCall.CalledPersonName = (sdr["CalledPersonName"].ToString() != "" ? (sdr["CalledPersonName"].ToString()) : serviceCall.CalledPersonName);
                                        serviceCall.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : serviceCall.GeneralNotes);
                                        serviceCall.Area = new Area();
                                        {
                                            serviceCall.Area.Code = (sdr["AreaCode"].ToString() != "" ? int.Parse(sdr["AreaCode"].ToString()) : serviceCall.Area.Code);
                                            serviceCall.Area.Description = (sdr["Area"].ToString() != "" ? sdr["Area"].ToString() : serviceCall.Area.Description);
                                        }
                                        serviceCall.ServiceTypeCode = (sdr["ServiceTypeCode"].ToString() != "" ? int.Parse(sdr["ServiceTypeCode"].ToString()) : serviceCall.ServiceTypeCode);
                                        serviceCall.ServiceType = new ServiceType();
                                        serviceCall.ServiceType.Name = (sdr["ServiceType"].ToString() != "" ? sdr["ServiceType"].ToString() : serviceCall.ServiceType.Name);
                                        serviceCall.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : serviceCall.FilteredCount);
                                        serviceCall.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : serviceCall.FilteredCount);
                                    }
                                    serviceCallList.Add(serviceCall);
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

            return serviceCallList;
        }
        #endregion Get All ServiceCall

        #region Get ServiceCall
        public ServiceCall GetServiceCall(Guid id)
        {
            ServiceCall serviceCall = new ServiceCall();
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
                        cmd.CommandText = "[PSA].[GetServiceCall]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    serviceCall.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : serviceCall.ID);
                                    serviceCall.ServiceCallNo = (sdr["ServiceCallNo"].ToString() != "" ? sdr["ServiceCallNo"].ToString() : serviceCall.ServiceCallNo);
                                    serviceCall.ServiceCallDate = (sdr["ServiceCallDate"].ToString() != "" ? DateTime.Parse(sdr["ServiceCallDate"].ToString()) : serviceCall.ServiceCallDate);
                                    serviceCall.ServiceCallDateFormatted = (sdr["ServiceCallDate"].ToString() != "" ? DateTime.Parse(sdr["ServiceCallDate"].ToString()).ToString(_settings.DateFormat) : serviceCall.ServiceCallDateFormatted);
                                    serviceCall.ServiceCallTimeFormatted = (sdr["ServiceCallTime"].ToString() != "" ? DateTime.Parse(sdr["ServiceCallTime"].ToString()).ToString("h:mm tt") : serviceCall.ServiceCallTimeFormatted);
                                    serviceCall.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : serviceCall.CustomerID);
                                    serviceCall.Customer = new Customer();
                                    serviceCall.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : serviceCall.Customer.ID);
                                    serviceCall.Customer.CompanyName = (sdr["CustomerCompanyName"].ToString() != "" ? sdr["CustomerCompanyName"].ToString() : serviceCall.Customer.CompanyName);
                                    serviceCall.Customer.ContactPerson = (sdr["CustomerContactPerson"].ToString() != "" ? sdr["CustomerContactPerson"].ToString() : serviceCall.Customer.ContactPerson);
                                    serviceCall.Customer.Mobile = (sdr["CustomerMobile"].ToString() != "" ? sdr["CustomerMobile"].ToString() : serviceCall.Customer.Mobile);
                                    serviceCall.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : serviceCall.DocumentStatusCode);
                                    serviceCall.DocumentStatus = new DocumentStatus();
                                    serviceCall.DocumentStatus.Code = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : serviceCall.DocumentStatus.Code);
                                    serviceCall.DocumentStatus.Description = (sdr["DocumentStatusDescription"].ToString() != "" ? (sdr["DocumentStatusDescription"].ToString()) : serviceCall.DocumentStatus.Description);
                                    serviceCall.AttendedBy = (sdr["AttendedBy"].ToString() != "" ? Guid.Parse(sdr["AttendedBy"].ToString()) : serviceCall.AttendedBy);
                                    serviceCall.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : serviceCall.BranchCode);
                                    serviceCall.Branch = new Branch();
                                    serviceCall.Branch.Description = (sdr["Branch"].ToString() != "" ? sdr["Branch"].ToString() : serviceCall.Branch.Description);
                                    serviceCall.ServiceTypeCode = (sdr["ServiceTypeCode"].ToString() != "" ? int.Parse(sdr["ServiceTypeCode"].ToString()) : serviceCall.ServiceTypeCode);
                                    //serviceCall.ServiceType = new ServiceType();
                                    //serviceCall.ServiceType.Name = (sdr["ServiceType"].ToString() != "" ? sdr["ServiceType"].ToString() : serviceCall.ServiceType.Name);
                                    serviceCall.ReferenceInvoice = (sdr["ReferenceInvoice"].ToString() != "" ? sdr["ReferenceInvoice"].ToString() : serviceCall.ReferenceInvoice);
                                    serviceCall.ReferenceInvoiceDate = (sdr["ReferenceInvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["ServiceCallDate"].ToString()) : serviceCall.ReferenceInvoiceDate);
                                    serviceCall.ReferenceInvoiceDateFormatted = (sdr["ReferenceInvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["ServiceCallDate"].ToString()).ToString(_settings.DateFormat) : serviceCall.ReferenceInvoiceDateFormatted);
                                    //2 Employee Problem
                                    serviceCall.ServicedBy = (sdr["ServicedBy"].ToString() != "" ? Guid.Parse(sdr["ServicedBy"].ToString()) : serviceCall.ServicedBy);
                                    serviceCall.ServiceDate = (sdr["ServiceDate"].ToString() != "" ? DateTime.Parse(sdr["ServiceDate"].ToString()) : serviceCall.ServiceDate);
                                    serviceCall.ServiceDateFormatted = (sdr["ServiceDate"].ToString() != "" ? DateTime.Parse(sdr["ServiceDate"].ToString()).ToString(_settings.DateFormat) : serviceCall.ServiceDateFormatted);
                                    serviceCall.CalledPersonName = (sdr["CalledPersonName"].ToString() != "" ? (sdr["CalledPersonName"].ToString()) : serviceCall.CalledPersonName);
                                    serviceCall.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : serviceCall.GeneralNotes);
                                    serviceCall.ServiceComments = (sdr["ServiceComments"].ToString() != "" ? sdr["ServiceComments"].ToString() : serviceCall.ServiceComments);
                                    serviceCall.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : serviceCall.DocumentOwnerID);
                                    serviceCall.DocumentOwners = (sdr["DocumentOwners"].ToString() != "" ? (sdr["DocumentOwners"].ToString()).Split(',') : serviceCall.DocumentOwners);
                                    serviceCall.DocumentOwner = (sdr["DocumentOwner"].ToString() != "" ? (sdr["DocumentOwner"].ToString()) : serviceCall.DocumentOwner);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return serviceCall;
        }
        #endregion Get ServiceCall

        #region GetServiceCallDetailListByServiceCallID
        public List<ServiceCallDetail> GetServiceCallDetailListByServiceCallID(Guid serviceCallID)
        {
            List<ServiceCallDetail> serviceCallDetailList = new List<ServiceCallDetail>();
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
                        cmd.CommandText = "[PSA].[GetServiceCallDetailListByServiceCallID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ServiceCallID", SqlDbType.UniqueIdentifier).Value = serviceCallID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    ServiceCallDetail serviceCallDetail = new ServiceCallDetail();
                                    {
                                        serviceCallDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : serviceCallDetail.ID);
                                        serviceCallDetail.ServiceCallID = (sdr["ServiceCallID"].ToString() != "" ? Guid.Parse(sdr["ServiceCallID"].ToString()) : serviceCallDetail.ServiceCallID);
                                        serviceCallDetail.Product = new Product()
                                        {
                                            ID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty),
                                            Code = (sdr["ProductCode"].ToString() != "" ? sdr["ProductCode"].ToString() : string.Empty),
                                            Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : string.Empty),
                                            HSNCode = (sdr["HSNCode"].ToString() != "" ? sdr["HSNCode"].ToString() : String.Empty)
                                        };
                                        serviceCallDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty);
                                        serviceCallDetail.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        serviceCallDetail.ProductModel = new ProductModel();
                                        serviceCallDetail.ProductModel.ID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        serviceCallDetail.ProductModel.Name = (sdr["ProductModelName"].ToString() != "" ? (sdr["ProductModelName"].ToString()) : serviceCallDetail.ProductModel.Name);
                                        serviceCallDetail.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString() : serviceCallDetail.ProductSpec);
                                        serviceCallDetail.GuaranteeYN = (sdr["GuaranteeYN"].ToString() != "" ? bool.Parse(sdr["GuaranteeYN"].ToString()) : serviceCallDetail.GuaranteeYN);
                                        serviceCallDetail.DocumentStatus = new DocumentStatus();
                                        serviceCallDetail.ServiceStatusCode = (sdr["ServiceStatusCode"].ToString() != "" ? int.Parse(sdr["ServiceStatusCode"].ToString()) : serviceCallDetail.ServiceStatusCode);
                                        serviceCallDetail.DocumentStatus.Code = (int)(sdr["ServiceStatusCode"].ToString() != "" ? int.Parse(sdr["ServiceStatusCode"].ToString()) : serviceCallDetail.DocumentStatus.Code);
                                        serviceCallDetail.DocumentStatus.Description = (sdr["Status"].ToString() != "" ? sdr["Status"].ToString() : serviceCallDetail.DocumentStatus.Description);
                                        serviceCallDetail.InstalledDate = (sdr["InstalledDate"].ToString() != "" ? DateTime.Parse(sdr["InstalledDate"].ToString()) : serviceCallDetail.InstalledDate);
                                        serviceCallDetail.InstalledDateFormatted = (sdr["InstalledDate"].ToString() != "" ? DateTime.Parse(sdr["InstalledDate"].ToString()).ToString(_settings.DateFormat) : serviceCallDetail.InstalledDateFormatted);
                                        serviceCallDetail.Spare = new Spare();
                                        serviceCallDetail.SpareID= (sdr["SpareID"].ToString() != "" ? Guid.Parse(sdr["SpareID"].ToString()) : Guid.Empty);
                                        serviceCallDetail.Spare.Name = (sdr["Spare"].ToString() != "" ? sdr["Spare"].ToString() : string.Empty);
                                        serviceCallDetail.Spare.Code = (sdr["SpareCode"].ToString() != "" ? sdr["SpareCode"].ToString() : serviceCallDetail.Spare.Code);
                                    }
                                    serviceCallDetailList.Add(serviceCallDetail);
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

            return serviceCallDetailList;
        }
        #endregion GetServiceCallDetailsByServiceCallID

        #region GetServiceCallChargeListByServiceCallID
        public List<ServiceCallCharge> GetServiceCallChargeDetailListByServiceCallID(Guid serviceCallID)
        {
            List<ServiceCallCharge> serviceCallChargeList = new List<ServiceCallCharge>();
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
                        cmd.CommandText = "[PSA].[GetServiceCallChargeListByServiceCallID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ServiceCallID", SqlDbType.UniqueIdentifier).Value = serviceCallID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    ServiceCallCharge serviceCallCharge = new ServiceCallCharge();
                                    {
                                        serviceCallCharge.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : serviceCallCharge.ID);
                                        serviceCallCharge.ServiceCallID = (sdr["ServiceCallID"].ToString() != "" ? Guid.Parse(sdr["ServiceCallID"].ToString()) : serviceCallCharge.ServiceCallID);
                                        serviceCallCharge.OtherChargeCode = (sdr["OtherChargeCode"].ToString() != "" ? int.Parse(sdr["OtherChargeCode"].ToString()) : serviceCallCharge.OtherChargeCode);
                                        serviceCallCharge.ChargeAmount = (sdr["ChargeAmount"].ToString() != "" ? decimal.Parse(sdr["ChargeAmount"].ToString()) : serviceCallCharge.ChargeAmount);
                                        serviceCallCharge.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : serviceCallCharge.TaxTypeCode);
                                        serviceCallCharge.TaxType = new TaxType();
                                        serviceCallCharge.TaxType.Code = (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : serviceCallCharge.TaxType.Code);
                                        serviceCallCharge.TaxType.ValueText = (sdr["TaxTypeText"].ToString() != "" ? (sdr["TaxTypeText"].ToString()) : serviceCallCharge.TaxType.ValueText);
                                        serviceCallCharge.CGSTPerc = (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : serviceCallCharge.CGSTPerc);
                                        serviceCallCharge.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : serviceCallCharge.SGSTPerc);
                                        serviceCallCharge.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : serviceCallCharge.IGSTPerc);
                                        serviceCallCharge.AddlTaxPerc = (sdr["AddlTaxPerc"].ToString() != "" ? decimal.Parse(sdr["AddlTaxPerc"].ToString()) : serviceCallCharge.AddlTaxPerc);
                                        serviceCallCharge.AddlTaxAmt = (sdr["AddlTaxAmt"].ToString() != "" ? decimal.Parse(sdr["AddlTaxAmt"].ToString()) : serviceCallCharge.AddlTaxAmt);
                                        serviceCallCharge.OtherCharge = new OtherCharge();
                                        serviceCallCharge.OtherCharge.Description = (sdr["OtherCharge"].ToString() != "" ? sdr["OtherCharge"].ToString() : serviceCallCharge.OtherCharge.Description);
                                        serviceCallCharge.OtherCharge.SACCode = (sdr["SACCode"].ToString() != "" ? sdr["SACCode"].ToString() : serviceCallCharge.OtherCharge.SACCode);
                                    }
                                    serviceCallChargeList.Add(serviceCallCharge);
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
            return serviceCallChargeList;
        }
        #endregion GetServiceCallChargeListByServiceCallID

        #region Insert Update ServiceCall
        public object InsertUpdateServiceCall(ServiceCall serviceCall)
        {
            SqlParameter outputStatus, outputID, outputServiceCallNo = null;
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
                        cmd.CommandText = "[PSA].[InsertUpdateServiceCall]";
                        cmd.CommandType = CommandType.StoredProcedure; //ServiceCallTimeFormatted
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = serviceCall.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = serviceCall.ID;
                        cmd.Parameters.Add("@ServiceCallNo", SqlDbType.VarChar, 20).Value = serviceCall.ServiceCallNo;
                        cmd.Parameters.Add("@ServiceCallDate", SqlDbType.DateTime).Value = serviceCall.ServiceCallDateFormatted;
                        cmd.Parameters.Add("@ServiceCallTime", SqlDbType.DateTime).Value = serviceCall.ServiceCallTimeFormatted;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = serviceCall.CustomerID;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = serviceCall.DocumentStatusCode;
                        cmd.Parameters.Add("@AttendedBy", SqlDbType.UniqueIdentifier).Value = serviceCall.AttendedBy;
                        cmd.Parameters.Add("@CalledPersonName", SqlDbType.VarChar, 50).Value = serviceCall.CalledPersonName;
                        cmd.Parameters.Add("@ServicedBy", SqlDbType.UniqueIdentifier).Value = serviceCall.ServicedBy;
                        cmd.Parameters.Add("@ServiceDate", SqlDbType.DateTime).Value = serviceCall.ServiceDateFormatted;
                        cmd.Parameters.Add("@ServiceComments", SqlDbType.NVarChar, -1).Value = serviceCall.ServiceComments;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = serviceCall.DetailXML;
                        cmd.Parameters.Add("@CallChargeXML", SqlDbType.Xml).Value = serviceCall.CallChargeXML;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = serviceCall.hdnFileID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = serviceCall.GeneralNotes;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = serviceCall.BranchCode;
                        cmd.Parameters.Add("@ServiceTypeCode", SqlDbType.Int).Value = serviceCall.ServiceTypeCode;
                        cmd.Parameters.Add("@ReferenceInvoice", SqlDbType.VarChar, 20).Value = serviceCall.ReferenceInvoice;
                        cmd.Parameters.Add("@ReferenceInvoiceDate", SqlDbType.DateTime).Value = serviceCall.ReferenceInvoiceDateFormatted;
                        //-----------------------//
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = serviceCall.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = serviceCall.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = serviceCall.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = serviceCall.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@StatusOut", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        outputServiceCallNo = cmd.Parameters.Add("@ServiceCallNoOut", SqlDbType.VarChar, 20);
                        outputServiceCallNo.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        serviceCall.ID = Guid.Parse(outputID.Value.ToString());
                        serviceCall.ServiceCallNo = outputServiceCallNo.Value.ToString();
                        return new
                        {
                            ID = serviceCall.ID,
                            ServiceCallNo = serviceCall.ServiceCallNo,
                            Status = outputStatus.Value.ToString(),
                            Message = serviceCall.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                ID = serviceCall.ID,
                ServiceCallNo = serviceCall.ServiceCallNo,
                Status = outputStatus.Value.ToString(),
                Message = serviceCall.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion Insert Update ServiceCall

        #region Delete ServiceCall
        public object DeleteServiceCall(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteServiceCall]";
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
        #endregion Delete ServiceCall

        #region Delete ServiceCall Detail
        public object DeleteServiceCallDetail(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteServiceCallDetail]";
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
        #endregion Delete ServiceCall Detail

        #region Delete ServiceCall OtherCharge
        public object DeleteServiceCallChargeDetail(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteServiceCallCharge]";
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
        #endregion Delete ServiceCall OtherCharge

    }
}