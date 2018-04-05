using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IBranchBusiness
    {
        List<SelectListItem> GetBranchForSelectList();
    }
}
