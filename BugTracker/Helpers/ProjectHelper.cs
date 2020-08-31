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
        private UserHelper userHelper = new UserHelper();

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
        public List<Project> GetProjecstByIds(List<int> projectIds)
        {
            List<Project> projects= new List<Project>();
            foreach(var projectId in projectIds)
            {
                projects.Add(db.Projects.Find(projectId));
            }
            return projects;
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
        public void AddGroupOfUsersToProject(List<string> userIds, int projectId)
        {
            foreach (var userId in userIds.ToList())
            {
                AddUserToProject(userId, projectId);
            }
        }
        public void AddGroupOfProjectsToUser(List<int> projectIds, string userId)
        {
            foreach (var projectId in projectIds)
            {
                AddUserToProject(userId, projectId);
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
        public ICollection<Project> ListUserNotOnProjectsbyRole(string userId, string RoleName)
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
        public void RemoveGroupOfUsersFromProject(List<ApplicationUser> users, int projectId)
        {
            foreach (var user in users)
            {
                RemoveUserFromProject(user.Id, projectId);
            }

        }
        public void RemoveAllUsersFromProject(int projectId)
        {
            foreach (var user in ListUsersOnProject(projectId))
            {
                RemoveUserFromProject(user.Id, projectId);
            }
        }
        public void RemoveAllProjectsFromUser(string userId)
        {
            var user = db.Users.Find(userId);
            foreach (var project in user.Projects.ToList())
            {
                RemoveUserFromProject(user.Id, project.Id);
            }
        }
        public void UpdateProjectUserIds(string userIds, int projectId)
        {
            RemoveAllUsersFromProject(projectId);
            if (!String.IsNullOrWhiteSpace(userIds))
            {
                List<string> userIdsList = userIds.Split(',').ToList();
                AddGroupOfUsersToProject(userIdsList, projectId);
            }
        }
        public void UpdateProjectUsers(string users, string role, int projectId)
        {
            var usersList = ListUserOnProjectInRole(projectId, role);
            RemoveGroupOfUsersFromProject(usersList, projectId);
            if (!String.IsNullOrWhiteSpace(users))
            {
                List<string> userIds = users.Split(',').ToList();
                if (userIds.Count > 0)
                {
                    AddGroupOfUsersToProject(userIds, projectId);
                }
            }

            //if (DeveloperIds.Count > 0)
            //{
            //    var devs = ListUserOnProjectInRole(projectId, "Developer");
            //    RemoveGroupOfUsersFromProject(devs, projectId);
            //    AddGroupOfUsersToProject(DeveloperIds, projectId);
            //}
            //if (SubmitterIds.Count > 0)
            //{
            //    var subs = ListUserOnProjectInRole(projectId, "Submitter");
            //    RemoveGroupOfUsersFromProject(subs, projectId);
            //    AddGroupOfUsersToProject(SubmitterIds, projectId);
            //}
        }
        public void updateUserProjects(string userId, string projectIds)
        {
            RemoveAllProjectsFromUser(userId);
            if (!String.IsNullOrWhiteSpace(projectIds))
            {
                List<int> projectIdList = projectIds.Split(',').Select(i => int.Parse(i)).ToList();
                AddGroupOfProjectsToUser(projectIdList, userId);
            }
            db.SaveChanges();
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
                if (!isUserOnProject(user.Id, projectId))
                {
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
            return project.Users.Any(u => u.Id == userId);
        }
        public List<ApplicationUser> ListUsesOnMyProjects(string userId)
        {
            List<ApplicationUser> userList = new List<ApplicationUser>();
            var user = db.Users.Find(userId);
            return user.Projects.SelectMany(p => p.Users).Distinct().OrderByDescending(p => p.Email).ToList();

        }
        public List<ApplicationUser> ListUserOnProjectInRole(int projectId, string roleName)
        {
            var userList = ListUsersOnProject(projectId);
            var resultList = new List<ApplicationUser>();
            foreach (var user in userList)
            {
                if (userRoleHelper.IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }
        public List<ApplicationUser> ListUserOnProjectExceptInRole(int projectId, string roleName)
        {
            var userList = ListUsersOnProject(projectId);
            var resultList = new List<ApplicationUser>();
            foreach (var user in userList)
            {
                if (!userRoleHelper.IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }
        public List<ApplicationUser> ListUserNotOnProjectInRole(int projectId, string roleName)
        {
            var userList = ListUsersNotOnProject(projectId);
            var resultList = new List<ApplicationUser>();
            foreach (var user in userList)
            {
                if (userRoleHelper.IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }
        public List<ApplicationUser> ListUsesNotOnProjectInExceptInRole(int projectId, string roleName)
        {
            var userList = ListUsersNotOnProject(projectId);
            var resultList = new List<ApplicationUser>();
            foreach (var user in userList)
            {
                if (!userRoleHelper.IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }
        public List<ApplicationUser> GetUsersOnMyProjects()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                    return db.Users.ToList();
                default:

                    List<ApplicationUser> UserList = new List<ApplicationUser>();
                    List<Project> projects = ListUserProjects(userId).ToList();
                    foreach (var project in projects)
                    {
                        UserList.AddRange(project.Users.Except(UserList));
                    }
                    return UserList;
            }
        }

        public List<Project> GetUserProjects()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                case "ProjectManager":
                    return db.Projects.ToList();
                case "Submitter":
                case "Developer":
                    var user = userHelper.getUser(userId);
                    return user.Projects.ToList();
                default:
                    return null;
            }
        }
        //public void ProjectEdits(Project oldProject, Project newProject, List<ApplicationUser> OldUserList)
        //{
        //    historyHelper.ProjectHistoriesEdit( oldProject,  newProject);
        //    notificationHelper.ProjectChangedNotification(newProject, oldProject, OldUserList);
        //}

    }
    }