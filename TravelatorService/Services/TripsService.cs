using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TravelatorDataAccess.EntityModels;
using TravelatorDataAccess.Interfaces;
using TravelatorDataAccess.Repositories;
using TravelatorService.DTO_s;
using TravelatorService.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TravelatorService.Services
{
    public class TripsService: ITripsService
    {
        private readonly ITripsRepo _tripsRepo;
        private readonly IMapper _mapper;
        public TripsService(ITripsRepo tripsRepo, IMapper mapper)
        {
            _tripsRepo = tripsRepo;
            _mapper = mapper;
        }
        public async Task<bool> TravelRequest(TravelRequestDTO details)
        {
            try
            {
                var tripDetails = _mapper.Map<TravelRequest>(details);
                tripDetails.Status = "Requested";
                return await _tripsRepo.TravelRequest(tripDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing request: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> AddExpense(ExpenseDTO expense)
        {
            try
            {
                var _expense = _mapper.Map<Expense>(expense);

                return await _tripsRepo.AddExpense(_expense);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing request: {ex.Message}");
                return false;
            }
        }
        public async Task<object> TravelRequests(string status)
        {
            try
            {
                return await _tripsRepo.TravelRequests(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting requests: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> ApproveRequest(Guid id)
        {
            try
            {
                return await _tripsRepo.ApproveRequest(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error approving request: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DirectorApproval(Guid id)
        {
            try
            {
                return await _tripsRepo.DirectorApproval(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error requesting: {ex.Message}");
                return false;
            }
        }
    }
}
