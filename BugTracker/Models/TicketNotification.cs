using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketNotification : Notification
    {
        public int TicketId { get; set; }
    }
}