using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace BugTracker.Helpers
{
    public class CustomComparer
    {
        public class ProjectComparer : IEqualityComparer<Project>
        {
            public bool Equals(Project project1, Project project2)
            {
                if (object.ReferenceEquals(project1, project2))
                    return true;
                if (project1 == null || project2 == null)
                    return false;
                return project1.Id == project2.Id;
            }
            public int GetHashCode(Project item)
            {
                return new { item.Id }.GetHashCode();
            }
        }
        public class UserComparer : IEqualityComparer<ApplicationUser>
        {
            public bool Equals(ApplicationUser user1, ApplicationUser user2)
            {
                if (object.ReferenceEquals(user1, user2))
                    return true;
                if (user1 == null || user2 == null)
                    return false;
                return user1.Id == user2.Id;
            }
            public int GetHashCode(ApplicationUser item)
            {
                return new { item.Id }.GetHashCode();
            }
        }
    }
}