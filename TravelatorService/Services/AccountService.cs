using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelatorDataAccess.EntityModels;
using TravelatorDataAccess.Interfaces;
using TravelatorService.Interfaces;

namespace TravelatorService.Services
{
    public class AccountService: IAccountService
    {
        private readonly IAccountRepo _accountRepo;
        public AccountService(IAccountRepo accountRepo) 
        {
            _accountRepo = accountRepo;
        }
        public async Task<(bool Succeeded, Guid UserId, IEnumerable<IdentityError> Errors)> RegisterUser(RegisterModel model)
        {
            return await _accountRepo.RegisterUser(model);
        }
    }
}
