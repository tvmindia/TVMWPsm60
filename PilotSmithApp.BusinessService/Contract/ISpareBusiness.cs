using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface ISpareBusiness
    {
        List<Spare> GetAllSpare(SpareAdvanceSearch spareAdvanceSearch);
        object DeleteSpare(Guid id);
        object InsertUpdateSpare(Spare spare);
        string GetSpareCode();
        bool CheckSpareCodeExist(Spare spare);
        Spare GetSpare(Guid id);
        List<SelectListItem> GetSpareForSelectList();
    }
}
