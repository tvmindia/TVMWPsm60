using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class DocumentLog
    {
        public int Code { get; set; }
        public string DocumentNo { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string DateFormatted { get; set; }
        public string Remarks { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        //addtional fields
        public Guid DocumentOwnerId { get; set; }
        public string DocType { get; set; }
        public string OldUserName { get; set; }
        public string OldUserEmail { get; set; }
        public string NewDocumentOwner { get; set; }
        public Guid DocumentID { get; set; }
    }
}
