using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;

namespace PilotSmithApp.RepositoryService.Contract
{
    public  interface IFileUploadRepository
    {
        FileUpload InsertAttachment(FileUpload fileUploadObj);
        List<FileUpload> GetAttachments(Guid ID);
        object DeleteFile(Guid ID);
    }
}
