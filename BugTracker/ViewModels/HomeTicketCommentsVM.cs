using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class HomeTicketCommentsVM
    {
        public int TicketId { get; set; }        
        public string UserName { get; set; }
        public string AvatarPath { get; set; }

        public string Comment { get; set; }
        public string Created { get; set; }
    }
}