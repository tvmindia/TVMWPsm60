using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class UserInBranchViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "User")]
        [Required(ErrorMessage = "User is missing")]
        public Guid UserID { get; set; }
        public int BranchCode { get; set; }
        public bool IsDefault { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }


        //additional fields 
        public PSAUserViewModel PSAUser { get; set; }
        public BranchViewModel Branch { get; set; }
        public bool HasAccess { get; set; }
        public string HasAccessBranch { get; set; }
        public string DefaultBranch { get; set; }
    }
}