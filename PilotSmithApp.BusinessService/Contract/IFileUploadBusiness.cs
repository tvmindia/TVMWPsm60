using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;

namespace PilotSmithApp.BusinessService.Contracts
{
    public interface IFileUploadBusiness
    {
        FileUpload InsertAttachment(FileUpload fileUploadObj);
        List<FileUpload> GetAttachments(Guid ID);
        object DeleteFile(Guid ID);
    }
}