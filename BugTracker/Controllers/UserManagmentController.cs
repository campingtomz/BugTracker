using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.ViewModels;
using PagedList;
using PagedList.Mvc;


namespace BugTracker.Controllers
{
    public class UserManagmentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper roleHelper = new UserRoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        // GET: UserManagment
        public ActionResult Index(int? page)
        {
           
            var users = db.Users.ToList();
            int pageSize = 10; //Specifies the number of posts per page
            int pageNumber = (page ?? 1); //?? null coalescing operator
            var model = users.ToPagedList(pageNumber, pageSize);
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
            var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name", userRole);
            var model = new ManageUserVM();
            model.user = db.Users.Find(userId);
            model.UserProjects = projectHelper.ListUserProjects(userId).ToList();
            model.NotUserProjects = projectHelper.ListUserNotOnProjects(userId).ToList();
           
          
            return View("~/Views/UserManagment/ManageUser.cshtml", model);
        }
        //public ActionResult AddUserToProject(string userId, int projectId)
        //{
        //    projectHelper.AddUserToProject(userId, projectId);
        //    var model = new ManageUserVM();
        //    model.user = db.Users.Find(userId);
        //    model.UserProjects = projectHelper.ListUserProjects(userId).ToList();
        //    model.NotUserProjects = projectHelper.ListUserNotOnProjects(userId).ToList();
        //    model.userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
        //    return View("~/Views/UserManagment/ManageUser.cshtml", model);
        //}
        //public ActionResult RemoveUserFromProject(string userId, int projectId)
        //{
        //    projectHelper.RemoveUserFromProject(userId, projectId);
        //    var model = new ManageUserVM();
        //    model.user = db.Users.Find(userId);
        //    model.UserProjects = projectHelper.ListUserProjects(userId).ToList();
        //    model.NotUserProjects = projectHelper.ListUserNotOnProjects(userId).ToList();
        //    model.userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
        //    return View("~/Views/UserManagment/ManageUser.cshtml", model);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUser(ManageUserVM userVM, string roleName)
        {
           
            userVM.user.UserName = userVM.user.Email;
            db.Entry(userVM.user).State = EntityState.Modified;
            db.SaveChanges();
            {
                foreach (var role in roleHelper.ListUserRoles(userVM.user.Id))
                {
                    roleHelper.RemoveUserFromRole(userVM.user.Id, role);
                }
                if (!string.IsNullOrEmpty(roleName))
                {
                    roleHelper.AddUserToRole(userVM.user.Id, roleName);
                }
            }

                return RedirectToAction("Index");
        }

        public ActionResult ManageUserRoles(string id)
        {
            var userRole = roleHelper.ListUserRoles(id).FirstOrDefault();
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name", userRole);
            return View(db.Users.Find(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserRoles(string id, string roleName)
        {
            foreach (var role in roleHelper.ListUserRoles(id))
            {
                roleHelper.RemoveUserFromRole(id, role);
            }
            if (!string.IsNullOrEmpty(roleName))
            {
                roleHelper.AddUserToRole(id, roleName);
            }
            return RedirectToAction("ManageUserRoles", new { id });
        }
        
    }
}
//Things i want to do here
//    add user
//    edit user
//    index= View all users
//    delete user
// paging 
// search
//    view User Profile
