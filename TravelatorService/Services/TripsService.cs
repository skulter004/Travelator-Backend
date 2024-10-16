using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelatorDataAccess.EntityModels;
using TravelatorDataAccess.Interfaces;
using TravelatorService.DTO_s;
using TravelatorService.Interfaces;

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
    }
}
