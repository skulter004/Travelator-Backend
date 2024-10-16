using Microsoft.EntityFrameworkCore;
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
    public class TripsRepo: ITripsRepo
    {
        private readonly TravelatorContext _context;

        public TripsRepo(TravelatorContext context)
        {
            _context = context;
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
    }
}
