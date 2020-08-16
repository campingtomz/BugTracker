
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class ManageUserVM
    {
        public ApplicationUser user { get; set; }
       
        public List<Project> UserProjects { get; set; }
        public List<Project> NotUserProjects { get; set; }
        public HttpPostedFileBase Avatar { get; set; }
        #region Constructor
        public ManageUserVM()
        {
            UserProjects = new List<Project>();
            NotUserProjects= new List<Project>();
        }
        #endregion
    }
}