using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelatorDataAccess.EntityModels;

namespace TravelatorDataAccess.Interfaces
{
    public interface IAccountRepo
    {
        Task<(bool Succeeded, Guid UserId, IEnumerable<IdentityError> Errors)> RegisterUser(RegisterModel model);
        Task<Employee> GetEmployeeById(Guid id);
    }
}
