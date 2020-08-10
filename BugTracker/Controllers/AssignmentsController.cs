﻿using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class AssignmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper roleHelper = new UserRoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        // GET: Assignments
        #region Roles Assigment
        public ActionResult ManageRoles()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name");

            return View();
        }
        public ActionResult ManageUserRoles()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string roleName)
        {
            if (userIds == null)
            {
                return RedirectToAction("RolesIndex");
            }
            foreach (var userId in userIds)
            {
                foreach (var roleList in roleHelper.ListUserRoles(userId))
                {
                    roleHelper.RemoveUserFromRole(userId, roleList);
                }
                if (!string.IsNullOrEmpty(roleName))
                {
                    roleHelper.AddUserToRole(userId, roleName);
                }
            }
            return RedirectToAction("ManageRoles");
        }
        #endregion

        #region Project Assignment

        public ActionResult ManageProjectUsers()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");
            ViewBag.ProjectIds = new MultiSelectList(db.Projects, "Id", "Name");

            return View(db.Users.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectUsers(List<string> userIds, List<int> projectIds) 
        {
        
            if (userIds == null || projectIds == null)
            {
                return RedirectToAction("ManageProjectUsers");
            }
          //iterate over each user and add to each project
          foreach(var userId in userIds)
            {
                foreach(var projectId in projectIds)
                {
                    projectHelper.AddUserToProject(userId, projectId);
                }
            }
            return RedirectToAction("ManageProjectUsers");
        }

        #endregion
    }
}