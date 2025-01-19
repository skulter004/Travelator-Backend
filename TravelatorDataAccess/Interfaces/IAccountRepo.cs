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
        Task<bool> RegisterUser(RegisterModel model, string userId);
        Task<Employee> GetEmployeeById(Guid id);
    }
}
