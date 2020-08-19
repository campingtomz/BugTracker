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
            TicketTicketStatusIdChange(oldTicket, newTicket);
            TickeTIssueChange(oldTicket, newTicket);
            TicketTicketIssueDiscriptionChange(oldTicket, newTicket);
            db.SaveChanges();
        }
        private void DeveloperUpdate(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.DeveloperId != newTicket.DeveloperId)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    ChangedOn = DateTime.Now,
                    Property = "DeveloperId",
                    OldValue = oldTicket.DeveloperId,
                    NewValue = newTicket.DeveloperId
                };
                db.TicketHistories.Add(history);
            }
        }
        private void TicketTypeIdChange(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    ChangedOn = DateTime.Now,
                    Property = "Ticket Type",
                    OldValue = oldTicket.TicketType.Name,
                    NewValue = newTicket.TicketType.Name
                };
                db.TicketHistories.Add(history);
            }
        }
        private void TicketPriorityIdChange(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    ChangedOn = DateTime.Now,
                    Property = "Ticket Priority",
                    OldValue = oldTicket.TicketPriority.Name,
                    NewValue = newTicket.TicketPriority.Name
                };
                db.TicketHistories.Add(history);
            }
        }
        private void TicketTicketStatusIdChange(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    ChangedOn = DateTime.Now,
                    Property = "Ticket Status",
                    OldValue = oldTicket.TicketStatus.Name,
                    NewValue = newTicket.TicketStatus.Name
                };
                db.TicketHistories.Add(history);
            }
        }
        private void TickeTIssueChange(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.Issue != newTicket.Issue)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    ChangedOn = DateTime.Now,
                    Property = "Issue",
                    OldValue = oldTicket.Issue,
                    NewValue = newTicket.Issue
                };
                db.TicketHistories.Add(history);
            }
        }
        private void TicketTicketIssueDiscriptionChange(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.IssueDiscription != newTicket.IssueDiscription)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    ChangedOn = DateTime.Now,
                    Property = "TicketTypeId",
                    OldValue = oldTicket.IssueDiscription,
                    NewValue = newTicket.IssueDiscription
                };
                db.TicketHistories.Add(history);
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
        }
        #endregion

        #region Ticket Commnet Methods
        #endregion
        #region project History
        //public void ProjectEdit(Project oldProject, Project newProject)
        //{

        //}
        #endregion
        #region user History
        #endregion
    }
}