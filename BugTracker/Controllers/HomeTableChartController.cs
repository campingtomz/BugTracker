using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.ViewModels;
using BugTracker.Helpers;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using System.Collections;

namespace BugTracker.Controllers
{
    public class HomeTableChartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketHelper ticketHelper = new TicketHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        private UserRoleHelper roleHelper = new UserRoleHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();
        // GET: HomeTableChart
        public JsonResult GetTicketPriorityData(List<int> projectIds)
        {
            var response = new JSChartData();
            var tickets = ticketHelper.ListProjectsTickets(projectIds);
            foreach (var priority in db.TicketPriorities.ToList())
            {
                response.label.Add(priority.Name);
                response.data.Add(tickets.Where(t => t.TicketPriorityId == priority.Id).Count());
            }
            return Json(response);
        }
        public JsonResult GetTicketStatusData(List<int> projectIds)
        {
            var response = new JSChartData();
            var tickets = ticketHelper.ListProjectsTickets(projectIds);
            foreach (var Status in db.TicketStatuses.ToList())
            {
                response.label.Add(Status.Name);
                response.data.Add(tickets.Where(t => t.TicketStatusId == Status.Id).Count());
            }
            return Json(response);
        }
        public JsonResult GetTicketTypeData(List<int> projectIds)
        {
            var response = new JSChartData();
            var tickets = ticketHelper.ListProjectsTickets(projectIds);
            foreach (var type in db.TicketTypes.ToList())
            {
                response.label.Add(type.Name);
                response.data.Add(tickets.Where(t => t.TicketTypeId == type.Id).Count());
            }
            return Json(response);
        }
        public JsonResult GetProjectTickets(List<int> projectIds)
        {

            var projectIdList = ticketHelper.ListProjectsTickets(projectIds);
            List<HomeTicketsTable> response = CreateHomeTicketsTable(projectIdList);
            return Json(response);
        }
        private List<HomeTicketsTable> CreateHomeTicketsTable(List<Ticket> Tickets)
        {
            List<HomeTicketsTable> TicketList = new List<HomeTicketsTable>();
            foreach (var ticket in Tickets)
            {
                HomeTicketsTable homeTicket = new HomeTicketsTable();
                homeTicket.Id = ticket.Id;
                homeTicket.Issue = ticket.Issue;
                homeTicket.ProjectId = ticket.ProjectId;
                homeTicket.ProjectName = ticket.project.Name;
                if (ticket.Submitter != null)
                {
                    homeTicket.SubmitterName = ticket.Submitter.FullName;
                }
                else
                {
                    homeTicket.SubmitterName = "No Submitter Assigned";
                }
                if (ticket.Developer != null)
                {
                    homeTicket.DeveloperName = ticket.Developer.FullName;
                }
                else
                {
                    homeTicket.DeveloperName = "No Submitter Assigned";
                }
                homeTicket.NumberOfComments = ticket.Comments.Count;
                TicketList.Add(homeTicket);
            }
            return TicketList;
        }
        public JsonResult GetTicketByPriority(string PropertyName, List<int> projectIds)
        {
            var projectIdList = ticketHelper.ListProjectsTickets(projectIds).Where(t => t.TicketPriority.Name == PropertyName).ToList();

            List<HomeTicketsTable> response = CreateHomeTicketsTable(projectIdList);

            return Json(response);
        }
        public JsonResult GetTicketByStatus(string PropertyName, List<int> projectIds)
        {
            var projectIdList = ticketHelper.ListProjectsTickets(projectIds).Where(t => t.TicketStatus.Name == PropertyName).ToList();

            List<HomeTicketsTable> response = CreateHomeTicketsTable(projectIdList);

            return Json(response);
        }
        public JsonResult GetTicketByType(string PropertyName, List<int> projectIds)
        {
            var projectIdList = ticketHelper.ListProjectsTickets(projectIds).Where(t => t.TicketType.Name == PropertyName).ToList();

            List<HomeTicketsTable> response = CreateHomeTicketsTable(projectIdList);

            return Json(response);
        }
        public JsonResult GetUsersByProjects(List<int> projectIds)

        {
            List<ApplicationUser> UserList = new List<ApplicationUser>();
            foreach (var projectId in projectIds)
            {
                UserList.AddRange(projectHelper.ListUsersOnProject(projectId).Except(UserList));
            }
            List<HomeUserTable> response = CreateHomeUserTable(UserList);
            return Json(response);
        }
        public List<HomeUserTable> CreateHomeUserTable(List<ApplicationUser> UserList)
        {
            List<HomeUserTable> listOfUsers = new List<HomeUserTable>();
            foreach (var user in UserList)
            {
                HomeUserTable homeUser = new HomeUserTable();
                homeUser.Id = user.Id;
                homeUser.FullName = user.FullName;
                homeUser.Email = user.Email;
                homeUser.PhoneNumber = user.PhoneNumber;
                homeUser.AvatarPath = user.AvatarPath;
                homeUser.TicketCount = ticketHelper.GetAllProjectTicketsForUser(user.Id).Count;
                homeUser.UserRole = roleHelper.ListUserRoles(user.Id).FirstOrDefault();
                listOfUsers.Add(homeUser);
            }
            return listOfUsers;
        }
        public JsonResult GetTicketInfo(int ticketId)
        {
            Ticket ticket = db.Tickets.Find(ticketId);
            TicketInfoHomeVM homeTicket = new TicketInfoHomeVM();
            homeTicket.Issue = ticket.Issue;
            homeTicket.IssueDescription = ticket.IssueDescription;
            homeTicket.SubmitterName = ticket.Submitter.FullName;
            homeTicket.DeveloperName = ticket.Developer.FullName;
            homeTicket.TicketPriority = ticket.TicketPriority.Name;
            homeTicket.TicketStatus = ticket.TicketStatus.Name;

            homeTicket.TicketType = ticket.TicketType.Name;
            homeTicket.Created = ticket.Created.ToString("MMM dd yyyy");


            return Json(homeTicket);
        }
        public JsonResult GetTicketComments(int ticketId)
        {
            Ticket ticket = db.Tickets.Find(ticketId);
            List<HomeTicketCommentsVM> listOfComments = new List<HomeTicketCommentsVM>();
            foreach (var comment in ticket.Comments)
            {
                HomeTicketCommentsVM newComment = new HomeTicketCommentsVM();
                newComment.TicketId = comment.Id;
                newComment.UserName = comment.User.FullName;
                newComment.AvatarPath = comment.User.AvatarPath;
                newComment.Comment = comment.Comment;
                newComment.Created = comment.Created.ToString("MMM dd yyyy");
                listOfComments.Add(newComment);
            }
            return Json(listOfComments);
        }
        public JsonResult createComment(string comment, int ticketId)
        {
            TicketComment ticketComment = new TicketComment();
            ticketComment.Created = DateTime.Now;
            ticketComment.TicketId = ticketId;
            ticketComment.Comment = comment;
            ticketComment.UserId = User.Identity.GetUserId();

            db.TicketComments.Add(ticketComment);
            db.SaveChanges();
            notificationHelper.TicketNewCommentAdded(ticketComment);

            return Json(true);
        }
        public JsonResult getTicketAttachments(int ticketId)
        {
            Ticket ticket = db.Tickets.Find(ticketId);
            List<HomeTicketAttachments> response = new List<HomeTicketAttachments>();
            foreach (var attachment in ticket.Attachments.ToList())
            {
                HomeTicketAttachments homeAttachments = new HomeTicketAttachments();
                homeAttachments.Name = attachment.FileName;
                homeAttachments.Uploader = attachment.User.FullName;
                homeAttachments.Description = attachment.Description;
                homeAttachments.FilePath = attachment.FilePath;
                homeAttachments.Created = attachment.Created.ToString("MMM dd yyyy");
                response.Add(homeAttachments);
            }

            return Json(response);
        }
        public JsonResult getUserNotificationS()
        {
            Dictionary<string, string> Type = new Dictionary<string, string> { { "new", "danger" }, { "added", "success" }, { "removed", "warning" }, { "newComment", "info" }, { "Closed", "info" } };
            List<HomeNotificationVM> response = new List<HomeNotificationVM>();
            var userId = User.Identity.GetUserId();
            foreach (var ticketNotfication in db.TicketNotifications.Where(tn => tn.UserId == userId))
            {

                if (!ticketNotfication.IsRead)
                {
                    var notType = ticketNotfication.NotificationType;
                    if (!Type.TryGetValue("XML_File", out notType))
                    {
                        notType = "info";
                    }
                    HomeNotificationVM newNotificationVM = new HomeNotificationVM();
                    newNotificationVM.notificationVersion = "ticket";

                    newNotificationVM.Id = ticketNotfication.Id;
                    newNotificationVM.Type = notType;
                    newNotificationVM.Subject = ticketNotfication.Subject;
                    newNotificationVM.Message = ticketNotfication.Message;
                    newNotificationVM.Icon = "fa-ticket";
                    newNotificationVM.Created = ticketNotfication.Created.ToString("MMM dd yyyy"); ;
                    response.Add(newNotificationVM);
                }
            }
            foreach (var projectNotification in db.ProjectNotifications.Where(tn => tn.UserId == userId))
            {
                if (!projectNotification.IsRead)
                {
                    var notType = projectNotification.NotificationType;
                    if (!Type.TryGetValue("XML_File", out notType))
                    {
                        notType = "info";
                    }
                    HomeNotificationVM newNotificationVM = new HomeNotificationVM();
                    newNotificationVM.notificationVersion = "project";

                    newNotificationVM.Id = projectNotification.Id;
                    newNotificationVM.Type = notType;
                    newNotificationVM.Subject = projectNotification.Subject;
                    newNotificationVM.Message = projectNotification.Message;
                    newNotificationVM.Icon = "fa-sitemap";
                    newNotificationVM.Created = projectNotification.Created.ToString("MMM dd yyyy"); ;
                    response.Add(newNotificationVM);
                }
            }
            response = response.OrderByDescending(r => r.Id).ToList();
            return Json(response);
        }
        public JsonResult setNotificationIsRead(string notificationId)
        {
            var list = notificationId.Split(',');
            if (list[1] == "ticket")
            {
                db.TicketNotifications.Find(int.Parse(list[0])).IsRead = true;
            }
            if (list[1] == "project")
            {
                db.TicketNotifications.Find(int.Parse(list[0])).IsRead = true;
            }
            db.SaveChanges();
            return Json(true);
        }
    }
}

