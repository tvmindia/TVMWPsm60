﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class Customer
    {
        public Guid ID { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        public string ContactTitle { get; set; }
        public string Website { get; set; }
        public string LandLine { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string OtherPhoneNos { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddressCus { get; set; }
        public string PaymentTermCode { get; set; }
        public string TaxRegNo { get; set; }
        public string PANNO { get; set; }
        public string GeneralNotes { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal OutStanding { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public int? AreaCode { get; set; }
        public int? DistrictCode { get; set; }
        public int? StateCode { get; set; }
        public int? CountryCode { get; set; }
        public string AadharNo { get; set; }
        public string TallyName { get; set; }

        //Additional properties
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public int[] CustomerCategoryList { get; set; }
        public CustomerCategory CustomerCategory { get; set; }
        // public int CustomerCategoryCode { get; set; }
        public string CustomerSelect { get; set; }      

    }
    public class CustomerAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string IsInternalComp { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
    public class Titles
    {
        public string Title { get; set; }
    }
}
