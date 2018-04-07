using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class DocumentStatusViewModel
    {
        public int Code { get; set; }
        [Required(ErrorMessage = "Description is missing")]
        public string Description { get; set; }
        public string DocumentTypeCode { get; set; }
        public DocumentTypeViewModel DocumentType { get; set; }
    }
}