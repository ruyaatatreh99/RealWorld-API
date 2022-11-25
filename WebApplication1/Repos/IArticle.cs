using Microsoft.Extensions.Hosting;
using System.ComponentModel.Design;
using WebApplication1.Model;

namespace WebApplication1.Repos
{
    public interface IArticle
    {
        List<article> GetArticleList();
        article GetArticleBySlug(string slug);
        article Add(string title, string description, string body, string tag, int user_id);
        article Update(article article);
        void Delete(string slug);
        article Favorite(string slug,int userid);
        article favoriteByname(string favorited, int userid,int articleid);
        user GetAuthor(int id);
        article Unfavorite(string slug, int userid);
        List<string> GetTagList();
        List<comment> GetCommentList(string slug);
        void Deletecomment(int commentId, int userid, string slug);
        comment Addcomment(string body, int userid, string slug);


    }
}