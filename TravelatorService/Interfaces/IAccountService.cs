using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelatorDataAccess.EntityModels;
using TravelatorService.DTO_s;

namespace TravelatorService.Interfaces
{
    public interface IAccountService
    {
        Task<bool> RegisterUser(RegisterModel model, string userId);
        Task<EmployeeDTO> GetEmployeeById(Guid id);
    }
}
