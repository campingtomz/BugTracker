﻿using System;
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
            var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name", userRole);
            var model = new ManageUserVM();
            model.user = db.Users.Find(userId);
            model.UserProjects = projectHelper.ListUserProjects(userId).ToList();
            model.NotUserProjects = projectHelper.ListUserNotOnProjects(userId).ToList();


            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUser(ManageUserVM userVM, string roleName)
        {

            userVM.user.UserName = userVM.user.Email;
            var avatarpath = userVM.user.AvatarPath;
            //userVM.user.AvatarPath = userHelper.getUser(userVM.user.Id).AvatarPath;

            if (FileUploadValidator.IsWebFriendlyImage(userVM.Avatar))
            {
                var fileName = FileStamp.MakeUnique(userVM.Avatar.FileName);
                var serverFolder = WebConfigurationManager.AppSettings["DefaultAvatarFolder"];
                userVM.Avatar.SaveAs(Path.Combine(Server.MapPath(serverFolder), fileName));
                userVM.user.AvatarPath = $"{serverFolder}{fileName}";
            }

            db.Entry(userVM.user).State = EntityState.Modified;
            db.SaveChanges();

            if (roleName != null) { 
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
    }
}

