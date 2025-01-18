using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelatorDataAccess.EntityModels;
using TravelatorService.Interfaces;

namespace TravelatorBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]     
    public class AccountController: ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;

        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 IConfiguration configuration,
                                 RoleManager<IdentityRole> roleManager,
                                 IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
                {
            if (ModelState.IsValid)
            {
                var (succeeded, userId, errors) = await _accountService.RegisterUser(model);

                if (succeeded)
                {
                    return Ok(new { message = "User registered successfully!", userId });
                }

                return BadRequest(errors);
            }

            return BadRequest(ModelState);
        }
    

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    try
                    {
                        var token = GenerateJwtToken(user);
                        return Ok(new { token });
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine(ex);
                    }
                   
                }

                return Unauthorized();
            }

            return BadRequest(ModelState);
        }

        [HttpGet("profileDetails")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeDetails()
        {
            var employeeId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return Ok(new
            {
                details = await _accountService.GetEmployeeById(new Guid(employeeId))
            });
            
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var userRoles = _userManager.GetRolesAsync(user).Result;

            var claims = new List<Claim>
                            {
                                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                            };
            foreach (var role in userRoles)
            {
                claims.Add(new Claim("roles", role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),  
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
