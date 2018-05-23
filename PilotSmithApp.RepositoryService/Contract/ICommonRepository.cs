using System;
using System.Collections.Generic;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface ICommonRepository
    {
        string SendMessage(string message, string MobileNo,string provider,string type);
        bool CheckDocumentIsDeletable(string docType, Guid? id);
    }
}
