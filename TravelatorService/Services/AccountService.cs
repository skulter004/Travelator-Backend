using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
    public class AccountService: IAccountService
    {
        private readonly IAccountRepo _accountRepo;
        private readonly IMapper _mapper;
        public AccountService(IAccountRepo accountRepo, IMapper mapper) 
        {
            _accountRepo = accountRepo;
            _mapper = mapper;
        }
        public async Task<bool> RegisterUser(RegisterModel model, string userId)
        {
            return await _accountRepo.RegisterUser(model, userId);
        }
        public async Task<EmployeeDTO> GetEmployeeById(Guid id)
        {
            try
            {
                EmployeeDTO employee = _mapper.Map<EmployeeDTO>(await _accountRepo.GetEmployeeById(id));
                return employee;
            }
            catch (Exception ex){
                return null;
            }
        }
    }
}
