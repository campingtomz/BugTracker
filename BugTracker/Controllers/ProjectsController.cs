﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //private UserHelper userHelper = new UserHelper();
        private UserRoleHelper roleHelper = new UserRoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        private HistoryHelper historyHelper = new HistoryHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();
        // GET: Projects
        public ActionResult Index()
        {

            if (projectHelper.CanViewAllProjects())
            {
                ViewBag.AllProjects = new List<Project>(db.Projects.ToList().OrderByDescending(p => p.Created).ToList());
            }


            return View(projectHelper.ListUserProjects(User.Identity.GetUserId()));

        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.Projects.Find(id);
            if (model == null)
            {
                return HttpNotFound();

            }

            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View(model);
        }


        public ActionResult ProjectWizard()
        {
            ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("ProjectManager"), "Id", "FullName");
            ViewBag.DeveloperIds = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "FullName");
            ViewBag.SubmitterIds = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "FullName");
            ViewBag.Errors = "";
            var model = new ProjectWizardVM();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectWizard(ProjectWizardVM model)
        {
            #region fail cases
            ViewBag.Errors = "";
            if (model.ProjectManagerId == null)
            {
                ViewBag.Errors += "<p> You must select a Project Manager</p>";
            }
            if (model.DeveloperIds.Count < 1)
            {
                ViewBag.Errors += "<p> You must select at least one Developer </p>";
            }
            if (model.SubmitterIds.Count < 1)
            {
                ViewBag.Errors += "<p> You must select at least one Submmiter</p>";
            }
            if (ViewBag.Errors.Length > 0)
            {
                ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("ProjectManager"), "Id", "FullName");
                ViewBag.DeveloperIds = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "FullName");
                ViewBag.SubmitterIds = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "FullName");

                return View(model);
            }
            #endregion
            if (ModelState.IsValid)
            {
                Project project = new Project();
                project.Name = model.Name;
                project.Description = model.Description;
                project.Created = DateTime.Now;
                project.DueDate = DateTime.Now.AddDays(+100);
                project.IsArchive = false;
                db.Projects.Add(project);
                db.SaveChanges();
                projectHelper.AddUserToProject(model.ProjectManagerId, project.Id);
                foreach (var userId in model.DeveloperIds)
                {
                    projectHelper.AddUserToProject(userId, project.Id);
                }
                foreach (var userId in model.SubmitterIds)
                {
                    projectHelper.AddUserToProject(userId, project.Id);
                }

                notificationHelper.NewProjectCreated(project);
                return RedirectToAction("Index");
            }
            else
            {

                ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("ProjectManager"), "Id", "FullName");
                ViewBag.DeveloperIds = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "FullName");
                ViewBag.SubmitterIds = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "FullName");

                return View(model);
            }
        }
        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.Projects.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            var projectPM = projectHelper.ListUserOnProjectInRole(model.Id, "ProjectManager");
            if (projectPM.Count > 0)
            {
                ViewBag.ProjectManagers = new SelectList(roleHelper.UsersInRole("ProjectManager"), "Id", "FullName", projectPM.FirstOrDefault().Id);
            }
            else
            {
                ViewBag.ProjectManagers = new SelectList(roleHelper.UsersInRole("ProjectManager"), "Id", "FullName");
            }
            ViewBag.DevelopersNotOnProject = new MultiSelectList(projectHelper.ListUserNotOnProjectInRole(model.Id, "Developer"), "Id", "FullName");
            ViewBag.SubmittersNotOnProject = new MultiSelectList(projectHelper.ListUserNotOnProjectInRole(model.Id, "Submitter"), "Id", "FullName");

            ViewBag.Submitters = new MultiSelectList(projectHelper.ListUserOnProjectInRole(model.Id, "Submitter"), "Id", "FullName");
            ViewBag.Developers = new MultiSelectList(projectHelper.ListUserOnProjectInRole(model.Id, "Developer"), "Id", "FullName");
            return View(model);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project project, string ProjectManagers)
        {

            var oldProject = db.Projects.AsNoTracking().FirstOrDefault(p => p.Id == project.Id);
            var oldUserList = project.Users.ToList();
            //projectVM.projectValue.IsArchive = false;
            db.Entry(project).State = EntityState.Modified;
            //var userIdList = $"{ProjectManagers},{DeveloperId},{SubmitterId}";
            projectHelper.UpdateProjectUserIds(ProjectManagers, project.Id);
           
            db.SaveChanges();
            var newProject = db.Projects.AsNoTracking().FirstOrDefault(p => p.Id == project.Id);

            historyHelper.ProjectHistoriesEdit(oldProject, newProject);
            notificationHelper.ProjectChangedNotification(newProject, oldProject, oldUserList);
            return RedirectToAction("Index");
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
