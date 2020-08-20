using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
namespace BugTracker.Helpers
{
    public class HistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        #region Ticket History
        public void TicketHistoryEdit(Ticket oldTicket, Ticket newTicket)
        {
            DeveloperUpdate(oldTicket, newTicket);
            TicketTypeIdChange(oldTicket, newTicket);
            TicketPriorityIdChange(oldTicket, newTicket);
            TicketStatusIdChange(oldTicket, newTicket);
            TickeTIssueChange(oldTicket, newTicket);
            TicketTicketIssueDescriptionChange(oldTicket, newTicket);
            db.SaveChanges();
        }
        private void CreateHistory(Ticket newTicket, string oldValue, string newValue, string property)
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            var history = new TicketHistory()
            {

                TicketId = newTicket.Id,
                User = user,
                UserId = user.Id,
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
        #region Ticket Attachment Methods
        private void TicketAttachmentDelete(TicketAttachment oldAttachment)
        {
            var history = new AttachmentHistory()
            {
                TicketId = oldAttachment.TicketId,
                UserId = HttpContext.Current.User.Identity.GetUserId(),
                ChangedOn = DateTime.Now,
                Message = "Attachment Deleted",
                FilePath = oldAttachment.FilePath,
                FileName = oldAttachment.FileName
            };
            db.AttachmentHistories.Add(history);
            db.SaveChanges();
        }
        #endregion

        #region Ticket Commnet Methods
        #endregion
        #region project History
        public void ProjectHistoriesEdit(Project oldProject, Project newProject)
        {
            ProjectUsersChanged(oldProject, newProject);
            ProjectDescriptionChange(oldProject, newProject);
            ProjectNameChange(oldProject, newProject);
            db.SaveChanges();
        }
        private void ProjectUsersChanged(Project oldProject, Project newProject)
        {
            var addedUsers = newProject.Users.Except(oldProject.Users);
            var removedUsers = oldProject.Users.Except(newProject.Users);
            foreach (var user in addedUsers)
            {
                var NewHistory = new ProjectHistory()
                {
                    ProjectId = newProject.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    ChangedOn = DateTime.Now,
                    Property = $"User added to: {newProject.Name}",
                    OldValue = "",
                    NewValue = $"{user.Email}"
                };
                db.ProjectHistories.Add(NewHistory);
            }
            foreach (var user in removedUsers)
            {
                var NewHistory = new ProjectHistory()
                {
                    ProjectId = newProject.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    ChangedOn = DateTime.Now,
                    Property = $"User removed to: {newProject.Name}",
                    OldValue = "",
                    NewValue = $"{user.Email}"
                };
                db.ProjectHistories.Add(NewHistory);
            }

        }
        private void ProjectDescriptionChange(Project oldProject, Project newProject)
        {
            var NewHistory = new ProjectHistory()
            {
                ProjectId = newProject.Id,
                UserId = HttpContext.Current.User.Identity.GetUserId(),
                ChangedOn = DateTime.Now,
                Property = "Description",
                OldValue = oldProject.Description,
                NewValue = newProject.Description
            };
            db.ProjectHistories.Add(NewHistory);
        }
        private void ProjectNameChange(Project oldProject, Project newProject)
        {
            var NewHistory = new ProjectHistory()
            {
                ProjectId = newProject.Id,
                UserId = HttpContext.Current.User.Identity.GetUserId(),
                ChangedOn = DateTime.Now,
                Property = "Description",
                OldValue = oldProject.Name,
                NewValue = newProject.Name
            };
            db.ProjectHistories.Add(NewHistory);
        }

        #endregion
        #region user History
        #endregion
    }
}