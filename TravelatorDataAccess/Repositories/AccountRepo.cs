using Microsoft.AspNetCore.Identity;
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
    public class AccountRepo: IAccountRepo
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TravelatorContext _context;

        public AccountRepo(UserManager<IdentityUser> userManager, TravelatorContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<(bool Succeeded, Guid UserId, IEnumerable<IdentityError> Errors)> RegisterUser(RegisterModel model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                var Employee = new Employee
                {
                    EmployeeId = new Guid(user.Id),
                    Name = model.Name,
                    Department = "Development",
                    BudgetLimit = 5000,
                    UsedBudget = 0,
                    Email = model.Email,
                };
                
                await _context.Employees.AddAsync(Employee);
                await _context.SaveChangesAsync();
                return (true, Employee.EmployeeId, null);
            }

            return (false, Guid.Empty, result.Errors);
        }

        public async Task<Employee> GetEmployeeById(Guid id)
        {
            try
            {
                Employee employee = await _context.Employees.Where(e => e.EmployeeId == id).FirstOrDefaultAsync();

                return employee;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
