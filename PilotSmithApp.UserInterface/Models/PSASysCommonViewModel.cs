using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class PSASysCommonViewModel
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDatestr { get; set; }
        public string CreatedDateString { get; set; }        
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateString { get; set; }
    }
    public class Select2Model
    {
        public string id { get; set; }
        public string text { get; set; }
    }
    public class RecentDocumentViewModel
    {
        public Guid DocumentID { get; set; }
        public string DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentDateFormatted { get; set; }
        public string DocumentLink { get; set; }
        public string Particulars { get; set; }
        public List<RecentDocumentViewModel> RecentDocumentList { get; set; }
    }
}