﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketHistory: History
    {
        public int Id { get; set; }
        
        public int TicketId { get; set; }
        public virtual Ticket ticket { get; set; }

       
    }
}