using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("User")]
    public class AdminController : ControllerBase
    {
        private ITokenService TokenService;
        private ITaskService TaskService;
        private IUserService UserService;
        private User user;
        public AdminController(IUserService userService, ITokenService tokenService, ITaskService taskService)
        {
            this.TokenService = tokenService;
            this.TaskService = taskService;
            this.UserService = userService;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] User user)
        {
            user.UserId = UserService.findId(user.UserName, user.Password);
            var claims = new List<Claim>();

            if (user.UserName != "Yossy"
            || user.Password != "1234")
            {
                if (UserService.isExist(user.UserName, user.Password))
                {
                    claims.Add(new Claim("type", "User"));
                    claims.Add(new Claim("name", user.UserName));
                    claims.Add(new Claim("user", user.Password));
                    claims.Add(new Claim("UserId", user.UserId.ToString()));
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                claims.Add(new Claim("type", "Admin"));
                claims.Add(new Claim("name", user.UserName));
                claims.Add(new Claim("user", user.Password));
                claims.Add(new Claim("UserId", user.UserId.ToString()));
            }
            var token = TokenService.GetToken(claims);
            this.user=user;
            return new OkObjectResult(TokenService.WriteToken(token));
        }
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public ActionResult<List<User>> Get()
        {
            return UserService.GetAll();
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "Admin")]

        public ActionResult<User> Get(int id)
        {
            var user = UserService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public ActionResult Post(User user)
        {
            UserService.Add(user);
            return CreatedAtAction(nameof(Post), new { password = user.Password }, user);
        }

        [HttpDelete("{userId}")]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int userId)
        {
            var user = UserService.Get(userId);
            if (user == null)
            {
                return NotFound();
            }
            UserService.Delete(userId);
            return Content(UserService.Count.ToString());
        }
    }
}

