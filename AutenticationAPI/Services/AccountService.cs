using AutenticationAPI.Contexts;
using AutenticationAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutenticationAPI.Services
{
    public class AccountService : IAccountService
    {

        #region Variables

        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        #endregion

        #region Ctor

        public AccountService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        #endregion

        #region Methods

        public UserToken BuildToken(UserInfo userInfo)
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
            var expiration = DateTime.UtcNow.AddMinutes(1);

            //Create JwtSecurityToken Token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
                );           
            
            return new UserToken()
            {
                id = userInfo.id,
                Name = userInfo.Name,
                Email = userInfo.Email,                
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration                
            };
        }

        public ApplicationUser GetUserByEmail(string email) {

            return _context.Users.Where(u => u.Email == email).FirstOrDefault();           
        }

        #endregion

    }
}
