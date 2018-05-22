using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IUserInBranchBusiness
    {
        List<UserInBranch> GetAllUserInBranchByUserId(Guid userId);
        object InsertUpdateUserInBranch(UserInBranch userInBranch);
    }
}
