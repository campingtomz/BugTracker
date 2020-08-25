using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class UserHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        #region Actual Properties { get; set; }
        //This property of the ticket that was changed( status, type attachemtn
        public string Property { get; set; }
        //what the property was originally set to 
        public string OldValue { get; set; }
        //what the property is now set to
        public string NewValue { get; set; }
        public DateTime ChangedOn { get; set; }
        #endregion 


    }
}