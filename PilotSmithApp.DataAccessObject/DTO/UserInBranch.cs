using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class UserInBranch
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public int BranchCode { get; set; }
        public bool IsDefault { get; set; }
        public PSASysCommon PSASysCommon { get; set; }

        //additional fields 
        public PSAUser PSAUser { get; set; }
        public Branch Branch { get; set; }
        public bool HasAccess { get; set; }
        public string HasAccessBranch { get; set; }
        public string DefaultBranch { get; set; }
    }
}
