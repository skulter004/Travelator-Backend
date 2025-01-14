using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelatorDataAccess.Context;
using TravelatorDataAccess.EntityModels;
using TravelatorDataAccess.Interfaces;
using TravelatorDataAccess.NotificationHub;

namespace TravelatorDataAccess.Repositories
{
    public class CabsRepo : ICabsRepo
    {

        private readonly TravelatorContext _context;
        private readonly INotificationPublisher _notificationPublisher;

        public CabsRepo(TravelatorContext context, INotificationPublisher publisher)
        {
            _context = context;
            _notificationPublisher = publisher;
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
                _notificationPublisher.PublishNotification(booking.EmployeeId, "Cab booked successfully check my bookings for cab details", "cab_bookings");
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
        public async Task<object> Requests()
        {
            try
            {
                var cabRequests = await (from request in _context.CabRequests
                                         join employee in _context.Employees
                                         on request.EmployeeId equals employee.EmployeeId
                                         where request.EmployeeId == employee.EmployeeId
                                         select new
                                         {
                                             RequestId = request.Id,
                                             EmployeeId = employee.EmployeeId,
                                             Name = employee.Name,
                                             PickUp = request.PickUp,
                                             DropOff = request.DropOff,
                                             Time = request.Time,
                                         }).ToListAsync();
                return cabRequests;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting requests : {ex.Message}");
                return false;
            }
        }
        public async Task<object> AvailableCabs()
        {
            try
            {
                var availableCabs = await _context.Cabs.ToListAsync();
                var bookingCabs = await _context.CabBookings
                    .GroupBy(c => c.CabId)
                    .Select(g => new
                    {
                        CabId = g.Key,
                        OccupiedSeats = g.Count(),
                        Departure = g.Min(cb => cb.PickUp)
                    }).ToListAsync();

                var cabStatus = availableCabs.Select(cab =>
                {
                    var cabBooking = bookingCabs.FirstOrDefault(bCab => bCab.CabId == cab.Id);

                    return new
                    {
                        CabId = cab.Id,
                        CabName = cab.VehicleName,
                        DriverName = cab.DriverName,
                        TotalCapacity = cab.Capacity,
                        RemainingCapacity = cab.Capacity - (cabBooking?.OccupiedSeats ?? 0),
                        DepartureTime = cabBooking?.Departure
                    };
                });
                return cabStatus;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting requests : {ex.Message}");
                return null;
            }

        }

        public async Task<object> MyBooking(Guid eId, DateTime date)
        {
            try
            {
                var bookings = await (from booking in _context.CabBookings
                                      join cab in _context.Cabs
                                      on booking.CabId equals cab.Id
                                      where booking.EmployeeId == eId && booking.Time.Date == date.Date
                                      select new
                                      {
                                          DriverName = cab.DriverName,
                                          VehicleName = cab.VehicleName,
                                          VehicleNumber = cab.VehicleNumber,
                                          ContactNumber = cab.ContactNumber,
                                          PickUp = booking.PickUp,
                                          Time = booking.Time,
                                      }).ToListAsync();

                return bookings;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
