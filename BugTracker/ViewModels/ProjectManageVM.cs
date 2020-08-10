using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;
using BugTracker.Helpers;

namespace BugTracker.ViewModels
{
    public class ProjectManageVM
    {
        private UserRoleHelper roleHelper = new UserRoleHelper();
        public Project projectValue { get; set; }
        public ApplicationUser projectManager { get; set; }

        public List<ApplicationUser> Developers { get; set; }
        public List<ApplicationUser> Submitters { get; set; }
        public ProjectManageVM(Project projectValue)
        {
            this.projectValue = projectValue;
            Developers = GetPojectUsersByRole(projectValue.Id, projectValue.Users.ToList(), "Developer");
            Submitters = GetPojectUsersByRole(projectValue.Id, projectValue.Users.ToList(), "Submitter");
            projectManager = getProjectManager(projectValue.Id, projectValue.Users.ToList());
        }
        public List<ApplicationUser> GetPojectUsersByRole(int projectId, List<ApplicationUser> projectUsers, string RoleName)
        {
            List<ApplicationUser> Developers = new List<ApplicationUser>();
            foreach (var user in projectUsers)
            {
                if (roleHelper.IsUserInRole(user.Id, RoleName))
                {
                    Developers.Add(user);
                }

            }
            return Developers;
        }

        public ApplicationUser getProjectManager(int projectId, List<ApplicationUser> projectUsers)
        {
            foreach (var user in projectValue.Users)
            {
                if (roleHelper.IsUserInRole(user.Id, "ProjectManager"))
                {
                    return user;
                }

            }
            return null;
        }
    }
}