using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelatorDataAccess.Context;
using TravelatorDataAccess.EntityModels;
using TravelatorDataAccess.Interfaces;

namespace TravelatorDataAccess.Repositories
{
    public class CabsRepo: ICabsRepo
    {

        private readonly TravelatorContext _context;

        public CabsRepo(TravelatorContext context)
        {
            _context = context;
        }
        public async Task<bool> RequestBooking(CabRequest booking)
        {
            try
            {
                await _context.CabRequests.AddAsync(booking);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error booking cab: {ex.Message}");
                return false;
            }
        }
    }
}
