using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Gleac.Assignment.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Gleac.Assignment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GenerateToken")]
        public IActionResult Login()
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser();

            if (user != null)
            {
                //GENERATE TOKEN
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            string signingKey = _config.GetValue<string>("Jwt:Key");
            string issuer = _config.GetValue<string>("Jwt:Issuer");
            int hours = _config.GetValue<int>("Jwt:HoursValid");
            System.DateTime expireDateTime = System.DateTime.UtcNow.AddHours(hours);

            byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);
            SymmetricSecurityKey secKey = new SymmetricSecurityKey(signingKeyBytes);
            SigningCredentials creds = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userInfo.Username),
                new Claim(ClaimTypes.Email, userInfo.EmailAddress)
            };

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                claims: authClaims,
                expires: System.DateTime.UtcNow.AddHours(hours),
                signingCredentials: creds
            );
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string writtenToken = handler.WriteToken(token);

            return writtenToken;
        }
        private UserModel AuthenticateUser()
        {
            UserModel user = null;

            //Validate the User Credentials    
            //Demo Purpose, I have Passed HardCoded User Information    

            user = new UserModel { Username = "Shaili Khatri", EmailAddress = "shailikhatri@gmail.com" };

            return user;
        }

    }
}
