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
    }
}
