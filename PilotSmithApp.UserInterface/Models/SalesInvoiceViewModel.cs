using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class SalesInvoiceViewModel
    {
    }
    public class SalesSummaryViewModel
    {
        public string Month { get; set; }
        public int MonthCode { get; set; }
        public int Year { get; set; }
        public int LastYear { get; set; }
        public decimal Sales { get; set; }
        public decimal LastYearSales { get; set; }
    }
}