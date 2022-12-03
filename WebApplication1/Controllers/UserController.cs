using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Model;
using WebApplication1.Repos;
namespace WebApplication1.Controllers
{
    [ApiController]
    public class UserController:ControllerBase
    {
        
        private readonly IUser _inner;
       // private readonly userContext _context;
        public UserController(IUser inner)
        {
            _inner = inner;
       
        }

        //get profile user
        [Route("/profiles/celeb_{username}")]
        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult Get([FromHeader] int user_id, string username)
        {
            try
            {
                var user = _inner.GetProfileByName(username);
                bool status = _inner.followingStatus(user_id, username);
                if (user == null) return NotFound();
                else {
                    return Ok(new { username = "celeb_" +user.username, bio = user.bio, image = user.image, following = status });
                }  
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }
        }

        //get user by id
        [Route("/users/{id}")]
        [HttpGet]
        public IActionResult GetById([FromHeader] int user_id)
        {
            try {
                if (user_id <= 0) return NotFound(new { status = "error", message = "missing authorization credentials" });
                else
                {
                    var user = _inner.GetUserByid(user_id);

                    if (user == null) return NotFound();
                    return Ok(user);
                }
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }
        }

        //Add user
        [HttpPost]
        [Route("/user")]
        public IActionResult Post([FromBody] Dictionary<string, string> data)
        {
            try
            {
                
                user user = _inner.Add(data["email"], data["password"], data["username"]);
                if(user == null) return NotFound(new { errors = "Email or username has already been taken" });
               else return Ok(new { user=user });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }

        }

        //update user
        [HttpPut]
        [Route("/user")]
        [Authorize(Roles = "User")]
        public IActionResult Put([FromHeader] int user_id,[FromBody] user u)
        {
            try
            {
                if (user_id > 0)
                {
                    user user = _inner.Update(u);
                    return Ok(user);
                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }
        }

        //get All User
        [HttpGet]
        [Route("/user")]
        public IActionResult GetAllUser()
        {
            try
            {
                var users = _inner.GetUsersList();
                if (users == null) return NotFound();
                return Ok(users);
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }

        }

        //Login User
        [HttpPost]
        [Route("/user/login")]
        public IActionResult GetLoginUser([FromBody] Dictionary<string, string> data)
        {
            try
            {
                var users = _inner.GetLoginUser(data["email"], data["password"]);
                if (users == null) return BadRequest("email or password invaild");
                else
                {
                    List<Claim> claims = new List<Claim>
                {
                     new Claim("type",data["email"]),
                     new Claim(ClaimTypes.Role,"User"),
                };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));

                    var Token = new JwtSecurityToken(
            "https://fbi-demo.com",
            "https://fbi-demo.com",
           claims,
            expires: DateTime.Now.AddDays(90),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

                    var jwt = new JwtSecurityTokenHandler().WriteToken(Token);
                    return Ok(new { User = users, Token = jwt });
                }
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }

        }
       
        //Delete user
        [HttpDelete]
        [Route("/user/{id}")]
        [Authorize(Roles = "User")]
        public IActionResult DeleteById([FromHeader] int user_id)
        {
            try
            {
                if (user_id > 0)
                {
                    _inner.DeleteById(user_id);
                    return Ok("Deleted successfuly");
                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }

        }

        //Follow user
        [HttpPost]
        [Route("/profiles/celeb_{username}/follow")]
        [Authorize(Roles = "User")]
        public IActionResult follow([FromHeader] int user_id,string username, [FromBody] string email)
        {
            try
            {
                if (user_id > 0)
                {
                    user profile = _inner.follow(username, email);
                    return Ok(new { username = "celeb_" + profile.username, bio = profile.bio, image = profile.image, following = true });
                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }

        }
       
        //unFollow user
        [HttpDelete]
        [Route("/profiles/celeb_{username}/follow")]
        [Authorize(Roles = "User")]
        public IActionResult unfollow([FromHeader] int user_id,string username)
        {
            try
            {
                if (user_id > 0)
                {
                    var user = _inner.GetUserByid(user_id);
                    user profile = _inner.unfollow(username, user.email);
                    if (profile != null)
                    return Ok(new { username = "celeb_" + profile.username, bio = profile.bio, image = profile.image, following = false });
                    else
                    return Ok("user not found");
                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }
        }
    }
}