using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelatorService.DTO_s;
using TravelatorService.Interfaces;

namespace TravelatorBackend.Controllers
{
    public class TripsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ITripsService _tripsService;

        public TripsController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 IConfiguration configuration,
                                 RoleManager<IdentityRole> roleManager,
                                 ITripsService tripsService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _tripsService = tripsService;
        }

        [HttpPost("requestTravel")]
        public async Task<IActionResult> TravelRequest(TravelRequestDTO details)
        {
            var success = await _tripsService.TravelRequest(details);
            if (success)
            {
                return Ok("Travel requested successfully.");
            }
            return StatusCode(500, "Error requesting booking.");

        }

        [HttpPost("addExpense")]
        public async Task<IActionResult> AddExpense(ExpenseDTO expense)
        {
            var success = await _tripsService.AddExpense(expense);
            if (success)
            {
                return Ok("Travel requested successfully.");
            }
            return StatusCode(500, "Error requesting booking.");

        }
    }
}
