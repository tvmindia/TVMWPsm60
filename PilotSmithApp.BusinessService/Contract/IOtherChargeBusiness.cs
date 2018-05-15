using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IOtherChargeBusiness
    {
        object InsertUpdateOtherCharge(OtherCharge otherCharge);
        List<OtherCharge> GetAllOtherCharge(OtherChargeAdvanceSearch otherChargeAdvanceSearch);
        OtherCharge GetOtherCharge(int code);
        bool CheckOtherChargeCodeExist(OtherCharge otherCharge);
        object DeleteOtherCharge(int code);
        List<SelectListItem> GetOtherChargeForSelectList();
    }
}
