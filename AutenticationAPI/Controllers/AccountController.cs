using AutenticationAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;
        }

        [HttpPost("Add")]
        public async Task<ActionResult<UserToken>> AddUser([FromBody] UserInfo model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return BuildToken(model);
            }
            else
            {
                return BadRequest("Username or Password invalid");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password ,isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)            
            {
                return BuildToken(userInfo);
            }
            else
            {
                ModelState.AddModelError(string.Empty ,"Invalid login attempt.");
                return BadRequest(ModelState);
            }
        }
        private UserToken BuildToken(UserInfo userInfo)
        {

            //Create Claim
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName,userInfo.Email),
                new Claim("Value", "MyDescripion" ),
                //Jti: This is a unique Token Value !!!
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
             };

            //Create Key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Token Expiration
            var expiration = DateTime.UtcNow.AddHours(1);

            //Create JwtSecurityToken Token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer:null,
                audience:null,
                claims :claims,
                expires: expiration,
                signingCredentials:creds
                );

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration            
            };
        }
    }
}
