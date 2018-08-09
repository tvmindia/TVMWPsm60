﻿using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface ITakeOwnershipBusiness
    {
        DocumentLog InsertTakeOwnership(DocumentLog documentLog);
        List<DocumentLog> GetOwnershipHistory(Guid DocumentID, string DocumentTypeCode);
    }
}
