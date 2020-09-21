using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class HomeTicketAttachments
    {
        public string Name { get; set; }
        public string Uploader { get; set; }
        public string Description { get; set; }
        public string Created { get; set; }
        public string FilePath { get; set; }
    }
}