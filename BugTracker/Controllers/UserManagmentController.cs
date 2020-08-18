using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    
    [Authorize]
    public class UserManagmentController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private UserHelper userHelper = new UserHelper();
        private UserRoleHelper roleHelper = new UserRoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        // GET: UserManagment 
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var model = db.Users.ToList();
            if (!roleHelper.IsUserInRole(userId, "Admin"))
            {
                model = projectHelper.ListUsesOnMyProjects(userId);
            }
            return View(model);
        }
        public ActionResult ViewAllUsers()
        {
            var model = db.Users.ToList();
            return View(model);
        }
        public ActionResult ManageUser(string userId, int? addRemove, int? projectId)
        {
            if (addRemove != null && projectId != null)
            {
                if (addRemove == 0)
                {
                    projectHelper.AddUserToProject(userId, (int)projectId);
                }
                if (addRemove == 1)
                {
                    projectHelper.RemoveUserFromProject(userId, (int)projectId);
                }
            }

            var user = userHelper.getUser(userId);
            var model = new ManageUserVM();
            model.UserId = user.Id;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;
            model.PhoneNumber = user.PhoneNumber;
            model.FullName = user.FullName;
            model.AvatarPath = user.AvatarPath;
            model.userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            //model.FirstName = projectHelper.ListUserProjects(userId).ToList();
            //model.NotUserProjects = projectHelper.ListUserNotOnProjects(userId).ToList();
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name", model.userRole);
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUser(ManageUserVM userVM, string roleName)
        {
            var user = db.Users.Find(userVM.UserId);
            user.UserName = userVM.Email;
            user.FirstName = userVM.FirstName;
            user.LastName = userVM.LastName;
            user.PhoneNumber = userVM.PhoneNumber;
            user.AvatarPath = userVM.AvatarPath;
            //userVM.user.AvatarPath = userHelper.getUser(userVM.user.Id).AvatarPath;

            if (FileUploadValidator.IsWebFriendlyImage(userVM.Avatar))
            {
                var fileName = FileStamp.MakeUnique(userVM.Avatar.FileName);
                var serverFolder = WebConfigurationManager.AppSettings["DefaultAvatarFolder"];
                userVM.Avatar.SaveAs(Path.Combine(Server.MapPath(serverFolder), fileName));
                user.AvatarPath = $"{serverFolder}{fileName}";
            }

            //db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            if (roleName != null)
            {
                foreach (var role in roleHelper.ListUserRoles(user.Id))
                {
                    roleHelper.RemoveUserFromRole(user.Id, role);
                }
                if (!string.IsNullOrEmpty(roleName))
                {
                    roleHelper.AddUserToRole(user.Id, roleName);
                }
            }



            return RedirectToAction("Index");
        }
    }
}

