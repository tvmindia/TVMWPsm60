using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;


namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IDynamicUIRepository
    {
        List<PSASysMenu> GetAllMenues();
        Boolean IsFileExisting(Guid id, string doctype);
    }
}
