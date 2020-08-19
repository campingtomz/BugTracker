using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class TicketHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper userRoleHelper = new UserRoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        private HistoryHelper historyHelper = new HistoryHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();

        #region ticket Permission methods
        public bool CanEditTicket(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                    return true;
                case "ProjectManager":
                    var user = db.Users.Find(userId);
                    return user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId);
                case "Developer":
                    var ticket = db.Tickets.Find(ticketId);
                    if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }
        public bool CanCreateTicket()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                case "Submitter":
                    return true;
               default:
                    return false;
            }
        }
        public bool CanEditTicketDev(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                    return true;
                case "ProjectManager":
                    var user = db.Users.Find(userId);
                    return user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId);
                default:
                    return false;
            }
        }
        public bool CanEditComment(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                    return true;
                case "ProjectManager":
                    var user = db.Users.Find(userId);
                    return user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId);
                case "Developer":
                    var ticket = db.Tickets.Find(ticketId);
                    if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }
        public bool CanMakeComment(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();
         
            switch (myRole)
            {
                case "Admin":
                    return true;
                case "ProjectManager":
                    var user = db.Users.Find(userId);
                    return user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId);
                case "Developer":
                    var ticket = db.Tickets.Find(ticketId);
                    if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }
        public bool CanAddAttachment(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                    return true;
                case "ProjectManager":
                    var user = db.Users.Find(userId);
                    return user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId);
                case "Developer":
                    var ticket = db.Tickets.Find(ticketId);
                    if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }
        #endregion
        //public bool CanRemoveAttachment() { }
        public List<Ticket> ListUserTicketsOnProject(string userId, int projectId){
            var project = db.Projects.Find(projectId);
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":             
                case "Project Manager":
                   return project.Tickets.ToList();
                case "Developer":
                    return project.Tickets.Where(t => t.SubmitterId == userId).ToList();
                case "Submitter":
                    return project.Tickets.Where(t => t.DeveloperId == userId).ToList();

                default:
                    return null;
            }
        }
        //gets all the tickets assigned to the  logged in user. 
        public List<Ticket> GetMyTickets()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case "Admin":
                    return db.Tickets.ToList();
                case "ProjectManager":
                    var ticketList = new List<Ticket>();
                    foreach(var project in projectHelper.ListUserProjects(userId).ToList())
                    {
                        ticketList.AddRange(project.Tickets.ToList());
                    }
                    return ticketList;
                case "Developer":
                    return db.Tickets.Where(t => t.DeveloperId == userId).ToList();
                case "Submitter":
                    return db.Tickets.Where(t => t.SubmitterId == userId).ToList();
                default:
                    return null;
            }

        }
        //lists all the tickets in every project the user is assinged  
        public List<Ticket> GetAllProjectTicketsForUser(string userId)
        {
            var user = db.Users.Find(userId);
            var ticketList = new List<Ticket>();
            return user.Projects.SelectMany(p => p.Tickets).ToList();
        }
        //lists all the tickets on a project
        public ICollection<Ticket> ListProjectTickets(int projectId)
        {
            var projects = db.Projects.Find(projectId);
            return projects.Tickets;
        }
        public List<Ticket> ListTicketByStatus(string status)
        {
            return db.Tickets.Where(t => t.TicketStatus.Name == status).ToList();
        }
        public List<TicketPriority> ListTicketProities()
        {
            List<TicketPriority> ticketPriorities = new List<TicketPriority>();
            foreach (var priority in db.TicketPriorities)
            {
                ticketPriorities.Add(priority);
            }
            return ticketPriorities;
        }
        public List<TicketStatus> ListTicketStatuses()
        {
            List<TicketStatus> ticketStatuses = new List<TicketStatus>();
            foreach (var status in db.TicketStatuses)
            {
                ticketStatuses.Add(status);
            }
            return ticketStatuses;
        }
        public List<TicketType> ListTicketTypes()
        {
            List<TicketType> ticketTypes = new List<TicketType>();
            foreach (var types in db.TicketTypes)
            {
                ticketTypes.Add(types);
            }
            return ticketTypes;
        }
        #region tickethistory and notifications
        public void TicketEdit(Ticket oldTicket, Ticket newTicket)
        {
            historyHelper.TicketHistoryEdit(oldTicket, newTicket);
            notificationHelper.TicketChangeNotification(oldTicket, newTicket);
        }
        #endregion
        //public void ManageTicketNotifications(Ticket oldTicket, Ticket newTicket)
        //{
        //    if(oldTicket.DeveloperId != newTicket.DeveloperId && newTicket.DeveloperId != null)
        //    {
        //        var newNotification = new TicketNotification()
        //        {
        //            TicketId = newTicket.Id,
        //            UserId = newTicket.DeveloperId,
        //            Created = DateTime.Now,
        //            Subject =$"New Assignment to Ticket Id: {newTicket.Id}",
        //            Message= $"Hello, {newTicket.Developer.FullName} you have been assigned to Ticket: {newTicket.Issue} on Project {newTicket.project.Name}"
        //        };
        //        db.TicketNotifications.Add(newNotification);
        //        db.SaveChanges();
        //    }
        //}
    }
} 