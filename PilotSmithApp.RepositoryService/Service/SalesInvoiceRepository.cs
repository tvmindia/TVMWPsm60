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
    public class SalesInvoiceRepository: ISalesInvoiceRepository
    {

        #region Constructor Injection
        private IDatabaseFactory _databaseFactory;
        Settings settings = new Settings();
        AppConst _appConst = new AppConst();
        public SalesInvoiceRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetSalesSummary
        public List<SalesSummary> GetSalesSummary()
        {
            List<SalesSummary> SalesInvoiceSummaryList = new List<SalesSummary>();
            SalesSummary SalesInvoiceSummary = null;
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
                        cmd.CommandText = "[PSA].[GetSalesSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    SalesInvoiceSummary = new SalesSummary();
                                    SalesInvoiceSummary.Month = (sdr["Month"].ToString() != "" ? sdr["Month"].ToString() : SalesInvoiceSummary.Month);
                                    SalesInvoiceSummary.MonthCode = (sdr["MonthCode"].ToString() != "" ? int.Parse(sdr["MonthCode"].ToString()) : SalesInvoiceSummary.MonthCode);
                                    SalesInvoiceSummary.Year = (sdr["Year"].ToString() != "" ? int.Parse(sdr["Year"].ToString()) : SalesInvoiceSummary.Year);
                                    SalesInvoiceSummary.Sales = (sdr["Sales"].ToString() != "" ? decimal.Parse(sdr["Sales"].ToString()) : SalesInvoiceSummary.Sales);
                                    SalesInvoiceSummary.LastYear = (sdr["LastYear"].ToString() != "" ? int.Parse(sdr["LastYear"].ToString()) : SalesInvoiceSummary.LastYear);
                                    SalesInvoiceSummary.LastYearSales = (sdr["LastYearSales"].ToString() != "" ? decimal.Parse(sdr["LastYearSales"].ToString()) : SalesInvoiceSummary.LastYearSales);
                                    SalesInvoiceSummaryList.Add(SalesInvoiceSummary);
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
            return SalesInvoiceSummaryList;
        }
        #endregion GetSalesSummary
    }
}
