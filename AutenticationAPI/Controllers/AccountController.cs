using AutenticationAPI.Models;
using AutenticationAPI.Services;
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
        private readonly IAccountService _accountService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
             IAccountService accountService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._accountService = accountService;
        }

        [HttpPost("Add")]
        public async Task<ActionResult<UserToken>> AddUser([FromBody] UserInfo model)
        {
            var user = new ApplicationUser { UserName = model.Name, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {                               
                return this._accountService.BuildToken(model); 
            }
            else
            {
                return BadRequest(result.Errors.Select(e=>e.Description).FirstOrDefault());
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password ,isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)            
            {
                return this._accountService.BuildToken(userInfo);
            }
            else
            {
                ModelState.AddModelError(string.Empty ,"Invalid login attempt.");
                return BadRequest(ModelState);
            }
        }
        
    }
}
