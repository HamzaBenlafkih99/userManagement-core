using Microsoft.AspNetCore.Mvc;
using Please.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Please.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class registerController : ControllerBase
    {
        private readonly ConnectionDbClass _context;

        public registerController(ConnectionDbClass context)
        {
            _context = context;
        }

        // GET: api/<registerController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<registerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<registerController>
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            if(user.Cne == "" || user.Email == "" || user.Username == "" && user.Password == "")
            {
                return Ok(new Response() { Status = "Error", Message = "Invalid User input" });
            }
            User newUser = new User();
            newUser.Email = user.Email;
            newUser.Password = user.Password;
            newUser.Cne = user.Cne;
            newUser.Username = user.Username;
            newUser.Role = "User";

            _context.Add(newUser);
            _context.SaveChanges();

            return Ok(new Response() { Status = "Success", Message = "Profile created successfuly" });

        }

        // PUT api/<registerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<registerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
