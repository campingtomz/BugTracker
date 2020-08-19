using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Notification
    {
        
        #region Parents/Children
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        #endregion
        #region Actual Property
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public bool IsRead { get; set; }

        public string NotificationType { get; set; }

        #endregion
    }
}