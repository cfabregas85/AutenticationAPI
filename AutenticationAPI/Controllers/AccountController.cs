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

        #region Variables

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountService _accountService;

        #endregion

        #region Ctor

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
             IAccountService accountService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._accountService = accountService;
        }

        #endregion

        #region End Points

        [HttpPost("Add")]
        public async Task<ActionResult<UserToken>> AddUser([FromBody] UserInfo model)
        {
            try
            {
                var user = new ApplicationUser { UserName = model.Name, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded) { return this._accountService.BuildToken(model); }
                return BadRequest(result.Errors.Select(e => e.Description).FirstOrDefault());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }                       
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
        {
            try
            {
                var user = _accountService.GetUserByEmail(userInfo.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        userInfo.Name = user.UserName;
                        userInfo.id = user.Id;
                        return this._accountService.BuildToken(userInfo);
                    }

                    ModelState.AddModelError("errorLogin", "Invalid login attempt.");
                    return BadRequest(ModelState);                   
                }
                else
                {
                    ModelState.AddModelError("errorLogin", userInfo.Email +  " do not exist");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }                                
        }

        #endregion

    }
}
