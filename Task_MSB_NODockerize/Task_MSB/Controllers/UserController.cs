using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Task_MSB.Context;
using Task_MSB.Models;

namespace Task_MSB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private TaskContext db;

        public UserController(TaskContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Get All User if user Authorize
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult GetAllUser()
        {
            var result = new ObjectResult(db.Users);
            return result;
        }

        /// <summary>
        /// Register User by class User
        /// </summary>
        /// <param name="user">Class User</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("the input data invalid");
            }
            try
            {
                db.Users.Add(user);
                db.SaveChanges();
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest($"error message : {e.Message}");
            }
        }

        /// <summary>
        /// Login User by Username and Password
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        [HttpPost("{username}/{password}")]
        public IActionResult Login([FromRoute] string username, [FromRoute] string password)
        {
            if (username?.Length < 0 && password?.Length < 0)
            {
                return BadRequest("the username and password should have data");
            }

            var user = db.Users
                .SingleOrDefault(u => u.Username == username && u.Password == password);
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OurVerifyTopLearn"));

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOption = new JwtSecurityToken(
                issuer: "https://localhost:44354",
                claims: new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,user.Username),
                },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOption);

            return Ok(new { token = tokenString });

        }
    }
}
