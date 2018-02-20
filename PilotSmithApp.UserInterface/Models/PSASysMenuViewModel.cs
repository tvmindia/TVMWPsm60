﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class AMCSysMenuViewModel
    {
        public Int16 ID { get; set; }
        public Int16 ParentID { get; set; }
        public string Module { get; set; }
        public string MenuText { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string IconClass { get; set; }
        public string IconURL { get; set; }
        public string Parameters { get; set; }
        public decimal MenuOrder { get; set; }
        public AMCSysModuleViewModel AMCSysModuleObj { get; set; }
    }

    public class SAMPanelViewModel
    {
        public List<SysMenuViewModel> _LHSSysMenuViewModel { get; set; }
        public List<SysMenuViewModel> _RHSSysMenuViewModel { get; set; }

    }
    public class SysMenuViewModel
    {
        public Guid ID { get; set; }
        public string LinkName { get; set; }
        public string LinkDescription { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
    }

}