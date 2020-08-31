using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class HomeViewHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper userRoleHelper = new UserRoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        public Dictionary<string, int> ProjectTicketsTypeCount(List<Project> projects)
        {
            Dictionary<string, int> ticketType = new Dictionary<string, int>();
            foreach (var project in projects)
            {
                foreach (var type in db.TicketTypes)
                {
                    if (ticketType.ContainsKey(type.Name))
                    {
                        ticketType[type.Name] += project.Tickets.Where(t => t.TicketType.Name == type.Name).ToList().Count;
                    }

                    else
                    {
                        ticketType.Add(type.Name, project.Tickets.Where(t => t.TicketType.Name == type.Name).ToList().Count);

                    }
                }

            }
            return ticketType;
        }
        public Dictionary<string, int> ProjectTicketsPriorityCount(List<Project> projects)
        {
            Dictionary<string, int> ticketPriority = new Dictionary<string, int>();
            foreach (var project in projects)
            {
                foreach (var Priority in db.TicketPriorities)
                {
                    if (ticketPriority.ContainsKey(Priority.Name))
                    {
                        ticketPriority[Priority.Name] += project.Tickets.Where(t => t.TicketPriority.Name == Priority.Name).ToList().Count;

                    }
                    else
                    {
                        ticketPriority.Add(Priority.Name, project.Tickets.Where(t => t.TicketPriority.Name == Priority.Name).ToList().Count);

                    }
                }
            }
            return ticketPriority;
        }
        public Dictionary<string, int> ProjectTicketsStatusCount(List<Project> projects)
        {
            Dictionary<string, int> ticketStatus = new Dictionary<string, int>();
            foreach (var project in projects)
            {
                foreach (var status in db.TicketStatuses)
                {
                    if (ticketStatus.ContainsKey(status.Name))
                    {
                        ticketStatus[status.Name] += project.Tickets.Where(t => t.TicketStatus.Name == status.Name).ToList().Count;
                    }
                    else
                    {
                        ticketStatus.Add(status.Name, project.Tickets.Where(t => t.TicketStatus.Name == status.Name).ToList().Count);
                    }
                }

            }
            return ticketStatus;
        }
    }
}