
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class ManageUserVM
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }    
        public string userRole { get; set; }
        public string AvatarPath { get; set; }
        public string ProjectIds { get; set; }
        public HttpPostedFileBase Avatar { get; set; }

    }
}