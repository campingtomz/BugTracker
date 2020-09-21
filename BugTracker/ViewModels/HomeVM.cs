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
        public List<int> ticketPropertyValues { get; set; }
        public int TicketsCount { get; set; }
        public int UsersCount { get; set; }
        public List<int> ProjectIds { get; set; }




        
      
    }
    
}