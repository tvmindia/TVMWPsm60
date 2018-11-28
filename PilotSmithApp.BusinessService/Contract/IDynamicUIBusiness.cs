using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;


namespace PilotSmithApp.BusinessService.Contracts
{
    public interface IDynamicUIBusiness
    {
        List<PSASysMenu> GetAllMenues();
        Boolean IsFileExisting(Guid id, string doctype);
    }
} 
