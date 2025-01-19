using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelatorDataAccess.EntityModels;
using TravelatorService.Interfaces;
using TravelatorService.Services;

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
        private readonly IEmailService _emailService;

        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 IConfiguration configuration,
                                 RoleManager<IdentityRole> roleManager,
                                 IAccountService accountService,
                                 IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _accountService = accountService;
            _emailService = emailService;
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string name, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("Invalid email confirmation request.");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("User not found."); 

            if (user.EmailConfirmed)
                return BadRequest("Email already verified.");

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                return BadRequest("Email confirmation failed.");


            RegisterModel model = new RegisterModel
            {
                Email = user.Email,
                Name = name
            };

            if (model != null)
            {
                await _accountService.RegisterUser(model, userId);
            }
            return Ok( new
            {
                msg="Email verified successfully! You can now log in."
            });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
                {
                if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = false
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, "User");
                var newUser = await _userManager.FindByEmailAsync(model.Email);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var encodedToken = Uri.EscapeDataString(token);
                    var verificationUrl = $"https://travlator.netlify.app/auth/verify-email?token={encodedToken}&name={Uri.EscapeDataString(model.Name)}&userId={user.Id}";
                    await _emailService.sendVerificationAsync(model.Name, model.Email, verificationUrl);
                    return Ok();
                }
                else
                {
                    return BadRequest(result);
                }

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
