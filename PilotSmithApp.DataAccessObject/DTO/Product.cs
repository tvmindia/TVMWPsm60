using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class Product
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ProductCategoryCode { get; set; }
        public DateTime? IntroducedDate { get; set; }
        public Guid? CompanyID { get; set; }
        public string HSNCode { get; set; }
        public string TallyName { get; set; }

        //Additional Fields
        public string IntroducedDateFormatted { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public Company Company { get; set; }
        public ProductModel ProductModel { get; set; }
        public Guid hdnPopupFileID { get; set; }
    }

    public class ProductAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
}
