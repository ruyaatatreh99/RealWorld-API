using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Get([FromHeader] string user_id, string username)
        {
            try
            {
                int userid = Int16.Parse(user_id);
                var user = _inner.GetProfileByName(username);
                bool status = _inner.followingStatus(userid, username);
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
        public IActionResult GetById([FromHeader] string user_id)
        {
            try {
                int userid = Int16.Parse(user_id);
                if (userid <= 0) return NotFound(new { status = "error", message = "missing authorization credentials" });
                else
                {
                    var user = _inner.GetUserByid(userid);

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
        public IActionResult Put([FromHeader] string user_id,[FromBody] user u)
        {
            try
            {
                int userid = Int16.Parse(user_id);
                if (userid > 0)
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
        [HttpGet]
        [Route("/user/login")]
        public IActionResult GetLoginUser([FromBody] Dictionary<string, string> data)
        {
            try
            {
                var users = _inner.GetLoginUser(data["email"], data["password"]);
                if (users == null) return BadRequest("email or password invaild");
                return Ok(users);
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }

        }
       
        //Delete user
        [HttpDelete]
        [Route("/user/{id}")]
        public IActionResult DeleteById([FromHeader] string user_id)
        {
            try
            {
                int userid = Int16.Parse(user_id);
                if (userid > 0)
                {
                    _inner.DeleteById(userid);
                    return Ok("Deleted successfuly");
                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }

        }

        //Follow user
        [HttpPost]
        [Route("/profiles/celeb_{username}/follow")]
        public IActionResult follow([FromHeader] string user_id,string username, [FromBody] string email)
        {
            try
            {
                int userid = Int16.Parse(user_id);
                if (userid > 0)
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
        public IActionResult unfollow([FromHeader] string user_id,string username)
        {
            try
            {
                int userid = Int16.Parse(user_id);
                
                if (userid > 0)
                {
                    var user = _inner.GetUserByid(userid);
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