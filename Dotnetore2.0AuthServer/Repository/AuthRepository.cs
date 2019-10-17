using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetCore2.Infrastrucutre.Data.Identity; 

namespace DotnetCore2.AuthServer.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private AppIdentityDbContext db;

        public AuthRepository(AppIdentityDbContext context)
        {
            db = context;
        }

        public AppUser GetUserById(string id)
        {
            var user = db.Users.Where(u => u.Id == id).FirstOrDefault();
            return user;
        }

        public AppUser GetUserByUsername(string username)
        {
            var user = db.Users.Where(u => String.Equals(u.Email, username)).FirstOrDefault();
            return user;
        }

        public bool EmailConfirmed(string username)
        {
            try
            {
                var user = db.Users.Where(u => String.Equals(u.Email, username)).FirstOrDefault();
                return user.EmailConfirmed;
            }
            catch
            {
                return false;
            }
            
        }

        public bool ValidatePassword(string username, string plainTextPassword)
        {
            var user = db.Users.Where(u => String.Equals(u.Email, username)).FirstOrDefault();
            if (user == null) return false;
            if (String.Equals(plainTextPassword, user.PasswordHash)) return true;
            return false;
        }
    }

    public interface IAuthRepository
    {
        AppUser GetUserById(string id);
        AppUser GetUserByUsername(string username);
        bool ValidatePassword(string username, string plainTextPassword);
        bool EmailConfirmed(string username);
    }
}
