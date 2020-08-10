using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        #region Parents/Children
        public int TicketId { get; set; }
        public virtual Ticket ticket { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        #endregion
        #region Actual Properties
        //This property of the ticket that was changed( status, type attachemtn
        public string Property { get; set; }
        //what the property was originally set to 
        public  string OldValue { get; set; }
        //what the property is now set to
        public string NewValue { get; set; }
        public DateTime ChangedOn { get; set; }
        #endregion  
    }
}