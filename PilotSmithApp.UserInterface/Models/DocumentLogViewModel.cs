using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class DocumentLogViewModel
    {
        public int Code { get; set; }
        public string DocumentNo { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Reason is missing")]
        public string Remarks { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        //addtional fields
        public Guid DocumentOwnerId { get; set; }
        public string DocType { get; set; }
        public string OldUserName { get; set; }
        public string OldUserEmail { get; set; }
        public string NewDocumentOwner { get; set; }
        public Guid DocumentID { get; set; }
    }
}