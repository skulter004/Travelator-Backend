using Microsoft.EntityFrameworkCore;
using TravelatorDataAccess.Context;
using TravelatorDataAccess.EntityModels;
using TravelatorDataAccess.Interfaces;

namespace TravelatorDataAccess.Repositories
{
    public class TripsRepo: ITripsRepo
    {
        private readonly TravelatorContext _context;
        private readonly INotificationPublisher _notificationPublisher;

        public TripsRepo(TravelatorContext context, INotificationPublisher notificationPublisher)
        {
            _context = context;
            _notificationPublisher = notificationPublisher;
        }
        public async Task<bool> TravelRequest(TravelRequest details)
        {
            try
            {
                await _context.TravelRequests.AddAsync(details);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {                
                Console.WriteLine($"Error requesting travel: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> AddExpense(Expense expense)
        {
            try
            {
                await _context.Expenses.AddAsync(expense);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding expense cab: {ex.Message}");
                return false;
            }
        }
        public async Task<object> TravelRequests(string status)
        {
            try
            {
                var travelRequests = await (from request in _context.TravelRequests
                                            join employee in _context.Employees
                                            on request.EmployeeId equals employee.EmployeeId
                                            where request.Status == status
                                            select new
                                            {
                                                Id = request.RequestId,
                                                Name = employee.Name,
                                                TravelType = request.TravelType,
                                                Purpose = request.Purpose,
                                                Cost = request.EstimatedCost,
                                                StartDate = request.StartDate,
                                                EndDate = request.EndDate,
                                            }).ToListAsync();
                return travelRequests;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> ApproveRequest(Guid id)
        {
            try
            {
                TravelRequest request = await _context.TravelRequests.Where(req => req.RequestId == id).FirstOrDefaultAsync();
                request.Status = "Approved";
                await _context.SaveChangesAsync();
                _notificationPublisher.PublishNotification(request.EmployeeId, string.Format("Your Travel Request for {0} is Approved", request.TravelType), "travel_request");
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }
        public async Task<bool> DirectorApproval(Guid id)
        {
            try
            {
                TravelRequest request = await _context.TravelRequests.Where(req => req.RequestId == id).FirstOrDefaultAsync();
                request.Status = "Review";
                await _context.SaveChangesAsync();
                _notificationPublisher.PublishNotification(request.EmployeeId, string.Format("Your Travel Request for {0} is Approved", request.TravelType), "travel_request");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
