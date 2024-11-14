using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelatorDataAccess.EntityModels;
using TravelatorService.DTO_s;

namespace TravelatorService.Mapper
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles() 
        {
            CreateMap<CabBookingDTO, CabRequest>();
            CreateMap<CabBookingDTO, CabBooking>();
            CreateMap<TravelRequestDTO, TravelRequest>();
            CreateMap<ExpenseDTO, Expense>();
        }
    }
}
