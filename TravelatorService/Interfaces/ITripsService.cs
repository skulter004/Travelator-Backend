using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelatorDataAccess.EntityModels;
using TravelatorService.DTO_s;

namespace TravelatorService.Interfaces
{
    public interface ITripsService
    {
        Task<bool> TravelRequest(TravelRequestDTO details);
        Task<bool> AddExpense(ExpenseDTO expense);
        Task<object> TravelRequests(string status);
        Task<bool> ApproveRequest(Guid id);
        Task<bool> DirectorApproval(Guid id);
    }
}
