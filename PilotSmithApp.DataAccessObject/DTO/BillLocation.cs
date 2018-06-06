using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
   public class BillLocation
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
    }
}
