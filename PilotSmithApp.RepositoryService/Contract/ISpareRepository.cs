using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface ISpareRepository
    {
        List<Spare> GetAllSpare(SpareAdvanceSearch spareAdvanceSearch);
        object DeleteSpare(Guid id);
        object InsertUpdateSpare(Spare spare);
        string GetSpareCode();
        bool CheckSpareCodeExist(Spare spare);
        Spare GetSpare(Guid id);
        List<Spare> GetSpareForSelectList();
    }
}
