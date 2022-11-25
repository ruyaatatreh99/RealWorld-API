using WebApplication1.Model;

namespace WebApplication1.Repos
{
    public interface IUser
    {
        List<user> GetUsersList();
        user GetProfileByName(string usernamne);
        user Add(string email, string password, string username);
        user Update(user UserModel);
        user GetLoginUser(string email, string password);
        void DeleteById(int id);
        user GetUserByid(int id);
        user follow(string username, string email);
        user unfollow(string username, string email);
        bool followingStatus(int id,string username);

    }
}
