using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelatorService.DTO_s;
using TravelatorService.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TravelatorDataAccess.EntityModels;
using TravelatorService.Services;

namespace TravelatorBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            var employeeId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;


            details.EmployeeId = new Guid(employeeId);

            var success = await _tripsService.TravelRequest(details);
            if (success)
            {
                return Ok( new
                {
                    msg = "Travel requested successfully."
                });
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

        [HttpGet("travelRequests")]
        public async Task<IActionResult> TravelRequests([FromQuery]string status)
        {
            return Ok(new
            {
                requests = await _tripsService.TravelRequests(status)
            });
        }

        [Authorize(Roles = "Admin,Manager,Director")]
        [HttpPost("approveRequest")]
        public async Task<IActionResult> ApproveBooking([FromQuery]Guid bookingId)
        {
            var success = await _tripsService.ApproveRequest(bookingId);
            if (success)
            {
                return Ok( new
                {
                    msg = "Travel Approved successfully."
                });
            }
            return StatusCode(500, "Error Approving booking.");
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("directorApproval")]
        public async Task<IActionResult> DirectorApproval([FromQuery] Guid bookingId)
        {
            var success = await _tripsService.DirectorApproval(bookingId);
            if (success)
            {
                return Ok(new
                {
                    msg = "Sent to director"
                });
            }
            return StatusCode(500, "Error Approving booking.");
        }
    }
}
