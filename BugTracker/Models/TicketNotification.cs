using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketNotification: Notification
    {
        //new public int Id { get; set; }

        #region Parents/Children
        public int TicketId { get; set; }

        public virtual ApplicationUser User { get; set; }

        #endregion
    }
}