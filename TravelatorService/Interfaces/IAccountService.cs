using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelatorDataAccess.EntityModels;

namespace TravelatorService.Interfaces
{
    public interface IAccountService
    {
        Task<(bool Succeeded, Guid UserId, IEnumerable<IdentityError> Errors)> RegisterUser(RegisterModel model);
    }
}
