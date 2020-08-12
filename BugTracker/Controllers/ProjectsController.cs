using System;
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
        // GET: Projects
        public ActionResult Index()
        {
            var projectList = new List<ProjectManageVM>();
            foreach (var project in db.Projects.ToList())
            {
                var projectVm = new ProjectManageVM(project);
                projectList.Add(projectVm);
            }


            var model = projectList;
            return View(model);

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
                project.Created = DateTime.Now;
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
        public ActionResult Edit(int? id, string userId, int? AddRemoveUser)
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
            if (AddRemoveUser != null && !String.IsNullOrWhiteSpace(userId))
            {
                if (AddRemoveUser == 0)
                {
                    projectHelper.AddUserToProject(userId, model.Id);
                }
                else if (AddRemoveUser == 1)
                {
                    projectHelper.RemoveUserFromProject(userId, model.Id);
                }
            }
            ViewBag.UsersNotInProject = new List<ApplicationUser>(projectHelper.ListUsersNotOnProject(model.Id)); 
             ViewBag.ProjectManagers = new SelectList(roleHelper.UsersInRole("ProjectManager"), "Id", "FullName", projectHelper.ListUserOnProjectInRole(model.Id, "ProjectManager").FirstOrDefault().Id);
            ViewBag.Submitters = new List<ApplicationUser>(projectHelper.ListUserOnProjectInRole(model.Id, "Submitter"));
            ViewBag.Developers = new List<ApplicationUser>(projectHelper.ListUserOnProjectInRole(model.Id, "Developer"));
            return View(model);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project project, string ProjectManagers)
        {

            //projectVM.projectValue.IsArchive = false;
            db.Entry(project).State = EntityState.Modified;
            db.SaveChanges();
            projectHelper.RemoveUserFromProject(projectHelper.ListUserOnProjectInRole(project.Id, "ProjectManager").FirstOrDefault().Id, project.Id);
            projectHelper.AddUserToProject(ProjectManagers, project.Id);

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
