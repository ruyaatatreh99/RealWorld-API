using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Model;

namespace WebApplication1.Repos
{


    public class UserPepos : IUser
    {
        public byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        private userContext _context;
        public UserPepos(userContext context)
        {
            _context = context;
        }
        //add user
        public user Add(string email, string passwordtext,  string username)
        {
            user u = new user();
            user checkemail= _context.user.FirstOrDefault(x => x.email == email||x.username==username);
            if (checkemail != null) return null;
            else
            {
                
                var password = Encoding.UTF8.GetBytes(passwordtext);
                u.password = Convert.ToBase64String(password);
                u.email = email;
                u.username = username;
                u.bio = "";
                u.token = Guid.NewGuid().ToString();
                u.image = ""; 
                _context.user.Add(u);
                _context.SaveChanges();
                return u;
            }
        }

        //get user by username
        public user GetProfileByName(string username)
        {
            user? currentuser;
            try
            {
                currentuser = _context.user.FirstOrDefault(x => x.username == username);
            }
            catch (Exception)
            {
                throw;
            }
            return currentuser;
        }

        //get all user
        public List<user> GetUsersList()
        {
            List<user> userList;
            try
            {
                userList = _context.Set<user>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return userList;
        }
      
        //login
        public user GetLoginUser(string email, string password)
        {
          
            var user= _context.user.FirstOrDefault(x => x.email == email );

            if (user == null) return null;
            else {
                var decryptedpassword = Convert.FromBase64String(user.password);
                var result = Encoding.UTF8.GetString(decryptedpassword);
                result=result.Substring(0, result.Length);
                if (result == password) return user;
                else return null;

}
        }

        //update
        public user Update(user UserModel)
        {
            _context.user.Update(UserModel);
            _context.SaveChanges();
            return UserModel;
        }

        // delete by id
        public void DeleteById(int id)
        {
            var u = _context.user.First(x => x.userid == id);
            _context.user.Remove(u);
            _context.SaveChanges();
        }

        public user GetUserByid(int id)
        {
            user? currentuser;
            try
            {
                currentuser = _context.user.FirstOrDefault(x => x.userid== id);
            }
            catch (Exception)
            {
                throw;
            }
            return currentuser;
        }

        public user follow(string username,string email)
        {
            follower f=new follower();
            f.following_name= username;
            f.user_email = email;
            _context.follower.Add(f);
            _context.SaveChanges();
            user ?currentuser = _context.user.FirstOrDefault(x => x.username == username);
            return currentuser;
        }

        public user unfollow(string username, string email)
        {
           var f= _context.follower.FirstOrDefault(x => x.following_name == username && x.user_email == email);
            if (f != null)
            {
                _context.follower.Remove(f);
                _context.SaveChanges();
                user? currentuser = _context.user.FirstOrDefault(x => x.username == username);
                return currentuser;
            }
            else return null;

        }

        public bool followingStatus(int id, string username)
        {
            user? currentuser = _context.user.FirstOrDefault(x => x.userid == id);
           follower? f = _context.follower.FirstOrDefault(x => x.following_name == username && x.user_email == currentuser.email);
            if (f == null)
                return false;
            else return true;
        }
    }
}