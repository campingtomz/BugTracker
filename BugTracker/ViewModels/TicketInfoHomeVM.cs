using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class TicketInfoHomeVM
    {
        public string Issue { get; set; }
        public string IssueDescription { get; set; }
        public string SubmitterName { get; set; }
        public string DeveloperName { get; set; }
        public string TicketPriority { get; set; }
        public string TicketStatus { get; set; }
        public string TicketType { get; set; }
        public string Created { get; set; }
    }
}