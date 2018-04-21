using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class PSAUserViewModel
    {
        public Guid ID { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string Password { get; set; }
        public string ResetLink { get; set; }
        public DateTime LinkExpiryTime { get; set; }
        public List<SelectListItem> UserSelectList { get; set; }
    }
}