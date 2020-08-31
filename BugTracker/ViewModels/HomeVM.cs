using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;
using BugTracker.Helpers;

namespace BugTracker.ViewModels
{
    public class HomeVM
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int TotalNotificationsCount { get; set; }
        public List<ApplicationUser> UsersOnMyProjects { get; set; }
        public List<int> ticketPropertyValues { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<int> ProjectIds { get; set; }




        public HomeVM()
        {
            UsersOnMyProjects = new List<ApplicationUser>();
            Tickets = new List<Ticket>();
           
        }
      
    }
    
}