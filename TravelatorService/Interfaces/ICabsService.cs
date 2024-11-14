using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelatorService.DTO_s;

namespace TravelatorService.Interfaces
{
    public interface ICabsService
    {
        Task<bool> RequestBooking(CabBookingDTO booking);
        Task<bool> ApproveBooking(CabBookingDTO booking);
        Task<object> GetCabDetails(Guid EmployeeId);
    }
}
