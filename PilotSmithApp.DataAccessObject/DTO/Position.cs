﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class Position
    {
        public int? Code { get; set; }
        public string Description { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
    }
}
