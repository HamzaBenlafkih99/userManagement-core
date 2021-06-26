using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Please.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Please.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class connectController : ControllerBase
    {
        private readonly ConnectionDbClass _context;
        private readonly IConfiguration _configuration;

        public connectController(ConnectionDbClass context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/<connectController>
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var idUser = HttpContext.User.Claims.Where(x => x.Type == "userId").SingleOrDefault();
            var utilisateur = _context.Users.Where(x => x.Id == int.Parse(idUser.Value));

            if (idUser == null)
            {
                return Ok(new Response() { Status = "Error", Message = "Something was wrong" });
            }
            return Ok(new { user = utilisateur });
        }



        // POST api/<connectController>
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            var jwtToken = generateJsonWebToken(user);
            if (jwtToken == null)
            {
                return Ok(new Response() { Status = "Error", Message = "Username doas not exist" });
            }
            return Ok(new { token = jwtToken });
        }


        private string generateJsonWebToken(User userInfo)
        {
            var user = _context.Users.Where(x => x.Username == userInfo.Username && x.Password == userInfo.Password).SingleOrDefault();
            if (user == null)
            {
                return null;
            }
            //var signingKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var expryDuration = int.Parse(_configuration["Jwt:ExpiryDuration"]);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = null,
                Audience = null,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expryDuration),
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("userId", user.Id.ToString()),
                    new Claim("roles", user.Role),
                    new Claim("usernames", user.Username.ToString()),
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = jwtHandler.WriteToken(jwtToken);
            return token;
        }
    }
}
