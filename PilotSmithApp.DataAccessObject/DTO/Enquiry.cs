using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class Enquiry
    {
        public Guid ID { get; set; }
        public string EnquiryNo { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string RequirementSpec { get; set; }
        public Guid CustomerID { get; set; }
        public int GradeCode { get; set; }
        public int StatusCode { get; set; }
        public int ReferredByCode { get; set; }
        public Guid ResponsiblePersonID { get; set; }
        public Guid AttendedByID { get; set; }
        public string GeneralNotes { get; set; }
        public Guid DocumentOwnerID { get; set; }
        public int BranchCode { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public List<EnquiryDetail> EnquiryDetailList { get; set; }
    }
    public class EnquiryDetail
    {
        public Guid ID { get; set; }
        public Guid EnquiryID { get; set; }
        public Guid ProductID { get; set; }
        public Guid ModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal Qty { get; set; }
        public int UnitCode { get; set; }
        public decimal Rate { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
    }
}
