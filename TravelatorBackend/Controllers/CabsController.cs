using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelatorService.DTO_s;
using TravelatorService.Interfaces;

namespace TravelatorBackend.Controllers
{
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
            var success = await _cabsService.RequestBooking(booking);
            if (success)
            {
                return Ok("Booking requested successfully.");
            }
            return StatusCode(500, "Error requesting booking.");

        }
    }
}
