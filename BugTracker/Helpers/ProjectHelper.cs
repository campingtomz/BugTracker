using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class ProjectHelper
    {
        //new access to the database 
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper userRoleHelper = new UserRoleHelper();
        public bool CanCreateProject()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                case "ProjectManager":
                    return true;
                default:
                    return false;
            }
        }
        public bool CanEditProject()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                case "ProjectManager":
                    return true;
                default:
                    return false;
            }
        }
        public bool CanViewAllProjects()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                case "ProjectManager":
                    return true;
                default:
                    return false;
            }
        }
        public void AddUserToProject(string userId, int projectId)
        {
            if (!isUserOnProject(userId, projectId))
            {
                var project = db.Projects.Find(projectId);
                var user = db.Users.Find(userId);
                project.Users.Add(user);
                db.SaveChanges();
            }
        }
        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var projects = user.Projects.ToList();
            return (projects);
        }
        public ICollection<Project> ListUserNotOnProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var projects = db.Projects.ToList();
            var userProjects = ListUserProjects(userId);
            projects = projects.Except(userProjects).ToList();
            //var projects = user.Projects.ToList();
            return (projects);
        }
        public bool RemoveUserFromProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var user = db.Users.Find(userId);
            var result = project.Users.Remove(user);
            db.SaveChanges();
            return result;

        }
        public List<ApplicationUser> ListUsersOnProject(int projectId)
        {
            var project = db.Projects.Find(projectId);
            var resultList = new List<ApplicationUser>();
            resultList.AddRange(project.Users);
            return resultList;
        }
        public List<ApplicationUser> ListUsersNotOnProject(int projectId)
        {
            var resultList = new List<ApplicationUser>();
            foreach (var user in db.Users.ToList())
            {
                if(!isUserOnProject(user.Id, projectId)){
                    resultList.Add(user);
                }
            }
            return resultList;

        }
        public bool isUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            //var user = db.Users.Find(userId);
            //return project.Users.Contains(user);
            return project.Users.Any(u=> u.Id == userId);
        }
        public List<ApplicationUser> ListUsesOnMyProjects(string userId)
        {
            List<ApplicationUser> userList = new List<ApplicationUser>();
            var user = db.Users.Find(userId);
            return user.Projects.SelectMany(p => p.Users).Distinct().OrderByDescending(p=>p.Email).ToList();
            
        }
        public List<ApplicationUser> ListUserOnProjectInRole(int projectId, string roleName)
        {
            var userList = ListUsersOnProject(projectId);
            var resultList = new List<ApplicationUser>();
            foreach (var user in userList)
            {
                if(userRoleHelper.IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }
    
    }
}