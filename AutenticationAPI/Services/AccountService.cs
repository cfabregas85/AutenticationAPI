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
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AccountService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

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
            var expiration = DateTime.UtcNow.AddHours(1);

            //Create JwtSecurityToken Token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
                );

            if (userInfo.Name == null)
            {
                userInfo.Name = this.GetNameByEmail(userInfo.Email);
            }
            
            return new UserToken()
            {
                Email = userInfo.Email,
                Name = userInfo.Name,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration                
            };
        }

        public string GetNameByEmail(string email) {

            return _context.Users.Where(u => u.Email == email).Select(s=>s.UserName).FirstOrDefault();           
        }

    }
}
