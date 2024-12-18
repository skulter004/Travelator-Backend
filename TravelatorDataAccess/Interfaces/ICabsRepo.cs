using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelatorDataAccess.EntityModels;

namespace TravelatorDataAccess.Interfaces
{
    public interface ICabsRepo
    {
        Task<bool> RequestBooking(CabRequest booking);
        Task<bool> ApproveBooking(CabBooking booking);
        Task<object> GetCabDetails(Guid EmployeeId);
        Task<object> Requests();
        Task<object> AvailableCabs();
        Task<object> MyBooking(Guid eId, DateTime date);
    }
}
