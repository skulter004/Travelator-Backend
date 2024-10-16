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
    public class CabsService: ICabsService
    {
        private readonly ICabsRepo _cabsRepo;
        private readonly IMapper _mapper;
        public CabsService(ICabsRepo cabsRepo, IMapper mapper) 
        {
            _cabsRepo = cabsRepo;
            _mapper = mapper;
        } 
        public async Task<bool> RequestBooking(CabBookingDTO booking)
        {
            try
            {
                var cabBooking = _mapper.Map<CabRequest>(booking);

                return await _cabsRepo.RequestBooking(cabBooking);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing booking: {ex.Message}");
                return false;
            }
        }
    }
}
