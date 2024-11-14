using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelatorDataAccess.Context;
using TravelatorDataAccess.EntityModels;
using TravelatorDataAccess.Interfaces;
using TravelatorDataAccess.Migrations;

namespace TravelatorDataAccess.Repositories
{
    public class CabsRepo : ICabsRepo
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

        public async Task<bool> ApproveBooking(CabBooking booking)
        {
            try
            {
                var cabRequest = await _context.CabRequests.Where(req => req.EmployeeId == booking.EmployeeId && req.MonthlyRequest == false && req.Time == booking.Time).FirstOrDefaultAsync();
                _context.CabRequests.Remove(cabRequest);
                await _context.CabBookings.AddAsync(booking);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error confirming : {ex.Message}");
                return false;
            }
        }

        public async Task<object> GetCabDetails(Guid EmployeeId)
        {
            try
            {
                var cabDetails = await (from booking in _context.CabBookings
                                        join cab in _context.Cabs
                                        on booking.CabId equals cab.Id
                                        where booking.CabId == cab.Id
                                        select new
                                        {
                                            PickUp = booking.PickUp,
                                            Time = booking.Time,
                                            DriverName = cab.DriverName,
                                            VehicleName = cab.VehicleName,
                                            VehicleNumber = cab.VehicleNumber,
                                            ContactNumber = cab.ContactNumber
                                        }).ToListAsync();
                return cabDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting details : {ex.Message}");
                return false;
            }
        }
    }
}
