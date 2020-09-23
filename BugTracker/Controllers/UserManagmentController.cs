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
        private HistoryHelper historyHelper = new HistoryHelper();
        // GET: UserManagment 
        public ActionResult Index()
        {
            var user = userHelper.getUser(User.Identity.GetUserId());
            var model = new List<ApplicationUser>();
            if (User.IsInRole("Admin"))
            { 
                model = db.Users.ToList();
            }
            else
            {
                foreach(var project in user.Projects)
                {
                    model.AddRange(project.Users);
                }
                //model = user.Projects;
               //model = projectHelper.ListUsesOnMyProjects(userId);

            }
            return View(model);
        }
        public ActionResult ManageRoleUsers()
        {
            ViewBag.Submitters = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "FullName");
            //ViewBag.NotSubmitters = new MultiSelectList(roleHelper.UsersNotInRole("Submitter"), "Id", "FullName");
            ViewBag.Developers = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "FullName");
            //ViewBag.NotDevelopers = new MultiSelectList(roleHelper.UsersNotInRole("Developer"), "Id", "FullName");
            ViewBag.ProjectManagers = new MultiSelectList(roleHelper.UsersInRole("ProjectManager"), "Id", "FullName");
            //ViewBag.NotProjectManagers = new MultiSelectList(roleHelper.UsersNotInRole("ProjectManager"), "Id", "FullName");
            ViewBag.DefaultRole = new MultiSelectList(roleHelper.UsersInRole("Default"), "Id", "FullName");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult ManageRoleUsers(List<string> Submitters, List<string> Developers, List<string> ProjectManagers, List<string> DefaultRole)
        {
            roleHelper.ManageUserRoles(Submitters, "Submitter");
            roleHelper.ManageUserRoles(Developers, "Developer");
            roleHelper.ManageUserRoles(ProjectManagers, "ProjectManager");
            roleHelper.ManageUserRoles(DefaultRole, "Default");

            return RedirectToAction("Index");
        }
        public ActionResult ViewAllUsers()
        {
            var model = db.Users.OrderByDescending(u=>u.FirstName). ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult ManageUser(string userId)
        {


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
            ViewBag.UserProjects = new MultiSelectList(projectHelper.ListUserProjects(userId), "Id", "Name");
            ViewBag.ListOfProjects = new MultiSelectList(projectHelper.ListUserNotOnProjects(userId), "Id", "Name");

            //model.FirstName = projectHelper.ListUserProjects(userId).ToList();
            //model.NotUserProjects = projectHelper.ListUserNotOnProjects(userId).ToList();
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name", model.userRole);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUser(ManageUserVM userVM, string roleName, string projects)
        {
            List<Project> oldProjects = projectHelper.ListUserProjects(userVM.UserId).ToList();
            var oldUser = db.Users.AsNoTracking().FirstOrDefault(u => u.Id == userVM.UserId);

            var user = db.Users.Find(userVM.UserId);

            user.Email = userVM.Email;
            user.UserName = userVM.Email;
            user.FirstName = userVM.FirstName;
            user.LastName = userVM.LastName;
            user.PhoneNumber = userVM.PhoneNumber;
            user.AvatarPath = userVM.AvatarPath;

            if (FileUploadValidator.IsWebFriendlyImage(userVM.Avatar))
            {
                var fileName = FileStamp.MakeUnique(userVM.Avatar.FileName);
                var serverFolder = WebConfigurationManager.AppSettings["DefaultAvatarFolder"];
                userVM.Avatar.SaveAs(Path.Combine(Server.MapPath(serverFolder), fileName));
                user.AvatarPath = $"{serverFolder}{fileName}";
            }

            db.SaveChanges();
            projectHelper.updateUserProjects(userVM.UserId, userVM.ProjectIds);



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
            var newUser = db.Users.AsNoTracking().FirstOrDefault(u => u.Id == userVM.UserId);
            historyHelper.CheckUserEdits(oldUser, newUser, oldProjects);
            return RedirectToAction("Index");
        }

        public ActionResult UserProfile(string userId)
        {
            return View(db.Users.Find(userId));
        }
    }
}

