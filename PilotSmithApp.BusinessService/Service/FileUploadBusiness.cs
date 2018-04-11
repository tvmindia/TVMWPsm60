using PilotSmithApp.BusinessService.Contracts;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;

namespace PilotSmithApp.BusinessService.Service
{
    public class FileUploadBusiness : IFileUploadBusiness
    {
        IFileUploadRepository _fileRepository;
        public FileUploadBusiness(IFileUploadRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        public FileUpload InsertAttachment(FileUpload fileUploadObj)
        {
            return _fileRepository.InsertAttachment(fileUploadObj);
        }
        public List<FileUpload> GetAttachments(Guid ID)
        {
            return _fileRepository.GetAttachments(ID);
        }
        public object DeleteFile(Guid ID)
        {
            return _fileRepository.DeleteFile(ID);
        }
    }
}