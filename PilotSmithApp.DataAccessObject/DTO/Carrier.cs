using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class Carrier
    {
        public int Code { get; set; }
        public string Name { get; set; }
        //Additional Fields
        public PSASysCommon PSASysCommon { get; set; }
    }
}
