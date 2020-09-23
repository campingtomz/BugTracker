using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
namespace BugTracker.Helpers
{
    public class SeedHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        TicketHelper ticketHelper = new TicketHelper();
        ProjectHelper projectHelper = new ProjectHelper();
        #region project History
        public void ProjectHistoriesEdit(Project oldProject, Project newProject)
        {
            ProjectUsersChanged(oldProject, newProject);
            ProjectDescriptionChange(oldProject, newProject);
            ProjectNameChange(oldProject, newProject);
        }
        private void ProjectUsersChanged(Project oldProject, Project newProject)
        {
            var userId = projectHelper.ListUserOnProjectInRole(newProject.Id, "ProjectManager").FirstOrDefault().Id;
            var addedUsers = newProject.Users.Except(oldProject.Users).ToList();
            var removedUsers = oldProject.Users.Except(newProject.Users).ToList();
            foreach (var user in addedUsers)
            {
                var NewHistory = new ProjectHistory()
                {
                    ProjectId = newProject.Id,
                    UserId = userId,
                    ChangedOn = DateTime.Now,
                    Property = $"User added to: {newProject.Name}",
                    OldValue = "",
                    NewValue = user.Email
                };
                db.ProjectHistories.Add(NewHistory);
            }
            foreach (var user in removedUsers)
            {
                var NewHistory = new ProjectHistory()
                {
                    ProjectId = newProject.Id,
                    UserId = userId,
                    ChangedOn = DateTime.Now,
                    Property = $"User removed from: {newProject.Name}",
                    OldValue = "",
                    NewValue = user.Email
                };
                db.ProjectHistories.Add(NewHistory);
            }

        }
        private void ProjectDescriptionChange(Project oldProject, Project newProject)
        {
            var userId = projectHelper.ListUserOnProjectInRole(newProject.Id, "ProjectManager").FirstOrDefault().Id;

            var NewHistory = new ProjectHistory()
            {
                ProjectId = newProject.Id,
                UserId = userId,
                ChangedOn = DateTime.Now,
                Property = "Description",
                OldValue = oldProject.Description,
                NewValue = newProject.Description
            };
            db.ProjectHistories.Add(NewHistory);
        }
        private void ProjectNameChange(Project oldProject, Project newProject)
        {
            var userId = projectHelper.ListUserOnProjectInRole(newProject.Id, "ProjectManager").FirstOrDefault().Id;

            var NewHistory = new ProjectHistory()
            {
                ProjectId = newProject.Id,
                UserId = userId,
                ChangedOn = DateTime.Now,
                Property = "Description",
                OldValue = oldProject.Name,
                NewValue = newProject.Name
            };
            db.ProjectHistories.Add(NewHistory);
        }
        #endregion

        #region Ticket History
        public void TicketHistoryEdit(Ticket oldTicket, Ticket newTicket)
        {
            DeveloperUpdate(oldTicket, newTicket);
            TicketTypeIdChange(oldTicket, newTicket);
            TicketPriorityIdChange(oldTicket, newTicket);
            TicketStatusIdChange(oldTicket, newTicket);
            TickeTIssueChange(oldTicket, newTicket);
            TicketTicketIssueDescriptionChange(oldTicket, newTicket);
            
        }
        private void CreateHistory(Ticket newTicket, string oldValue, string newValue, string property)
        {
            var user = newTicket.project.Users.FirstOrDefault();
            var history = new TicketHistory()
            {

                TicketId = newTicket.Id,
                User = user,
                ChangedOn = DateTime.Now,
                Property = property,
                OldValue = oldValue,
                NewValue = newValue
            };
            db.TicketHistories.Add(history);
        }
        private void DeveloperUpdate(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.DeveloperId != newTicket.DeveloperId)
            {
                CreateHistory(newTicket, oldTicket.Developer.FullName, newTicket.Developer.FullName, "Developer");
            }
        }
        private void TicketTypeIdChange(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                CreateHistory(newTicket, oldTicket.TicketType.Name, newTicket.TicketType.Name, "Ticket Type");
            }
        }
        private void TicketPriorityIdChange(Ticket oldTicket, Ticket newTicket)
        {

            CreateHistory(newTicket, oldTicket.TicketPriority.Name, newTicket.TicketPriority.Name, "Ticket Priority");
        }
        private void TicketStatusIdChange(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                CreateHistory(newTicket, oldTicket.TicketStatus.Name, newTicket.TicketStatus.Name, "Ticket Status");
            }
        }
        private void TickeTIssueChange(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.Issue != newTicket.Issue)
            {
                CreateHistory(newTicket, oldTicket.Issue, newTicket.Issue, "Issue");
            }
        }
        private void TicketTicketIssueDescriptionChange(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.IssueDescription != newTicket.IssueDescription)
            {
                CreateHistory(newTicket, oldTicket.IssueDescription, newTicket.IssueDescription, "Issue Description");
            }
        }
        #endregion


        public void NewProjectCreated(Project project)
        {
            var userId = projectHelper.ListUserOnProjectInRole(project.Id, "ProjectManager").FirstOrDefault().Id;
            foreach (var user in project.Users.Where(u => u.Id != userId))
            {
                var newNotification = new ProjectNotification()
                {
                    ProjectId = project.Id,
                    UserId = user.Id,
                    Created = DateTime.Now,
                    Icon = "fa-sitemap",
                    NotificationType = "success",
                    Subject = $"Added to Project Id: {project.Id}",
                    Message = $"Hello, {user.FullName} you have been Added to the project: {project.Name}",
                   


                };
                db.Notifications.Add(newNotification);

            }

        }
        public void TicketNewCommentAdded(TicketComment newComment)
        {
            var CurrUser = newComment.ticket.project.Users.FirstOrDefault();
            foreach (var user in ticketHelper.ListTicketUsers(newComment.ticket.Id))
            {
                var newNotification = new TicketNotification()
                {
                    TicketId = newComment.TicketId,
                    UserId = user.Id,
                    Created = DateTime.Now,
                    Icon = "fa-ticket",
                    NotificationType = "success",
                    Subject = $"New Comment Added to  {newComment.TicketId}",
                    Message = $"Hello {user.FullName}, A new Comment has been added to the Ticket: {newComment.TicketId}, by {CurrUser.FullName}",
                };
                db.Notifications.Add(newNotification);
            }


        }
        public void TicketNewAttachmentAdded(TicketAttachment newAttachment)
        {
            var CurrUser = newAttachment.ticket.project.Users.FirstOrDefault();

            foreach (var user in ticketHelper.ListTicketUsers(newAttachment.ticket.Id))
            {
                var newNotification = new TicketNotification()
                {
                    TicketId = newAttachment.TicketId,
                    UserId = user.Id,
                    Created = DateTime.Now,
                    Icon = "fa-ticket",
                    NotificationType = "success",
                    Subject = $"New Comment Added to  {newAttachment.TicketId}",
                    Message = $"Hello {user.FullName}, A new Comment has been added to the Ticket: {newAttachment.TicketId}, by {CurrUser.FullName}",
                };
                db.Notifications.Add(newNotification);
            }

        }
        public void ProjectChangedNotification(Project oldProject, Project newProject, List<ApplicationUser> OldUserList)
        {
            ProjectUserChange(oldProject, OldUserList);
            db.SaveChanges();
        }
        public void ProjectUserChange(Project project, List<ApplicationUser> OldUserList)
        {
            var addedUsers = project.Users.Except(OldUserList);
            var removedUsers = OldUserList.Except(project.Users);
            foreach (var user in addedUsers)
            {
                var newNotification = new ProjectNotification()
                {
                    ProjectId = project.Id,
                    UserId = user.Id,
                    Created = DateTime.Now,
                    Icon = "fa-sitemap",
                    NotificationType = "success",
                    Subject = $"Added to Project: {project.Name}",
                    Message = $"Hello, {user.FullName} you have been Added to the project: {project.Name}",
                };
                db.Notifications.Add(newNotification);
            }
            foreach (var user in removedUsers)
            {
                var newNotification = new ProjectNotification()
                {
                    ProjectId = project.Id,
                    UserId = user.Id,
                    Created = DateTime.Now,
                    Icon = "fa-sitemap",
                    NotificationType = "warning",
                    Subject = $"Removed from Project: {project.Name}",
                    Message = $"Hello, {user.FullName} you have been removed from the project: {project.Name}",
                };
                db.Notifications.Add(newNotification);
            }



        }



        #region Ticket Notification methods
        public void TicketChangeNotification(Ticket oldTicket, Ticket newTicket)
        {
            TicketDeveloperChange(oldTicket, newTicket);
            TicketClosedNotification(newTicket);
           

        }
        public void TicketClosedNotification(Ticket ticket)
        {
            if (ticket.IsResolved == true)
            {
                foreach (var user in ticketHelper.ListTicketUsers(ticket.Id))
                {
                    var newNotification = new TicketNotification()
                    {
                        TicketId = ticket.Id,
                        UserId = user.Id,
                        Created = DateTime.Now,
                        Icon = "fa-ticket",
                        Subject = $"Ticket Id: {ticket.Id} Has been Closed",
                        NotificationType = "info",

                        Message = $"Hello, {user.FullName} you have been assigned to Ticket: {ticket.Issue} on Project {ticket.project.Name}",
                    };
                    db.Notifications.Add(newNotification);
                }
             
            }
        }
        private void TicketDeveloperChange(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.DeveloperId != newTicket.DeveloperId && newTicket.DeveloperId != null)
            {
                var newNotification = new TicketNotification()
                {
                    TicketId = newTicket.Id,
                    UserId = newTicket.DeveloperId,
                    Created = DateTime.Now,
                    Icon = "fa-ticket",
                    NotificationType = "success",
                    Subject = $"New Assignment to Ticket Id: {newTicket.Id}",
                    Message = $"Hello, {newTicket.Developer.FullName} you have been assigned to Ticket: {newTicket.Issue} on Project {newTicket.project.Name}",
                };
                db.Notifications.Add(newNotification);
            }

            if (oldTicket.DeveloperId != null)
            {
                var newNotification = new TicketNotification()
                {
                    TicketId = newTicket.Id,
                    UserId = oldTicket.DeveloperId,
                    Created = DateTime.Now,
                    Icon = "fa-ticket",
                    NotificationType = "warning",
                    Subject = $"Removed from Id: {newTicket.Id}",
                    Message = $"Hello, {oldTicket.Developer.FullName} you have been Removed from Ticket: {newTicket.Issue} on Project {newTicket.project.Name}",
                };
                db.Notifications.Add(newNotification);
            }



        }

        public void NewTicketNotification(Ticket newTicket)
        {
            var projectManager = projectHelper.ListUserOnProjectInRole(newTicket.ProjectId, "ProjectManager").FirstOrDefault();
            var newNotification = new TicketNotification()
            {
                TicketId = newTicket.Id,
                UserId = projectManager.Id,
                Created = DateTime.Now,
                Icon = "fa-ticket",
                NotificationType = "success",
                Subject = $"New Ticket has been Created Id: {newTicket.Id}",
                Message = $"Hello {projectManager.FullName}, A New Ticket new Ticket has been Created {newTicket.Issue} on Project {newTicket.project.Name}"
            };
            db.Notifications.Add(newNotification);
           
        }
        #endregion

    }
}