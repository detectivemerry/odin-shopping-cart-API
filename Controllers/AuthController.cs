using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System;
using OdinShopping.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace OdinShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public AuthController(DataContext context, IConfiguration configuration, IUserService userService)
        {
            _context = context;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetMe() //Implement this HTTP context when using [Authorize]
        {
            // One way to retrieve token information
            // May lead to Fat controllers, for best practices, use Service Dependency Injection instead
            //var userName = User?.Identity?.Name; // From Claim
            //if (userName == null)
            //{
            //    return BadRequest();
            //}

            ////var userName2 = await _context.Users.FindFirstValue(userName);
            //return Ok(userName);

            // Best practice retrieving HTTP context
            var userName = _userService.GetUserName();
            return Ok(userName);
        }


        [HttpPost("register")]
        public async Task<ActionResult<Models.User>> Register(UserDto request)
        {
            Models.User user = new Models.User();
            user.Username = request.Username;
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = "User";
            user.Carts = null;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            string token = CreateToken(user);
            return Ok(token);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var username = await _context.Users.Where(x => x.Username == request.Username).ToListAsync();

            if (username != null && username.Count == 1)
            {
                if (VerifyPasswordHash(request.Password, username[0].PasswordHash, username[0].PasswordSalt))
                {
                    string token = CreateToken(username[0]);
                    return Ok(token);
                }                
                else
                    return BadRequest("Username and password combination is incorrect.");
            }
            else
            {
                return BadRequest("Username not found.");
            }
        }

        private static bool VerifyPasswordHash(string password, byte[]? passwordHash, byte[]? passwordSalt)
        {
            if(passwordHash == null || passwordSalt == null)
                return false;

            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            if(user == null)
                return string.Empty;
            
            List<Claim> claims = new List<Claim> 
            { 
                new Claim(ClaimTypes.Name, user.Username), //describes the user that is authenticated, for e.g setting Name to be Username in token
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            };
            
            string? tokenValue = _configuration.GetSection("AppSettings:Token").Value;
            if (tokenValue != null)
            {
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenValue));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var token = new JwtSecurityToken(
                    claims : claims,
                    expires : DateTime.Now.AddHours(1),
                    signingCredentials : cred
                );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }
            
            return string.Empty;
        }
    }
}
