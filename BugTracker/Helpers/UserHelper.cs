using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
  
    public class UserHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
    
        public string GetFirstName(string userId)
        {
            var user = db.Users.Find(userId);
            return user.FirstName;
        }
        public string GetLastName(string userId)
        {
            var user = db.Users.Find(userId);
            return user.LastName;
        }
        public string GetFullName(string userId)
        {
            var user = db.Users.Find(userId);
            return user.FullName;
        }
        public string GetEmail(string userId)
        {
            var user = db.Users.Find(userId);
            return user.Email;
        }
        public string GetAvatar(string userId)
        {
            var user = db.Users.Find(userId);
            return user.AvatarPath;
        }

    }
}