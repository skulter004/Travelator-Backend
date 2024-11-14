using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelatorDataAccess.Migrations;
using TravelatorService.DTO_s;
using TravelatorService.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace TravelatorBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CabsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ICabsService _cabsService;

        public CabsController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 IConfiguration configuration,
                                 RoleManager<IdentityRole> roleManager,
                                 ICabsService cabsService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _cabsService = cabsService;
        }

        [HttpPost("requestBooking")]
        public async Task<IActionResult> CabBooking(CabBookingDTO booking)
        {
            var employeeId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            booking.EmployeeId = new Guid(employeeId);
            var success = await _cabsService.RequestBooking(booking);
            if (success)
            {
                return Ok("Booking requested successfully.");
            }
            return StatusCode(500, "Error requesting booking.");

        }

        [HttpPost("approveBooking")]
        public async Task<IActionResult> ApproveBooking(CabBookingDTO bookingDetails)
        {
            var success = await _cabsService.ApproveBooking(bookingDetails);
            if (success)
            {
                return Ok("Booking approved successfully.");
            }
            return StatusCode(500, "Error requesting booking.");
        }

        [HttpGet("cabDetails")]
        public async Task<IActionResult> GetCabDetails()
        {
            var employeeId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrEmpty(employeeId))
            {
                 return Ok(new
                {
                    details = await _cabsService.GetCabDetails(new Guid(employeeId))
                });
            }
            return StatusCode(500, "Error requesting details.");
        }
    }
}
