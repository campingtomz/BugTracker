using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketHistory: History
    {
        public ApplicationUser User { get; set; }
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

    }
}