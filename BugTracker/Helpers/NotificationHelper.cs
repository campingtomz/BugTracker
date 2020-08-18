using System;
using System.Collections.Generic;
using System.Linq;
using BugTracker.Models;
using System.Web;
using Microsoft.AspNet.Identity;

namespace BugTracker.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper userRoleHelper = new UserRoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        private UserHelper userHelper = new UserHelper();
        public void ManageTicketNotifications(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.DeveloperId != newTicket.DeveloperId && newTicket.DeveloperId != null)
            {
                var newNotification = new TicketNotification()
                {
                    TicketId = newTicket.Id,
                    UserId = newTicket.DeveloperId,
                    Created = DateTime.Now,
                    Subject = $"New Assignment to Ticket Id: {newTicket.Id}",
                    Message = $"Hello, {newTicket.Developer.FullName} you have been assigned to Ticket: {newTicket.Issue} on Project {newTicket.project.Name}"
                };
                db.TicketNotifications.Add(newNotification);
            }

            if (oldTicket.DeveloperId != null)
            {
                var newNotification = new TicketNotification()
                {
                    TicketId = newTicket.Id,
                    UserId = oldTicket.DeveloperId,
                    Created = DateTime.Now,
                    Subject = $"Removed from Id: {newTicket.Id}",
                    Message = $"Hello, {oldTicket.Developer.FullName} you have been Removed from Ticket: {newTicket.Issue} on Project {newTicket.project.Name}"
                };
                db.TicketNotifications.Add(newNotification);
            }

            db.SaveChanges();

        }
        public void NewTicketNotification(Ticket newTicket)
        {
            var projectManager = projectHelper.ListUserOnProjectInRole(newTicket.ProjectId, "ProjectManager").FirstOrDefault();
            var newNotification = new TicketNotification()
            {
                TicketId = newTicket.Id,
                UserId = projectManager.Id,
                Created = DateTime.Now,
                Subject = $"New Ticket has been Created Id: {newTicket.Id}",
                Message = $"Hello {projectManager.FullName}, A New Ticket new Ticket has been Created {newTicket.Issue} on Project {newTicket.project.Name}"
            };
            db.TicketNotifications.Add(newNotification);
            db.SaveChanges();
        }
        public void NotificationRead(int notificationId)
        {
            var notification = GetNotification(notificationId);
            notification.IsRead = true;
            db.SaveChanges();
        }
        public TicketNotification GetNotification(int notificationId)
        {
            return db.TicketNotifications.Find(notificationId);
        }

        public void NewProjectCreated(Project project)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            foreach (var user in project.Users.Where(u => u.Id != userId))
            {
                var newNotification = new ProjectNotification()
                {
                    ProjectId = project.Id,
                    UserId = user.Id,
                    Created = DateTime.Now,
                    Subject = $"Added to Project Id: {project.Id}",
                    Message = $"Hello, {user.FullName} you have been Added to the project: {project.Name}"
                };
                db.ProjectNotifications.Add(newNotification);

            }
            db.SaveChanges();
        }
        public void ProjectUserChange(Project oldProject, Project newProject)
        {
            var addedUsers = newProject.Users.Except(oldProject.Users);
            var removedUsers = oldProject.Users.Except(newProject.Users);
            foreach (var user in addedUsers)
            {
                var newNotification = new ProjectNotification()
                {
                    ProjectId = newProject.Id,
                    UserId = user.Id,
                    Created = DateTime.Now,
                    Subject = $"Added to Project: {newProject.Name}",
                    Message = $"Hello, {user.FullName} you have been Added to the project: {newProject.Name}"
                };
                db.ProjectNotifications.Add(newNotification);
            }
            foreach (var user in removedUsers)
            {
                var newNotification = new ProjectNotification()
                {
                    ProjectId = newProject.Id,
                    UserId = user.Id,
                    Created = DateTime.Now,
                    Subject = $"Removed from Project: {newProject.Name}",
                    Message = $"Hello, {user.FullName} you have been removed from the project: {newProject.Name}"
                };
                db.ProjectNotifications.Add(newNotification);
            }

            db.SaveChanges();

        }
    }
}