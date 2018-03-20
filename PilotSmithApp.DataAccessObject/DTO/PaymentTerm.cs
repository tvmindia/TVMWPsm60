﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class PaymentTerm
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int NoOfDays { get; set; }
        public PSASysCommon common { get; set; }
    }
}
