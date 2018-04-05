using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class ReferenceTypeViewModel
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }
}