﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelatorDataAccess.EntityModels;

namespace TravelatorDataAccess.Interfaces
{
    public interface ITripsRepo
    {
        Task<bool> TravelRequest(TravelRequest details);
        Task<bool> AddExpense(Expense expense);
        Task<object> TravelRequests(string status);
        Task<bool> ApproveRequest(Guid id);
        Task<bool> DirectorApproval(Guid id);
    }
}
