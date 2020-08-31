using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.ViewModels;
using BugTracker.Helpers;
using Newtonsoft.Json;

namespace BugTracker.Controllers
{
    public class HomeTableChartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketHelper ticketHelper = new TicketHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

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
            List<HomeTicketsTable> response = new List<HomeTicketsTable>();
            foreach (var ticket in ticketHelper.ListProjectsTickets(projectIds))
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
                response.Add(homeTicket);
            }

            return Json(response);
        }
    }
}
