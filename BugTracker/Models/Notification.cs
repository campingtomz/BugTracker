using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Notification
    {
        public int Id { get; set; }
        #region Parents/Children
        public int TicketId { get; set; }
        public string UserId { get; set; }
        #endregion
        #region Actual Property
        public string Icon { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string NotificationType { get; set; }
        public DateTime Created { get; set; }
        public bool IsRead { get; set; }
        #endregion
    }
}