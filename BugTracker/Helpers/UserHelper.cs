using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Helpers;
using Microsoft.AspNet.Identity;

namespace BugTracker.Helpers
{
    public class UserHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper roleHelper = new UserRoleHelper();
        public ApplicationUser getUser(string userId)
        {
            return db.Users.Find(userId);
        }
       
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
        public bool CanEditUser()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                    return true;
                default:
                    return false;
            }
        }

    }
}