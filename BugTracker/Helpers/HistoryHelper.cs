using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using BugTracker.Helpers;
namespace BugTracker.Helpers
{

    public class HistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper roleHelper = new UserRoleHelper();
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
        private void CreateTicketHistory(Ticket newTicket, string oldValue, string newValue, string property)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var history = new TicketHistory()
            {

                TicketId = newTicket.Id,
                UserId = userId,
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
                CreateTicketHistory(newTicket, oldTicket.Developer.FullName, newTicket.Developer.FullName, "Developer");
            }
        }
        private void TicketTypeIdChange(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                CreateTicketHistory(newTicket, oldTicket.TicketType.Name, newTicket.TicketType.Name, "Ticket Type");
            }
        }
        private void TicketPriorityIdChange(Ticket oldTicket, Ticket newTicket)
        {

            CreateTicketHistory(newTicket, oldTicket.TicketPriority.Name, newTicket.TicketPriority.Name, "Ticket Priority");
        }
        private void TicketStatusIdChange(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                CreateTicketHistory(newTicket, oldTicket.TicketStatus.Name, newTicket.TicketStatus.Name, "Ticket Status");
            }
        }
        private void TickeTIssueChange(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.Issue != newTicket.Issue)
            {
                CreateTicketHistory(newTicket, oldTicket.Issue, newTicket.Issue, "Issue");
            }
        }
        private void TicketTicketIssueDescriptionChange(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.IssueDescription != newTicket.IssueDescription)
            {
                CreateTicketHistory(newTicket, oldTicket.IssueDescription, newTicket.IssueDescription, "Issue Description");
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
            var addedUsers = newProject.Users.Except(oldProject.Users, new CustomComparer.UserComparer());

            var removedUsers = oldProject.Users.Except(newProject.Users, new CustomComparer.UserComparer());
            foreach (var user in addedUsers)
            {
                var NewHistory = new ProjectHistory()
                {
                    ProjectId = newProject.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
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
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = $"User removed from: {newProject.Name}",
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
        public void CheckUserEdits(ApplicationUser oldUser, ApplicationUser newUser, List<Project> oldProjects)
        {
            FirstNameChange(oldUser, newUser);
            LastNameChange(oldUser, newUser);
            EmailChange(oldUser, newUser);
            PhoneNumberChange(oldUser, newUser);
            AvatarPathChange(oldUser, newUser);
            UserProjectChange(oldProjects, newUser);
            UserRoleChange(oldUser, newUser);
            db.SaveChanges();
        }
        private void CreateUserHistory(string userId, string oldValue, string newValue, string property)
        {
            var changedByUserId = HttpContext.Current.User.Identity.GetUserId();
            var history = new UserHistory()
            {
                UserId = userId,
                ChangedByUserId = changedByUserId,
                ChangedOn = DateTime.Now,
                Property = property,
                OldValue = oldValue,
                NewValue = newValue
            };
            db.UserHistories.Add(history);
           
        }
        private void FirstNameChange(ApplicationUser oldUser, ApplicationUser newUser)
        {
            if (oldUser.FirstName != newUser.FirstName)
            {
                CreateUserHistory(newUser.Id, oldUser.FirstName, newUser.FirstName, "FirstName");
            }
        }
        private void LastNameChange(ApplicationUser oldUser, ApplicationUser newUser)
        {
            if (oldUser.LastName != newUser.LastName)
            {
                CreateUserHistory(newUser.Id, oldUser.LastName, newUser.LastName, "LastName");
            }
        }
        private void EmailChange(ApplicationUser oldUser, ApplicationUser newUser)
        {
            if (oldUser.Email != newUser.Email)
            {
                CreateUserHistory(newUser.Id, oldUser.Email, newUser.Email, "Email");
            }
        }
        private void PhoneNumberChange(ApplicationUser oldUser, ApplicationUser newUser)
        {
            if (oldUser.PhoneNumber != newUser.PhoneNumber)
            {
                CreateUserHistory(newUser.Id, oldUser.PhoneNumber, newUser.PhoneNumber, "PhoneNumber");
            }
        }
        private void AvatarPathChange(ApplicationUser oldUser, ApplicationUser newUser)
        {
            if (oldUser.AvatarPath != newUser.AvatarPath)
            {
                CreateUserHistory(newUser.Id, oldUser.AvatarPath, newUser.AvatarPath, "AvatarPath");
            }
        }
  
        private void UserProjectChange(List<Project> oldProjects, ApplicationUser newUser)
        {
            var addedProjects = newUser.Projects.Except(oldProjects, new CustomComparer.ProjectComparer()).ToList();
            var removedProjects = oldProjects.Except(newUser.Projects, new CustomComparer.ProjectComparer()).ToList();
            var changedByUserId = HttpContext.Current.User.Identity.GetUserId();

            foreach (var project in addedProjects)
            {
                var NewHistory = new UserHistory()
                {
                    UserId = newUser.Id,
                    ChangedByUserId = changedByUserId,
                    ChangedOn = DateTime.Now,
                    Property = $"Project added to: {newUser.FullName}",
                    OldValue = "",
                    NewValue = $"{project.Id}"
                };
                db.UserHistories.Add(NewHistory);
            }
            foreach (var project in removedProjects)
            {
                var NewHistory = new UserHistory()
                {
                    UserId = newUser.Id,
                    ChangedByUserId = changedByUserId,
                    ChangedOn = DateTime.Now,
                    Property = $"Project removed from: {newUser.FullName}",
                    OldValue = "",
                    NewValue = $"{project.Id}"
                };
                db.UserHistories.Add(NewHistory);
            }

        }
        private void UserRoleChange(ApplicationUser oldUser, ApplicationUser newUser)
        {
            var oldRole = roleHelper.ListUserRoles(oldUser.Id).FirstOrDefault();
            var newRole = roleHelper.ListUserRoles(newUser.Id).FirstOrDefault();
            if (oldRole != newRole)
            {
                CreateUserHistory(newUser.Id, oldRole, newRole, "Role");
            }
        }

        #endregion

    }
}