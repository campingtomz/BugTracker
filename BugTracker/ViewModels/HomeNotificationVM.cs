using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class HomeNotificationVM
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Created { get; set; }
        public string notificationVersion { get; set; }

    }
}