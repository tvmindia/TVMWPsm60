using PilotSmithApp.DataAccessObject.DTO;
using System.Collections.Generic;


namespace PilotSmithApp.BusinessService.Contracts
{
    public interface IDynamicUIBusiness
    {
        List<PSASysMenu> GetAllMenues();
    }
} 
