using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class HomeTicketsTable
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public string SubmitterName { get; set; }
        public string DeveloperName { get; set; }

        public int NumberOfComments { get; set; }
        public string Issue { get; set; }

    }
}