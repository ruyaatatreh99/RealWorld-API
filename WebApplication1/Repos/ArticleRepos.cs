using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Timers;
using WebApplication1.Controllers;
using WebApplication1.Model;

namespace WebApplication1.Repos
{
    public class ArticleRepos : IArticle
    {
        private userContext _context;
        public ArticleRepos(userContext context)
        {
            _context = context;
        }
        //add
        public article Add(string title, string description, string body, string tag, int user_id)
        {
            article a = new article();
            a.userid = user_id;
            a.title = title;
            a.description = description;
            a.body = body;
            a.tag = tag;
            a.favoritecount = 0;
            a.favorited = false;
            a.slug= title.Replace(' ', '-');
            _context.article.Add(a);
            _context.SaveChanges();
            return a; 
        }

        //delete
        public void Delete(string slug)
        {
            var article = _context.article.FirstOrDefault(x => x.slug == slug);
            if (article != null) {
                _context.article.Remove(article);
                _context.SaveChanges();
            }
           
        }

        //favorite atricle
        public article Favorite(string slug,int userid)
        {
            var favorite = new favorite();
            article ?article = _context.article.FirstOrDefault(x => x.slug == slug);
         
            if (article != null) {
                var check = _context.favorite.FirstOrDefault(x => x.id == article.id && x.userid == userid);
                if (check != null)
                {
                    article.favorited = true;
                    article.favoritecount++;

                    _context.article.Update(article);
                    _context.SaveChanges();

                    favorite.userid = userid;
                    favorite.id = article.id;
                    _context.favorite.Add(favorite);
                    _context.SaveChanges();
                    return article;
                }
                else return article;
            }
            else return null;

        }

        public article favoriteByname(string favorited, int userid,int articleid)
        {
            var favorite = new favorite();
            article? article = _context.article.FirstOrDefault(x => x.id == articleid);

            if (article != null)
            {
                var check = _context.favorite.FirstOrDefault(x => x.id == article.id && x.userid == userid);
                if (check != null)
                {
                    article.favorited = true;
                    article.favoritecount++;

                    _context.article.Update(article);
                    _context.SaveChanges();

                    favorite.userid = userid;
                    favorite.id = article.id;
                    _context.favorite.Add(favorite);
                    _context.SaveChanges();
                    return article;
                }
                else return article;
            }
            else return null;

        }
        //Unfavorite atricle
        public article Unfavorite(string slug, int userid)
        {
            article? article = _context.article.FirstOrDefault(x => x.slug == slug);
            if (article != null)
            {
                var check = _context.favorite.FirstOrDefault(x => x.id == article.id && x.userid == userid);
                if (check != null)
                {
                    article.favorited = false;
                    article.favoritecount--;
                    _context.Update<article>(article);
                    _context.SaveChanges();

                    _context.favorite.Remove(check);
                    _context.SaveChanges();

                    return article;
                }
                else return article;
            }
            else return null;
        }

        public user GetAuthor(int id)
        {
            var article= _context.article.FirstOrDefault(x => x.id == id);
            var author = _context.user.FirstOrDefault(x => x.userid == article.userid);
            return author;
           
        }
        public article GetArticleBySlug(string slug)
        {
            var allarticle= _context.article.FirstOrDefault(x => x.slug == slug);
            return allarticle;
        }
        
        //get all tag
        public List<string> GetTagList()
        {
            List<article> articleList;
            List<string> List = new List<string>();
            articleList = _context.Set<article>().ToList();

            foreach (article a in articleList) List.Add(a.tag);
            return List;
        }

            //get all article
            public List<article> GetArticleList()
        {
            List<article> articleList;

            articleList = _context.Set<article>().ToList();
            return articleList;
        }
      
        //update 
        public article Update(article article)
        {
            _context.article.Update(article);
            _context.SaveChanges();
            return article;
        }

        public List<comment> GetCommentList(string slug)
        {
            List<comment> commentList;
            List<comment> comment_article = new List<comment>();
            commentList = _context.Set<comment>().ToList();
            article ?a= _context.article.FirstOrDefault(x => x.slug == slug);
            if (a != null)
            {
               foreach(comment c in commentList)
                {
                    if (c.article_id == a.id) comment_article.Add(c);  
                }
               
            }

            return comment_article;
        }


        public comment Addcomment(string body, int userid,string slug)
        {
            var user = _context.user.FirstOrDefault(x=>x.userid == userid);
            var article = _context.article.FirstOrDefault(x => x.slug == slug);
            var comment =new comment();
            if (article != null)
            {
                comment.author = user;
                comment.body = body;
                comment.article_id = article.id;
                comment.createdAt= DateTime.Now;
                comment.updatedAt= DateTime.Now;
                _context.comment.Add(comment);
                _context.SaveChanges();
                return comment;
            }
            else return null; 
        }

        public void Deletecomment(int commentId, int userid, string slug)
        {
            var article = _context.article.FirstOrDefault(x => x.slug == slug);
            if (article != null)
            {
                var comment = _context.comment.FirstOrDefault(x => x.article_id == article.id && x.id== commentId);
                if (comment != null)
                {
                    _context.comment.Remove(comment);
                    _context.SaveChanges();
                }
            }
        }
    }
}