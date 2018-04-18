using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    class SalesInvoice
    {
    }
    public class SalesSummary
    {
        public string Month { get; set; }
        public int MonthCode { get; set; }
        public int Year { get; set; }
        public int LastYear { get; set; }
        public decimal Sales { get; set; }
        public decimal LastYearSales { get; set; }
    }
}
