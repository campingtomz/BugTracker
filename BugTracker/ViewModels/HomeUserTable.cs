using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class HomeUserTable
    {
        public string Id { get; set; }
        public int TicketCount { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string AvatarPath { get; set; }
        public string UserRole { get; set; }

    }
}
