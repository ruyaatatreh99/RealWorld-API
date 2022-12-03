using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Drawing;
using WebApplication1.Model;
using WebApplication1.Repos;
namespace WebApplication1.Controllers
{
    [ApiController]
    public class ArticleController : ControllerBase
    {

        private readonly IArticle _inner;
        private readonly IUser _user;
        private readonly IMapper _mapper;

        public ArticleController(IArticle inner, IMapper mapper, IUser _user1)
        {
            _inner = inner;
            _mapper = mapper;
            _user = _user1;

        }

        [Route("/articles/{slug}/favorite")]
        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult favorite([FromHeader] int user_id, string slug)
        {
            try
            {
                if (user_id > 0)
                {
                    user user = _user.GetUserByid(user_id);
                    var a = _inner.Favorite(slug, user_id);
                    if (a != null) return Ok(new { articles = a, favoritedBy = user, articlesCount = 1 });
                    else return Ok(new { articles = "", favoritedBy = "", articlesCount = 0 });
                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception)
            {
                return new JsonResult(new { status = 500, message = "Error" });
            }
        }

        //favorite by username
        [Route("/articles/{favorited}")]
        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult favoriteByname([FromHeader] int user_id, string favorited, int articleid)
        {
            try
            {
                if (user_id > 0)
                {
                    var a = _inner.favoriteByname(favorited, user_id, articleid);
                    if (a != null) return Ok(new { articles = a, articlesCount = 1 });
                    else return Ok(new { article = "", articlesCount = 0 });
                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }

        }

        [Route("/articles/{slug}/favorite")]
        [HttpDelete]
        [Authorize(Roles = "User")]
        public IActionResult Unfavorite([FromHeader] int user_id, string slug)
        {
            try
            {
                if (user_id > 0)
                {
                    var a = _inner.Unfavorite(slug, user_id);
                    if (a != null) return Ok(new { articles = a, favoritedBy = "", articlesCount = 1 });
                    else return Ok(new { article = "", favoritedBy = "", articlesCount = 0 });
                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }

        }

        [Route("/tags")]
        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult GetTagList()
        {
            try
            {
                var a = _inner.GetTagList();
                return Ok(new { articles = a, articlesCount = a.Count() });
            }
            catch (Exception) { return new JsonResult(new { data = "Error", staus = 500 }); }

        }

        [Route("/articles/{slug}/comments")]
        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult GetCommentList([FromHeader] int user_id, string slug)
        {
            try
            {
                if (user_id > 0)
                {
                    var a = _inner.GetCommentList(slug);
                    return Ok(a);
                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { data = "Error", staus = 500 }); }

        }

        [Route("/article")]
        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult Get()
        {
            try
            {
                var articles = _inner.GetArticleList();
                if (articles == null) return NotFound(new { articles = "", articlescount = 0 });
                return Ok(new { articles = articles, articlescount = articles.Count() });
            }
            catch (Exception) { return new JsonResult(new { data = "Error", staus = 500 }); }
        }

        //create comment
        [Route("/articles/{slug}/comments")]
        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult Addcomment([FromHeader] int user_id, [FromBody] Dictionary<string, string> data, string slug)
        {
            try
            {
                if (user_id > 0)
                {
                    var comment = _inner.Addcomment(data["body"], user_id, slug);
                    if (comment != null) return Ok(comment);
                    else return NotFound(new { });

                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }
        }

        //delete comment
        [Route("/articles/{slug}/comments/{commentId}")]
        [HttpDelete]
        [Authorize(Roles = "User")]
        public IActionResult Deletecomment([FromHeader]int user_id, int commentId, string slug)
        {
            try
            {
                if (user_id > 0)
                {
                    _inner.Deletecomment(commentId, user_id, slug);
                    return Ok(new { });


                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }
        }

        //Add Article
        [Route("/article")]
        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult Add([FromHeader] int user_id, [FromBody] Dictionary<string, string> data)
        {
            try
            {
                if (user_id > 0)
                {
                    var article = _inner.Add(data["title"], data["description"], data["body"], data["tag"], user_id);
                    return Ok(article);
                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }
        }

        [Route("/article")]
        [HttpPut]
        [Authorize(Roles = "User")]
        public IActionResult update([FromHeader] int user_id, [FromBody] article article)
        {
            try
            {
                if (user_id > 0)
                {
                    var a = _inner.Update(article);
                    return Ok(a);
                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }
        }

        [Route("/article/{slug}")]
        [HttpDelete]
        [Authorize(Roles = "User")]
        public IActionResult Delete([FromHeader] int user_id, string slug)
        {
            try
            {
                if (user_id > 0)
                {
                    _inner.Delete(slug);
                    return Ok(new { });
                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }
        }

        [Route("/articles")]
        [HttpGet]
        public IActionResult GetSlugArticle([FromHeader] int user_id, string slug)
        {
            try
            {
                if (user_id > 0)
                {
                    var articel = _inner.GetArticleList();
                    var Myarticle = articel.Where(a => a.slug == slug);
                    var articleVM = _mapper.Map<List<article>>(Myarticle);
                    if (articleVM == null) return NotFound(new { articles = 0, articlesCount = 0 });
                    else return Ok(new { articles = articleVM, articlesCount = articleVM.Count() });
                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }
        }

        [Route("/article/Author")]
        [HttpGet]
        public IActionResult GetArticle([FromHeader] int user_id, string username)
        {
            try
            {
                if (user_id > 0)
                {
                    user user = _user.GetProfileByName(username);
                    if (user != null)
                    {
                        var allarticle = _inner.GetArticleList();
                        var Myarticle = allarticle.Where(a => a.userid == user.userid);
                        var articleVM = _mapper.Map<List<article>>(Myarticle);

                        return Ok(new { articles = articleVM, articlesCount = articleVM.Count() });
                    }

                    else return Ok(new { articles = 0, articlesCount = 0 });
                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }

        }

        [Route("/article/{tag}")]
        [HttpGet]
        public IActionResult GetByFilter([FromHeader] int user_id, string tag)
        {
            try
            {
                if (user_id > 0)
                {
                    var articel = _inner.GetArticleList();
                    var Myarticle = articel.Where(a => a.tag == tag);
                    var articleVM = _mapper.Map<List<article>>(Myarticle);
                    if (articleVM == null) return NotFound(new { articles = 0, articlesCount = 0 });
                    else return Ok(new { articles = articleVM, articlesCount = articleVM.Count() });
                }
                else return NotFound(new { status = "error", message = "missing authorization credentials" });
            }
            catch (Exception) { return new JsonResult(new { status = 500, message = "Error" }); }
        }

    }
}
